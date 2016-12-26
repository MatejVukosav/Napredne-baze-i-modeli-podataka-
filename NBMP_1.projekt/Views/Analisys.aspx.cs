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
                    bool isDays = selectedValue.Equals("days");
                    bool isHours = selectedValue.Equals("hours");
                    if (isDays)
                    {
                        query = @getQuerySqlByDays(dateFrom, dateTo);
                    }
                    else if (isHours)
                    {
                        query = getQuerySqlByTime(dateFrom, dateTo);
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        cmd.CommandText = query;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            TableRow columnRow = new TableRow();
                            tablePrikaz.Rows.Add(columnRow);
                            if (isDays)
                            {
                            
                                TableCell columnCell = new TableCell();
                                columnCell.Text="query";
                                columnRow.Cells.Add(columnCell);

                                for (int i = 1; i < 32; i++)
                                {
                                    columnCell = new TableCell();
                                    columnCell.Text = "d"+i;
                                    columnRow.Cells.Add(columnCell);
                                }
                            }else
                            {
                                String columnNames = "\t\t";
                                columnNames += "query";
                                TableCell columnCell = new TableCell();
                                columnCell.Text = columnNames;
                                columnRow.Cells.Add(columnCell);
                                for (int i = 1; i < 25; i++)
                                {
                                    columnCell = new TableCell();
                                    columnCell.Text = "h" + i;
                                    columnRow.Cells.Add(columnCell);
                                }
                            }


                                while (reader.Read())
                            {
       
                                if (isDays)
                                {
                                    TableCell dataCell = new TableCell();
                                    TableRow dataRow = new TableRow();
                                    tablePrikaz.Rows.Add(dataRow);                                                  

                                    for (int i = 0; i < 32; i++)
                                    {
                                        String column = "";
                                        if (reader.IsDBNull(i))
                                        {
                                            column += "0";
                                        }
                                        else
                                        {
                                            column += reader.GetString(i);
                                        }
                                        dataCell = new TableCell();
                                        dataCell.Text = column;
                                        dataRow.Cells.Add(dataCell); 
                                    }             
                                }
                                else
                                {

                                    TableCell dataCell = new TableCell();
                                    TableRow dataRow = new TableRow();
                                    tablePrikaz.Rows.Add(dataRow);
                               
                                    for (int i = 0; i < 25; i++)
                                    {
                                        String column = "";
                                        if (reader.IsDBNull(i))
                                        {
                                            column += "0";
                                        }
                                        else
                                        {
                                            column += reader.GetString(i);
                                        }
                                        dataCell = new TableCell();
                                        dataCell.Text = column;
                                        dataRow.Cells.Add(dataCell);
                                    }
                                }
              

                            }

                            reader.Close();
                        }              
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