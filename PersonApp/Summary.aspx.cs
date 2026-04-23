using MyFlaglerWeb2026.PersonApp;
using Npgsql; // PostgreSQL (Supabase) driver
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace MyFlaglerWeb2026.PersonApp
{
    public partial class Summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            IsPostBack tells us if the page is loading for the first time
            or after a user action (like clicking a button).
            */
            if (!IsPostBack)
            {
                // Load all data initially (no filter)
                LoadSummary("");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Reload data with search keyword
            LoadSummary(txtSearch.Text.Trim());
        }

        private void LoadSummary(string keyword = "")
        {
            // Get database connection string from Web.config
            string connStr = ConfigurationManager.ConnectionStrings["PersonApp"].ConnectionString;

            // Create connection to PostgreSQL (Supabase)
            using (NpgsqlConnection conn = new NpgsqlConnection(connStr))
            {
                // SQL query to retrieve combined data from multiple tables
                // persons = main table
                // students, professors, staff = subtype tables
                string query = @"
                    SELECT 
                        p.person_id,
                        p.name,
                        p.person_code AS campus_id,
                        p.email,
                        p.person_type,
                        s.major,
                        s.gpa,
                        s.is_full_time,
                        s.enrollment_date,
                        pr.department,
                        pr.research_area,
                        pr.is_terminal_degree,
                        st.position,
                        st.division,
                        st.is_administrative
                    FROM persons p

                    -- LEFT JOIN ensures every person appears,
                    -- even if they are only in one role
                    LEFT JOIN students s ON p.person_id = s.person_id
                    LEFT JOIN professors pr ON p.person_id = pr.person_id
                    LEFT JOIN staff st ON p.person_id = st.person_id

                    -- Search filter:
                    -- If keyword is empty → return all records
                    -- Otherwise → search name or email
                    WHERE (@Keyword = '' 
                           OR p.name ILIKE '%' || @Keyword || '%'
                           OR p.email ILIKE '%' || @Keyword || '%')

                    -- Sort results by ID
                    ORDER BY p.person_id ASC;";

                // DataAdapter executes the query and fills a DataTable
                // (acts as a bridge between database and memory)
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, conn);

                // Add parameter value (prevents SQL injection)
                adapter.SelectCommand.Parameters.AddWithValue("@Keyword", keyword);

                // Create in-memory table to store results
                DataTable table = new DataTable();

                // Execute query and load results into DataTable
                adapter.Fill(table);

                // Bind data to GridView for display on the webpage
                gvSummary.DataSource = table;
                gvSummary.DataBind();
            } // connection automatically closes here
        }
    }
}