using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using cumulative2.Models;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Cors;


namespace cumulative2.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teaceherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            
            List<Teacher> Teachers = new List<Teacher> { };

            
            while (ResultSet.Read())
            {
                int Id = (int)ResultSet["teacherid"];
                string TfName = ResultSet["teacherfname"].ToString();
                string TlName = ResultSet["teacherlname"].ToString();
                string Tnumber = ResultSet["employeenumber"].ToString();
                DateTime Thiredate = (DateTime)ResultSet["hiredate"];
                decimal Tsalary = (decimal)ResultSet["salary"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.Id = Id;
                NewTeacher.TfName = TfName;
                NewTeacher.TlName = TlName;
                NewTeacher.Tnumber = Tnumber;
                NewTeacher.Thiredate = Thiredate;
                NewTeacher.Tsalary = Tsalary;

                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;
        }


        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int Id = (int)ResultSet["authorid"];
                string TfName = ResultSet["teacherfname"].ToString();
                string TlName = ResultSet["teacherlname"].ToString();
                string Tnumber = ResultSet["employeenumber"].ToString();
                DateTime Thiredate = (DateTime)ResultSet["hiredate"];
                decimal Tsalary = (decimal)ResultSet["salary"];

          
                NewTeacher.Id = Id;
                NewTeacher.TfName = TfName;
                NewTeacher.TlName = TlName;
                NewTeacher.Tnumber = Tnumber;
                NewTeacher.Thiredate = Thiredate;
                NewTeacher.Tsalary = Tsalary;
            }
            Conn.Close();

            return NewTeacher;
        }


        
        [HttpPost]
        public void DeleteTeacher(int id)
        {
           
            MySqlConnection Conn = School.AccessDatabase();

          
            Conn.Open();

          
            MySqlCommand cmd = Conn.CreateCommand();

           
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

        
        [HttpPost]
        //[EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewTeacher.TfName);

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TfName,@TlName,@Tnumber, CURRENT_DATE(), @Tsalary)";
            cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.TfName);
            cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.TlName);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.Tnumber);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.Tsalary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();



        }
    }
}
