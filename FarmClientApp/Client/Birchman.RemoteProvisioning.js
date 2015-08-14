var Birchman;
(function (Birchman) {
    var RemoteProvisioning;
    (function (RemoteProvisioning) {
        var FarmClientApp;
        (function (FarmClientApp) {
            var Client;
            (function (Client) {
                var Models;
                (function (Models) {
                    var Workspace = (function () {
                        function Workspace() {
                        }
                        return Workspace;
                    })();
                    Models.Workspace = Workspace;
                })(Models = Client.Models || (Client.Models = {}));
            })(Client = FarmClientApp.Client || (FarmClientApp.Client = {}));
        })(FarmClientApp = RemoteProvisioning.FarmClientApp || (RemoteProvisioning.FarmClientApp = {}));
    })(RemoteProvisioning = Birchman.RemoteProvisioning || (Birchman.RemoteProvisioning = {}));
})(Birchman || (Birchman = {}));
var Birchman;
(function (Birchman) {
    var RemoteProvisioning;
    (function (RemoteProvisioning) {
        var FarmClientApp;
        (function (FarmClientApp) {
            var Client;
            (function (Client) {
                var Services;
                (function (Services) {
                    var WorkspaceService = (function () {
                        function WorkspaceService() {
                            this.baseUrl = "/";
                        }
                        //Function to build WebService URL
                        WorkspaceService.prototype.getListItemsWsUrl = function (weburl, listTitle, nbitems) {
                            return String.format("{0}/_api/web/lists/GetByTitle('{1}')/Items?$top={2}", weburl, listTitle, nbitems);
                        };
                        WorkspaceService.prototype.getListItems = function (weburl, listTitle, nbItems) {
                            var result;
                            jQuery.ajax({
                                url: this.getListItemsWsUrl(weburl, listTitle, nbItems),
                                method: "GET",
                                headers: { "Accept": "application/json; odata=verbose" },
                                contentType: "application/json;odata=verbose;charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (data, textStatus, jqXhr) {
                                    result = data;
                                },
                                error: function (err) {
                                    console.log(JSON.stringify(err));
                                }
                            });
                            return result;
                        };
                        WorkspaceService.prototype.getListItemsFiltered = function (weburl, listTitle, nbItems, viewQuery) {
                            var result;
                            jQuery.ajax({
                                url: this.getListItemsWsUrl(weburl, listTitle, nbItems),
                                method: "GET",
                                data: "{ 'query' : {'__metadata': { 'type': 'SP.CamlQuery' }, \"ViewXml\": \"" + viewQuery + "\" }}",
                                headers: { "Accept": "application/json; odata=verbose" },
                                contentType: "application/json;odata=verbose;charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (data, textStatus, jqXhr) {
                                    result = data;
                                },
                                error: function (err) {
                                    console.log(JSON.stringify(err));
                                }
                            });
                            return result;
                        };
                        WorkspaceService.prototype.UpdateUserMarkDate = function (markDate, useraccount) {
                            var UserMarkDate = {
                                UserAccount: useraccount,
                                MarkDate: markDate
                            };
                            jQuery.ajax({
                                url: this.baseUrl + "API/UserProfilesSP/UpdateUserMarkDate",
                                method: "POST",
                                dataType: "json",
                                headers: {
                                    "Accept": "application/json; odata=verbose",
                                    "Content-Type": "application/json;odata=verbose",
                                    "X-HTTP-Method": "MERGE",
                                    "IF-MATCH": "*",
                                    "X-RequestDigest": jQuery('#__REQUESTDIGEST').val()
                                },
                                data: JSON.stringify(UserMarkDate),
                                async: false,
                                success: function (data, textStatus, jqXHR) {
                                    ;
                                },
                                error: function (err) {
                                    console.log(JSON.stringify(err));
                                }
                            });
                        };
                        return WorkspaceService;
                    })();
                    Services.WorkspaceService = WorkspaceService;
                })(Services = Client.Services || (Client.Services = {}));
            })(Client = FarmClientApp.Client || (FarmClientApp.Client = {}));
        })(FarmClientApp = RemoteProvisioning.FarmClientApp || (RemoteProvisioning.FarmClientApp = {}));
    })(RemoteProvisioning = Birchman.RemoteProvisioning || (Birchman.RemoteProvisioning = {}));
})(Birchman || (Birchman = {}));
var Birchman;
(function (Birchman) {
    var RemoteProvisioning;
    (function (RemoteProvisioning) {
        var FarmClientApp;
        (function (FarmClientApp) {
            var Client;
            (function (Client) {
                var ViewModels;
                (function (ViewModels) {
                    var WorkspaceService = Birchman.RemoteProvisioning.FarmClientApp.Client.Services.WorkspaceService;
                    var WorkspaceViewModel = (function () {
                        function WorkspaceViewModel() {
                            this._ws = WorkspaceService;
                            this.init();
                        }
                        WorkspaceViewModel.prototype.init = function () {
                            this.Title = ko.observable("Esto es el título");
                            this.Name = ko.observable("");
                            this.Description = ko.observable("");
                            this.Logo = ko.observable("");
                            this.Type = ko.observable("");
                            this.Template = ko.observable("");
                            this.UserOwner = ko.observable("");
                            this.UserMember = ko.observable("");
                            this.UserVisitors = ko.observable("");
                            this.Highlighted = ko.observable(false);
                            // this._ws = new WorkspaceService();
                        };
                        WorkspaceViewModel.prototype.save = function () {
                            alert("tus datos son: \n" + this.Name() + "\n" + this.Description() + "\n" + this.Logo() + "\n" + this.Type() + "\n" + this.Template() + "\n" + this.UserOwner() + "\n" + this.UserVisitors() + "\n" + this.Highlighted());
                            //this._ws.save()
                        };
                        WorkspaceViewModel.prototype.cancel = function () {
                            location.reload();
                        };
                        return WorkspaceViewModel;
                    })();
                    ViewModels.WorkspaceViewModel = WorkspaceViewModel;
                })(ViewModels = Client.ViewModels || (Client.ViewModels = {}));
            })(Client = FarmClientApp.Client || (FarmClientApp.Client = {}));
        })(FarmClientApp = RemoteProvisioning.FarmClientApp || (RemoteProvisioning.FarmClientApp = {}));
    })(RemoteProvisioning = Birchman.RemoteProvisioning || (Birchman.RemoteProvisioning = {}));
})(Birchman || (Birchman = {}));
var Birchman;
(function (Birchman) {
    var RemoteProvisioning;
    (function (RemoteProvisioning) {
        var FarmClientApp;
        (function (FarmClientApp) {
            var Client;
            (function (Client) {
                var WorkspaceViewModel = Birchman.RemoteProvisioning.FarmClientApp.Client.ViewModels.WorkspaceViewModel;
                var App = (function () {
                    function App() {
                    }
                    App.initAutoprovision = function (obj) {
                        try {
                            if (obj == null)
                                throw new Error("Null container");
                            var vm = new WorkspaceViewModel();
                            ko.applyBindings(vm, obj);
                        }
                        catch (e) {
                        }
                    };
                    return App;
                })();
                Client.App = App;
            })(Client = FarmClientApp.Client || (FarmClientApp.Client = {}));
        })(FarmClientApp = RemoteProvisioning.FarmClientApp || (RemoteProvisioning.FarmClientApp = {}));
    })(RemoteProvisioning = Birchman.RemoteProvisioning || (Birchman.RemoteProvisioning = {}));
})(Birchman || (Birchman = {}));
/// <reference path="client/models/workspace.ts" />
/// <reference path="client/services/WorkspaceService.ts" />
/// <reference path="client/viewmodels/workspaceviewmodel.ts" />
/// <reference path="Client/App.ts" />  
//# sourceMappingURL=Birchman.RemoteProvisioning.js.map