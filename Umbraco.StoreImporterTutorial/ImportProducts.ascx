<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportProducts.ascx.cs" Inherits="Umbraco.StoreImporterTutorial.ImportProducts" %>
<asp:Panel ID="PanelUpload" runat="server">
    Pick a CSV file containing the products to import<br />
    <asp:FileUpload ID="ProductFile" runat="server" />
    <br />
    <br />
    <asp:Button ID="StartImport" runat="server" Text="Import" OnClick="startImport_Click" />
</asp:Panel>
<asp:Panel ID="PanelWoot" runat="server" Visible="false">
    <div class="success">
        <p>
            Ooooh la la - we got new products. 
        <strong>
            <asp:Literal runat="server" ID="ProductCount"></asp:Literal></strong> in fact.
        </p>
        <iframe width="560" height="315" src="http://www.youtube.com/embed/oP3ezq7G5E8" frameborder="0" allowfullscreen></iframe>
    </div>
</asp:Panel>
<asp:Panel ID="PanelFail" runat="server" Visible="false">
    <div class="error">
        <p>
        Epic fail - couldn't import products. Here's the details:<br/>
        <asp:Literal runat="server" ID="failDetail"></asp:Literal></p>
        <iframe width="420" height="315" src="http://www.youtube.com/embed/37OWL7AzvHo" frameborder="0" allowfullscreen></iframe>
    </div>
</asp:Panel>

