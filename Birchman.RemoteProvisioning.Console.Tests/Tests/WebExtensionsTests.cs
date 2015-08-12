using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Birchman.RemoteProvisioning.Console.Tests;
using Birchman.RemoteProvisioning.Domain.Extensions;
using Birchman.RemoteProvisioning.Domain.Model;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.SharePoint.Client.Tests
{
    public class WebExtensionsTests
    {
        const string INDEXED_PROPERTY_KEY = "vti_indexedpropertykeys";
        private string _key = null;
        private string _value_string = null;
        private int _value_int = 12345;
     
        private ClientContext clientContext;

        #region Test initialize and cleanup
        
        public void Initialize()
        {
            clientContext = TestCommon.CreateClientContext();

            _key = "TEST_KEY_" + DateTime.Now.ToFileTime();
            _value_string = "TEST_VALUE_" + DateTime.Now.ToFileTime();
        }

        
        public void Cleanup()
        {
            // Deactivate sideloading
            clientContext.Load(clientContext.Site);
            clientContext.ExecuteQueryRetry();
            
            var props = clientContext.Web.AllProperties;
            clientContext.Load(props);
            clientContext.ExecuteQueryRetry();

            if (props.FieldValues.ContainsKey(_key))
            {
                props[_key] = null;
                props.FieldValues.Remove(_key);
            }
            if (props.FieldValues.ContainsKey(INDEXED_PROPERTY_KEY))
            {
                props[INDEXED_PROPERTY_KEY] = null;
                props.FieldValues.Remove(INDEXED_PROPERTY_KEY);
            }
            clientContext.Web.Update();
            clientContext.ExecuteQueryRetry();
            clientContext.Dispose();
        }
        #endregion

        #region Property bag tests
        [TestMethod()]
        public void SetPropertyBagValueIntTest()
        {
            clientContext.Web.SetPropertyBagValue(_key, _value_int);

            var props = clientContext.Web.AllProperties;
            clientContext.Load(props);
            clientContext.ExecuteQueryRetry();
            Assert.IsTrue(props.FieldValues.ContainsKey(_key));
            Assert.AreEqual(_value_int, props.FieldValues[_key] as int?);
        }

        [TestMethod()]
        public void SetPropertyBagValueStringTest()
        {
            clientContext.Web.SetPropertyBagValue(_key, _value_string);

            var props = clientContext.Web.AllProperties;
            clientContext.Load(props);
            clientContext.ExecuteQueryRetry();
            Assert.IsTrue(props.FieldValues.ContainsKey(_key), "Entry not added");
            Assert.AreEqual(_value_string, props.FieldValues[_key] as string, "Entry not set with correct value");
        }

        [TestMethod()]
        public void SetPropertyBagValueMultipleRunsTest()
        {
            string key2 = _key + "_multiple";
            clientContext.Web.SetPropertyBagValue(key2, _value_string);
            clientContext.Web.SetPropertyBagValue(_key, _value_string);

            var props = clientContext.Web.AllProperties;
            clientContext.Load(props);
            clientContext.ExecuteQueryRetry();
            Assert.IsTrue(props.FieldValues.ContainsKey(_key), "Entry not added");
            Assert.AreEqual(_value_string, props.FieldValues[_key] as string, "Entry not set with correct value");
        }

        [TestMethod()]
        public void RemovePropertyBagValueTest()
        {
            var web = clientContext.Web;
            var props = web.AllProperties;
            web.Context.Load(props);
            web.Context.ExecuteQueryRetry();

            props[_key] = _value_string;

            web.Update();
            web.Context.ExecuteQueryRetry();

            web.RemovePropertyBagValue(_key);

            props.RefreshLoad();
            clientContext.ExecuteQueryRetry();
            Assert.IsFalse(props.FieldValues.ContainsKey(_key), "Entry not removed");
        }

        [TestMethod()]
        public void GetPropertyBagValueIntTest()
        {
            var web = clientContext.Web;
            var props = web.AllProperties;
            web.Context.Load(props);
            web.Context.ExecuteQueryRetry();

            props[_key] = _value_int;

            web.Update();
            web.Context.ExecuteQueryRetry();

            var intValue = web.GetPropertyBagValueInt(_key, -1);

            Assert.IsInstanceOfType(intValue, typeof(int?), "No int value returned");
            Assert.AreEqual(_value_int, intValue, "Incorrect value returned");

            // Check for non-existing key
            intValue = web.GetPropertyBagValueInt("_key_" + DateTime.Now.ToFileTime(), -12345);
            Assert.IsInstanceOfType(intValue, typeof(int?), "No int value returned");
            Assert.AreEqual(-12345, intValue, "Incorrect value returned");
        }

        [TestMethod()]
        public void GetPropertyBagValueStringTest()
        {
            var notExistingKey = "NOTEXISTINGKEY";
            var web = clientContext.Web;
            var props = web.AllProperties;
            web.Context.Load(props);
            web.Context.ExecuteQueryRetry();

            props[_key] = _value_string;

            web.Update();
            web.Context.ExecuteQueryRetry();

            var stringValue = web.GetPropertyBagValueString(_key, notExistingKey);

            Assert.IsInstanceOfType(stringValue, typeof(string), "No string value returned");
            Assert.AreEqual(_value_string, stringValue, "Incorrect value returned");

            // Check for non-existing key
            stringValue = web.GetPropertyBagValueString("_key_" + DateTime.Now.ToFileTime(), notExistingKey);
            Assert.IsInstanceOfType(stringValue, typeof(string), "No string value returned");
            Assert.AreEqual(notExistingKey, stringValue, "Incorrect value returned");
        }

        [TestMethod()]
        public void PropertyBagContainsKeyTest()
        {
            var web = clientContext.Web;
            var props = web.AllProperties;
            web.Context.Load(props);
            web.Context.ExecuteQueryRetry();

            props[_key] = _value_string;

            web.Update();
            web.Context.ExecuteQueryRetry();

            Assert.IsTrue(web.PropertyBagContainsKey(_key));
        }

        [TestMethod()]
        public void GetIndexedPropertyBagKeysTest()
        {
            var web = clientContext.Web;
            var props = web.AllProperties;
            var keys = web.GetIndexedPropertyBagKeys();

            Assert.IsInstanceOfType(keys, typeof(IEnumerable<string>), "No correct object returned");

            var keysList = keys.ToList();
            // Manually add an indexed property bag value
            if (!keysList.Contains(_key))
            {
                keysList.Add(_key);
                var encodedValues = GetEncodedValueForSearchIndexProperty(keysList);

                web.Context.Load(props);
                web.Context.ExecuteQueryRetry();

                props[INDEXED_PROPERTY_KEY] = encodedValues;

                web.Update();
                clientContext.ExecuteQueryRetry();
            }
            keys = web.GetIndexedPropertyBagKeys();
            Assert.IsTrue(keys.Contains(_key), "Key not present");

            // Local Cleanup
            props.RefreshLoad();
            clientContext.ExecuteQueryRetry();
            props[INDEXED_PROPERTY_KEY] = null;
            props.FieldValues.Remove(INDEXED_PROPERTY_KEY);
            web.Update();
            clientContext.ExecuteQueryRetry();
        }

        [TestMethod()]
        public void AddIndexedPropertyBagKeyTest()
        {
            var web = clientContext.Web;
            var props = web.AllProperties;
            clientContext.Load(props);
            clientContext.ExecuteQueryRetry();

            web.AddIndexedPropertyBagKey(_key);

            props.RefreshLoad();
            clientContext.ExecuteQueryRetry();

            Assert.IsTrue(props.FieldValues.ContainsKey(INDEXED_PROPERTY_KEY));

            // Local cleanup
            props[INDEXED_PROPERTY_KEY] = null;
            props.FieldValues.Remove(INDEXED_PROPERTY_KEY);
            web.Update();
            clientContext.ExecuteQueryRetry();
        }

        [TestMethod()]
        public void RemoveIndexedPropertyBagKeyTest()
        {
            var web = clientContext.Web;
            var props = web.AllProperties;

            // Manually add an indexed property bag value
            var encodedValues = GetEncodedValueForSearchIndexProperty(new List<string>() { _key });

            web.Context.Load(props);
            web.Context.ExecuteQueryRetry();

            props[INDEXED_PROPERTY_KEY] = encodedValues;

            web.Update();
            clientContext.ExecuteQueryRetry();

            // Remove the key
            Assert.IsTrue(web.RemoveIndexedPropertyBagKey(_key));
            props.RefreshLoad();
            clientContext.ExecuteQueryRetry();
            // All keys should be gone
            Assert.IsFalse(props.FieldValues.ContainsKey(_key), "Key still present");
        }
        #endregion

        #region Provisioning Tests


        public void GetProvisioningTemplateTest()
        {
            using (var clientContext = TestCommon.CreateClientContext())
            {
                var template = clientContext.Web.GetProvisioningTemplate();
                Assert.IsInstanceOfType(template, typeof (ProvisioningTemplate));
            }
        }
        #endregion

        #region Helper methods
        private static string GetEncodedValueForSearchIndexProperty(IEnumerable<string> keys)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string current in keys)
            {
                stringBuilder.Append(Convert.ToBase64String(Encoding.Unicode.GetBytes(current)));
                stringBuilder.Append('|');
            }
            return stringBuilder.ToString();
        }
        #endregion

    }
}
