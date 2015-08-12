﻿using System;
using System.Linq;
using System.Xml.Linq;
using Birchman.RemoteProvisioning.Console.Tests;
using Birchman.RemoteProvisioning.Domain.Extensions;
using Birchman.RemoteProvisioning.Domain.Model;
using Birchman.RemoteProvisioning.Domain.Providers.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Birchman.RemoteProvisioning.Utilities;
using Microsoft.SharePoint.Client;

namespace Birchman.RemoteProvisioning.Console.Tests.ProvisioningTemplates
{
    [TestClass]
    public class DomainModelTests
    {
        private string _provisioningTemplatePath1 = string.Empty;
        private string _provisioningTemplatePath2 = string.Empty;
        private const string TEST_CATEGORY = "Framework Provisioning Domain Model";

        [TestInitialize()]
        public void Intialize()
        {
            this._provisioningTemplatePath1 = string.Format(@"Resources\Templates\{0}", "ProvisioningTemplate-2015-03-Sample-01.xml");
            this._provisioningTemplatePath2 = string.Format(@"Resources\Templates\{0}", "ProvisioningTemplate-2015-03-Sample-02.xml");
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanDeserializeXMLToDomainObject1()
        {
            this.GetProvisioningTemplate();
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath1);
            var _pt = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
            Assert.IsNotNull(_pt);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanSerializeDomainObjectToXML1()
        {
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath1);
            var _pt = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
            var _spt = _pt.ToXml();

            Assert.IsTrue(_spt.IsValidSharePointProvisioningTemplate());
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanSerializeDomainObjectToXMLString1()
        {
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath1);
            var _pt = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
            var _xmlString = _pt.ToXmlString();

            Assert.IsTrue(!String.IsNullOrEmpty(_xmlString));
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanSerializeDomainObjectToXMLStream1()
        {
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath1);
            var _pt = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
            var _xmlStream = _pt.ToXmlStream();

            Assert.IsNotNull(_xmlStream);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanDeserializeXMLToDomainObject2()
        {
            this.GetProvisioningTemplate();
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath2);
            var _pt = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
            Assert.IsNotNull(_pt);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanSerializeDomainObjectToXML2()
        {
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath2);
            var _pt = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
            var _spt = _pt.ToXml();

            Assert.IsTrue(_spt.IsValidSharePointProvisioningTemplate());
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetTemplateNameandVersion()
        {
            var _expectedID = "SPECIALTEAM";
            var _expectedVersion = 1.0;

            var _pt = this.GetProvisioningTemplate();
            Assert.AreEqual(_expectedID, _pt.ID);
            Assert.AreEqual(_expectedVersion, _pt.Version);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetPropertyBagEntries()
        {
            var _expectedCount = 2;
            var _pt = GetProvisioningTemplate();

            var _pb1KEY = "KEY1";
            var _pb1Value = "value1";
            var _pb2KEY = "KEY2";
            var _pb2Value = "value2";

            Assert.AreEqual(_expectedCount, _pt.PropertyBagEntries.Count);

            var _pb1 = _pt.PropertyBagEntries[0];
            Assert.AreEqual(_pb1KEY, _pb1.Key);
            Assert.AreEqual(_pb1Value, _pb1.Value);

            var _pb2 = _pt.PropertyBagEntries[1];
            Assert.AreEqual(_pb2KEY, _pb2.Key);
            Assert.AreEqual(_pb2Value, _pb2.Value);
        }
     
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetOwners()
        {
            var _pt = GetProvisioningTemplate();
            var _expectedCount = 2;
            var _expectedUser1 = "user@contoso.com";
            var _expectedUser2 = "U_SHAREPOINT_ADMINS";

            var _siteSecurity = _pt.Security;
            var _users = _pt.Security.AdditionalOwners;
            Assert.AreEqual(_expectedCount, _users.Count);

            var _u1 = _pt.Security.AdditionalOwners[0].Name;
            var _u2 = _pt.Security.AdditionalOwners[1].Name;

            Assert.AreEqual(_expectedUser1, _u1);
            Assert.AreEqual(_expectedUser2, _u2);
        }
        
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetAdminstrators()
        {
            var _pt = GetProvisioningTemplate();
            var _expectedCount = 2;
            var _expectedUser1 = "user@contoso.com";
            var _expectedUser2 = "U_SHAREPOINT_ADMINS";

            var _siteSecurity = _pt.Security;
            var _users = _pt.Security.AdditionalAdministrators;
            Assert.AreEqual(_expectedCount, _users.Count);

            var _u1 = _pt.Security.AdditionalAdministrators[0].Name;
            var _u2 = _pt.Security.AdditionalAdministrators[1].Name;

            Assert.AreEqual(_expectedUser1, _u1);
            Assert.AreEqual(_expectedUser2, _u2);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetMembers()
        {
            var _pt = GetProvisioningTemplate();
            var _expectedCount = 2;
            var _expectedUser1 = "user@contoso.com";
            var _expectedUser2 = "U_SHAREPOINT_ADMINS";

            var _siteSecurity = _pt.Security;
            var _users = _pt.Security.AdditionalMembers;
            Assert.AreEqual(_expectedCount, _users.Count);

            var _u1 = _pt.Security.AdditionalMembers[0].Name;
            var _u2 = _pt.Security.AdditionalMembers[1].Name;

            Assert.AreEqual(_expectedUser1, _u1);
            Assert.AreEqual(_expectedUser2, _u2);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetVistors()
        {
            var _pt = GetProvisioningTemplate();
            var _expectedCount = 2;
            var _expectedUser1 = "user@contoso.com";
            var _expectedUser2 = "U_SHAREPOINT_ADMINS";

            var _siteSecurity = _pt.Security;
            var _additionalAdmins = _pt.Security.AdditionalVisitors;
            Assert.AreEqual(_expectedCount, _additionalAdmins.Count);

            var _u1 = _pt.Security.AdditionalVisitors[0].Name;
            var _u2 = _pt.Security.AdditionalVisitors[1].Name;

            Assert.AreEqual(_expectedUser1, _u1);
            Assert.AreEqual(_expectedUser2, _u2);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetFeatures()
        {
            var _pt = this.GetProvisioningTemplate();
            var _sfs = _pt.Features.SiteFeatures;

            var _expectedSiteFeaturesCount = 3;
            var _expectedWebFeaturesCount = 4;

            Assert.AreEqual(_expectedSiteFeaturesCount, _pt.Features.SiteFeatures.Count);
            Assert.AreEqual(_expectedWebFeaturesCount, _pt.Features.WebFeatures.Count);

            foreach(var _f in _sfs)
            {
                Assert.IsTrue(_f.ID != Guid.Empty);
            }
            //Birchman.RemoteProvisioning.Domain.Model
            var f = new Domain.Model.Feature();
            _pt.Features.SiteFeatures.Add(f);

            
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanGetCustomActions()
        {
            var _pt = this.GetProvisioningTemplate();
            var _csa = _pt.CustomActions.SiteCustomActions.FirstOrDefault();
            Assert.IsNotNull(_csa.Rights);
      
        }
     
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanSerializeToJSon()
        {
           var _pt = this.GetProvisioningTemplate();
           var _json = JsonUtility.Serialize<ProvisioningTemplate>(_pt);
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void CanSerializeToXml()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var template = ctx.Web.GetProvisioningTemplate();
                string xml = XMLSerializer.Serialize(template);
            }
        }

        #region Comparison Tests

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void AreTemplatesEqual()
        {
            XDocument _doc1 = XDocument.Load(this._provisioningTemplatePath1);
            var _pt1 = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc1).ToProvisioningTemplate();

            XDocument _doc2 = XDocument.Load(this._provisioningTemplatePath2);
            var _pt2 = XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc2).ToProvisioningTemplate();

            Assert.IsFalse(_pt1.Equals(_pt2));
            Assert.IsTrue(_pt1.Equals(_pt1));
        }

        #endregion

        #region Test Support
        /// <summary>
        /// Test Support to return ProvisionTemplate 
        /// </summary>
        /// <returns></returns>
        protected ProvisioningTemplate GetProvisioningTemplate()
        {
            XDocument _doc = XDocument.Load(this._provisioningTemplatePath1);
            return XMLSerializer.Deserialize<SharePointProvisioningTemplate>(_doc).ToProvisioningTemplate();
        }

        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void GetRemoteTemplateTest()
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                var template = ctx.Web.GetProvisioningTemplate();
                Assert.IsTrue(template.Lists.Any());
            }
        }

        #endregion
    }
}
