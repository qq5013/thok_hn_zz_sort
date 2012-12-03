<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HandleSupplyDetail.aspx.cs" Inherits="Code_Query_HandleSupplyDetail" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AspNetPager" Namespace="AspNetPager" TagPrefix="NetPager" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../css/css.css?p=12" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css?l=0" rel="Stylesheet" type="text/css" />
    <script language="JavaScript" type="text/javascript" src="../../JScript/setday9.js"></script>
    <script language="JavaScript" type="text/javascript" src="../../JScript/Check.js"></script>
</head>
<body topmargin = 0 leftmargin="0">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel style="LEFT: 0px; POSITION: relative; TOP: 0px" id="pnlMain" runat="server" Width="100%" Height="100%"><asp:Panel id="pnlList" runat="server" Width="100%" Height="401px"><TABLE style="HEIGHT: 30px" borderColor=#111111 cellSpacing=0 cellPadding=0 width="100%" border=0><TBODY><TR><TD style="HEIGHT: 30px" borderColor=#cccc99>&nbsp; 日期: <asp:TextBox onpropertychange="GetBatchNo();" id="txtOrderDate" runat="server" Width="90px" CssClass="TextBox"></asp:TextBox>&nbsp;<INPUT id="btnDate" class="ButtonDate" onclick="setday(document.getElementById('txtOrderDate')) " type=button /> &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; 批次:<asp:DropDownList id="ddlBatchNo" runat="server">
                                            <asp:ListItem Selected="True">1</asp:ListItem>
                                        </asp:DropDownList>&nbsp; <asp:Button id="btnQuery" onclick="btnQuery_Click" runat="server" CssClass="ButtonQuery" Text="查询" OnClientClick="return CheckCondition();"></asp:Button><asp:Button id="btnExit" onclick="btnExit_Click" runat="server" CssClass="ButtonExit" Text="退出"></asp:Button>&nbsp; <asp:LinkButton id="lnkBtnGetBatchNo" onclick="lnkBtnGetBatchNo_Click" runat="server"></asp:LinkButton></TD></TR></TBODY></TABLE><asp:Panel id="pnlGrid" runat="server" Width="100%" Height="460px"><asp:GridView id="gvMain" runat="server" Width="100%" AutoGenerateColumns="False"><Columns>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:yyyy-MM-dd}" DataField="ORDERDATE" HeaderText="分拣日期">
<HeaderStyle Width="60px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="BATCHNO" HeaderText="批次">
<HeaderStyle Width="50px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="LINECODE" HeaderText="分拣线代码">
<HeaderStyle Width="80px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="CHANNELCODE" HeaderText="烟道编码"></asp:BoundField>
<asp:BoundField DataField="CIGARETTECODE" HeaderText="卷烟代码">
<HeaderStyle Width="80px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="CIGARETTENAME" HeaderText="卷烟名称"></asp:BoundField>
<asp:BoundField DataField="QUANTITY" HeaderText="数量">
<ItemStyle HorizontalAlign="Right"></ItemStyle>

<HeaderStyle Width="60px"></HeaderStyle>
</asp:BoundField>
</Columns>

<RowStyle BackColor="White" Height="28px"></RowStyle>

<HeaderStyle CssClass="gridheader"></HeaderStyle>

<AlternatingRowStyle BackColor="#E8F4FF"></AlternatingRowStyle>
</asp:GridView> </asp:Panel> <NetPager:AspNetPager id="pager" runat="server" Width="555px" Height="24px" ShowInputBox="Never" OnPageChanging="pager_PageChanging" AlwaysShow="True" ShowPageIndex="False"></NetPager:AspNetPager> </asp:Panel> </asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel>
        
    </form>
</body>
</html>
