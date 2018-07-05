<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebClient.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="Form" runat="server">
        <div>
            <asp:Label ID="FormatLabel" runat="server" Text="Format"></asp:Label><asp:RadioButton ID="Xml" AutoPostBack="true" Checked="true" Text="XML" runat="server" /><asp:RadioButton AutoPostBack="true" ID="Json" Text="JSON" runat="server" />
        </div>
        <div>
            <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
            <ajaxToolkit:TabContainer AutoPostBack="true" ID="TabContainer" runat="server" ActiveTabIndex="0">
                <ajaxToolkit:TabPanel runat="server" HeaderText="Get all" ID="GetAll">
                    <ContentTemplate>
                        <asp:Label ID="GetAllResult" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" HeaderText="Get by Id" ID="GetById">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="GetByIdButton" runat="server" Text="Get" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="GetByIdIdLabel" runat="server" Text="ID"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="GetByIdId" type="text" />
                        </div>
                        <br />
                        <br />
                        <div>
                            <asp:Label ID="GetByIdResult" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" HeaderText="Get by title" ID="GetByTitle">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="GetByTitleButton" runat="server" Text="Get" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="GetByTitleTitleLabel" runat="server" Text="Title"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="GetByTitleTitle" type="text" />
                        </div>
                        <br />
                        <br />
                        <div>
                            <asp:Label ID="GetByTitleResult" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" HeaderText="Add to inventory" ID="AddToInventory">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="AddToInventoryButton" runat="server" Text="Add" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="AddToInventoryIdLabel" runat="server" Text="Id"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="AddToInventoryId" type="text" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="AddToInventoryTitleLabel" runat="server" Text="Title"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="AddToInventoryTitle" type="text" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="AddToInventoryPriceLabel" runat="server" Text="Price"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="AddToInventoryPrice" type="text" />
                        </div>
                        <br />
                        <br />
                        <div>
                            <asp:Label ID="AddToInventoryResult" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" HeaderText="Sell" ID="Sell">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="SellButton" runat="server" Text="Sell" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="SellIdLabel" runat="server" Text="ID"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="SellId" type="text" />
                        </div>
                        <br />
                        <br />
                        <div>
                            <asp:Label ID="SellResult" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" HeaderText="Change price" ID="ChangePrice">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="ChangePriceButton" runat="server" Text="Change" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="ChangePriceIdLabel" runat="server" Text="ID"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="ChangePriceId" type="text" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="ChangePricePriceLabel" runat="server" Text="Price"></asp:Label>
                        </div>
                        <div>
                            <input runat="server" id="ChangePricePrice" type="text" />
                        </div>
                        <br />
                        <br />
                        <div>
                            <asp:Label ID="ChangePriceResult" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </form>
</body>
</html>
