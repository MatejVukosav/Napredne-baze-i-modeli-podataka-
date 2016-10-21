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
                    char[] delimiterChars = { ' ' };
                    string[] separatedData = inputQuery.Split(delimiterChars);
                    string operatorInput = "";
                    if (selectedOperator.Equals("and"))
                    {
                        int skipFirst = 0;
                        operatorInput = "AND";
                        foreach (string s in separatedData)
                        {

                            if (!string.IsNullOrWhiteSpace(s) && skipFirst > 0 && separatedData.Length > 1)
                            {
                                searchQueryWithOperators = string.Concat(searchQueryWithOperators, " & ");
                            }
                            skipFirst++;
                            searchQueryWithOperators = string.Concat(searchQueryWithOperators, s);
                        }
                    }
                    else if (selectedOperator.Equals("or"))
                    {
                        int skipFirst = 0;
                        operatorInput = "OR";
                        foreach (string s in separatedData)
                        {
                            if (!string.IsNullOrWhiteSpace(s) && skipFirst > 0 && separatedData.Length > 1)
                            {
                                searchQueryWithOperators = string.Concat(searchQueryWithOperators, " | ");
                            }
                            skipFirst++;
                            searchQueryWithOperators = string.Concat(searchQueryWithOperators, s);
                        }
                    }

                    cmd.Parameters.Add(new NpgsqlParameter("searchQueryWithOperators", searchQueryWithOperators));
                    cmd.Parameters.Add(new NpgsqlParameter("selectedMethod", selectedMethod));
                    String query = "";
                    if (selectedMethod.Equals("morphology_&_semantic"))
                    {

                        query = "SELECT " + "id ," +
                        "ts_headline(title, to_tsquery('english', ('" + searchQueryWithOperators + "'))), " +
                        "title , " +
                            "(ts_rank(setWeight(to_tsvector(title), 'A'), to_tsquery('english', ('" + searchQueryWithOperators + "'))) +" +
                            "ts_rank(setWeight(to_tsvector(body), 'D'), to_tsquery('english', ('" + searchQueryWithOperators + "'))) +" +
                            "ts_rank(setWeight(to_tsvector(summary), 'C'), to_tsquery('english', ('" + searchQueryWithOperators + "'))) +" +
                            "ts_rank(setWeight(to_tsvector(keywords), 'B'), to_tsquery('english', ('" + searchQueryWithOperators + "'))) " +
                                   ") / 4 as rank " +
                        "FROM texttable " +
                        "WHERE   titletsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "titletsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "bodytsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "bodytsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "summarytsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "summarytsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "keywordstsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " + operatorInput + " " +
                                "keywordstsv @@ to_tsquery('english', '" + searchQueryWithOperators + "') " +
                        "ORDER BY rank DESC";

                    }
                    else if (selectedMethod.Equals("fuzzy_string_matching"))
                    {
                        query = "SELECT id, ts_headline(title, to_tsquery('english', 'ma')), title, " +
                                "similarity(title, 'ma') AS st, similarity(body,'ma') AS sb, similarity(keywords,'ma') AS sk, similarity(summary,'ma') AS ss " +
                                "FROM texttable " +
                                "where(similarity(title, 'ma') + similarity(body, 'ma') + similarity(keywords, 'ma') + similarity(summary, 'ma')) / 4 > 0.2 " +
                                "ORDER BY title DESC";
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
                    search_result.Text = sb.ToString().Replace(Environment.NewLine, "<br />");

                    //DODAJ ZAPIS U QUERY TABLICU

                    saveQueryToDatabase(searchQueryWithOperators, cmd);



                }
            }
        }

        private void saveQueryToDatabase(string query, NpgsqlCommand cmd)
        {
            DateTime date = DateTime.Now;
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