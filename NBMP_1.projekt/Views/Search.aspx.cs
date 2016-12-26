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
                    bool isMorphology = selectedMethod.Equals("morphology_&_semantic");
                    bool isFuzzy = selectedMethod.Equals("fuzzy_string_matching");
                    if (isMorphology)
                    {

                        query = "SELECT " + "id ," +
                        "ts_headline(title, to_tsquery('english', ('" + searchQueryWithOperators + "'))), " +
                     
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
                    else if (isFuzzy)
                    {
                        query = "SELECT id, ts_headline(title, to_tsquery('english', '" + searchQueryWithOperators + "')), " +
                                "(similarity(title, '" + searchQueryWithOperators + "') + similarity(body, '" + searchQueryWithOperators + "') + similarity(keywords, '" + searchQueryWithOperators + "') + similarity(summary, '" + searchQueryWithOperators + "')) / 4 as similarity "+
                                "FROM texttable " +
                                "where(similarity(title, '" + searchQueryWithOperators + "') + similarity(body, '" + searchQueryWithOperators + "') + similarity(keywords, '" + searchQueryWithOperators + "') + similarity(summary, '" + searchQueryWithOperators + "')) / 4 > 0.2 " +
                                "ORDER BY title DESC";
                    }
                    cmd.CommandText = query;
                    search_query_textarea.Value = query;

                    StringBuilder sb = new StringBuilder();
                    int numOfElements = 0;
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Prikaz naslova kolone
                        TableRow columnRow = new TableRow();
                        tablePrikaz.Rows.Add(columnRow);
                        if (isMorphology)
                        {
                            TableCell columnCell = new TableCell();
                            columnCell.Text = "query";
                            columnRow.Cells.Add(columnCell);

                            columnCell = new TableCell();
                            columnCell.Text = "headline";
                            columnRow.Cells.Add(columnCell);

                            columnCell = new TableCell();
                            columnCell.Text = "rank";
                            columnRow.Cells.Add(columnCell);
                        }
                        else if (isFuzzy)
                        {
                            TableCell columnCell = new TableCell();
                            columnCell.Text = "query";
                            columnRow.Cells.Add(columnCell);

                            columnCell = new TableCell();
                            columnCell.Text = "headline";
                            columnRow.Cells.Add(columnCell);

                            columnCell = new TableCell();
                            columnCell.Text = "similarity";
                            columnRow.Cells.Add(columnCell);
                        }
                        //Prikaz podataka
                        while (reader.Read())
                        {      
                            numOfElements++;

                            TableCell dataCell = new TableCell();
                            TableRow dataRow = new TableRow();
                            tablePrikaz.Rows.Add(dataRow);

                            for (int i = 0; i < 3; i++)
                            {
                                String column = "";
                                if (reader.IsDBNull(i))
                                {
                                    column += "0";
                                }
                                else
                                {
                                    //ako je rank ili similarity broj, tj double
                                    if (i == 2)
                                    {
                                        column += reader.GetDouble(i).ToString();
                                    }
                                    else
                                    {
                                        column += reader.GetString(i);
                                    }
                                }
                                dataCell = new TableCell();
                                dataCell.Text = column;
                                dataRow.Cells.Add(dataCell);
                            }


                        }

                        reader.Close();
                    }

                    num_of_search_items.Text = numOfElements.ToString();
                 
                    //DODAJ ZAPIS U QUERY TABLICU
                    saveQueryToDatabase(searchQueryWithOperators, cmd);



                }
            }
        }

        private void saveQueryToDatabase(string query, NpgsqlCommand cmd)
        {
            DateTime date = DateTime.Now;
            int hour = Int32.Parse(DateTime.Now.ToString("HH"));

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