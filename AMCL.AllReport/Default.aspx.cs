using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;



public partial class _Default : System.Web.UI.Page 
{
    CommonGateway commonGatewayObj = new CommonGateway();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Excel.ApplicationClass app = new Excel.ApplicationClass();
        loginErrorLabel.Visible = false;
        loginIDTextBox.Focus();
    }
    protected void loginButton_Click(object sender, EventArgs e)
    {

        if (IsUesrCheck(loginIDTextBox.Text.Trim().ToString(),loginPasswardTextBox.Text.Trim().ToString()))
        {
            Response.Redirect("UI/Home.aspx");
        }
        else
        {
            loginErrorLabel.Visible = true;
            loginErrorLabel.Text = "Invalid LoginID or Passward";
            loginIDTextBox.Text = "";
            loginPasswardTextBox.Text = "";
        }
    }
    public bool IsUesrCheck(string loginID,string loginPassword)
    {


        if (string.Compare(loginIDTextBox.Text.ToString(), "amcl", true) == 0 && string.Compare(loginPasswardTextBox.Text.ToString(), "amcl", true) == 0)
        {
          
            return true;
        }
        else
        {
            return false;
        }
    }
}
