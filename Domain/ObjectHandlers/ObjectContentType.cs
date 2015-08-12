using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Domain.Model;
using Birchman.RemoteProvisioning.Domain.Connectors;
using Birchman.RemoteProvisioning.Domain.Extensions;
using Birchman.RemoteProvisioning.Core;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers
{
    public class ObjectContentType : ObjectHandlerBase
    {
        public override void ProvisionObjects(Web web, ProvisioningTemplate template)
        {
            // if this is a sub site then we're not provisioning content types. Technically this can be done but it's not a recommended practice
            if (web.IsSubSite())
            {
                return;
            }

            var existingCts = web.AvailableContentTypes;
            web.Context.Load(existingCts, cts => cts.Include(ct => ct.StringId));
            web.Context.ExecuteQuery();

            //TODO: Upgrade to SharePoint.Client v16.0 so refactoring can be done (uncomment following line when done!)            
            //var existingCtsIds = existingCts.Select(cts => cts.StringId.ToLower()).ToList();
            var existingCtsIds = existingCts.AsEnumerable().Select(cts => cts.StringId.ToLower()).ToList();

            foreach (var ct in template.ContentTypes)
            {
                // find the id of the content type
                XDocument document = XDocument.Parse(ct.SchemaXml);
                var contentTypeId = document.Root.Attribute("ID").Value;
                if (!existingCtsIds.Contains(contentTypeId.ToLower()))
                {
                    web.CreateContentTypeFromXMLString(ct.SchemaXml);
                    existingCtsIds.Add(contentTypeId);
                }
            }
        }

        public override ProvisioningTemplate CreateEntities(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo)
        {
            // if this is a sub site then we're not creating content type entities. 
            if (web.IsSubSite())
            {
                return template;
            }

            var cts = web.ContentTypes;
            web.Context.Load(cts);
            web.Context.ExecuteQuery();

            foreach (var ct in cts)
            {
                if (!BuiltInContentTypeId.Contains(ct.StringId))
                {
                    template.ContentTypes.Add(new Birchman.RemoteProvisioning.Domain.Model.ContentType() { SchemaXml = ct.SchemaXml });
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
            foreach (var ct in baseTemplate.ContentTypes)
            {
                XDocument xDoc = XDocument.Parse(ct.SchemaXml);
                var id = xDoc.Root.Attribute("ID") != null ? xDoc.Root.Attribute("ID").Value : null;
                if (id != null)
                {
                    int index = template.ContentTypes.FindIndex(f => f.SchemaXml.IndexOf(id, StringComparison.InvariantCultureIgnoreCase) > -1);

                    if (index > -1)
                    {
                        template.ContentTypes.RemoveAt(index);
                    }
                }
            }

            return template;
        }
    }
}
