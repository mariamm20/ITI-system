using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using myiti;

namespace ITI_System
{
    internal class Instructor : User 
    {
        public int Salary { get; set; }
        public List<Course> Courses { get; set; }
        public List<Track> Tracks { get; set; }
        private List<Instructor> instructors { get; set; }
        private static Instructor loggedInInstructor;
        private void LoadInstructorData()
        {
            string InstructorsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\ITI-system\Database\InstructorsData.json");
            instructors = JsonConvert.DeserializeObject<List<Instructor>>(InstructorsData);
        }

        public Instructor Login(string email, string password)
        {
            if (instructors == null)
            {
                LoadInstructorData();
            }
            bool flag = false;
            Instructor account = null;
            foreach (var instructor in instructors)
            {
                if (instructor.Email == email)
                {
                    if (instructor.Password == password)
                    {
                        flag = true;
                        loggedInInstructor = instructor;
                        account =  instructor;
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

        // Get data of instructor
        public Instructor GetLoggedInInstructor()
        {
            return loggedInInstructor;
        }
        public void ViewData()
        {
            Instructor inst = GetLoggedInInstructor();
            Console.WriteLine(inst.Email);
        }


    }

}
