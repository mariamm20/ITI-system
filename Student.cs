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
    internal class Student : User 
    {
        public Track Track { get; set; }
        public List<Course> Courses { get; set; }
        public string EnrollmentDate { get; set; }
        private List<Student> students { get; set; }
        private static Student loggedInStudent;
        public void LoadStudentsData()
        {
            string StudentsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\ITI-system\Database\StudentsData.json");
            students = JsonConvert.DeserializeObject<List<Student>>(StudentsData);
            
        }
        public Student Login(string email, string password)
        {
            if (students == null)
            {
                LoadStudentsData();
            }
            bool flag = false;
            Student account = null;
            foreach (var student in students)
            {
                if (student.Email == email)
                {
                    if (student.Password == password)
                    {
                        flag = true; 
                        loggedInStudent = student;
                        account = student;
                        break;
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
            if (flag)
            {
                return account;
            }
            else
            {
                return null;
            }



        }
        // Get data of student
        public Student GetLoggedInStudent()
        {
            return loggedInStudent;
        }
        public void ViewData()
        {
            Student stud = GetLoggedInStudent();
            Console.WriteLine(stud.Courses);
        }
    }
}
