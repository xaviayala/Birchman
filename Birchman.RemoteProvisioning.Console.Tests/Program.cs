using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Birchman.RemoteProvisioning.Console.Tests.ProvisioningTemplates;
using Birchman.RemoteProvisioning.Domain.Connectors;
using Birchman.RemoteProvisioning.Domain.Extensions;
using Birchman.RemoteProvisioning.Domain.Model;
using Microsoft.SharePoint.Client.Tests;
using Birchman.RemoteProvisioning.Domain.ObjectHandlers;
using Birchman.RemoteProvisioning.Domain.Providers.Xml;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Birchman.RemoteProvisioning.Console.Tests
{
    class Program
    {
        static void WorkspaceProvisioner()
        {
            using (var cc = TestCommon.CreateClientContext())
            {
                ProvisioningTemplateCreationInformation ptCreationInfo = new ProvisioningTemplateCreationInformation(cc.Web);
                ProvisioningTemplate pt = ptCreationInfo.BaseTemplate;


                cc.Web.ApplyProvisioningTemplate(pt);
            }
        }

        static void Main(string[] args)
        {
            //SetDefaultContentTypeToListTest();

            FieldAndContentTypeExtensionsTests FaCT = new FieldAndContentTypeExtensionsTests();
            WorkspaceProvisioner();
        }
    }
}
