<%@ Page Title="About Document Imaging Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DocumentImporter.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Open a Docx, PDF, Image, TXT file. View, Manipulate and Save the file in PDF and/or TIFF format..</h3>

    <p>
        <asp:BulletedList ID="BulletedList1" runat="server" DisplayMode="Text" BulletStyle="Square">
            <asp:ListItem>Let's user open any supported file. If user tries to open any UnSupportedFile format, a warning message is displayed.</asp:ListItem>
            <asp:ListItem>File can be opened thru - Browse it, Load from URI and easy go - Drag N Drop.</asp:ListItem>
            <asp:ListItem>On opening a file, it's file name is visible on top right of the File.</asp:ListItem>
            <asp:ListItem>View the file contents.</asp:ListItem>
            <asp:ListItem>Zoom In, Zoom Out the contents of the file.</asp:ListItem>
            <asp:ListItem>Pagination and Page Numbers also accessible.</asp:ListItem>
            <asp:ListItem>Rotate any page(s) of PDF & image file formats. - Rotated pages will be saved in that way. </asp:ListItem>
            <asp:ListItem>File can be Saved in PDF and/or Tiff format.</asp:ListItem>
            <asp:ListItem>File can be Print. </asp:ListItem>
            <asp:ListItem>TimeStamp can be added to the current Page</asp:ListItem>
            <asp:ListItem>Signature can be added, moved around and also removed if don't want.</asp:ListItem>
            <asp:ListItem>"Approved" and "Rejected" Stamp can be added. Can also be moved around, resized and/or delete it.</asp:ListItem>
            <asp:ListItem>Scan document(s).</asp:ListItem>
            <asp:ListItem>Save Scanned docs as a PDF or TIFF file </asp:ListItem>
            <asp:ListItem>Remove/Delete any of the Scanned page.</asp:ListItem>
            <asp:ListItem> Delete Page from an existing PDF file</asp:ListItem>
            <asp:ListItem> Delete Page from an existing TIFF file</asp:ListItem>
            <asp:ListItem>Clean up Image facilities like - Remove Black Borders, Remove Punch hole marks, 
                Auto Deskew (Straighten an image) & Remove dots from image(Despeckle) 
            </asp:ListItem>
        </asp:BulletedList>

        



    </p>
</asp:Content>
