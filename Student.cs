using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myiti;

namespace ITI_System
{
    internal class Student : User , IUser
    {
        public Track Track { get; set; }
        public List<Course> Courses { get; set; }
        public string EnrollmentDate { get; set; }
        private List<Student> students { get; set; }

        public void LoadInstructorData()
        {
            string StudentsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\StudentsData.json");
            students = JsonConvert.DeserializeObject<List<Student>>(StudentsData);
        }
        public bool Login(string email, string password)
        {
            if (students == null)
            {
                LoadInstructorData();
            }
            bool flag = false;
            foreach (var student in students)
            {
                if (student.Email == email)
                {
                    if (student.Password == password)
                    {
                        flag = true; break;

                    }
                    else
                    {
                        flag = false; break;
                    }

                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        
    }
}
