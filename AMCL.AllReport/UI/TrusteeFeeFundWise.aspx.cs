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
using System.Text;

public partial class UI_TrusteeFeeFundWise : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillfundNameDropDownList();
        }
    }
    protected void ShowReportButton_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        string dateFrom = fromDateTextBox.Text.ToString();
        string dateTo = ToDateTextBox.Text.ToString();
        int fundCode= Convert.ToInt32(fundNameDropDownList.SelectedValue);
        string fundName = fundNameDropDownList.SelectedItem.Text.ToString();
       // Response.Redirect("ReportViewer/TrusteeFeeViewer.aspx?dateFrom=" + dateFrom + "&Todate=" + dateTo + "&FundCod=" + fundCode + "&FundName=" + fundName);
        sb.Append("window.open('ReportViewer/TrusteeFeeViewer.aspx?dateFrom=" + dateFrom + "&Todate=" + dateTo + "&FundCod=" + fundCode + "&FundName=" + fundName + "');");
        ClientScript.RegisterStartupScript(this.GetType(), "ReportViwer", sb.ToString(), true);
       
    }
    private void FillfundNameDropDownList()
    {
        DataTable dtFundName = commonGatewayObj.Select("SELECT *FROM INVEST.FUND WHERE  F_TYPE='OPEN END' ORDER BY F_CD ");
        DataTable dtFundDropDown = new DataTable();
        dtFundDropDown.Columns.Add("FundCode", typeof(int));
        dtFundDropDown.Columns.Add("FundName", typeof(string));

        DataRow drFundDropDown = dtFundDropDown.NewRow();
        drFundDropDown["FundName"] = " ";
        drFundDropDown["FundCode"] = "0";
        dtFundDropDown.Rows.Add(drFundDropDown);
   
        for(int loop=0;loop<dtFundName.Rows.Count;loop++)
        {
            drFundDropDown = dtFundDropDown.NewRow();
            drFundDropDown["FundName"] = dtFundName.Rows[loop]["F_NAME"].ToString();
            drFundDropDown["FundCode"] = Convert.ToInt32(dtFundName.Rows[loop]["F_CD"]);
            dtFundDropDown.Rows.Add(drFundDropDown);
        }

        fundNameDropDownList.DataSource = dtFundDropDown;
        fundNameDropDownList.DataTextField = "FundName";
        fundNameDropDownList.DataValueField = "FundCode";
        fundNameDropDownList.DataBind();
       
    }
}
