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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class ReportViwer_TrusteeFeeViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    protected void Page_Load(object sender, EventArgs e)
     {
        string FY = "";
        DateTime DateFrom = Convert.ToDateTime(Request.QueryString["dateFrom"]);        
        DateTime DateTo = Convert.ToDateTime(Request.QueryString["Todate"]);
        FY = DateFrom.ToString("dd - MMM - yyyy") + " to " + DateTo.ToString("dd - MMM - yyyy");
        int fundCode = Convert.ToInt32(Request.QueryString["FundCod"]);
        string fundName = Request.QueryString["FundName"].ToString();
       
     
        DataTable dtReprtSource = new DataTable();
        DataTable dtTotal = new DataTable();
        decimal TotalNav = 0;
        decimal Average = 0;
        decimal totalTrusteeFee = 0;

        dtReprtSource.TableName = "Report";
        string queryTotalNav = "select round(sum(navtotalmarketprice),2) as total, round(avg(navtotalmarketprice),2) as Average from Nav_Master where navdate between'" + DateFrom.ToString("dd-MMM-yyyy") + "' and '" + DateTo.ToString("dd-MMM-yyyy") + "' and navfundid=" + fundCode;
        string queryNav = "select navdate, navtotalmarketprice from Nav_Master where navdate between'" + DateFrom.ToString("dd - MMM - yyyy") + "' and '" + DateTo.ToString("dd-MMM-yyyy") + "' and navfundid=" + fundCode + " order by navdate ";

        dtReprtSource = commonGatewayObj.Select(queryNav);
        dtTotal = commonGatewayObj.Select(queryTotalNav);
        string totalTrusteeFeeWords = "";
        string totalTrusteeFeeString = "";
        TimeSpan dateDifference = Convert.ToDateTime(DateTo.ToString("dd-MMM-yyyy")).Subtract(Convert.ToDateTime(DateFrom.ToString("dd-MMM-yyyy")));
        int days = dateDifference.Days + 1;
        if (days > 365)
        {
            days = 365;
        }

        if (dtReprtSource.Rows.Count > 0)
        {
            TotalNav = Convert.ToDecimal(dtTotal.Rows[0]["total"]);
            Average = Convert.ToDecimal(dtTotal.Rows[0]["Average"]);
            totalTrusteeFee = (Average * Convert.ToDecimal(0.1))/100;
            totalTrusteeFee = (totalTrusteeFee * days) / 365;
            totalTrusteeFee = Decimal.Round(totalTrusteeFee, 2);
            totalTrusteeFeeString = " " + Average.ToString("N2") + "*0.10%";



            // dtReprtSource.WriteXmlSchema(@"D:\Project\Web\AMCL.AllReport\UI\ReportViewer\Report\crtTrusteeFee.xsd");

            NumberToEnglish numberToEnglishObj = new NumberToEnglish();

            totalTrusteeFeeWords = "(Taka " + numberToEnglishObj.changeNumericToWords(Decimal.Round(totalTrusteeFee, 2)) + ")";
            ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/crtTrusteeFee.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CrystalReportViewer1.ReportSource = rdoc;            
            rdoc.SetParameterValue("prmFundName", fundName);
            rdoc.SetParameterValue("prmFY", FY);
            rdoc.SetParameterValue("prmTotalNav", TotalNav);
            rdoc.SetParameterValue("prmAvgNav", Average);
            rdoc.SetParameterValue("prmTotalTrusteeFee", totalTrusteeFee.ToString("N2"));
            rdoc.SetParameterValue("prmTrusteeFeeString", totalTrusteeFeeString);
            rdoc.SetParameterValue("prmTotalTrusteeFeeWords", totalTrusteeFeeWords);  
        }
        else
        {
            Response.Write("No Data Found");
        }
       
    }
}
