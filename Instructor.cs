using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using myiti;
using ITI_system;

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
            string InstructorsData = File.ReadAllText(@"D:\OneDrive - Alexandria University\Desktop\.NET\C#\OopProject2\Database\InstructorsData.json");
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
       public void ViewInformation()
        {
            Console.WriteLine("Name : " + this.Name);
            Console.WriteLine("Id : " + this.Id);
            Console.WriteLine("Email : "+this.Email);
            Console.WriteLine("Salary : " + this.Salary);
            Console.WriteLine("Specialization : " + this.Specialization);

        }
        public void ViewCoursesByInstructor(int instructorId)
        {
            

            List<Instructor> instructors = LoadData<Instructor>("InstructorsData.json");
            var instructor = instructors.FirstOrDefault(i => i.Id == instructorId);

            if (instructor != null)
            {
                Console.WriteLine($"Courses taught by instructor {instructor.Name}:");
                foreach (var course in instructor.Courses)
                {
                    Console.WriteLine($"Course Name: {course.CourseName}, Course Code: {course.CourseCode}, Track Code: {course.TrackCode}");
                }
            }
            else
            {
                Console.WriteLine($"Instructor with ID {instructorId} not found.");
            }
        }



        public void ViewTimetable( int id)
        {

           
            List<Timetable> AllTimetable = LoadData<Timetable>("TimetablesData.json"); ;

            var instructorTimetables = AllTimetable.Where(t => t.InstructorID == id).ToList();

            if (instructorTimetables.Count > 0)
            {
                Console.WriteLine("Timetable for Instructor " + this.Name + ":");
                Console.Write("Course Name" + "\t");
                Console.Write("Course Code" + "\t");

                Console.Write("Track Name " + "\t");
                Console.Write("Date" + "\t");
                Console.Write("From" + "\t");
                Console.Write("To" + "\t");
                Console.WriteLine();
                foreach (var timetable in instructorTimetables)
                {

                    Console.Write(timetable.CourseName + "\t");
                    Console.Write(timetable.CourseCode + "\t");
                    Console.Write(timetable.TrackName + "\t");
                    Console.Write(timetable.Date + "\t");
                    Console.Write(timetable.From + "\t");
                    Console.Write(timetable.To + "\t");
                    Console.WriteLine();
                }
            }
        
            }
        

        public void GiveGrade(Student student, Course course, double grade)
        {
            
            List<Grade> AllGradeData = LoadData<Grade>("GradesData.json");
            Grade existingGrade = AllGradeData.FirstOrDefault(g => g.StudentId == student.Id && g.CourseCode == course.CourseCode);
            
            List<Student> students = LoadData<Student>("StudentsData.json");
            if (students.Any(s => s.Id == student.Id && s.Courses.Any(c => c.CourseCode == course.CourseCode)))
            {

                if (existingGrade != null)
                {
                    Console.WriteLine($"Grade for student {student.Name} in course {course.CourseName} already exists , you need to edit grade ?");
                    Console.WriteLine("1- Yes ");
                    Console.WriteLine("2- No ");
                    int x = int.Parse(Console.ReadLine());
                    if (x == 1)
                    {
                        existingGrade.GradeNumber = grade;
                       
                        SaveDataToJson("GradesData.json", AllGradeData, "Edit", "Grade");
                    }
                    else if (x == 2)
                    {
                        Console.WriteLine("no change ");
                    }
                    else
                    {
                        Console.WriteLine("Invalid change");
                    }
                }
                else
                {
                    Grade newGrade = new Grade
                    {
                        StudentId = student.Id,
                        CourseCode = course.CourseCode,
                        GradeNumber = grade
                    };

                    AllGradeData.Add(newGrade);
                    
                    SaveDataToJson("GradesData.json", AllGradeData, "Add","Grade");
                }
            }
            else { Console.WriteLine("the student id is not found  in this course "); }
        }



        public void EditGrade(Student student, Course course, double grade)
        {
            
            List<Grade> AllGradeData = LoadData<Grade>("GradesData.json");
            Grade existingGrade = AllGradeData.FirstOrDefault(g => g.StudentId == student.Id && g.CourseCode == course.CourseCode);

            if (existingGrade != null)
            {
                Console.WriteLine($"Editing grade for student {student.Name} in course {course.CourseName}...");
                existingGrade.GradeNumber = grade;

               
                SaveDataToJson("GradesData.json", AllGradeData, "Edit", "Grade");

            }
            else
            {
                Console.WriteLine($"Grade for student {student.Name} in course {course.CourseName} does not exist, please add grade first");
            }
        }


        public void ViewStudents(int trackcode)
        {
            
            List<Student> students = LoadData<Student>("StudentsData.json"); ;
            {
                var filteredStudents = students.Where(s => s.Track.TrackCode == trackcode).ToList();
                Console.WriteLine($"Students taught in track :");
                foreach (var student in filteredStudents)
                {
                    Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}");
                }




            }
        }


        public void ReportStudent(int studentId, int instructorId, string reportText)
        {
           
            List<Student> students = LoadData<Student>("StudentsData.json"); 
            Student studentToAddFeedback = students.FirstOrDefault(s => s.Id == studentId);
            if (studentToAddFeedback != null)
            {
                Report report = new Report
                {
                    StudentId = studentId,
                    InstructorId = instructorId,
                    ReportText = reportText
                };
                
                List<Report> AllReportdata = LoadData<Report>("ReportData.json");
                AllReportdata.Add(report);
                SaveDataToJson("ReportData.json", AllReportdata, "Add", "Report");

            }
            else
            {
                Console.WriteLine("Student ID not found");
            }

        }


    }

}
