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

public partial class UI_CustoFeeFundWise : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillfundNameDropDownList();
        }
    }
    protected void ShowReportButton_Click(object sender, EventArgs e)
    {
        DataTable dtFeeData = new DataTable();
        dtFeeData.Columns.Add("SI", typeof(int));
        dtFeeData.Columns.Add("FUND_CD", typeof(int));
        dtFeeData.Columns.Add("FUND_NAME", typeof(string));
        dtFeeData.Columns.Add("FEE", typeof(decimal));
        string feeType = "";

        DateTime DateFrom = Convert.ToDateTime(fromDateTextBox.Text.ToString());
        DateTime DateTo = Convert.ToDateTime(ToDateTextBox.Text.ToString());
        TimeSpan dateDifference = Convert.ToDateTime(DateTo.ToString("dd-MMM-yyyy")).Subtract(Convert.ToDateTime(DateFrom.ToString("dd-MMM-yyyy")));
        int days = dateDifference.Days + 1;
        if (days > 365)
        {
            days = 365;
        }

        StringBuilder sbQuery = new StringBuilder();
        if (MfeeRadioButton.Checked)
        {
            sbQuery.Append("SELECT INVEST.FUND.F_NAME, ROUND(AVG(NAV_MASTER.NAVTOTALMARKETPRICE), 2) AS AVERAGE , NAV_MASTER.NAVFUNDID FROM  NAV_MASTER INNER JOIN");
            sbQuery.Append(" INVEST.FUND ON NAV_MASTER.NAVFUNDID = INVEST.FUND.F_CD WHERE  INVEST.FUND.IS_F_CLOSE IS NULL AND INVEST.FUND.F_CD NOT IN (18,21) AND (NAV_MASTER.NAVDATE BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(ToDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy") + "')");
            sbQuery.Append(" GROUP BY INVEST.FUND.F_NAME, NAV_MASTER.NAVFUNDID ORDER BY NAV_MASTER.NAVFUNDID");
            feeType = "Mangement Fee";
        }
        else if (NAVRadioButton.Checked)
        {
            sbQuery.Append("SELECT NAV_MASTER.NAVFUNDID, ROUND(AVG(NAV_MASTER.NAVLASTVAULTVALUE), 2) AS AVERAGE, INVEST.FUND.F_NAME FROM  NAV_MASTER INNER JOIN");
            sbQuery.Append(" INVEST.FUND ON NAV_MASTER.NAVFUNDID = INVEST.FUND.F_CD WHERE INVEST.FUND.IS_F_CLOSE IS NULL AND INVEST.FUND.F_CD NOT IN (18,21) AND (NAV_MASTER.NAVDATE IN (SELECT MAX(NAVDATE) AS MAXNAVDATE FROM NAV_MASTER NAV_MASTER_1");
            sbQuery.Append(" WHERE (NAVDATE BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(ToDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy") + "')  GROUP BY TO_CHAR(NAVDATE, 'MM')))");
            sbQuery.Append(" GROUP BY NAV_MASTER.NAVFUNDID, INVEST.FUND.F_NAME ORDER BY NAV_MASTER.NAVFUNDID");            
            feeType = "Custodian Fee";
        }
        else if (SecuritiesRadioButton.Checked)
        {
            sbQuery.Append("  SELECT PP_BKK.F_CD AS NAVFUNDID, FF.F_NAME, ROUND(AVG(PP_BKK.TOT_NOS * PP_BKK.ADC_RT), 2) AS AVERAGE ");
            sbQuery.Append("  FROM   INVEST.PFOLIO_BK PP_BKK INNER JOIN   INVEST.FUND FF ON PP_BKK.F_CD = FF.F_CD WHERE  (FF.IS_F_CLOSE IS NULL) AND ");
            sbQuery.Append("  (PP_BKK.BAL_DT_CTRL IN (SELECT MAX( PFOLIO_BK_1.BAL_DT_CTRL) AS MAXNAVDATE  FROM  INVEST.PFOLIO_BK PFOLIO_BK_1 ");
            sbQuery.Append("  WHERE (PFOLIO_BK_1.BAL_DT_CTRL BETWEEN '" + Convert.ToDateTime(DateFrom).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(DateTo).ToString("dd-MMM-yyyy") + "') AND (PFOLIO_BK_1.F_CD > 1) AND PFOLIO_BK_1.F_CD NOT IN (18,21)  ");
            sbQuery.Append("  GROUP BY TO_CHAR(PFOLIO_BK_1.BAL_DT_CTRL, 'MM')))   GROUP BY PP_BKK.F_CD,FF.F_NAME ");
           
            feeType = "Custodian Fee";
        }   
        else if (TrusteeRadioButton.Checked)
        {
            sbQuery.Append("SELECT INVEST.FUND.F_NAME, ROUND(AVG(NAV_MASTER.NAVTOTALMARKETPRICE), 2) AS AVERAGE , NAV_MASTER.NAVFUNDID FROM  NAV_MASTER INNER JOIN");
            sbQuery.Append(" INVEST.FUND ON NAV_MASTER.NAVFUNDID = INVEST.FUND.F_CD WHERE   (NAV_MASTER.NAVDATE BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(ToDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy") + "') AND INVEST.FUND.F_CD NOT IN (18,21) ");
            sbQuery.Append(" AND NAV_MASTER.NAVFUNDID IN (SELECT F_CD FROM INVEST.FUND WHERE F_TYPE='OPEN END' AND IS_F_CLOSE IS NULL)");
            sbQuery.Append(" GROUP BY INVEST.FUND.F_NAME, NAV_MASTER.NAVFUNDID ORDER BY NAV_MASTER.NAVFUNDID");
            feeType = "Trustee Fee";
        }

        DataTable dtNavInfo = commonGatewayObj.Select(sbQuery.ToString());
       DataRow drFeeData=dtFeeData.NewRow();
       if (dtNavInfo.Rows.Count > 0)
       {
           int SI = 0;

           for (int loop = 0; loop < dtNavInfo.Rows.Count; loop++)
           {
               drFeeData = dtFeeData.NewRow();
               SI = loop + 1;
               drFeeData["SI"] = SI;
               drFeeData["FUND_NAME"] = dtNavInfo.Rows[loop]["F_NAME"].ToString();
               drFeeData["FUND_CD"] = Convert.ToInt32(dtNavInfo.Rows[loop]["NAVFUNDID"].ToString());
               if (MfeeRadioButton.Checked)
               {
                   drFeeData["FEE"] = CalculateMFee(Convert.ToInt16(dtNavInfo.Rows[loop]["NAVFUNDID"].ToString()), Convert.ToDecimal(dtNavInfo.Rows[loop]["AVERAGE"].ToString()));
               }
               else if (NAVRadioButton.Checked)
               {
                   drFeeData["FEE"] = CalcalateCustoFee(Convert.ToInt16(dtNavInfo.Rows[loop]["NAVFUNDID"].ToString()));
               }
               else if (SecuritiesRadioButton.Checked)
               {
                   drFeeData["FEE"] = CalcalateCustoFee(Convert.ToInt16(dtNavInfo.Rows[loop]["NAVFUNDID"].ToString()));
               }
               else if (TrusteeRadioButton.Checked)
               {
                   decimal Average = Convert.ToDecimal(dtNavInfo.Rows[loop]["AVERAGE"]);
                   decimal totalTrustee = 0;
                   totalTrustee = (Average * Convert.ToDecimal(0.1)) / 100;
                   totalTrustee = (totalTrustee * days) / 365;
                   totalTrustee = Decimal.Round(totalTrustee, 2);

                   drFeeData["FEE"] = totalTrustee;
               }
               dtFeeData.Rows.Add(drFeeData);
           }

           DataTable dt = dtFeeData;
           Session["dtFeeData"] = dtFeeData;
           Session["feeType"] = feeType;
           Session["fromDate"] = fromDateTextBox.Text.Trim().ToString();
           Session["ToDate"] = ToDateTextBox.Text.Trim().ToString();
           ClientScript.RegisterStartupScript(this.GetType(), "Nav Summary Report", "window.open('ReportViewer/SummaryFeeViewer.aspx')", true);
       }
       else
       {
           ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "alert ('No Data Found');", true);
       }
       
    }
    private decimal CalculateMFee(int fundCode, decimal Average)
    {
        DateTime DateFrom = Convert.ToDateTime( fromDateTextBox.Text.ToString());
        DateTime DateTo = Convert.ToDateTime(ToDateTextBox.Text.ToString());
        TimeSpan dateDifference = Convert.ToDateTime(DateTo.ToString("dd-MMM-yyyy")).Subtract(Convert.ToDateTime(DateFrom.ToString("dd-MMM-yyyy")));
        int days = dateDifference.Days + 1;
        if (days > 365)
        {
            days = 365;
        }
       
        decimal mFee1 = 0;
        decimal mFee2 = 0;
        decimal mFee3 = 0;
        decimal mFee4 = 0;
        decimal totalManagementFee = 0;
        //DataTable dtReprtSource = new DataTable();
        //DataTable dtTotal = new DataTable();
        //decimal TotalNav = 0;
        //decimal Average = 0;
        if (fundCode == 8) //Management Fee For Prime Finance Mutual Fund
        {
            if (Average > 500000000)
            {
               
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
               
                mFee1 = Convert.ToDecimal(50000000 * 0.0225);
                mFee1 = (mFee1 * days) / 365;
                mFee2 = Convert.ToDecimal((Average - 50000000) * Convert.ToDecimal(0.0175));
                mFee2 = (mFee2 * days) / 365;
                totalManagementFee = Convert.ToDecimal(mFee2 + mFee1);
                //totalManagementFee = (totalManagementFee * days) / 365;
            }
            else if (Average <= 50000000 && Average >= 0)
            {
               
                mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.0225));
                mFee1 = (mFee1 * days) / 365;
                totalManagementFee = mFee1;
                //totalManagementFee = (totalManagementFee * days) / 365;
            }
        }
        else if (fundCode == 14)//Management Fee For IFIL Mutual Fund
        {
           
            mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.005));
            mFee1 = (mFee1 * days) / 365;
            totalManagementFee = mFee1;
        }
        else if (fundCode == 20)//Management Fee For Islamic Unit Fund
        {
           
            mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.005));
            mFee1 = (mFee1 * days) / 365;
            totalManagementFee = mFee1;
        }
        else if (fundCode == 17)//Management Fee For BD Fund
        {
            
            mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.015));
            mFee1 = (mFee1 * days) / 365;
            totalManagementFee = mFee1;
        }
        else
        {
            if (Average > 500000000) //Management Fee For Rimining  Mutual Fund
            {
                
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
                
                mFee1 = Convert.ToDecimal(50000000 * 0.025);
                mFee1 = (mFee1 * days) / 365;
                mFee2 = Convert.ToDecimal((Average - 50000000) * Convert.ToDecimal(0.020));
                mFee2 = (mFee2 * days) / 365;
                totalManagementFee = mFee2 + mFee1;
                //totalManagementFee = (totalManagementFee * days) / 365;
            }
            else if (Average <= 50000000 && Average >= 0)
            {              
                mFee1 = Convert.ToDecimal(Average * Convert.ToDecimal(0.025));
                mFee1 = (mFee1 * days) / 365;
                totalManagementFee = mFee1;
                //totalManagementFee = (totalManagementFee * days) / 365;
            }
        }
        return decimal.Round(totalManagementFee,2);
    }
    private decimal CalcalateCustoFee(int fundCode)
    {

        StringBuilder sbSQLTotalSecurities = new StringBuilder();
        DateTime DateFrom = Convert.ToDateTime(fromDateTextBox.Text.ToString());
        DateTime DateTo = Convert.ToDateTime(ToDateTextBox.Text.ToString());
        TimeSpan dateDifference = Convert.ToDateTime(DateTo.ToString("dd-MMM-yyyy")).Subtract(Convert.ToDateTime(DateFrom.ToString("dd-MMM-yyyy")));
        int days = dateDifference.Days + 1;
        if (days > 365)
        {
            days = 365;
        }
        else if (NAVRadioButton.Checked)
        {
            sbSQLTotalSecurities.Append(" SELECT SUM(NAVLASTVAULTVALUE) AS TOTAL, ROUND(AVG(NAVLASTVAULTVALUE),2) AS AVERAGE FROM NAV_MASTER WHERE (NAVDATE IN");
            sbSQLTotalSecurities.Append(" (SELECT MAX(NAVDATE) AS MAXNAVDATE FROM NAV_MASTER NAV_MASTER_1 WHERE (NAV_MASTER_1.NAVDATE BETWEEN '" + Convert.ToDateTime(DateFrom).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(DateTo).ToString("dd-MMM-yyyy") + "') AND (NAV_MASTER_1.NAVFUNDID = " + fundCode + ")");
            sbSQLTotalSecurities.Append(" GROUP BY TO_CHAR(NAV_MASTER_1.NAVDATE, 'MM'))) AND (NAV_MASTER.NAVFUNDID = " + fundCode + ")");
        }
        else
        {
            sbSQLTotalSecurities.Append(" SELECT  ROUND(SUM(B.MARKETPRICE),2) AS TOTAL, ROUND(AVG(B.MARKETPRICE ),2) AS AVERAGE FROM ( ");
            sbSQLTotalSecurities.Append("  SELECT NAVDATE, SUM(MARKETPRICE) AS MARKETPRICE FROM (SELECT  TO_CHAR(BAL_DT_CTRL, 'DD-MON-YYYY') AS NAVDATE, TOT_NOS * ADC_RT AS MARKETPRICE ");
            sbSQLTotalSecurities.Append("  FROM  INVEST.PFOLIO_BK  WHERE (BAL_DT_CTRL IN (SELECT  MAX(BAL_DT_CTRL) AS MAXNAVDATE FROM  INVEST.PFOLIO_BK PFOLIO_BK_1 ");
            sbSQLTotalSecurities.Append("  WHERE (BAL_DT_CTRL BETWEEN '" + Convert.ToDateTime(DateFrom).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(DateTo).ToString("dd-MMM-yyyy") + "') AND (F_CD  = " + fundCode + ")");
            sbSQLTotalSecurities.Append("  GROUP BY TO_CHAR(BAL_DT_CTRL, 'MM')))  AND (F_CD = " + fundCode + ")) A  GROUP BY NAVDATE ORDER BY TO_DATE(NAVDATE, 'DD-MON-YYYY') ) B");
        }


        DataTable dtTotal = commonGatewayObj.Select(sbSQLTotalSecurities.ToString());




        decimal Average = Convert.ToDecimal(dtTotal.Rows[0]["AVERAGE"]);
        decimal totalCustoFee = 0;
        if (fundCode == 15 || fundCode == 16)
        {
            totalCustoFee = (Average * Convert.ToDecimal(0.075)) / 100;
        }
        else
        {
            totalCustoFee = (Average * Convert.ToDecimal(0.1)) / 100;
        }
        totalCustoFee = (totalCustoFee * days) / 365;
        totalCustoFee = Decimal.Round(totalCustoFee, 2);
        return  decimal.Round(totalCustoFee,2);
    }
   
   
}
