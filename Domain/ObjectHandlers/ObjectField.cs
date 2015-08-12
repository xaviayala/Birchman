﻿using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Data.Enums;
using Birchman.RemoteProvisioning.Domain.Extensions;
using Birchman.RemoteProvisioning.Domain.Model;
using Field = Birchman.RemoteProvisioning.Domain.Model.Field;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers
{
    public class ObjectField : ObjectHandlerBase
    {
        public override void ProvisionObjects(Web web, ProvisioningTemplate template)
        {

            // if this is a sub site then we're not provisioning fields. Technically this can be done but it's not a recommended practice
            if (web.IsSubSite())
            {
                return;
            }

            var parser = new TokenParser(web);
            var existingFields = web.Fields;

            web.Context.Load(existingFields, fs => fs.Include(f => f.Id));
            web.Context.ExecuteQuery();
            //TODO: Upgrade to SharePoint.Client v16.0 so refactoring can be done (uncomment following line when done!)            
            //var existingFieldIds = existingFields.Select(l => l.Id).ToList();
            var existingFieldIds = existingFields.AsEnumerable().Select(l => l.Id).ToList();

            var fields = template.SiteFields;

            foreach (var field in fields)
            {
                XDocument document = XDocument.Parse(field.SchemaXml);
                var fieldId = document.Root.Attribute("ID").Value;


                if (!existingFieldIds.Contains(Guid.Parse(fieldId)))
                {
                    var fieldXml = parser.Parse(field.SchemaXml);
                    web.Fields.AddFieldAsXml(fieldXml, false, AddFieldOptions.DefaultValue);
                    web.Context.ExecuteQuery();
                }
            }
        }


        public override ProvisioningTemplate CreateEntities(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo)
        {
            // if this is a sub site then we're not creating field entities.
            if (web.IsSubSite())
            {
                return template;
            }

            var existingFields = web.Fields;
            web.Context.Load(existingFields, fs => fs.Include(f => f.Id, f => f.SchemaXml));
            web.Context.ExecuteQuery();


            foreach (var field in existingFields)
            {
                if (!BuiltInFieldId.Contains(field.Id))
                {
                    template.SiteFields.Add(new Field() { SchemaXml = field.SchemaXml });
                }
            }
            // If a base template is specified then use that one to "cleanup" the generated template model
            if (creationInfo.BaseTemplate != null)
            {
                template = CleanupEntities(template, creationInfo.BaseTemplate);
            }

            return template;
        }

        private ProvisioningTemplate CleanupEntities(ProvisioningTemplate template, ProvisioningTemplate baseTemplate)
        {
            foreach (var field in baseTemplate.SiteFields)
            {

                XDocument xDoc = XDocument.Parse(field.SchemaXml);
                var id = xDoc.Root.Attribute("ID") != null ? xDoc.Root.Attribute("ID").Value : null;
                if (id != null)
                {
                    int index = template.SiteFields.FindIndex(f => f.SchemaXml.IndexOf(id, StringComparison.InvariantCultureIgnoreCase) > -1);

                    if (index > -1)
                    {
                        template.SiteFields.RemoveAt(index);
                    }
                }
            }

            return template;
        }
    }
}
