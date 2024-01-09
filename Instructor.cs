﻿using System;
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
    internal class Instructor : User , IUser
    {
        public int Salary { get; set; }
        public List<Course> Courses { get; set; }
        public List<Track> Tracks { get; set; }
        private List<Instructor> instructors { get; set; }

        private void LoadInstructorData()
        {
            string InstructorsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\InstructorsData.json");
            instructors = JsonConvert.DeserializeObject<List<Instructor>>(InstructorsData);
        }

        public bool Login(string email, string password)
        {
            if (instructors == null)
            {
                LoadInstructorData();
            }
            bool flag = false;
            foreach (var instructor in instructors)
            {
                if (instructor.Email == email)
                {
                    if (instructor.Password == password)
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
        
        // rest of functions 
       


    }

}