module Birchman.RemoteProvisioning.FarmClientApp.Client.ViewModels {
    import Workspace = Birchman.RemoteProvisioning.FarmClientApp.Client.Models.Workspace;

    export class WorkspaceViewModel {
        public Name: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;
        public Logo: KnockoutObservable<string>;
        public Type: KnockoutObservable<string>;
        public Template: KnockoutObservable<string>;
        public UserOwner: KnockoutObservable<string>;
        public UserMember: KnockoutObservable<string>;
        public UserVisitors: KnockoutObservable<string>;
        public Highlighted: KnockoutObservable<boolean>;
        public Title: KnockoutObservable<string>;

        constructor() {
            this.init();
        }

        protected init() {
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

        }


        public save(): void {
            alert("tus datos son: \n" + this.Name() + "\n" +
                this.Description() + "\n" +
                this.Logo() + "\n" +
                this.Type() + "\n" +
                this.Template() + "\n" +
                this.UserOwner() + "\n" +
                this.UserVisitors() + "\n" +
                this.Highlighted()
                );

        }

        public cancel(): void {
            location.reload();
        }
    }
} 