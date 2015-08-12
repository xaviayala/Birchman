﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Birchman.RemoteProvisioning.Domain.Connectors;
using Birchman.RemoteProvisioning.Domain.Model;
using Microsoft.SharePoint.Client;


namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers
{
    public class ProvisioningTemplateCreationInformation
    {
        private ProvisioningTemplate baseTemplate;
        private FileConnectorBase fileConnector;
        private bool persistComposedLookFiles = false;
        
        public ProvisioningTemplateCreationInformation(Web web)
        {
            this.baseTemplate = web.GetBaseTemplate();
        }

        /// <summary>
        /// Base template used to compare against when we're "getting" a template
        /// </summary>
        public ProvisioningTemplate BaseTemplate 
        { 
            get
            {
                return this.baseTemplate;
            }
            set
            {
                this.baseTemplate = value;
            }
        }

        /// <summary>
        /// Connector used to persist files when needed
        /// </summary>
        public FileConnectorBase FileConnector
        {
            get
            {
                return this.fileConnector;
            }
            set
            {
                this.fileConnector = value;
            }
        }

        /// <summary>
        /// Do composed look files (theme files, site logo, alternate css) need to be persisted to storage when 
        /// we're "getting" a template
        /// </summary>
        public bool PersistComposedLookFiles
        {
            get
            {
                return this.persistComposedLookFiles;
            }
            set
            {
                this.persistComposedLookFiles = value;
            }
        }


    }
}
