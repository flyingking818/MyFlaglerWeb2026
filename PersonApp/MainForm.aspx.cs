using System;
using System.Configuration;   // Access Web.config (connection string)
using System.Web.UI;
using Npgsql;                // PostgreSQL (Supabase) driver

//Make sure the name space matches your project and folder structure.

namespace MyFlaglerWeb2026.PersonApp
{

    public partial class MainForm : System.Web.UI.Page
    {
        // Runs every time the page loads
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only initialize UI on first load (not after button clicks)
            if (!IsPostBack)
            {
                pnlProfessor.Visible = false;
                pnlStudent.Visible = false;
                pnlStaff.Visible = false;
            }
        }

        // Triggered when user selects Professor / Student / Staff
        protected void rblPersonType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset all panels
            pnlProfessor.Visible = false;
            pnlStudent.Visible = false;
            pnlStaff.Visible = false;

            // Show only the selected panel
            if (rblPersonType.SelectedValue == "Professor")
                pnlProfessor.Visible = true;
            else if (rblPersonType.SelectedValue == "Student")
                pnlStudent.Visible = true;
            else if (rblPersonType.SelectedValue == "Staff")
                pnlStaff.Visible = true;
        }

        // Display profile info without saving
        protected void btnDisplayProfile_Click(object sender, EventArgs e)
        {
            try
            {
                // Create object based on UI input (factory pattern)
                Person person = CreatePerson();

                if (person == null) return;

                // Polymorphism: calls correct GetDetails() for each type
                lblResult.Text = person.GetDetails();
            }
            catch (Exception ex)
            {
                lblResult.Text = $"Error: {ex.Message}";
            }

            ShowSelectedPanel();
        }

        // Keeps correct panel visible after postback
        private void ShowSelectedPanel()
        {
            pnlProfessor.Visible = false;
            pnlStudent.Visible = false;
            pnlStaff.Visible = false;

            if (rblPersonType.SelectedValue == "Professor")
                pnlProfessor.Visible = true;
            else if (rblPersonType.SelectedValue == "Student")
                pnlStudent.Visible = true;
            else if (rblPersonType.SelectedValue == "Staff")
                pnlStaff.Visible = true;
        }

        // Factory method: creates correct object type
        private Person CreatePerson()
        {
            Person person = null;

            if (rblPersonType.SelectedValue == "Professor")
            {
                // Create Professor object
                person = new Professor(
                    txtName.Text,
                    txtID.Text,
                    txtEmail.Text,
                    ddlDepartment.Text,
                    txtResearchArea.Text,
                    chkTerminalDegree.Checked
                );
            }
            else if (rblPersonType.SelectedValue == "Student")
            {
                person = new Student(
                    txtName.Text,
                    txtID.Text,
                    txtEmail.Text,
                    txtMajor.Text,
                    double.Parse(txtGPA.Text),
                    chkFullTime.Checked,
                    Convert.ToDateTime(txtEnrollmentDate.Text)
                );
            }
            else if (rblPersonType.SelectedValue == "Staff")
            {
                person = new Staff(
                    txtName.Text,
                    txtID.Text,
                    txtEmail.Text,
                    txtPosition.Text,
                    txtDivision.Text,
                    chkAdministrative.Checked
                );
            }

            return person;
        }

        // Save profile to database
        protected void btnAddProfile_Click(object sender, EventArgs e)
        {
            try
            {
                // Create object from UI input
                Person person = CreatePerson();

                if (person == null) return;

                // Call database method
                InsertToDatabase(person);

                // Hide form after saving
                PanelForm.Visible = false;
            }
            catch (Exception ex)
            {
                lblResult.Text = $"Error: {ex.Message}";
            }
        }

        // Handles all database operations (PostgreSQL / Supabase)
        private void InsertToDatabase(Person person)
        {
            // Read connection string from Web.config
            string connString = ConfigurationManager.ConnectionStrings["PersonApp"].ConnectionString;
            //string connString = "db=...dfadfdfadsfdaf"; NEVER NEVER do this!!

            // Create connection to PostgreSQL  //running time protection.
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open(); // must open connection before executing SQL

                // Transaction ensures all operations succeed or fail together
                using (NpgsqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // STEP 1: Insert into main table (persons)
                        // This table stores common fields for ALL people
                        string insertPersonSql = @"
                        INSERT INTO persons (name, person_code, email, person_type, created_at)
                        VALUES (@name, @person_code, @email, @person_type, @created_at)
                        RETURNING person_id;"; // PostgreSQL returns the new ID

                        using (NpgsqlCommand cmdPerson = new NpgsqlCommand(insertPersonSql, conn, transaction))
                        {
                            // Add parameter values (safe, prevents SQL injection)
                            cmdPerson.Parameters.AddWithValue("@name", person.Name);
                            cmdPerson.Parameters.AddWithValue("@person_code", person.ID);
                            cmdPerson.Parameters.AddWithValue("@email", person.Email);
                            cmdPerson.Parameters.AddWithValue("@person_type", person.GetType().Name);
                            cmdPerson.Parameters.AddWithValue("@created_at", DateTime.Now);

                            // Execute query and get generated person_id
                            int personID = Convert.ToInt32(cmdPerson.ExecuteScalar());
                           //cmdperson.ExecuteNonQuery();

                            // STEP 2: Insert into subtype table (example: Professor)
                            // Each role has its own table with additional fields
                            // person_id links the tables together

                            if (person is Professor professor)
                            {
                                string insertProfessorSql = @"
                                INSERT INTO professors (person_id, department, research_area, is_terminal_degree)
                                VALUES (@person_id, @department, @research_area, @is_terminal_degree);";

                                using (NpgsqlCommand cmd = new NpgsqlCommand(insertProfessorSql, conn, transaction))
                                {
                                    // Link to persons table (this would be the foreign key in the Person table)
                                    cmd.Parameters.AddWithValue("@person_id", personID);

                                    // Professor-specific fields
                                    cmd.Parameters.AddWithValue("@department", professor.Department ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@research_area", professor.ResearchArea ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@is_terminal_degree", professor.IsTerminalDegree);

                                    cmd.ExecuteNonQuery(); // run INSERT
                                }
                            }
                            else if (person is Student student)
                            {
                                string insertStudentSql = @"
                                INSERT INTO students (person_id, major, gpa, is_full_time, enrollment_date)
                                VALUES (@person_id, @major, @gpa, @is_full_time, @enrollment_date);";

                                using (NpgsqlCommand cmd = new NpgsqlCommand(insertStudentSql, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@person_id", personID);
                                    cmd.Parameters.AddWithValue("@major", student.Major ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@gpa", student.GPA);
                                    cmd.Parameters.AddWithValue("@is_full_time", student.IsFullTime);
                                    cmd.Parameters.AddWithValue("@enrollment_date", student.EnrollmentDate);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else if (person is Staff staff)
                            {
                                string insertStaffSql = @"
                                INSERT INTO staff (person_id, position, division, is_administrative)
                                VALUES (@person_id, @position, @division, @is_administrative);";

                                using (NpgsqlCommand cmd = new NpgsqlCommand(insertStaffSql, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@person_id", personID);
                                    cmd.Parameters.AddWithValue("@position", staff.Position ?? (object)DBNull.Value);  //optionals
                                    cmd.Parameters.AddWithValue("@division", staff.Division ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@is_administrative", staff.IsAdministrative);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            //Classes match the relational tables!!! Entity Framework approach!
                        }

                        // Save all changes permanently
                        transaction.Commit();

                        lblResult.Text += "Profile saved to Supabase database. <a href=\"Summary.aspx\">View Summary</a>";
                    }
                    catch (Exception ex)
                    {
                        // Undo everything if any step fails
                        transaction.Rollback();

                        lblResult.Text += $"Database error: {ex.Message}";
                    }
                }
            }
        }
    }
}