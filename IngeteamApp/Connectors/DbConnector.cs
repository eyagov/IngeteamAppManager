using IngeteamApp.MODEL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using static IngeteamApp.MODEL.Programmer;

namespace IngeteamApp.Connectors
{
    public class DbConnector
    {
    SqlConnection lCon = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; " +
    @"AttachDbFilename = C:\Users\Eduardo\source\repos\IngeteamApp - Copy (2)\IngeteamApp\Database1.mdf;" +
    "Integrated Security = True; " +
    "User Instance = False");
        public ProgrammerCollection loadProgrammers()
        {
            ProgrammerCollection list = new ProgrammerCollection()
            {
               /*new Programmer(1, "Eduardo", "Yago", "eyagov@gmail.com", "Valencia: C.Malilla 81","671300176"),
               new Programmer(2, "Jose", "Yago", "jogo@gmail.com", "Valencia: C.Malilla 81", "621320776"),
               new Programmer(3, "Carmen", "Vicent", "carmi@gmail.com", "Valencia: C.Malilla 81", "671200476"),
               new Programmer(4, "Salvador", "Yago", "voroi@gmail.com", "Valencia: C.Malilla 81", "621200456"),
               new Programmer(5, "Pedro", "Giner", "eyhf@gmail.com", "Valencia: C.Malilla 81", "671300176"),
               new Programmer(6, "Maria", "Jose", "johfd@gmail.com", "Valencia: C.Malilla 81", "621420776")*/
            };

            SqlCommand lcom = new SqlCommand("SELECT * FROM Programmer", lCon);
            lCon.Open();
            SqlDataReader reader =  lcom.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    //can't be null
                    int a = (int) reader["Id"]; 
                    string b = (string) reader["Name"]; b.Trim();
                    string c = (string) reader["Surname"]; c.Trim();
                    //can be null
                    string d = (reader["Email"].Equals(System.DBNull.Value)) ? "" : (string)reader["Email"]; d.Trim();
                    string e = (reader["Residence"].Equals(System.DBNull.Value)) ? "" : (string)reader["Residence"]; e.Trim();
                    string f = (reader["Number"].Equals(System.DBNull.Value)) ? "" : (string)reader["Number"]; f.Trim();
                                        
                    Programmer p = new Programmer(a, b, c, d, e, f);
                    list.Add(p);

                }
            }
            finally
            {
                // Close reader when done reading.
                reader.Close();
            }

            lCon.Close();//Close connection at the end
            return list;
        }
        public void removeProgrammers(Programmer p)
        {
            SqlCommand lcom = new SqlCommand();
            lcom.CommandText = $"DELETE FROM Programmer WHERE Id='{p.Id}'";
            lcom.Connection = lCon;
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (response == 0) MessageBox.Show("Fail removing programmer."); else MessageBox.Show("Programmer sucessfully removed.");
            lCon.Close();

        }

        public void addProgrammer(Programmer prog)
        {
            SqlCommand lcom = new SqlCommand();
            lcom.CommandText = $"insert into Programmer([name],Surname, Email, Residence, Number) values ('{prog.Name}','{prog.Surname}','{prog.Email}', '{prog.Residence}', '{prog.Number}')";
            lcom.Connection = lCon;
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (response == 0) MessageBox.Show("Fail adding programmer."); else MessageBox.Show("Programmer sucessfully added.");
            lCon.Close();
                                   
        }

        public void modifyProgrammer(Programmer p, Programmer curr)
        {
            SqlCommand lcom = new SqlCommand();
            lcom.CommandText = $"UPDATE Programmer SET [name] = '{p.Name}', Surname = '{p.Surname}', Email = '{p.Email}', Residence = '{p.Residence}', Number = '{p.Number}' WHERE Id = { curr.Id}";
            lcom.Connection = lCon;
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (response == 0) MessageBox.Show("Fail modifying programmer."); else MessageBox.Show("Programmer sucessfully modified.");
            lCon.Close();

        }

        public TaskCollection loadTasks()
        {
            DateTime dat = new DateTime(1998, 01, 01, 0, 0, 0);
            //DateTime dat2 = dat.Date;
            //TimeSpan ts = dat.TimeOfDay;
            TaskCollection list = new TaskCollection();

            SqlCommand lcom = new SqlCommand("SELECT * FROM Task WHERE TaskParentId IS NULL ", lCon);
            lCon.Open();
            SqlDataReader reader = lcom.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    //can't be null
                    int a = (int)reader["Id"];
                    string b = (string)reader["Name"]; b.Trim();
                    
                    //can be null
                    string g = (reader["Description"].Equals(System.DBNull.Value)) ? "" : (string)reader["Description"]; g.Trim();
                    string f = (reader["State"].Equals(System.DBNull.Value)) ? "" : (string)reader["State"]; f.Trim();
                    double c = (reader["Time"].Equals(System.DBNull.Value)) ? 0 : (double)reader["Time"]; 
                    DateTime e = (reader["Close"].Equals(System.DBNull.Value)) ? dat : ((DateTime)reader["Close"]);
                    DateTime d = (reader["Start"].Equals(System.DBNull.Value)) ? dat : ((DateTime)reader["Start"]); 
                    int h = (reader["TaskParentId"].Equals(System.DBNull.Value)) ? -1 : (int)reader["TaskParentId"]; 
                    int i = (reader["ProgrammerId"].Equals(System.DBNull.Value)) ? -1 : (int)reader["ProgrammerId"]; 
                    Task t = new Task(a, b, c, d, e, f, g, h, i);
                    list.Add(t);

                }
            }
            finally
            {
                // Close reader when done reading.
                reader.Close();
            }

            lCon.Close();//Close connection at the end
            return list;
        }

    }
    
}
