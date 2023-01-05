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
using System.Text;
public partial class ReportViwer_SummaryFeeViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    AllReportDAO reportDAO = new AllReportDAO();
    protected void Page_Load(object sender, EventArgs e)
     {

         DataTable dtReprtSource = (DataTable)Session["dtFeeData"];
        string feeType=(string)Session["feeType"];
        string fromDate = (string)Session["fromDate"];
        string ToDate = (string)Session["ToDate"];



        dtReprtSource.TableName = "SummaryReport";

        if (dtReprtSource.Rows.Count > 0)
        {
            //dtReprtSource.WriteXmlSchema(@"D:\Project\Web\AMCL.AllReport\UI\ReportViewer\Report\crtSummaryFee.xsd");
            ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/crtSummaryFee.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CrystalReportViewer1.ReportSource = rdoc;

            rdoc.SetParameterValue("feeType", feeType);
            rdoc.SetParameterValue("fromDate", fromDate);
            rdoc.SetParameterValue("toDate", ToDate);
        }
        else
        {
            Response.Write("No Data Found");
        }
       
    }
}
