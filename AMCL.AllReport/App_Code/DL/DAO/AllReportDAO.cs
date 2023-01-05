using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for Pf1s1DAO
/// </summary>
public class AllReportDAO
{
    CommonGateway CommonGetwayObj = new CommonGateway();
    public AllReportDAO()
    {
        ////
        // TODO: Add constructor logic here
        //
    }

   // public string GetMaxNAVDate(int yearFrom, int yearTo, int fundCode)
    //{
    //    string MaxNavDate = "";
        
    //    //for July
    //    DataTable dtMonth = new DataTable();
    //    dtMonth = CommonGetwayObj.Select("SELECT MAX (NAV_MASTER.NAVDATE) AS NAV_DATE FROM NAV_MASTER WHERE  (NAV_MASTER.NAVFUNDID=" + fundCode + ") AND (NAV_MASTER.NAVDATE BETWEEN '01-Jul-" + yearFrom + "' AND '31-Jul-" + yearTo + "')");
    //    if (dtMonth.Rows.Count > 0)
    //    {
    //        MaxNavDate = dtMonth.Rows[0]["NAV_DATE"].Equals(DBNull.Value) ? "" : dtMonth.Rows[0]["NAV_DATE"].ToString();
    //    }
    //    return MaxNavDate;

    //}
     
}

