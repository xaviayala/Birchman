using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Birchman.RemoteProvisioning.Console.Tests;
using Birchman.RemoteProvisioning.Domain.Providers.Xml;

namespace Birchman.RemoteProvisioning.Console.Tests.Providers
{
    [TestClass]
    public class XMLProvidersTests
    {
        #region Test variables
        static string testContainer = "pnptest";
        static string testContainerSecure = "pnptestsecure";
        #endregion

       
        #region XML File System tests

        [TestMethod]
        public void XMLFileSystemGetTemplatesTest()
        {
            XMLTemplateProvider provider = 
                new XMLFileSystemTemplateProvider(
                    String.Format(@"{0}\..\..\Resources", 
                    AppDomain.CurrentDomain.BaseDirectory), 
                    "Templates");

            var result = provider.GetTemplates();

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Files.Count == 1);
            Assert.IsTrue(result[1].Files.Count == 5);
        }

        [TestMethod]
        public void XMLFileSystemGetTemplate1Test()
        {
            var _expectedID = "SPECIALTEAM";
            var _expectedVersion = 1.0;

            XMLTemplateProvider provider =
                new XMLFileSystemTemplateProvider(
                    String.Format(@"{0}\..\..\Resources",
                    AppDomain.CurrentDomain.BaseDirectory),
                    "Templates");

            var result = provider.GetTemplate("ProvisioningTemplate-2015-03-Sample-01.xml");

            Assert.AreEqual(_expectedID, result.ID);
            Assert.AreEqual(_expectedVersion, result.Version);
            Assert.IsTrue(result.Lists.Count == 1);
            Assert.IsTrue(result.Files.Count == 1);
            Assert.IsTrue(result.SiteFields.Count == 4);
        }

        [TestMethod]
        public void XMLFileSystemGetTemplate2Test()
        {
            var _expectedID = "SPECIALTEAM";
            var _expectedVersion = 1.0;

            XMLTemplateProvider provider =
                new XMLFileSystemTemplateProvider(
                    String.Format(@"{0}\..\..\Resources",
                    AppDomain.CurrentDomain.BaseDirectory),
                    "Templates");

            var result = provider.GetTemplate("ProvisioningTemplate-2015-03-Sample-02.xml");

            Assert.AreEqual(_expectedID, result.ID);
            Assert.AreEqual(_expectedVersion, result.Version);
            Assert.IsTrue(result.Lists.Count == 2);
            Assert.IsTrue(result.Files.Count == 5);
            Assert.IsTrue(result.SiteFields.Count == 4);
        }

        #endregion

        #region XML Azure Storage tests

        //[TestMethod]
        //public void XMLAzureStorageGetTemplatesTest()
        //{
        //    XMLTemplateProvider provider = 
        //        new XMLAzureStorageTemplateProvider(
        //            TestCommon.AzureStorageKey, testContainer);

        //    var result = provider.GetTemplates();

        //    Assert.IsTrue(result.Count == 2);
        //    Assert.IsTrue(result[0].Files.Count == 1);
        //    Assert.IsTrue(result[1].Files.Count == 5);
        //}

        //[TestMethod]
        //public void XMLAzureStorageGetTemplate1Test()
        //{
        //    var _expectedID = "SPECIALTEAM";
        //    var _expectedVersion = 1.0;

        //    XMLTemplateProvider provider =
        //        new XMLAzureStorageTemplateProvider(
        //            TestCommon.AzureStorageKey, testContainer);

        //    var result = provider.GetTemplate("ProvisioningTemplate-2015-03-Sample-01.xml");

        //    Assert.AreEqual(_expectedID, result.ID);
        //    Assert.AreEqual(_expectedVersion, result.Version);
        //    Assert.IsTrue(result.Lists.Count == 1);
        //    Assert.IsTrue(result.Files.Count == 1);
        //    Assert.IsTrue(result.SiteFields.Count == 4);
        //}

        //[TestMethod]
        //public void XMLAzureStorageGetTemplate2SecureTest()
        //{
        //    var _expectedID = "SPECIALTEAM";
        //    var _expectedVersion = 1.0;

        //    XMLTemplateProvider provider =
        //        new XMLAzureStorageTemplateProvider(
        //            TestCommon.AzureStorageKey, testContainerSecure);

        //    var result = provider.GetTemplate("ProvisioningTemplate-2015-03-Sample-02.xml");

        //    Assert.AreEqual(_expectedID, result.ID);
        //    Assert.AreEqual(_expectedVersion, result.Version);
        //    Assert.IsTrue(result.Lists.Count == 2);
        //    Assert.IsTrue(result.Files.Count == 5);
        //    Assert.IsTrue(result.SiteFields.Count == 4);
        //}

        #endregion
    }
}
