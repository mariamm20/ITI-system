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

        public void ViewTimeTable()
        {
            List<Timetable> timetables = LoadData<Timetable>("TimetablesData.json");
            Console.WriteLine($"Time Table for {Track.TrackName} Track:");
            
            List<Timetable> studentTimetable = new List<Timetable>();

            foreach (var table in timetables)
            {
                foreach(var course in loggedInStudent.Courses)
                {
                    if(table.CourseCode == course.CourseCode)
                    {
                        studentTimetable.Add(table); 
                    }
                }
            }
  

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
        public void ViewGrade()
        {
            List<Grade> studentGrade = LoadData<Grade>("GradesData.json");

            foreach(var grade in studentGrade)
            {
                if(grade.StudentId == loggedInStudent.Id)
                {
                    foreach(var course in loggedInStudent.Courses)
                    {
                        if(course.CourseCode == grade.CourseCode)
                        {
                            Console.WriteLine(course.CourseName + "\t");
                            Console.WriteLine(grade.GradeNumber);

                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("No Available Grades");
                }
            }
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