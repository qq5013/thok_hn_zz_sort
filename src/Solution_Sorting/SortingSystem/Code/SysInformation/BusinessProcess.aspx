﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BusinessProcess.aspx.cs" Inherits="Code_SysInfomation_BusinessProcessSetup_BusinessProcess" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" href="../../css/css.css" type="text/css" />
    <style type="text/css">
    A
    {
	    color:black;
	    text-decoration:none;
    }

    A:hover
    {
	    color:#FE6103;
	    text-decoration:none; /*underline;*/
    }
    A:Active
    {
	    text-decoration:none;
    }    
    td { 
      font-size: 12px;
      color: #000000;
      line-height: 150%;
      }
    .sec1 { 
      background-color: #EEEEEE;
      cursor: hand;
      color: #000000;
      border-left: 1px solid #FFFFFF;
      border-top: 1px solid #FFFFFF;
      border-right: 1px solid gray;
      border-bottom: 1px solid #FFFFFF
      }
    .sec2 { 
      background-color: #D4D0C8;
      cursor: hand;
      color: #000000;
      border-left: 1px solid #FFFFFF; 
      border-top: 1px solid #FFFFFF; 
      border-right: 1px solid gray; 
      font-weight: bold; 
      }
    .main_tab {
      background-color: #D4D0C8;
      color: #000000;
      border-left:1px solid #FFFFFF;
      border-right: 1px solid gray;
      border-bottom: 1px solid gray; 
      }
      
    </style>
    <script type="text/javascript" src="../../../JScript/InfHintDiv.js"></script>

       <script language="javascript" type="text/javascript">

        
        function LostFocus(obj)
        {
            obj.blur();
        }
        
        function HasChanged(obj)
        {
            var objValue=obj.value;
            if(objValue==1)
            {
               document.getElementById("txtSys_BufferCache").readOnly=false;
            }
            else
            {
               document.getElementById("txtSys_BufferCache").readOnly=true;
            }
        }
        function DoResult()
        {
            alert('[提示]',strMessage,'确认')
        }
    </script>
</head>
<body style=" margin:10 30 10 30;">
    <form id="form1" runat="server">
    <div>
        
        <table width="100%" cellpadding="2" cellspacing="0" style="left: 0px; position: relative; top: 0px">
            <tr >
                <td colspan="4" style="font-weight: <% =HeadFont[3]%>; font-size: <%=HeadFont[2] %>pt; color: <% =HeadFont[1]%>; font-family: <%=HeadFont[0] %>; ">
                    <asp:Label ID="Label1" runat="server" Text="格式设置"></asp:Label></td>
            </tr>
            <tr style="height: 21px;">
                <td style="font-weight: <% =TableFont[3]%>; font-size: <%=TableFont[2] %>pt; color: <% =TableFont[1]%>; font-family: <%=TableFont[0] %>; ">
                                <asp:Label ID="Label19" runat="server" Text="数值格式："></asp:Label></td>
                <td colspan="3" style="width: 510px">
                        <asp:DropDownList ID="ddlSys_FormatNumberMode" runat="server" Width="150px">
                        </asp:DropDownList></td>
            </tr>
            <tr style="height: 21px;">
                <td style="font-weight: <% =TableFont[3]%>; font-size: <%=TableFont[2] %>pt; color: <% =TableFont[1]%>; font-family: <%=TableFont[0] %>; ">
                                <asp:Label ID="Label20" runat="server" Text="日期时间格式："></asp:Label></td>
                <td colspan="3" style="width: 510px">
                        <asp:DropDownList ID="ddlSys_FormatDateTimeMode" runat="server" Width="150px">
                        </asp:DropDownList></td>
            </tr>
            <tr style="height: 21px;">
                <td style="font-weight: <% =TableFont[3]%>; font-size: <%=TableFont[2] %>pt; color: <% =TableFont[1]%>; font-family: <%=TableFont[0] %>; ">
                                <asp:Label ID="Label21" runat="server" Text="货币格式："></asp:Label></td>
                <td colspan="3" style="width: 510px">
                        <asp:DropDownList ID="ddlSys_FormatMoneyMode" runat="server">
                        </asp:DropDownList></td>
            </tr>
            <tr style="height: 21px;">
                <td style="font-weight: <% =TableFont[3]%>; font-size: <%=TableFont[2] %>pt; color: <% =TableFont[1]%>; font-family: <%=TableFont[0] %>; ">
                    <asp:Label ID="labLimitTime" runat="server" Text="操作时限："></asp:Label></td>
                <td colspan="3" style="width: 510px">
                    <asp:TextBox ID="txtSessionTimeOut" runat="server" Width="150px"></asp:TextBox>(以分钟为单位)</td>
            </tr>
            <tr style="height:30px;">
                <td colspan="4" rowspan="3" style="">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="ButtonCss" OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="退出" CssClass="ButtonCss" OnClientClick="return Exit();" OnClick="btnClear_Click" /></td>
            </tr>

        </table>
    </div>
    </form>
</body>
</html>
<script>
  function Exit()
  {
     window.parent.location="../../../MainPage.aspx";
     return false;
  }
</script>
