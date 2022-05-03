using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;

namespace CMA_Libary
{
    public class GetData
    {
        #region GetUser
        public string getUser(string username, string password)
        {
            if(username == string.Empty || password ==string.Empty)
            {
                return "1";
            }
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";
            string sql = "SELECT * FROM `users` where username=@Username AND  password=@Password";

            MySqlConnection con = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, con);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            try
            {
                con.Open();

                MySqlDataReader dr = command.ExecuteReader();

                if (dr.Read() == true)
                {
                    var postNum = dr["Postnum"];
                    if (postNum != null)
                    {
                        return postNum.ToString();

                    }
                    else
                    {
                        return "0";
                    }
                }
                else if (dr.Read() == false)
                {
                    return "0";
                }
                else
                {
                    return "0";

                }
                con.Close();
                return "0";


            }
            catch (Exception ex)
            {
                return "0";

            }

        }
        #endregion
        #region GetRole
        public string GetRole(string postNum)
        {
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";
            string sql = "SELECT * FROM `users` where Postnum=@postNum";

            MySqlConnection con = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, con);
            command.Parameters.AddWithValue("@postNum", postNum);
            try
            {
                con.Open();

                MySqlDataReader dr = command.ExecuteReader();

                if (dr.Read() == true)
                {
                    var pos = dr["Role"];
                    if (pos != null)
                    {
                        return pos.ToString();

                    }
                    else
                    {
                        return "1";
                    }
                }
                return "2";




            }
            catch (Exception ex)
            {
                return "3";

            }


        }
        #endregion
        #region GetUserName
        public string getUserName(string PostNum)
        {
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";
            string sql = "SELECT * FROM `users` where Postnum=@postNum";

            MySqlConnection con = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, con);
            command.Parameters.AddWithValue("@postNum", PostNum);
            try
            {
                con.Open();

                MySqlDataReader dr = command.ExecuteReader();

                if (dr.Read() == true)
                {
                    var pos = dr["Username"];
                    if (pos != null)
                    {
                        return pos.ToString();

                    }
                    else
                    {
                        return "1";
                    }
                }
                return "2";




            }
            catch (Exception ex)
            {
                return "3";

            }


        }
        #endregion
        #region Get Tabels
        public List<company> GetTabels()
        {
            List<company> Tablenames = new List<company>();
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
            MySqlConnection conn = new MySqlConnection(sqlC);
            conn.Open();

            using (MySqlConnection connection = new MySqlConnection(sqlC))
            {
                connection.Open();
                string query = "show tables from cma_company";
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                       var g = reader.GetString(0);
                        company comp = new company();
                        comp.Name = g;
                        string sql = $"SELECT * FROM `{g}`";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                      using(MySqlDataReader reader2 = cmd.ExecuteReader())
                        {
                            int a = 0;
                            while(reader2.Read())
                            {
                                a++;
                            }
                            var workers =a ;
                            comp.WorkerLength = workers;
                        }
                        
                           
                        
                        Tablenames.Add(comp);
                    }
                }
                return Tablenames;
            }
        }
        #endregion
        #region CreatCompany
        public void CreatCompany(string CompanyName)
        {
         var com =   CompanyName.Replace(" ", "_");

            string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
            string sql = @$"CREATE TABLE {CompanyName} (
WorkerNum VARCHAR(255) NOT NULL,
WorkerName VARCHAR(255) NOT NULL,
WorkerDate VARCHAR(255) NOT NULL,
WorkerSalary VARCHAR(255) NOT NULL,
SalaryUp VARCHAR(255) NOT NULL,
Tax VARCHAR(255) NOT NULL
)
";
            MySqlConnection connection = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();

        }
        #endregion
        #region Get Data From Company
        public List<f> GetDataFromCompany(string CompanyName)
        {
            var com = CompanyName.Replace(" ", "_");

            string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
            string sql = $"SELECT * FROM `{CompanyName}`";

            MySqlConnection con = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, con);
            con.Open();

            MySqlDataReader dt = command.ExecuteReader();
            List<f> lala = new List<f>();

            while (dt.Read())
            {
                f aa = new f();
                
                aa.WorkerNum = dt["WorkerNum"].ToString();
                aa.WorkerName = dt["WorkerName"].ToString();
                aa.WorkerSalary = dt["WorkerDate"].ToString();
                aa.SalaryUp = dt["WorkerSalary"].ToString();
                aa.Tax = dt["SalaryUp"].ToString();
                aa.WorkerDate = dt["Tax"].ToString();
                lala.Add(aa);
            }

            return lala;
        }
        #endregion
        #region add worker
        private string lastNumInMysql(string company)
        {
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
            MySqlConnection conn = new MySqlConnection(sqlC);
            string sql = $"SELECT WorkerNum FROM `{company}`";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            int sum = 0;
            while (dr.Read())
            {
                sum++;
            }
            if(sum == 0)
            {
                return "0";
            }
            else
            {
                
                dr.Close();
                string lastnum = $"SELECT * FROM `{company}` WHERE WorkerNum=(SELECT MAX(WorkerNum) FROM `{company}`);";

                MySqlCommand cmdlastname = new MySqlCommand(lastnum, conn);
                MySqlDataReader drlastname = cmdlastname.ExecuteReader();
                drlastname.Read();
                var ruslt = drlastname["WorkerNum"];
                string lastR = ruslt.ToString();
                return lastR;


            }

        }
        #endregion
        #region delete all worker
        public string DeleteAllWorker(string companyname)
        {
            var status = 0;
            var a = GetTabels();
            for(int i = 0; i < a.Count; i++)
            {
                if(a[i].Name == companyname)
                {
                    status = 1;
                }

            }
            if(status == 1)
            {
                string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
                string sql = $@"
TRUNCATE TABLE {companyname};
DELETE FROM {companyname};
COMMIT;";
                MySqlConnection con = new MySqlConnection(sqlC);
                MySqlCommand command = new MySqlCommand(sql, con);
                con.Open();
                command.ExecuteNonQuery();
                return "done";

            }
            else
            {
                return "none";
            }

        }
        public void addTocompany(string CompanyName,string workername,string workerdate,string workersalary,string salaryup,string tax)
        {

            var desc =  int.Parse(lastNumInMysql(CompanyName));
            desc++;
            string Number = desc.ToString();
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
            string sql = $"INSERT INTO `{CompanyName}`(`WorkerNum`, `WorkerName`, `WorkerDate`, `WorkerSalary`, `SalaryUp`, `Tax`) VALUES ('{Number}','{workername}','{workerdate}','{workersalary}','{salaryup}','{tax}')";

            MySqlConnection con = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, con);

            con.Open();
            command.ExecuteNonQuery();
        }
        #endregion
        #region delete Company
        public string DeleteCompany(string companyname)
        {
            var status = 0;
            var a = GetTabels();
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Name == companyname)
                {
                    status = 1;
                }

            }
            if (status==1)
            {
                string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
                string sql = $"DROP TABLE {companyname}";

                MySqlConnection con = new MySqlConnection(sqlC);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
                return "done";

            }
            else
            {
                return "none";
            }
        }
        #endregion
        #region Rename Company
        public string RenameCompany(string CompanyOldName, string CompanyNewName)
        {
            var status = 0;
            var a = GetTabels();
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Name == CompanyOldName)
                {
                    status = 1;
                }

            }
            if (status == 1)
            {
                string sqlC = "datasource=127.0.0.1;username=root;password=;database=cma_company;SslMode=none";
                string mysqlcommand = $"RENAME TABLE {CompanyOldName} TO {CompanyNewName}";
                MySqlConnection con = new MySqlConnection(sqlC);
                MySqlCommand cmd = new MySqlCommand(mysqlcommand, con);
                con.Open();
                cmd.ExecuteNonQuery();
                return "done";

            }
            else
            {
                return "none";
            }

        }
        #endregion
        #region get company info


        #endregion
        #region update
        public void update(string update,string postnum,string newupdate)
        {
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";
            string sqlA = $"SELECT * FROM `users` where Postnum={postnum}";

            string sql;
            string OldPassword="none";
            string OldUsername="none";
            string oldRole = "none";
            using (MySqlConnection cona1 = new MySqlConnection(sqlC))
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlA, cona1))
                {
                    cona1.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       OldUsername= reader["Username"].ToString();
                        OldPassword = reader["Password"].ToString();
                        oldRole = reader["Role"].ToString();
                        break;
                    }
                    cona1.Close();
                }
            }
            
            if(update == "username")
            {
                sql = $"UPDATE `users` SET `Username`='{newupdate}' WHERE Postnum={postnum}";

            }
            else
            {
                sql = $"UPDATE `users` SET `Password`='{newupdate}' WHERE Postnum={postnum}";

            }
            MySqlConnection conn = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, conn);
            conn.Open();
            command.ExecuteNonQuery();

        }
        #endregion
        #region aa
        public List<Employee> fillViewUsers()
        {
            List<Employee> Tablenames = new List<Employee>();
       ;


            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";
            MySqlConnection conn = new MySqlConnection(sqlC);
            conn.Open();

            MySqlConnection connection = new MySqlConnection(sqlC);
            
                connection.Open();
                string query = "SELECT * FROM `users`";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                

                    int a = 0;

                    while (reader.Read())
                    {
                Employee emp = new Employee();

                emp.username = reader["Username"].ToString();
                        emp.password = reader["Password"].ToString();
                        emp.role = reader["Role"].ToString();
                        Tablenames.Add(emp);
                        a++;

                    }

                return Tablenames;





            



        }
        #endregion add emp
        #region add empelwe
        public string addEmplwe(string username,string password,string role)
        {
            var codebytes = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(codebytes);
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";

            var code = BitConverter.ToString(codebytes).ToLower().Replace("-", "");
            string qury = "SELECT Username FROM `users`";
            MySqlConnection con = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(qury, con);
            con.Open();
            command.Parameters.AddWithValue("username", username);
            MySqlDataReader r = command.ExecuteReader();
            
           while (r.Read())
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    if(r[i] ==username)
                    {
                        return "1";
                    }
                }
            }

               
              
            
          

           
           
                string sql = $"INSERT INTO `users`(`Username`, `Password`, `Role`, `Postnum`) VALUES ('{password}','{username}','{role}','{code}')";
                MySqlConnection connection = new MySqlConnection(sqlC);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                return "0";

            


        }
        #endregion

        public void updUser(string username, string password,string role, string updateOld)
        {
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";
            string sql;

            string sqlA = $"SELECT * FROM `users` where Username=@olduser";


            string postnum = "none"; 
            using (MySqlConnection cona1 = new MySqlConnection(sqlC))
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlA, cona1))
                {
                    cmd.Parameters.AddWithValue("olduser", updateOld);
                    cona1.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                      
                    ;
                        postnum = reader["Postnum"].ToString();
                        break;
                    }
                    cona1.Close();
                }
            }
            sql = $"UPDATE `users` SET `Username`=@user,`Password`=@pass,`Role`=@Rol WHERE Postnum=@postnum";

            
          
            MySqlConnection conn = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sql, conn);


            conn.Open();
            command.Parameters.AddWithValue("postnum", postnum);
            command.Parameters.AddWithValue("pass", password);
            command.Parameters.AddWithValue("user", username);
            command.Parameters.AddWithValue("Rol", role);

            command.ExecuteNonQuery();

        }
        public void Delet(string username)
        {
            string sqlC = "datasource=127.0.0.1;username=root;password=;database=CMA_DB;SslMode=none";

            string sqlA = $"DELETE FROM `users` WHERE Username=@username";


           

            MySqlConnection conn = new MySqlConnection(sqlC);
            MySqlCommand command = new MySqlCommand(sqlA, conn);


            conn.Open();
            command.Parameters.AddWithValue("username", username);

            command.ExecuteNonQuery();

        }


    }

}

public class f {
        public string WorkerNum { get; set; }
        public string WorkerName { get; set; }
        public string WorkerDate { get; set; }
        public string WorkerSalary { get; set; }
        public string SalaryUp { get; set; }
        public string Tax { get; set; }
    }
    public class company {

        public  string Name { get; set; }
        public int WorkerLength { get; set; }
}
public class Employee
{
    public string username { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    
}
