using System;
using System.Collections;
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

public partial class UI_AMCLCommon : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        mnuMenu.Items.Clear();        
        skmMenu.MenuItem item;
        skmMenu.MenuItem Subitem;
       

        //Menu for Fee Calculation Report
        item = new skmMenu.MenuItem("Fee Calculation Report");
        Subitem = new skmMenu.MenuItem("Management Fee");                   
        Subitem.Url = "MgtFeeFundWise.aspx";
       
        item.SubItems.Add(Subitem);
             
        Subitem = new skmMenu.MenuItem("Trustee Fee");

        Subitem.Url = "TrusteeFeeFundwise.aspx";

        item.SubItems.Add(Subitem); 

        Subitem = new skmMenu.MenuItem("Custodian Fee");
        
        Subitem.Url = "CustoFeeFundWise.aspx";
            
        item.SubItems.Add(Subitem); 
       


        
        Subitem = new skmMenu.MenuItem("Summary Fee Calculation");
        Subitem.Url = "SummaryMgtCostoFee.aspx";
        item.SubItems.Add(Subitem);
        mnuMenu.Items.Add(item);

       
        //Logout
        item = new skmMenu.MenuItem("Logout");
        item.Url = "../Default.aspx";
        mnuMenu.Items.Add(item);

    }
}
