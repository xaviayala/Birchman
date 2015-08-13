module Birchman.RemoteProvisioning.FarmClientApp.Client.ViewModels {
    import Workspace = Birchman.RemoteProvisioning.FarmClientApp.Client.Models.Workspace;

    export class WorkspaceViewModel {
        public discussions: KnockoutObservableArray<Workspace>;

        protected Name: KnockoutObservable<string>;
        protected Description: KnockoutObservable<string>;
        protected Logo: KnockoutObservable<string>;
        protected Type: KnockoutObservable<string>;
        protected Template: KnockoutObservable<string>;
        protected UserOwner: KnockoutObservable<string>;
        protected UserMember: KnockoutObservable<string>;
        protected UserVisitor: KnockoutObservable<string>;
        protected Highlighted: KnockoutObservable<boolean>;

        constructor() {           
            this.init();  
        }

        protected init() {

            this.Name = ko.observable("");
            this.Description = ko.observable("");
            this.Logo = ko.observable("");
            this.Type = ko.observable("");
            this.Template = ko.observable("");
            this.UserOwner = ko.observable("");
            this.UserMember = ko.observable("");
            this.UserVisitor = ko.observable("");
            this.Highlighted = ko.observable(false);


        }

        private refresh(): void {
            var data = this.discussions.slice(0);
            this.discussions([]);
            this.discussions(data);
        }

        public save(): void {
            alert("tus datos son: " + this.Name() + "</br>" +
                this.Description() + "</br>" +
                this.Logo() + "</br>" +
                this.Type() + "</br>" +
                this.Template() + "</br>" +
                this.UserOwner() + "</br>" +
                this.UserVisitor() + "</br>" +
                this.Highlighted()
                );

        }

        public cancel(): void {
            this.init();
        }
    }
} 