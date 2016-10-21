using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using NpgsqlTypes;

namespace NBMP_1.projekt
{
    public partial class Analisys : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        List<ReportDataModel> model = new List<ReportDataModel>();
        protected void search_queries_btn_Click(object sender, EventArgs e)
        {

            using (var conn = new NpgsqlConnection("Host=127.0.0.1;Username=postgres;Password=root1;Database=NMBP_1_projekt"))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    string dateToString = date_to.Value;
                    string dateFromString = date_from.Value;

                    string selectedValue = RadioButtonChooseQueriesSearch.SelectedValue.ToString();

                    string dateFrom = dateFromString;
                    string dateTo = dateToString;

                    //cmd.Parameters.Add("dateFrom", NpgsqlDbType.Date).Value= dateFrom;
                    //cmd.Parameters.Add("dateTo", NpgsqlDbType.Date).Value = dateTo;
                    cmd.Parameters.AddWithValue(":dateFrom", dateFrom);
                    cmd.Parameters.AddWithValue(":dateTo", dateTo);

                    cmd.CommandType = CommandType.Text;
                    String query = "";
                    if (selectedValue.Equals("days"))
                    {
                        query = @getQuerySqlByDays(dateFrom, dateTo);
                    }
                    else if (selectedValue.Equals("hours"))
                    {
                        query = getQuerySqlByDays(dateFrom, dateTo);
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        cmd.CommandText = query;
                        StringBuilder sb = new StringBuilder();
                        //iznad je sve oke

                        DataTable dt = new DataTable();
                        NpgsqlDataAdapter ad = new NpgsqlDataAdapter(cmd);
                        ad.Fill(dt);
                        NpgsqlDataReader dRead = cmd.ExecuteReader();
                        try
                        {
                            Console.WriteLine("Contents of table in database: \n");
                            while (dRead.Read())
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        System.Diagnostics.Debug.WriteLine("{0} \t \n", row[i].ToString());
                                        sb.AppendLine(row[i].ToString());
                                    }
                                }
                            }
                        }
                        catch (NpgsqlException ne)
                        {
                            Console.WriteLine("Problem connecting to server, Error details {0}", ne.ToString());
                        }
                        finally
                        {
                            search_queries_result.Text = sb.ToString();
                            Console.WriteLine("Closing connections");
                            dRead.Close();
                        }



                        //ovo je oke
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                              
                                String element = reader.GetString(0);
                                Console.WriteLine(element);
                                sb.AppendLine(element);
                            }

                            reader.Close();
                        }
                      //  search_queries_result.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
                    }

                }
            }

        }

        private string getQuerySqlByTime(string dateFrom, string dateTo)
        {
            string whereQuery = "WHERE date > $$" + dateFrom + "$$ AND date < $$" + dateTo + "$$";
            return " select* from " +
                    "crosstab('select query as query," +
                            "time as sat, " +
                            "cast(count(*) as int) as brIsp " +
                            "from queries " +
                            whereQuery +
                            "group by query, sat " +
                            "order by query, sat'," +
                        "  'select sat from sati order by sat'" +
                        " )" +
                    "as pivotTable (query text," +
                        "h1 int,h2 int, h3 int,h4 int, h5 int, h6 int, h7 int,h8 int,h9 int," +
                        "h10 int, h11 int, h12 int, h13 int, h14 int, h15 int, h16 int, h17 int, h18 int, h19 int," +
                        "h20 int, h21 int, h22 int, h23 int, h24 int" +
                                ")" +
                    "order by query ";
        }

        private string getQuerySqlByDays(string dateFrom, string dateTo)
        {
            string whereQuery = "WHERE date > $$" + dateFrom + "$$ AND date < $$" + dateTo + "$$";
            return " select* from " +
                    "crosstab('select query as query," +
                            "cast(extract(day from date) as int) as dan, " +
                            "cast(count(*) as int) as brIsp " +
                            "from queries " +
                            whereQuery +
                            " group by query, dan " +
                            "order by query, dan'," +
                        "  'select dan from dani order by dan'" +
                        " )" +
                    "as pivotTable (query text," +
                                    "d1 int,d2 int,d3 int,d4 int,d5 int,d6 int,d7 int,d8 int,d9 int," +
                                    "d10 int,d11 int,d12 int,d13 int,d14 int,d15 int,d16 int,d17 int,d18 int,d19 int," +
                                    "d20 int,d21 int,d22 int,d23 int,d24 int,d25 int,d26 int,d27 int,d28 int,d29 int," +
                                    "d30 int,d31 int" +
                                ")" +
                    "order by query ";
        }
    }
}