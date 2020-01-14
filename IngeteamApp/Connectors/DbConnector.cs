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
    @"AttachDbFilename = C:\Users\Eduardo\source\repos\IngeteamAppManager\IngeteamApp\Database1.mdf;" +
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
                    string b = (string) reader["Name"]; 
                    string c = (string) reader["Surname"]; 
                    //can be null
                    string d = (reader["Email"].Equals(System.DBNull.Value)) ? "" : (string)reader["Email"]; 
                    string e = (reader["Residence"].Equals(System.DBNull.Value)) ? "" : (string)reader["Residence"]; 
                    string f = (reader["Number"].Equals(System.DBNull.Value)) ? "" : (string)reader["Number"]; 
                                        
                    Programmer p = new Programmer(a, b.Trim(), c.Trim(), d.Trim(), e.Trim(), f.Trim());
                    list.Add(p);//why here the strings are added wih blanks, maybe Trim() is not owrking before? I have to repeat it in the Programmers toString() for finally remove the blanks

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
            SqlCommand lcom = new SqlCommand($"DELETE FROM Programmer WHERE Id='{p.Id}'", lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (response == 0) MessageBox.Show("Fail removing programmer."); else MessageBox.Show("Programmer removed successfully.");
            lCon.Close();

        }

        public void addProgrammer(Programmer prog)
        {
            SqlCommand lcom = new SqlCommand($"insert into Programmer([name],Surname, Email, Residence, Number) values ('{prog.Name}','{prog.Surname}','{prog.Email}', '{prog.Residence}', '{prog.Number}')",lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (response == 0) MessageBox.Show("Fail adding programmer."); else MessageBox.Show("Programmer added successfully.");
            lCon.Close();
                                   
        }

        public void modifyProgrammer(Programmer p, Programmer curr)
        {
            SqlCommand lcom = new SqlCommand($"UPDATE Programmer SET [name] = '{p.Name}', Surname = '{p.Surname}', Email = '{p.Email}', Residence = '{p.Residence}', Number = '{p.Number}' WHERE Id = { curr.Id}",lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (response == 0) MessageBox.Show("Fail modifying programmer."); else MessageBox.Show("Programmer modified successfully.");
            lCon.Close();

        }

        public TaskCollection loadTasks()
        {
            
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
                    string b = (string)reader["Name"];
                    
                    //can be null
                    string g = (reader["Description"].Equals(System.DBNull.Value)) ? "" : (string)reader["Description"];
                    string f = (reader["State"].Equals(System.DBNull.Value)) ? "" : (string)reader["State"];
                    double? c = (reader["Time"].Equals(System.DBNull.Value)) ? null : (double?)reader["Time"]; 
                    DateTime? e = (reader["Close"].Equals(System.DBNull.Value)) ? null : ((DateTime?)reader["Close"]);
                    DateTime? d = (reader["Start"].Equals(System.DBNull.Value)) ? null : ((DateTime?)reader["Start"]); 
                    int h = (reader["TaskParentId"].Equals(System.DBNull.Value)) ? -1 : (int)reader["TaskParentId"]; 
                    int i = (reader["ProgrammerId"].Equals(System.DBNull.Value)) ? -1 : (int)reader["ProgrammerId"]; 
                    Task t = new Task(a, b.Trim(), c, d, e, f.Trim(), g.Trim(), h, i);
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

        public void removeTasks(Task t)
        {
            SqlCommand lcom = new SqlCommand($"DELETE FROM Task WHERE TaskParentId='{t.Id}'",lCon);
            lCon.Open();
            int responseB = lcom.ExecuteNonQuery();
            lCon.Close();
            lcom.CommandText = $"DELETE FROM Task WHERE Id='{t.Id}'";
            lcom.Connection = lCon;
            lCon.Open();
            int responseA = lcom.ExecuteNonQuery();
            lCon.Close();
            if ((responseA == 0) & (responseB == 0)) MessageBox.Show("Something was wrong removing Task."); else MessageBox.Show("Task removed successfully.");
        }
        public void modifyTask(Task t, Task curr)
        {
            
            SqlCommand lcom = new SqlCommand($"UPDATE Task SET [name] = '{t.Name}',[description] = '{t.Description}', [state] = '{t.State}', [time] = '{t.Time}', [close] = '{t.Close}', [start] = '{t.Start}', ProgrammerId = '{t.ProgrammerId}' WHERE Id = {curr.Id}", lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            if (t.Start == null)
            {
                lcom.CommandText = $"UPDATE Task SET [start] = NULL WHERE Id = {curr.Id}";
                int responseA = lcom.ExecuteNonQuery();
            }
            if (t.Close == null)
            {
                lcom.CommandText = $"UPDATE Task SET [close] = NULL WHERE Id = {curr.Id}";
                int responseB = lcom.ExecuteNonQuery();
            }
            if (response == 0) MessageBox.Show("Fail modifying Task."); else MessageBox.Show("Task modified successfully.");
            lCon.Close();

        }
        public void addTask(Task t)
        {
            SqlCommand lcom = new SqlCommand($"insert into Task([name],[description],[state],[time],[close],[start],TaskParentId,ProgrammerId) values('{t.Name}','{t.Description}','New','{t.Time}','{t.Close}','{t.Start}',NULL,'{t.ProgrammerId}')", lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            lcom.CommandText = $"UPDATE Task SET [close] = NULL, [start] = NULL WHERE [state] = 'New' AND ([close] IS NOT NULL OR [start] IS NOT NULL)";
            int responseB = lcom.ExecuteNonQuery();
            lCon.Close();
            if (response == 0) MessageBox.Show("Fail adding SubTask."); else MessageBox.Show("Task added successfully.");

        }
        #region SUBTASKS REGION
        public TaskCollection loadSubTasks(Task parent)
        {
            
            //DateTime dat2 = dat.Date;
            //TimeSpan ts = dat.TimeOfDay;
            TaskCollection list = new TaskCollection();

            SqlCommand lcom = new SqlCommand($"SELECT * FROM Task WHERE TaskParentId='{parent.Id}'", lCon);
            lCon.Open();
            SqlDataReader reader = lcom.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    //can't be null
                    int a = (int)reader["Id"];
                    string b = (string)reader["Name"];

                    //can be null
                    string g = (reader["Description"].Equals(System.DBNull.Value)) ? "" : (string)reader["Description"];
                    string f = (reader["State"].Equals(System.DBNull.Value)) ? "" : (string)reader["State"];
                    double? c = (reader["Time"].Equals(System.DBNull.Value)) ? null : (double?)reader["Time"];
                    DateTime? e = (reader["Close"].Equals(System.DBNull.Value)) ? null : ((DateTime?)reader["Close"]);
                    DateTime? d = (reader["Start"].Equals(System.DBNull.Value)) ? null : ((DateTime?)reader["Start"]);
                    int? h = (reader["TaskParentId"].Equals(System.DBNull.Value)) ? null : (int?)reader["TaskParentId"];
                    int? i = (reader["ProgrammerId"].Equals(System.DBNull.Value)) ? null : (int?)reader["ProgrammerId"];
                    Task t = new Task(a, b.Trim(), c, d, e, f.Trim(), g.Trim(), h, i);
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

        public void removeSubTasks(Task t)
        {
            SqlCommand lcom = new SqlCommand($"DELETE FROM Task WHERE Id='{t.Id}'", lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            lCon.Close();
            if (response == 0) MessageBox.Show("Something was wrong removing the Subask."); else MessageBox.Show("Subtask removed successfully.");
        }
        public void addSubTask(Task t, Task parent)
        {
            SqlCommand lcom = new SqlCommand($"insert into Task([name],[description],[state],[time],[close],[start],TaskParentId,ProgrammerId) values('{t.Name}','{t.Description}','New','{t.Time}','{t.Close}','{t.Start}',{parent.Id},'{t.ProgrammerId}')", lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();

            lcom.CommandText = $"UPDATE Task SET [close] = NULL, [start] = NULL WHERE TaskParentId = '{parent.Id}' AND [state] = 'New' AND ([close] IS NOT NULL OR [start] IS NOT NULL)";
            int responseB = lcom.ExecuteNonQuery();
          
            lCon.Close();
            if (response == 0) MessageBox.Show("Fail adding SubTask."); else MessageBox.Show("SubTask added successfully.");
            

        }




        #endregion

        public bool existsProgrammer(int? Id)
        {
            
            SqlCommand lcom = new SqlCommand($"SELECT COUNT(*) FROM Programmer WHERE Id='{Id}'", lCon);
            lCon.Open();
            int response = lcom.ExecuteNonQuery();
            int records = (int)lcom.ExecuteScalar();
            lCon.Close();
            

            return records != 0 ? true : false;

        }

    }


}
