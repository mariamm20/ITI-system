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
using Spectre.Console;
using System.Xml;

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
            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Email[/]");
            table.AddRow($"[cyan1]{inst.Email}[/]");
            AnsiConsole.Write(table);


        }
       public void ViewInformation()
        {
            
            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Name[/]");
            table.AddColumn(new TableColumn("[grey58]Id[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Email[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Salary[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Specialization[/]").Centered());
            table.AddRow($"[cyan1]{this.Name}[/]", $"[cyan2]{this.Id}[/]", $"[mediumspringgreen]{this.Email}[/]", $"[springgreen2_1]{this.Salary}[/]", $"[green1]{this.Specialization}[/]");
            AnsiConsole.Write(table);


        }
        public void ViewCoursesByInstructor(int instructorId)
        {
            

            List<Instructor> instructors = LoadData<Instructor>("InstructorsData.json");
            var instructor = instructors.FirstOrDefault(i => i.Id == instructorId);

            if (instructor != null)
            {
                Console.WriteLine($"Courses taught by instructor {instructor.Name}:");
                var table = new Table();

                // Add some columns
                table.AddColumn("[grey58]Course Name[/]");
                table.AddColumn(new TableColumn("[grey58]Course Code[/]").Centered());
                table.AddColumn(new TableColumn("[grey58]Track Code[/]").Centered());
               

                foreach (var course in instructor.Courses)
                {
                    table.AddRow($"[cyan1]{course.CourseName}[/]", $"[cyan2]{course.CourseCode}[/]", $"[mediumspringgreen]{course.TrackCode}[/]");
                }
                AnsiConsole.Write (table);
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
                var table = new Table();

                // Add some columns
                table.AddColumn("[grey58]Course Name[/]");
                table.AddColumn(new TableColumn("[grey58]Course Code[/]").Centered());
                table.AddColumn(new TableColumn("[grey58]Track Name[/]").Centered());
                table.AddColumn(new TableColumn("[grey58]Instructor Name[/]").Centered());
                table.AddColumn(new TableColumn("[grey58]Date[/]").Centered());
                table.AddColumn(new TableColumn("From").Centered());
                table.AddColumn(new TableColumn("To").Centered());
                foreach (var timetable in instructorTimetables)
                {
                    table.AddRow($"[cyan1]{timetable.CourseName}[/]", $"[cyan2]{timetable.CourseCode}[/]", $"[mediumspringgreen]{timetable.TrackName}[/]", $"[springgreen2_1]{timetable.InstructorName}[/]", $"[green1]{timetable.Date}[/]", $"[red]{timetable.From}[/]", $"[red]{timetable.To}[/]");
                }
                AnsiConsole.Write(table);
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
                var table = new Table();

                // Add some columns
                table.AddColumn("[grey58]Student Id[/]");
                table.AddColumn(new TableColumn("[grey58]Student Name[/]").Centered());
                var filteredStudents = students.Where(s => s.Track.TrackCode == trackcode).ToList();
                Console.WriteLine($"Students taught in track :");
                foreach (var student in filteredStudents)
                {
                    table.AddRow($"[cyan1]{student.Id}[/]", $"[cyan2]{student.Name}[/]");

                }
                AnsiConsole.Write(table);




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
