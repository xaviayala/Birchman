<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoprovisioningForm.aspx.cs" Inherits="Birchman.RemoteProvisioning.FarmClientApp.Layouts.FarmClientApp.RemoteProvisoningForm" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <script type="text/javascript">
         jQuery(function () {
             Birchman.RemoteProvisioning.FarmClientApp.Client.App.initAutoprovision(document.getElementById("AutoprovisionModal-container"));
         });

         $(document).ready(function () {
             $('.modal-trigger').leanModal({
                 modal: false,
                 fitToView: false
             });
         });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
        <!-- Modal Trigger  http://materializecss.com/modals.html -->
    <a class="modal-trigger waves-effect waves-light btn" href="#AutoprovisionModal-container">Modal</a>
    <h2 class="title" data-bind="text: Title">Create your workspace</h2>

    <!-- Modal Structure -->
    <div id="AutoprovisionModal-container" class="modal autoprovision">
       
        <div class="wizard_autoprovision_module">

            <button type="button" class="close">
                <span class="closeButton"></span>
            </button>

            <h2 class="title" data-bind="text: Title">Create your workspace</h2>

            <form role="form" class="form_module">

                <div class="wrap-input">
                    <label for="name">Name</label>
                    <input type="text" id="inName" name="name" required data-bind="value: Name" />
                </div>

                <div class="wrap-input textarea">
                    <label for="textareaDescription">Description</label>
                    <textarea maxlength="140" cols="40" rows="8" name="textareaDescription" required data-bind="value: Description">Write something here</textarea>
                </div>

                <div class="wrap-input">
                    <label for="type">Upload logo</label>
                    <input id="inputFile" type="file" data-preview-file-type="text" data-bind="value: Logo">
                </div>

                <div class="wrap-input">
                    <label for="type">Type</label>
                    <select required data-bind="value: Type">
                        <option value="1">Option 1</option>
                        <option value="2">Option 2</option>
                        <option value="3">Option 3</option>
                    </select>
                </div>

                <div class="wrap-input">
                    <label for="template">Template</label>
                    <select required data-bind="value: Template">
                        <option value="1">Option 1</option>
                        <option value="2">Option 2</option>
                        <option value="3">Option 3</option>
                    </select>
                </div>

                <div class="wrap-input textarea">
                    <label for="textareaOwner">User Owner</label>
                    <textarea maxlength="140" cols="40" rows="8" name="textareaOwner" required data-bind="value: UserOwner">Write a name here</textarea>
                </div>

                <div class="wrap-input textarea">
                    <label for="textareaMamber">User Member</label>
                    <textarea maxlength="140" cols="40" rows="8" name="textareaMamber" required data-bind="value: UserMember">Write a name here</textarea>
                </div>

                <div class="wrap-input textarea">
                    <label for="textareaVisitor">User Visitors</label>
                    <textarea maxlength="140" cols="40" rows="8" name="textareaVisitor" required data-bind="value: UserVisitors">Write a name here</textarea>
                </div>

                <div class="wrap-checkbox">
                    <input id="checkboxHighlight" type="checkbox" name="check" data-bind="value: Highlighted">
                    <label for="checkboxHighlight">Highlighted</label>
                </div>

                <div class="wrap-buttons">
                    <input type="button" id="btnSave" accesskey="O" value="Save" data-bind="click: $root.save">
                    <input type="button" id="btnCancel" accesskey="1" value="Cancel" data-bind="click: $root.cancel">


            </form>
        </div>

    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
