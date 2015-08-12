﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Birchman.RemoteProvisioning.Domain.Extensions;
using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Domain.ObjectHandlers;
using Birchman.RemoteProvisioning.Domain.Model;
using Field = Microsoft.SharePoint.Client.Field;
using View = Birchman.RemoteProvisioning.Domain.Model.View;
using Birchman.RemoteProvisioning.Utilities;
using Birchman.RemoteProvisioning.Core;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers
{
    public class ObjectListInstance : ObjectHandlerBase
    {
        public override void ProvisionObjects(Web web, ProvisioningTemplate template)
        {
            var parser = new TokenParser(web);

            if (!web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                web.Context.Load(web, w => w.ServerRelativeUrl);
                web.Context.ExecuteQuery();
            }

            web.Context.Load(web.Lists, lc => lc.IncludeWithDefaultProperties(l => l.RootFolder.ServerRelativeUrl));
            web.Context.ExecuteQuery();

            //TODO: Upgrade to SharePoint.Client v16.0 so refactoring can be done (uncomment following line when done!)            
            //var existingLists = web.Lists.Select(existingList => existingList.RootFolder.ServerRelativeUrl).ToList();
            var existingLists = web.Lists.AsEnumerable().Select(existingList => existingList.RootFolder.ServerRelativeUrl).ToList();
            var serverRelativeUrl = web.ServerRelativeUrl;


            foreach (var list in template.Lists)
            {
                if (!existingLists.Contains(UrlUtility.Combine(serverRelativeUrl, list.Url)))
                {
                    var listCreate = new ListCreationInformation();
                    listCreate.Description = list.Description;
                    listCreate.TemplateType = list.TemplateType;
                    listCreate.Title = list.Title;
                    listCreate.QuickLaunchOption = list.OnQuickLaunch ? QuickLaunchOptions.On : QuickLaunchOptions.Off;
                    listCreate.Url = parser.Parse(list.Url);

                    var createdList = web.Lists.Add(listCreate);

                    createdList.EnableVersioning = list.EnableVersioning;
                    if (!String.IsNullOrEmpty(list.DocumentTemplate))
                    {
                        createdList.DocumentTemplateUrl = parser.Parse(list.DocumentTemplate);
                    }
                    createdList.Hidden = list.Hidden;
                    createdList.ContentTypesEnabled = list.ContentTypesEnabled;

                    createdList.Update();


                    web.Context.ExecuteQuery();


                    // TODO: handle 'removedefaultcontenttype'

                    foreach (var ctBinding in list.ContentTypeBindings)
                    {
                        createdList.AddContentTypeToListById(ctBinding.ContentTypeID);
                        if (ctBinding.Default)
                        {
                            createdList.SetDefaultContentTypeToList(ctBinding.ContentTypeID);
                        }
                    }

                    if (list.Fields.Any())
                    {
                        foreach (var field in list.Fields)
                        {
                            // Double check that the content type did not include the field before adding it in
                            if (!createdList.FieldExistsById(field.SchemaXml.Substring(field.SchemaXml.IndexOf("ID=\"{") + 5, 36)))
                            {
                                var fieldXml = parser.Parse(field.SchemaXml);
                                createdList.Fields.AddFieldAsXml(fieldXml, false, AddFieldOptions.DefaultValue);
                            }
                        }
                        createdList.Update();
                        web.Context.ExecuteQuery();
                    }

                    if (list.FieldRefs.Any())
                    {
                        foreach (var fieldRef in list.FieldRefs)
                        {
                            var field = web.GetFieldById<Field>(fieldRef.ID);
                            if (!createdList.FieldExistsById(fieldRef.ID))
                            {
                                createdList.Fields.Add(field);
                            }

                        }
                        createdList.Update();
                        web.Context.ExecuteQuery();
                    }

                    foreach (var view in list.Views)
                    {

                        var viewDoc = XDocument.Parse(view.SchemaXml);

                        var displayNameXml = viewDoc.Root.Attribute("DisplayName");
                        if (displayNameXml == null)
                        {
                            throw new ApplicationException("Invalid View element, missing a valid value for the attribute DisplayName.");
                        }
                        var viewTitle = displayNameXml.Value;

                        // Type
                        var viewTypeString = viewDoc.Root.Attribute("Type") != null ? viewDoc.Root.Attribute("Type").Value : "None";
                        viewTypeString = viewTypeString[0].ToString().ToUpper() + viewTypeString.Substring(1).ToLower();
                        var viewType = (ViewType)Enum.Parse(typeof(ViewType), viewTypeString);

                        // Fields
                        string[] viewFields = null;
                        var viewFieldsElement = viewDoc.Descendants("ViewFields").FirstOrDefault();
                        if (viewFieldsElement != null)
                        {
                            viewFields = (from field in viewDoc.Descendants("ViewFields").Descendants("FieldRef") select field.Attribute("Name").Value).ToArray();
                        }

                        // Default view
                        var viewDefault = viewDoc.Root.Attribute("DefaultView") != null && Boolean.Parse(viewDoc.Root.Attribute("DefaultView").Value);

                        // Row limit
                        bool viewPaged = true;
                        uint viewRowLimit = 30;
                        var rowLimitElement = viewDoc.Descendants("RowLimit").FirstOrDefault();
                        if (rowLimitElement != null)
                        {
                            if (rowLimitElement.Attribute("Paged") != null)
                            {
                                viewPaged = bool.Parse(rowLimitElement.Attribute("Paged").Value);
                            }
                            viewRowLimit = uint.Parse(rowLimitElement.Value);
                        }

                        // Query
                        var viewQuery = new StringBuilder();
                        foreach (var queryElement in viewDoc.Descendants("Query").Elements())
                        {
                            viewQuery.Append(queryElement.ToString());
                        }

                        var viewCI = new ViewCreationInformation
                        {
                            ViewFields = viewFields,
                            RowLimit = viewRowLimit,
                            Paged = viewPaged,
                            Title = viewTitle,
                            Query = viewQuery.ToString(),
                            ViewTypeKind = viewType,
                            PersonalView = false,
                            SetAsDefaultView = viewDefault
                        };

                        createdList.Views.Add(viewCI);
                        createdList.Update();
                        web.Context.ExecuteQuery();
                    }


                }

            }
        }

        public override ProvisioningTemplate CreateEntities(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo)
        {
            if (!web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                web.Context.Load(web, w => w.ServerRelativeUrl);
                web.Context.ExecuteQuery();
            }

            var serverRelativeUrl = web.ServerRelativeUrl;

            // For each list in the site
            ListCollection lists = web.Lists;
            web.Context.Load(lists, lc => lc.IncludeWithDefaultProperties(l => l.ContentTypes, l => l.Views, l => l.RootFolder.ServerRelativeUrl, l => l.Fields));
            web.Context.ExecuteQuery();
            foreach (var item in lists)
            {
                // Do not export system lists
                if (!item.Hidden)
                {

                    int index = -1;
                    if (creationInfo.BaseTemplate != null)
                    {
                        // Check if we need to skip this list...if so let's do it before we gather all the other information for this list...improves perf
                        index = creationInfo.BaseTemplate.Lists.FindIndex(f => f.Url.Equals(item.RootFolder.ServerRelativeUrl.Substring(serverRelativeUrl.Length)) &&
                                                                  f.TemplateType.Equals(item.BaseTemplate));
                    }

                    if (index == -1)
                    {
                        var contentTypeFields = new List<FieldRef>();
                        ListInstance list = new ListInstance();
                        list.Description = item.Description;
                        list.EnableVersioning = item.EnableVersioning;
                        list.TemplateType = item.BaseTemplate;
                        list.Title = item.Title;
                        list.Hidden = item.Hidden;
                        list.DocumentTemplate = Tokenize(item.DocumentTemplateUrl, web.Url);
                        list.ContentTypesEnabled = item.ContentTypesEnabled;
                        list.Url = item.RootFolder.ServerRelativeUrl.Substring(serverRelativeUrl.Length).TrimStart('/');

                        int count = 0;

                        foreach (var ct in item.ContentTypes)
                        {
                            web.Context.Load(ct, c => c.Parent);
                            web.Context.ExecuteQuery();
                            if (ct.Parent != null)
                            {
                                // Add the parent to the list of content types
                                if (!BuiltInContentTypeId.Contains(ct.Parent.StringId))
                                {
                                    list.ContentTypeBindings.Add(new ContentTypeBinding() {ContentTypeID = ct.Parent.StringId, Default = count == 0 ? true : false});
                                }
                            }
                            else
                            {
                                list.ContentTypeBindings.Add(new ContentTypeBinding() {ContentTypeID = ct.StringId, Default = count == 0});
                            }
                         
                            web.Context.Load(ct.FieldLinks);
                            web.Context.ExecuteQuery();
                            foreach (var fieldLink in ct.FieldLinks)
                            {
                                if (!fieldLink.Hidden)
                                {
                                    contentTypeFields.Add(new FieldRef() { ID = fieldLink.Id });
                                }
                            }
                            count++;
                        }

                        foreach (var view in item.Views)
                        {
                            if (!view.Hidden)
                            {
                                list.Views.Add(new View() { SchemaXml = view.ListViewXml });
                            }
                        }

                        var siteColumns = web.Fields;
                        web.Context.Load(siteColumns, scs => scs.Include(sc => sc.Id));
                        web.Context.ExecuteQuery();

                        foreach (var field in item.Fields)
                        {
                            if (!field.Hidden)
                            {
                                if (siteColumns.FirstOrDefault(sc => sc.Id == field.Id) != null)
                                {
                                    if (contentTypeFields.FirstOrDefault(c => c.ID == field.Id) == null)
                                    {
                                        list.FieldRefs.Add(new FieldRef() { ID = field.Id });
                                    }
                                }
                                else
                                {
                                    list.Fields.Add((new Model.Field() { SchemaXml = field.SchemaXml }));
                                }
                            }
                        }
                        template.Lists.Add(list);
                    }
                }
            }

            return template;
        }

        private string Tokenize(string url, string webUrl)
        {

            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            else
            {
                if (url.IndexOf("/_catalogs/theme", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    return url.Substring(url.IndexOf("/_catalogs/theme", StringComparison.InvariantCultureIgnoreCase)).Replace("/_catalogs/theme", "~themecatalog");
                }
                if (url.IndexOf("/_catalogs/masterpage", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    return url.Substring(url.IndexOf("/_catalogs/masterpage", StringComparison.InvariantCultureIgnoreCase)).Replace("/_catalogs/masterpage", "~masterpagecatalog");
                }
                if (url.IndexOf(webUrl, StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    return url.Replace(webUrl, "~site");
                }
                else
                {
                    Uri r = new Uri(webUrl);
                    if (url.IndexOf(r.PathAndQuery, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        return url.Replace(r.PathAndQuery, "~site");
                    }
                }

                // nothing to tokenize...
                return url;
            }
        }
    }
}

