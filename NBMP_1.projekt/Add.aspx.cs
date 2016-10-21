using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Devart;
using Npgsql;

namespace NBMP_1.projekt
{
    public partial class Add : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void add_text_btn_Click(object sender, EventArgs e)
        {

            String title = add_title.Value;
            String keywords = add_keywords.Value;
            String summary = add_summary.Value;
            String body = add_body.Value;

            //title = "naslov jedan";
            //keywords = "tesni keyword;proba";
            //summary = "Jedan dan je bilo sve suncano i tesni keyword i proba";
            //body = "Malo sunca malo kise i proba";

            using (var conn = new NpgsqlConnection("Host=127.0.0.1;Username=postgres;Password=root1;Database=NMBP_1_projekt"))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.Parameters.Add(new NpgsqlParameter("title", title));
                    cmd.Parameters.Add(new NpgsqlParameter("keywords", keywords));
                    cmd.Parameters.Add(new NpgsqlParameter("summary", summary));
                    cmd.Parameters.Add(new NpgsqlParameter("body", body));
                    String insertString = "INSERT INTO texttable (title,keywords,summary,body) VALUES (:title,:keywords,:summary,:body)";
                    // Insert some data
                     cmd.CommandText = insertString;
                     cmd.ExecuteNonQuery();

                }
            }


        }
    }
}