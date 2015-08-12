module Birchman.RemoteProvisioning.FarmClientApp.Client.ViewModels {
    import Workspace = Birchman.RemoteProvisioning.FarmClientApp.Client.Models.Workspace;

    export class WorkspaceViewModel {
        public discussions: KnockoutObservableArray<Workspace>;
        
        constructor() {
           
            this.init();   
        }

        protected init() {
        }

        private refresh(): void {
            var data = this.discussions.slice(0);
            this.discussions([]);
            this.discussions(data);
        }
    }
} 