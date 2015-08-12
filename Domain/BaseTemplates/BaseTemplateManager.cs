﻿using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Birchman.RemoteProvisioning.Domain.Model;
using Birchman.RemoteProvisioning.Domain.Providers.Xml;
using Birchman.RemoteProvisioning.Utilities;


namespace Microsoft.SharePoint.Client
{
    /// <summary>
    /// This class will be used to provide access to the right base template configuration
    /// </summary>
    public static class BaseTemplateManager
    {

        public static ProvisioningTemplate GetBaseTemplate(this Web web)
        {
            web.Context.Load(web, p => p.WebTemplate, p => p.Configuration);
            web.Context.ExecuteQueryRetry();

            ProvisioningTemplate provisioningTemplate = null;

            try
            {
                string baseTemplate = string.Format("Birchman.RemoteProvisioning.Domain.BaseTemplates.v{0}.{1}{2}Template.xml", GetSharePointVersion(), web.WebTemplate, web.Configuration);
                using (Stream stream = typeof(BaseTemplateManager).Assembly.GetManifestResourceStream(baseTemplate))
                {
                    // Get the XML document from the stream
                    XDocument doc = XDocument.Load(stream);

                    // And convert it into a ProvisioningTemplate
                    provisioningTemplate = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(doc).ToProvisioningTemplate();
                }
            }
            catch(Exception)
            {
                //TODO: log message
            }

            return provisioningTemplate;
        }

        private static int GetSharePointVersion()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Site));
            return asm.GetName().Version.Major;
        }

    }
}
