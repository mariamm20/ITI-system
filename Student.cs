using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using myiti;
using ITI_System;
using ITI_system;

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
            string StudentsData = File.ReadAllText(@"D:\OneDrive - Alexandria University\Desktop\.NET\C#\OopProject2\Database\StudentsData.json");
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
                        flag = false;
                        break;
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
            Console.WriteLine($"Student ID: {stud.Id}");
            Console.WriteLine($"Name: {stud.Name}");
            Console.WriteLine($"Email: {stud.Email}");
            Console.WriteLine($"Specialization: {stud.Specialization}");
            Console.WriteLine($"Enrollment Date: {stud.EnrollmentDate}");
            Console.WriteLine($"Track: {stud.Track.TrackName}");
            Console.WriteLine("Courses:");
            foreach (var course in stud.Courses)
            {
                Console.WriteLine($"{course.CourseName} (Code: {course.CourseCode})");
            }
            Console.WriteLine();
        }

        public void ViewTimeTable(List<Timetable> timetables)
        {
            Console.WriteLine($"Time Table for {Track.TrackName} Track:");
            var studentTimetable = timetables.FindAll(t => loggedInStudent.Courses.Exists(c => c.CourseCode == t.CourseCode)).ToList();
            if (studentTimetable.Count > 0)
            {
                Console.WriteLine($"{"Course",-15}{"Date",-12}{"From",-10}{"To"}");
                foreach (var timetable in studentTimetable)
                {
                    Console.WriteLine($"{timetable.CourseName,-15}{timetable.Date,-12}{timetable.From,-10}{timetable.To}");
                }
            }
            else
            {
                Console.WriteLine("No timetable available.");
            }
            Console.WriteLine();
        }

        public void ReportInstructor(int studentId, int instructorId, string reportText)
        {

            List<Instructor> instructors = LoadData<Instructor>("InstructorsData.json");
            Instructor instructorToAddFeedback = instructors.FirstOrDefault(i => i.Id == instructorId);
            if (instructorToAddFeedback != null)
            {
                Feedback feedback = new Feedback
                {
                    StudentId = studentId,
                    InstructorId = instructorId,
                    ReportText = reportText
                };

                List<Feedback> Allfeedbackdata = LoadData<Feedback>("FeedbackData.json");
                Allfeedbackdata.Add(feedback);
                SaveDataToJson("FeedbackData.json", Allfeedbackdata, "Add", "Feedback");

            }
            else
            {
                Console.WriteLine("Instructor ID not found");
            }

        }


    }
}