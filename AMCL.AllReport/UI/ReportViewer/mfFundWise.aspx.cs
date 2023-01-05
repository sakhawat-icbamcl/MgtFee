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

public partial class ReportViwer_mfFundWise : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    protected void Page_Load(object sender, EventArgs e)
     {
        string FY = "";
        DateTime DateFrom = Convert.ToDateTime(Request.QueryString["dateFrom"].ToString());
        DateTime DateTo = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
        TimeSpan dateDifference = Convert.ToDateTime(DateTo.ToString("dd-MMM-yyyy")).Subtract(Convert.ToDateTime(DateFrom.ToString("dd-MMM-yyyy")));
        int days = dateDifference.Days+1;
        if (days > 365)
        {
            days = 365;
        }
        
        FY = DateFrom.ToString("dd - MMM - yyyy") + " to " + DateTo.ToString("dd - MMM - yyyy");
        int fundCode = Convert.ToInt32(Request.QueryString["FundCod"]);
        string fundName = Request.QueryString["FundName"].ToString();
        string firstManagementFee = "";
        string secondManagementFee = "";
        string thirdManagementFee = "";
        string fourthManagementFee = "";
        string totalMFeeWords = "";
        decimal mFee1 = 0;
        decimal mFee2 = 0;
        decimal mFee3 = 0;
        decimal mFee4 = 0;
        decimal totalManagementFee = 0;
        DataTable dtReprtSource = new DataTable();
        DataTable dtTotal = new DataTable();
        decimal TotalNav = 0;
        decimal Average = 0;

        dtReprtSource.TableName = "Report";
        string queryTotalNav = "select round(sum(navtotalmarketprice),2) as total, round(avg(navtotalmarketprice),2) as Average from Nav_Master where navdate between'" + DateFrom.ToString("dd-MMM-yyyy") + "' and '" + DateTo.ToString("dd-MMM-yyyy") + "' and navfundid=" + fundCode;
        string queryNav = "select navdate, navtotalmarketprice from Nav_Master where navdate between'" + DateFrom.ToString("dd - MMM - yyyy") + "' and '" + DateTo.ToString("dd-MMM-yyyy") + "' and navfundid=" + fundCode + " order by navdate ";

        dtReprtSource = commonGatewayObj.Select(queryNav);
        dtTotal = commonGatewayObj.Select(queryTotalNav);

        if (dtReprtSource.Rows.Count > 0)
        {
            TotalNav = Convert.ToDecimal(dtTotal.Rows[0]["total"]);
            Average = Convert.ToDecimal(dtTotal.Rows[0]["Average"]);
            if (fundCode == 8) //Management Fee For Prime Finance Mutual Fund
            {
                if (Average > 500000000)
                {
                    firstManagementFee = "50000000*2.25%";
                    secondManagementFee = "200000000*1.75%";
                    thirdManagementFee = "250000000*1.25%";
                    fourthManagementFee = Average - 500000000 + "*.75%";
                    mFee1 = Convert.ToDecimal(50000000 * 0.0225);
                    mFee1 = (mFee1 * days) / 365;
                    mFee2 = Convert.ToDecimal(200000000 * 0.0175);
                    mFee2 = (mFee2 * days) / 365;
                    mFee3 = Convert.ToDecimal(250000000 * 0.0125);
                    mFee3 = (mFee3 * days) / 365;
                    mFee4 = Convert.ToDecimal((Average - 500000000) * Convert.ToDecimal(0.0075));
                    mFee4 = (mFee4 * days) / 365;
                    totalManagementFee = Convert.ToDecimal(mFee4 + mFee3 + mFee2 + mFee1);
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
                else if (Average <= 500000000 && Average > 250000000)
                {
                    firstManagementFee = "50000000*2.25%";
                    secondManagementFee = "200000000*1.75%";
                    thirdManagementFee = Average - 250000000 + "*1.25%";
                    mFee1 = Convert.ToDecimal(50000000 * 0.0225);
                    mFee1 = (mFee1 * days) / 365;
                    mFee2 = Convert.ToDecimal(200000000 * 0.0175);
                    mFee2 = (mFee2 * days) / 365;
                    mFee3 = Convert.ToDecimal((Average - 250000000) * Convert.ToDecimal(0.0125));
                    mFee3 = (mFee3 * days) / 365;
                    totalManagementFee = Convert.ToDecimal(mFee3 + mFee2 + mFee1);
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
                else if (Average <= 250000000 && Average > 50000000)//cut off point
                {
                    firstManagementFee = "50000000*2.25%";
                    secondManagementFee = Average - 50000000 + "*1.75%";
                    mFee1 = Convert.ToDecimal(50000000 * 0.0225);
                    mFee1 = (mFee1 * days) / 365;
                    mFee2 = Convert.ToDecimal((Average - 50000000) * Convert.ToDecimal(0.0175));
                    mFee2 = (mFee2 * days) / 365;
                    totalManagementFee = Convert.ToDecimal( mFee2 + mFee1);
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
                else if (Average <= 50000000 && Average >= 0)
                {
                    firstManagementFee = Average + "*2.25%";
                    mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.0225));
                    mFee1 = (mFee1 * days) / 365;
                    totalManagementFee = mFee1;
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
            }
            else if (fundCode == 14)//Management Fee For IFIL Mutual Fund
            {
                firstManagementFee = Average + "*0.5%";
                mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.005));
                mFee1 = (mFee1 * days) / 365;
                totalManagementFee = mFee1;
            }
            else if (fundCode == 20)//Management Fee For Islamic Unit Fund
            {
                firstManagementFee = Average + "*0.5%";
                mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.005));
                mFee1 = (mFee1 * days) / 365;
                totalManagementFee = mFee1;
            }
            else if (fundCode == 17)//Management Fee For BD Fund
            {
                firstManagementFee = Average + "*1.5%";
                mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.015));
                mFee1 = (mFee1 * days) / 365;
                totalManagementFee = mFee1;
            }
            else
            {
                if (Average > 500000000) //Management Fee For Rimining  Mutual Fund
                {
                    firstManagementFee = "50000000*2.5%";
                    secondManagementFee = "200000000*2.0%";
                    thirdManagementFee = "250000000*1.5%";
                    fourthManagementFee = Average - 500000000 + "*1.0%";
                    mFee1 = Convert.ToDecimal(50000000 * 0.025);
                    mFee1 = (mFee1 * days) / 365;
                    mFee2 = Convert.ToDecimal(200000000 * 0.020);
                    mFee2 = (mFee2 * days) / 365;
                    mFee3 = Convert.ToDecimal(250000000 * 0.015);
                    mFee3 = (mFee3 * days) / 365;
                    mFee4 = Convert.ToDecimal((Average - 500000000) * Convert.ToDecimal(0.01));
                    mFee4 = (mFee4 * days) / 365;
                    totalManagementFee = Convert.ToDecimal(mFee4 + mFee3 + mFee2 + mFee1);
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
                else if (Average <= 500000000 && Average > 250000000)
                {
                    firstManagementFee = "50000000*2.5%";
                    secondManagementFee = "200000000*2.0%";
                    thirdManagementFee = Average - 250000000 + "*1.5%";
                    mFee1 = Convert.ToDecimal(50000000 * 0.025);
                    mFee1 = (mFee1 * days) / 365;
                    mFee2 = Convert.ToDecimal(200000000 * 0.020);
                    mFee2 = (mFee2 * days) / 365;
                    mFee3 = Convert.ToDecimal((Average - 250000000) * Convert.ToDecimal(0.015));
                    mFee3 = (mFee3 * days) / 365;
                    totalManagementFee = Convert.ToDecimal(mFee3 + mFee2 + mFee1);
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
                else if (Average <= 250000000 && Average > 50000000)
                {
                    firstManagementFee = "50000000*2.5%";
                    secondManagementFee = Average - 50000000 + "*2.0%";
                    mFee1 = Convert.ToDecimal(50000000 * 0.025);
                    mFee1 = (mFee1 * days) / 365;
                    mFee2 = Convert.ToDecimal((Average - 50000000) * Convert.ToDecimal(0.020));
                    mFee2 = (mFee2 * days) / 365;
                    totalManagementFee = mFee2 + mFee1;
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
                else if (Average <= 50000000 && Average >= 0)
                {
                    firstManagementFee = Average + "*2.5%";
                    mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.025));
                    mFee1 = (mFee1 * days) / 365;
                    totalManagementFee = mFee1;
                    //totalManagementFee = (totalManagementFee * days) / 365;
                }
            }
           
            //dtReprtSource.WriteXmlSchema(@"D:\Project\Web\AMCL.AllReport\UI\ReportViewer\Report\crtmfFundWise.xsd");

            NumberToEnglish numberToEnglishObj = new NumberToEnglish();
            
			totalMFeeWords = "(Taka "+ numberToEnglishObj.changeNumericToWords(Decimal.Round(totalManagementFee,2))+ ")";
            ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/crtmfFundWise.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CrystalReportViewer1.ReportSource = rdoc;            
            rdoc.SetParameterValue("prmFundName", fundName);
            rdoc.SetParameterValue("prmFY", FY);
            rdoc.SetParameterValue("prmTotalNav", TotalNav);
            rdoc.SetParameterValue("prmAvgNav", Average);
            rdoc.SetParameterValue("prmFirstManagementFee", firstManagementFee);
            rdoc.SetParameterValue("prmSecondManagementFee", secondManagementFee);
            rdoc.SetParameterValue("prmThirdManagementFee", thirdManagementFee);
            rdoc.SetParameterValue("prmFourthManagementFee", fourthManagementFee);
            rdoc.SetParameterValue("prmMFee1", mFee1.Equals(0) ? "" : mFee1.ToString("N2"));
            rdoc.SetParameterValue("prmMFee2", mFee2.Equals(0) ? "" : mFee2.ToString("N2"));
            rdoc.SetParameterValue("prmMFee3", mFee3.Equals(0) ? "" : mFee3.ToString("N2"));
            rdoc.SetParameterValue("prmMFee4", mFee4.Equals(0) ? "" : mFee4.ToString("N2"));
            rdoc.SetParameterValue("prmTotalMFee", totalManagementFee.ToString("N2"));
            rdoc.SetParameterValue("prmTotalMFeeWords", totalMFeeWords.ToString());

        }
        else
        {
            Response.Write("No Data Found");
        }
       
    }
}
