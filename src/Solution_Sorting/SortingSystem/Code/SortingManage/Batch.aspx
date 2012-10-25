<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Batch.aspx.cs" Inherits="Code_SortingManage_Batch" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AspNetPager" Namespace="AspNetPager" TagPrefix="NetPager" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../css/css.css?p=12" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css?l=0" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JScript/Check.js"></script>
</head>
<body style="margin-top:0px; margin-left:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel style="LEFT: 0px; POSITION: relative; TOP: 0px" id="pnlMain" runat="server" Width="100%" Height="100%"><asp:Panel id="pnlList" runat="server" Width="100%" Height="401px"><TABLE style="HEIGHT: 30px" borderColor=#111111 cellSpacing=0 cellPadding=0 width="100%" border=0><TOBDY /><TBODY><TR><TD style="BORDER-LEFT-COLOR: #cccc99; BORDER-BOTTOM-COLOR: #cccc99; BORDER-TOP-COLOR: #cccc99; BORDER-RIGHT-COLOR: #cccc99"><asp:DropDownList id="ddlField" runat="server">
                                        <asp:ListItem Value="ORDERDATE">订单日期</asp:ListItem>
                                    </asp:DropDownList> <asp:TextBox id="txtValue" runat="server" Height="20px" CssClass="TextBox"></asp:TextBox> <asp:Button id="btnQuery" onclick="btnQuery_Click" runat="server" CssClass="ButtonQuery" Text="查询"></asp:Button>&nbsp; <asp:Button id="btnExit" onclick="btnExit_Click" runat="server" CssClass="ButtonExit" Text="退出"></asp:Button> </TD></TR></TBODY></TABLE><asp:Panel id="pnlGrid" runat="server" Width="100%" Height="460px"><asp:GridView id="gvMain" runat="server" Width="100%" OnRowDeleting="gvMain_RowDeleting" OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False"><Columns>
<asp:CommandField ShowCancelButton="False" ShowDeleteButton="True" DeleteText="清除优化" HeaderText="操作">
<HeaderStyle Width="60px" HorizontalAlign="Center"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:CommandField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:yyyy-MM-dd}" DataField="ORDERDATE" HeaderText="订单日期">
<HeaderStyle Width="80px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="BATCHNO" HeaderText="批次">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle Width="60px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="ISUPTONOONEPRO" HeaderText="是否上传">
<HeaderStyle Width="80px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="ISVALID" HeaderText="是否优化">
<HeaderStyle Width="80px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="EXECUTEUSER" HeaderText="操作员"></asp:BoundField>
<asp:BoundField DataField="EXECUTEIP" HeaderText="操作IP">
<HeaderStyle Width="100px"></HeaderStyle>
</asp:BoundField>
</Columns>

<RowStyle BackColor="White" Height="28px"></RowStyle>

<HeaderStyle CssClass="gridheader"></HeaderStyle>

<AlternatingRowStyle BackColor="#E8F4FF"></AlternatingRowStyle>
</asp:GridView> </asp:Panel> <NetPager:AspNetPager id="pager" runat="server" Width="555px" Height="24px" OnPageChanging="pager_PageChanging" AlwaysShow="True" ShowPageIndex="False" ShowInputBox="Never"></NetPager:AspNetPager> </asp:Panel> </asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel>        
    </form>
</body>
</html>
