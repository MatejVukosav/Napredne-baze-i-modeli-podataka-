using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

namespace NBMP_1.projekt
{
    public partial class Search : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void search_query_btn_Click(object sender, EventArgs e)
        {

            string inputQuery = search_input.Value;
            string selectedOperator = RadioButtonOperators.SelectedValue.ToString();
            string selectedMethod = RadioButtonSearchMethod.SelectedValue.ToString();


            using (NpgsqlConnection conn = new NpgsqlConnection("Host=127.0.0.1;Username=postgres;Password=root1;Database=NMBP_1_projekt"))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    string searchQueryWithOperators = "";
                    char[] delimiterChars = { ' ', '"' };
                    string[] separatedData = inputQuery.Split(delimiterChars);
                    if (selectedOperator.Equals("and"))
                    {

                        foreach (string s in separatedData)
                        {
                            if (separatedData.Length > 1)
                            {
                                searchQueryWithOperators = string.Concat(searchQueryWithOperators, "&");
                            }
                            searchQueryWithOperators = string.Concat(searchQueryWithOperators, s);
                        }
                    }
                    else if (selectedOperator.Equals("or"))
                    {
                        foreach (string s in separatedData)
                        {
                            if (separatedData.Length > 1)
                            {
                                searchQueryWithOperators = string.Concat(searchQueryWithOperators, "|");
                            }
                            searchQueryWithOperators = string.Concat(searchQueryWithOperators, s);
                        }
                    }

                    cmd.Parameters.Add(new NpgsqlParameter("searchQueryWithOperators", searchQueryWithOperators));
                    cmd.Parameters.Add(new NpgsqlParameter("selectedMethod", selectedMethod));
                    String query = "";
                    if (selectedMethod.Equals("morphology_&_semantic"))
                    {

                        query = "SELECT " +
"ts_headline(title, to_tsquery('english', (:searchQueryWithOperators ))), " +
"title , " +
"ts_rank(setWeight(to_tsvector(title), 'A'), to_tsquery('english', (:searchQueryWithOperators))) AS rank " +
"FROM texttable " +
"WHERE to_tsvector(title) @@ to_tsquery('english', :searchQueryWithOperators) " +
"AND to_tsvector(title) @@ to_tsquery('english', :searchQueryWithOperators) " +
"ORDER BY rank DESC";

                    }
                    else if (selectedMethod.Equals("fuzzy_string_matching"))
                    {

                    }



                    cmd.CommandText = query;
                    search_query_textarea.Value = query;

                    StringBuilder sb = new StringBuilder();
                    int numOfElements = 0;
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            String element = reader.GetString(0);
                            Console.WriteLine(element);
                            sb.AppendLine(element);
                            numOfElements++;
                        }

                        reader.Close();
                    }

                    num_of_search_items.Text = numOfElements.ToString();
                    search_result.Text = sb.ToString().Replace(Environment.NewLine, "<br />"); ;

                    //DODAJ ZAPIS U QUERY TABLICU

                    saveQueryToDatabase(searchQueryWithOperators, cmd);



                }
            }
        }

        private void saveQueryToDatabase(string query, NpgsqlCommand cmd)
        {
            int date = Int32.Parse(DateTime.Now.ToString("yyyyMd"));
            int hour = Int32.Parse(DateTime.Now.ToString("hh"));


            cmd.Parameters.Add(new NpgsqlParameter("query", query));
            cmd.Parameters.Add(new NpgsqlParameter("date", date));
            cmd.Parameters.Add(new NpgsqlParameter("hour", hour));

            String insertString = "INSERT INTO queries (query,date,time) VALUES (:query,:date,:hour)";
            // Insert some data
            cmd.CommandText = insertString;
            cmd.ExecuteNonQuery();
        }
    }
}