using HRLeaves.Services.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Resources;
using System.Text;

namespace HRLeaves.Services.Data
{
    public class DataAccessDB : ILeavesDataAccess
    {
        public string ConnectionString { get; set; }
        public DataAccessDB(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public List<Leave> GetAllEmployeesLeavesRequests()
        {
            List<Leave> LeavesList = new List<Leave>();
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT ID
                                             ,Name
	                                         ,OffTypeID
	                                         ,TypeID	 
                                                 FROM Leaves                                     		   
                                            WHERE StatusID=2";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Leave leave=new Leave();
                    leave.ID = Convert.ToInt32(reader["ID"]);
                    leave.EmployeeName = reader["Name"].ToString();
                    leave.offType = (OffTypes)Enum.Parse(typeof(OffTypes), reader["offTypeID"].ToString());
                    leave.Type = (LeavesTypes)Enum.Parse(typeof(LeavesTypes), reader["TypeID"].ToString());
                    LeavesList.Add(leave);
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return LeavesList;
        }

        public Leave GetEmployeeLeave(int empLeaveID)
        {
            Leave leave = new Leave();
            try
            {              
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT empL.ID
											     ,Name,ef.filePath
                                                 ,TypeID
                                                 ,OffTypeID
												 ,do.StartDate
												 ,do.EndDate
	                                             ,ho.Day
	                                             ,ho.StartTime
	                                             ,ho.numberOfHours												 
	                                             ,tp.Type	                                             
	                                             ,note
                                                 FROM Leaves empL
                                                 Join LeavesTypes tp ON tp.ID =empL.TypeID 
												 left Join EmployeeFiles ef ON ef.LeavesID=empL.ID
	                                             Join OffTypes ot ON ot.ID=empL.OffTypeID
	                                             left Join HoursOff ho ON ho.LeavesID=empL.ID	
												 left Join DaysOff do ON do.LeavesID=empL.ID
                                                 WHERE empL.ID=@ID";

                command.Parameters.AddWithValue("ID", empLeaveID);

                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    leave.EmployeeName = row["Name"].ToString();
                    leave.Type= (LeavesTypes)Enum.Parse(typeof(LeavesTypes), row["TypeID"].ToString());
                    if (leave.Type.ToString() == "SickLeave")
                    {
                        leave.FilePath = row["filePath"].ToString();
                    }  
                    
                    leave.offType = (OffTypes)Enum.Parse(typeof(OffTypes), row["OffTypeID"].ToString());                    
                    if (leave.offType.ToString() == "DaysOff")
                    {
                        leave.StartDate = Convert.ToDateTime(row["StartDate"].ToString());
                        leave.EndDate = Convert.ToDateTime(row["EndDate"]);
                    }
                    else if (leave.offType.ToString() == "HoursOff") {
                        leave.Day = Convert.ToDateTime(row["Day"]);
                        leave.StartTime = Convert.ToDateTime(row["StartTime"].ToString());
                        leave.numberOfHours = Convert.ToInt32(row["numberOfHours"]);
                    }
                    leave.note = row["note"].ToString();
                }
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return leave;
        }


        public string GetEmployeeLeaveFile(int empLeaveID)
        {
            string employeeFile=null;
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT filePath
                                          FROM [LeavesManagement].[dbo].[EmployeeFiles]
                                            WHERE LeavesID=@ID";

                command.Parameters.AddWithValue("ID", empLeaveID);
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    employeeFile = row["filePath"].ToString();                    
                }
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeeFile;
        }


        public void CreateLeave(Leave leave)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            try
            {
                command.Connection.Open();
                command.CommandText = @"INSERT INTO [dbo].[Leaves]
                                               ([Name]
                                               ,[TypeID]
                                               ,[OffTypeID]
                                               ,[note]
                                               ,[StatusID])
                                         VALUES
                                               (@Name
                                               ,@TypeID
                                               ,@OffTypeID
                                               ,@note
                                               ,@StatusID)";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("Name", leave.EmployeeName);
                command.Parameters.AddWithValue("TypeID", (int)leave.Type);
                command.Parameters.AddWithValue("OffTypeID", (int)leave.offType);              
                command.Parameters.AddWithValue("note", leave.note);
                command.Parameters.AddWithValue("StatusID", 2);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                if (Convert.ToInt32(leave.Type) == 1)
                {
                    command.CommandText = @"INSERT INTO [dbo].[EmployeeFiles]
                                               ([LeavesID]
                                               ,[filePath])
                                         VALUES
                                               (@LeavesID
                                               ,@filePath)";

                    
                    command.Parameters.AddWithValue("LeavesID", GetAllEmployeesLeavesRequests().Last().ID);                               
                    command.Parameters.AddWithValue("filePath", leave.FilePath);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                }


                if (Convert.ToInt32(leave.offType) == 1)
                {
                    command.CommandText = @"INSERT INTO [dbo].[DaysOff]
                                               ([LeavesID]
                                               ,[StartDate]
                                               ,[EndDate])
                                         VALUES
                                               (@LeavesID
                                               ,@StartDate
                                               ,@EndDate)";


                    command.Parameters.AddWithValue("LeavesID", GetAllEmployeesLeavesRequests().Last().ID);
                    command.Parameters.AddWithValue("StartDate", leave.StartDate);
                    command.Parameters.AddWithValue("EndDate", leave.EndDate);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                }
                else if (Convert.ToInt32(leave.offType) == 2)
                {
                    command.CommandText = @"INSERT INTO [dbo].[HoursOff]
                                               ([LeavesID]
                                               ,[Day]
                                               ,[StartTime]
                                               ,[numberOfHours])
                                         VALUES
                                               (@LeavesID
                                               ,@Day
                                               ,@StartTime
                                               ,@numberOfHours)";


                    command.Parameters.AddWithValue("LeavesID", GetAllEmployeesLeavesRequests().Last().ID);
                    command.Parameters.AddWithValue("Day", leave.Day);
                    command.Parameters.AddWithValue("StartTime", leave.StartTime);
                    command.Parameters.AddWithValue("numberOfHours", leave.numberOfHours);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                command.Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditLeave(Leave leave)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            try
            {                                
                command.CommandText = @"UPDATE [dbo].[Leaves]
                                                   SET [StatusID] = @StatusID                                                      
                                                 WHERE ID =@ID";

                command.Parameters.AddWithValue("ID", leave.ID);
                command.Parameters.AddWithValue("StatusID", (int)leave.Status);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
