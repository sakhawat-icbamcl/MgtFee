<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="SummaryMgtCostoFee.aspx.cs" Inherits="UI_CustoFeeFundWise" Title="CustodianFee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            font-size: x-large;
            color: #593D84;
            text-decoration: underline;
        }
        .style4
        {
            height: 20px;
        }
    </style>
         <script language="javascript" type="text/javascript"> 
     function fnInputCheck()
     {
          
        if(document.getElementById("<%=fromDateTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=fromDateTextBox.ClientID%>").focus();
            alert("Please Enter  Date");
            return false;
            
        }
        
         if(document.getElementById("<%=ToDateTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=ToDateTextBox.ClientID%>").focus();
            alert("Please Enter  Date");
            return false;
            
        }
        if(document.getElementById("<%=fromDateTextBox.ClientID%>").value!="")
        {
            var checkDate=/^([012]?\d|3[01])-([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][Uu][Ll]|[aA][Uu][gG]|[Ss][eE][pP]|[Oo][Cc][Tt]|[Nn][Oo][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/;
            if(!checkDate.test(document.getElementById("<%=fromDateTextBox.ClientID%>").value))
            {
            document.getElementById("<%=fromDateTextBox.ClientID%>").focus();
            alert("Plese Select Date From The Calender");
             return false;
            }

        }
         if(document.getElementById("<%=ToDateTextBox.ClientID%>").value!="")
        {
            var checkDate=/^([012]?\d|3[01])-([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][Uu][Ll]|[aA][Uu][gG]|[Ss][eE][pP]|[Oo][Cc][Tt]|[Nn][Oo][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/;
            if(!checkDate.test(document.getElementById("<%=ToDateTextBox.ClientID%>").value))
            {
            document.getElementById("<%=ToDateTextBox.ClientID%>").focus();
            alert("Plese Select Date From The Calender");
             return false;
            }

        }
     }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
<table width="400" align="center" cellpadding="0" cellspacing="0"> 
<tr>
<td align="center" class="FormTitle">Summary Fee Report</td>
</tr>
</table>
<br />
<br />
<table width="600" align="center" cellpadding="0" cellspacing="0">    
        <tr>
            <td align="right" class="style4">
                <b>From Date:</b>&nbsp;
            </td>
            <td align="left" class="style4">
            <asp:TextBox ID="fromDateTextBox" runat="server" CssClass="textInputStyle"></asp:TextBox>
                <asp:ImageButton ID="fromDateImageButton" runat="server" AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png" />
                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="fromDateTextBox" 
                 PopupButtonID="fromDateImageButton" Format="dd-MMM-yyyy"/>
                
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <b>To Date:</b>&nbsp;
            </td>
            <td align="left">
            <asp:TextBox ID="ToDateTextBox" runat="server" CssClass="textInputStyle"></asp:TextBox>
            <asp:ImageButton ID="ToDateImageButton" runat="server" AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png" />
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="ToDateTextBox" 
                 PopupButtonID="ToDateImageButton" Format="dd-MMM-yyyy" />
            </td>
        </tr>
         <tr>
             <td align="right">
                 <b>Fee Type</b>:&nbsp;
            </td>
            <td  align="left">
                <asp:RadioButton ID="MfeeRadioButton" runat="server" Checked="True" 
                    GroupName="fee" style="font-weight: 700" Text="Management Fee" />
                <asp:RadioButton ID="TrusteeRadioButton" runat="server" GroupName="fee" 
                    style="font-weight: 700" Text="Trustee Fee" />
             </td>
         </tr>
          <tr>
             <td align="right">
                 <b>Custodian Fee</b>:</td>
            <td  align="left">
                <asp:RadioButton ID="SecuritiesRadioButton" runat="server" 
                    GroupName="fee" style="font-weight: 700" 
                    Text="Using Monthly Securities Value" />
                <asp:RadioButton ID="NAVRadioButton" runat="server" GroupName="fee" 
                    style="font-weight: 700" Text="Using Monthly NAV" />
             </td>
         </tr>
          <tr>
            <td align="center" colspan="2">
                &nbsp;
            </td>
         </tr>
         <tr>
            <td align="center" colspan="2">
                <asp:Button ID="showReportButton" runat="server" Text="Show Report" 
                    onclick="ShowReportButton_Click"  CssClass="buttoncommon" OnClientClick="return fnInputCheck();"/>
            </td>
         </tr>
         <tr>
            <td align="center" colspan="2">
                <asp:Label runat="server" ID="loginErrorLabel" Visible="false" Text=""></asp:Label>
            </td>
          </tr>
        </table>
        <br />
         <br />
          <br />
           <br />
            <br />
             <br />
             <br />
             <br />
             <br />
</asp:Content>

