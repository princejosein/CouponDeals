using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace CouponDeals.Models
{
    public class UserEntity
    {
        SqlConnection _sqlConnection;
        public UserEntity(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public int CreateOrUpdateUser(string Email, 
            string FirstName, string LastName, int RoleID,int Status, 
            DateTime CreatedDate, string Password = null, int UserID = 0)
        {
            string query;
            if (UserID == 0)
            {
                query = "INSERT INTO tblUsers VALUES(@email, @password," +
                "@first_name,@last_name,@user_role_id, @status, @created_date, @password_code);SELECT CAST(scope_identity() AS int)";
            }
            else
            {
                query = "UPDATE tblUsers SET email = @email," +
                "first_name = @first_name, last_name = @last_name, " +
                "user_role_id = @user_role_id, status = @status " +
                " WHERE user_id = @user_id";
            }
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@email", Email);
            cmd.Parameters.AddWithValue("@first_name", FirstName);
            cmd.Parameters.AddWithValue("@last_name", LastName);
            cmd.Parameters.AddWithValue("@user_role_id", RoleID);
            cmd.Parameters.AddWithValue("@status", Status);
            int InsertID = 0;
            if (UserID == 0)
            {
                string HashPwd = HashPassword(Password);
                cmd.Parameters.AddWithValue("@created_date", CreatedDate);
                cmd.Parameters.AddWithValue("@password", HashPwd);
                cmd.Parameters.AddWithValue("@password_code", "");
                try
                {
                    InsertID = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@user_id", UserID);
                try
                {
                    InsertID = (int)cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return InsertID;
        }
        
        public DataTable GetUserRoles()
        {
            DataTable dt = new DataTable();
            string query = "SELECT user_role_id, name FROM tblUserRoles";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetUsersList(int filterStatus = 0, string filterName = "",
            string filterEmail = "")
        {
            DataTable dt = new DataTable();
            string query = "SELECT tblUsers.user_id, tblUsers.email, tblUsers.first_name, tblUsers.last_name, tblUsers.user_role_id, " +
                "tblUsers.status, tblUsers.created_date, CASE WHEN tblUsers.status = 1 THEN 'Active' ELSE 'Not Active' END AS status_name, " +
                "FORMAT (tblUsers.created_date, 'dd-MM-yyyy') as date, tblUserRoles.name as user_role FROM tblUsers LEFT JOIN tblUserRoles ON " +
                "tblUserRoles.user_role_id = tblUsers.user_role_id";
            int count = 0;
            if (filterStatus != 0)
            {
                query = query + " WHERE tblUsers.status = '" + filterStatus + "'";
                count++;
            }
            if (filterName != "")
            {
                if (count != 0)
                {
                    query = query + " AND tblUsers.first_name LIKE '%" + filterName + "%'";
                    count++;
                }
                else
                {
                    query = query + " WHERE tblUsers.first_name LIKE '%" + filterName + "%'";
                }
            }
            if (filterEmail != "")
            {
                if (count != 0)
                {
                    query = query + " AND tblUsers.email LIKE '%" + filterEmail + "%'";
                    count++;
                }
                else
                {
                    query = query + " WHERE tblUsers.email LIKE '%" + filterEmail + "%'";
                }
            }
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetUserByID(int UserID)
        {
            DataTable dt = new DataTable();
            string query = "SELECT tblUsers.user_id, tblUsers.email, tblUsers.first_name, tblUsers.last_name, tblUsers.user_role_id, " +
                "tblUsers.status, tblUsers.created_date, CASE WHEN tblUsers.status = 1 THEN 'Active' ELSE 'Not Active' END AS status_name, " +
                "tblUserRoles.name as user_role FROM tblUsers LEFT JOIN tblUserRoles ON " +
                "tblUserRoles.user_role_id = tblUsers.user_role_id WHERE tblUsers.user_id = @user_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
            dap.Fill(dt);
            return dt;
        }
        public int DeleteUserByID(int UserID)
        {
            int Result = 0;
            string query = "DELETE FROM tblUsers WHERE user_id = @user_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@user_id", UserID);
            try
            {
                Result = (int)cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Result;
        }
        private string HashPassword(string Password)
        {
            byte[] salt;
            byte[] buffer2;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(Password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public static bool VerifyHashedPassword(string HashPwd, string Password)
        {
            byte[] buffer4;
            byte[] src = Convert.FromBase64String(HashPwd);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(Password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer4.SequenceEqual(buffer3);
        }
        public bool Login(string Email, string Password)
        {
            string HashPassword = "";
            int UserRoleID = 0;
            int UserID = 0;
            string FirstName = "";
            string query = "SELECT TOP 1 * FROM tblUsers WHERE Email = @email";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@email", Email);
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HashPassword = reader.GetString(2);
                        UserRoleID = reader.GetInt32(5);
                        UserID = reader.GetInt32(0);
                        FirstName = reader.GetString(3);
                        HttpContext httpContext = HttpContext.Current;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            bool result = VerifyHashedPassword(HashPassword, Password);
            if (result == true)
            {
                HttpContext httpContext = HttpContext.Current;
                httpContext.Session["Email"] = Email;
                httpContext.Session["UserRoleID"] = UserRoleID;
                httpContext.Session["UserID"] = UserID;
                httpContext.Session["FirstName"] = FirstName;
                httpContext.Session["Cart"] = GetCartByUser(UserID);
                UpdateUserLogin(UserID);

            }
            return result;
        }
        public bool ResetEmail(string Email)
        {
            string MAIL_USERNAME = "aissemester1@gmail.com";
            string FROM_ADDRESS = "coupondeals@gmail.com";
            string MAIL_PASSWORD = "semester1@ais2020";
            string RESET_URL = "http://localhost:56830/ResetPassword.aspx?reset=";
            string ToMessage = "";
            string query = "SELECT TOP 1 * FROM tblUsers WHERE Email = @email";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@email", Email);
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            reader.Close();
            reader = null;
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            RESET_URL = RESET_URL + GuidString;
            MailAddress to = new MailAddress(Email);
            ToMessage = "To reset password click on this link " + RESET_URL;
            MailAddress from = new MailAddress(FROM_ADDRESS);

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Reset Password of Coupon Deals";
            message.Body = ToMessage;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(MAIL_USERNAME, MAIL_PASSWORD),
                EnableSsl = true
            };
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            int InsertID = 0;
            query = "UPDATE tblUsers SET password_code = @password_code " +
                "WHERE email = @email";
            SqlCommand cmdUpdate = new SqlCommand(query, _sqlConnection);
            cmdUpdate.Parameters.AddWithValue("@email", Email);
            cmdUpdate.Parameters.AddWithValue("@password_code", GuidString);
            try
            {
                InsertID = (int)cmdUpdate.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        public bool ResetPassword(string Password, string PassCode)
        {
            string Email = "";
            string HaPassword = "";
            string query = "SELECT TOP 1 * FROM tblUsers WHERE password_code = @pass_code";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@pass_code", PassCode);
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return false;
                } else
                {
                    while(reader.Read())
                    {
                        Email = reader.GetString(1);
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            reader.Close();
            reader = null;
            HaPassword = HashPassword(Password);
            query = "UPDATE tblUsers SET password_code = @password_code, password = @password " +
                "WHERE email = @email";
            SqlCommand cmdUpdate = new SqlCommand(query, _sqlConnection);
            cmdUpdate.Parameters.AddWithValue("@email", Email);
            cmdUpdate.Parameters.AddWithValue("@password_code", "");
            cmdUpdate.Parameters.AddWithValue("@password", HaPassword);
            try
            {
                cmdUpdate.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        public int GetCartByUser(int UserID)
        {
            int CartCount = 0;
            string query = "SELECT COUNT(cart_id) as CartCount FROM tblCarts WHERE user_id = @user_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@user_id", UserID);
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartCount = reader.GetInt32(0);
                    }
                }
                reader.Close();
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return CartCount;
        }
        
        private void UpdateUserLogin(int UserID)
        {
            string query = "SELECT TOP 1 * FROM tblUserLogins WHERE user_id = @user_id AND " +
                "login_date = @login_date";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@user_id", UserID);
            cmd.Parameters.AddWithValue("@login_date",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    int LoginID = 0;
                    int Count = 0;
                    //update
                    while (reader.Read())
                    {
                        LoginID = reader.GetInt32(0);
                        Count = reader.GetInt32(3);
                    }
                    reader.Close();
                    reader = null;
                    if(LoginID != 0 && Count != 0)
                    {
                        //update
                        Count++;
                        string queryUpdate = "UPDATE tblUserLogins SET count = @count WHERE login_id = @login_id";
                        SqlCommand cmdUpdate = new SqlCommand(queryUpdate, _sqlConnection);
                        cmdUpdate.Parameters.AddWithValue("@count", Count);
                        cmdUpdate.Parameters.AddWithValue("@login_id", LoginID);
                        try
                        {
                            cmdUpdate.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                } else
                {
                    if(reader != null)
                    {
                        reader.Close();
                        reader = null;
                    }                    
                    //insert
                    string queryInsert = "INSERT INTO tblUserLogins (user_id, login_date,count) " +
                "VALUES(@user_id, @login_date,@count);SELECT CAST(scope_identity() AS int)";
                    SqlCommand cmd1 = new SqlCommand(queryInsert, _sqlConnection);
                    cmd1.Parameters.AddWithValue("@user_id", UserID);
                    cmd1.Parameters.AddWithValue("@login_date", DateTime.Now);
                    cmd1.Parameters.AddWithValue("@count", 1);
                    int InsertID = 0;
                    try
                    {
                        InsertID = (int)cmd1.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public DataTable GetUserLogins(string filterName = "", 
            string filterFromDate = "", string filterToDate = "")
        {
            DataTable dt = new DataTable();
            string query = "SELECT tblUserLogins.user_id,SUM(tblUserLogins.count) as count, tblUsers.email,tblUsers.first_name " +
                "FROM tblUserLogins LEFT JOIN tblUsers ON " +
                "tblUsers.user_id = tblUserLogins.user_id";
            int count = 0;
            if (filterName != "")
            {
                query = query + " WHERE tblUsers.first_name LIKE '%" + filterName + "%'";
                count++;
            }
            if (filterFromDate != "")
            {
                DateTime fDate = DateTime.Parse(filterFromDate);
                string filDate = fDate.ToString("yyyy-MM-dd hh:mm:ss.fff");
                if (count != 0)
                {
                    query = query + " AND tblUserLogins.login_date >= '" + filDate + "'";
                }
                else
                {
                    query = query + " WHERE tblUserLogins.login_date >= '" + filDate + "'";
                }
                count++;
            }
            if (filterToDate != "")
            {
                DateTime TDate = DateTime.Parse(filterToDate);
                string filTDate = TDate.ToString("yyyy-MM-dd hh:mm:ss.fff");
                if (count != 0)
                {
                    query = query + " AND tblUserLogins.login_date <= '" + filTDate + "'";
                }
                else
                {
                    query = query + " WHERE tblUserLogins.login_date <= '" + filTDate + "'";
                }
                count++;
            }
            query = query + " GROUP BY tblUserLogins.user_id,  tblUsers.email,tblUsers.first_name";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
    }
}