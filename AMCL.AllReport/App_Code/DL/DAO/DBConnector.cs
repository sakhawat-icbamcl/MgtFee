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
using System.Data.OracleClient;



/// <summary>
/// Summary description for DBConnector
/// </summary>
public class DBConnector
{

    private string connectionString = null;
    private OracleConnection sqlConn = null;

    public DBConnector()
    {
        try
        {
            connectionString = ConfigReader.PF1S1.ToString();
            sqlConn = new OracleConnection(connectionString);
        }

        catch (Exception exceptionObj)
        {
            throw exceptionObj;
        }
    }

    public OracleConnection GetConnection
    {
        get
        {
            return sqlConn;
        }
    }

}
