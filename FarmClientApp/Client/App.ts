module Birchman.RemoteProvisioning.FarmClientApp.Client {
    import WorkspaceViewModel = Birchman.RemoteProvisioning.FarmClientApp.Client.ViewModels.WorkspaceViewModel;
  


    export class App {

        public static initAutoprovision(obj: Object) {
            try {
                if (obj == null)
                    throw new Error("Null container");
                var vm = new WorkspaceViewModel();
                ko.applyBindings(vm, obj);
            }
            catch (e) {
                // Log exception
            }
        }

       
    }
}