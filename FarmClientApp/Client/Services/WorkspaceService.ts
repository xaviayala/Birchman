module Birchman.RemoteProvisioning.FarmClientApp.Client.Services {
    import WorkspaceViewModel = Birchman.RemoteProvisioning.FarmClientApp.Client.ViewModels.WorkspaceViewModel;

    export interface IWorksapceService<T> {
        baseUrl: string;
        siteURl: string;
        selectUrl: string;
        getListItems(weburl: string, listTitle: string, nbItems: number): any;
        getListItemsFiltered(weburl: string, listTitle: string, nbItems: number, viewQuery: string): any;
    }

    declare module String {
        export var format: any;
    }

    export class WorksapceService<T> implements IWorksapceService<T> {

        public baseUrl: string;
        public siteURl: string;
        public selectUrl: string;

        constructor() {
            this.baseUrl = "/";
        }


        //Function to build WebService URL
        public getListItemsWsUrl(weburl: string, listTitle: string, nbitems: number) {
            return String.format("{0}/_api/web/lists/GetByTitle('{1}')/Items?$top={2}", weburl, listTitle, nbitems);
        }

        public getListItems(weburl: string, listTitle: string, nbItems: number): any {         
            var result: any;
            jQuery.ajax({
                url: this.getListItemsWsUrl(weburl, listTitle, nbItems),
                method: "GET",
                headers: { "Accept": "application/json; odata=verbose" },
                contentType: "application/json;odata=verbose;charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data: any, textStatus: string, jqXhr: JQueryXHR) {
                    result = data;
                },
                error: (err) => {
                    console.log(JSON.stringify(err));
                }
            });
            return result;
        }

        public getListItemsFiltered(weburl: string, listTitle: string, nbItems: number, viewQuery: string): any {
            var result: any;
            jQuery.ajax({
                url: this.getListItemsWsUrl(weburl, listTitle, nbItems),
                method: "GET",
                data: "{ 'query' : {'__metadata': { 'type': 'SP.CamlQuery' }, \"ViewXml\": \"" + viewQuery + "\" }}",
                headers: { "Accept": "application/json; odata=verbose" },
                contentType: "application/json;odata=verbose;charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data: any, textStatus: string, jqXhr: JQueryXHR) {
                    result = data;
                },
                error: (err) => {
                    console.log(JSON.stringify(err));
                }
            });
            return result;
        }

        
        public UpdateUserMarkDate(markDate: Date, useraccount: string) {

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
                success: function (data: any, textStatus: string, jqXHR: JQueryXHR) {
                    ;
                },
                error: (err) => {
                    console.log(JSON.stringify(err));
                }
            });
        }
    }
}