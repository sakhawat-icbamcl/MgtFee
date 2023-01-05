<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" Title="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
    <script language="javascript" type="text/javascript"> 
    function fnValidation()
    {
         if(document.getElementById("<%=loginIDTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=loginIDTextBox.ClientID%>").focus();
            alert("Please Enter LoginID");
            return false;
            
        }
        if(document.getElementById("<%=loginPasswardTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=loginPasswardTextBox.ClientID%>").focus();
            alert("Please Enter Login Password");
            return false;
            
        }
    }
  </script>
<link rel="Stylesheet" type="text/css" href="CSS/amcl.css"/>
    <style type="text/css">
        .style3
        {
            font-size: small;
            font-family: "Courier New";
            font-weight: 700;
        }
        .style4
        {
            font-family: "Courier New", Courier, monospace;
            font-weight: bold;
        }
        .style5
        {
            font-family: "Courier New";
            font-size: small;
            color:Red;
        }
        
        .auto-style1 {
            font-size: large;
            text-align: center;
        }
        
        .auto-style2 {
            font-family: "Courier New", Courier, monospace;
            font-weight: bold;
            text-align: right;
        }
        .auto-style3 {
            font-size: small;
            font-family: "Courier New";
            font-weight: 700;
            text-align: right;
        }
        .auto-style4 {
            text-align: center;
            color: #660066;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server" method="post" >
    <div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table width="800" align="center" cellpadding="0" cellspacing="0">   
         <tr>
            <td colspan="2" class="auto-style4">
             &nbsp;
            <span class="auto-style1">Management Fee, Custodian Fee and Trustee Fee Calculation</span></td>
        </tr>
         <tr>
            <td colspan="2">
             &nbsp;
            </td>
        </tr> 
        <tr>
            <td class="auto-style3">
             Login ID:
            </td>
            <td class="style3">
            <asp:TextBox ID="loginIDTextBox" runat="server" CssClass="textInputStyle" TabIndex="1" ></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td class="auto-style2">
                <span class="style3">Password</span>:
            </td>
            <td>
            <asp:TextBox ID="loginPasswardTextBox" runat="server" TextMode="Password" CssClass="textInputStyle"  TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
             &nbsp;
            </td>
        </tr>
         <tr>
             <td >
             &nbsp;
            </td>
            <td align="right" style="text-align: left" >
                <asp:Button ID="loginButton" runat="server" Text="Login" CssClass="buttoncommon" OnClientClick="return fnValidation();" 
                    onclick="loginButton_Click" Width="142px" />
            </td>
         </tr>
         <tr>
            <td align="center" colspan="2">
            <asp:Label runat="server" ID="loginErrorLabel" Visible="false" Text="" class="style5"></asp:Label>
            </td>
          </tr>
        </table>
    </div>
    </form>
</body>
</html>
