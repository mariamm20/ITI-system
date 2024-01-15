using ITI_system;
using ITI_System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Spectre.Console;



namespace myiti
{
    internal class Admin : User
    {
        private List<Account> Accounts { get; set; }
        private List<Instructor> Instructors { get; set; }
        private List<Student> Students { get; set; }
        private List<Track> Tracks { get; set; }
        private List<Course> Courses { get; set; }
        private List<Timetable> Timetables { get; set; }
        private List<Feedback> Feedbacks { get; set; }
        private List<Report> Reports { get; set; }


        // step 1

        public Admin()
        {
            Accounts = LoadData<Account>("PendingAccounts.json");
            Instructors = LoadData<Instructor>("InstructorsData.json");
            Students = LoadData<Student>("StudentsData.json");
            Tracks = LoadData<Track>("TracksData.json");
            Courses = LoadData<Course>("CoursesData.json");
            Timetables = LoadData<Timetable>("TimetablesData.json");
            Feedbacks = LoadData<Feedback>("FeedbackData.json");
            Reports = LoadData<Report>("ReportData.json");

        }


        #region Admin Login 
        public bool Login(string email, string password)
        {

            bool flag = false;
            foreach (var instructor in Instructors)
            {
                if (instructor.Email == email)
                {
                    if (email == "ghada@gmail.com")
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
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
        #endregion

        #region View Pending accounts
        public void ViewPendingAccounts()
        {
            Console.WriteLine("Data of pending accounts");
            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Id[/]");
            table.AddColumn(new TableColumn("[grey58]Name[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Email[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Role[/]").Centered());
            foreach (var item in Accounts)
            {
                table.AddRow($"[cyan1]{item.Id}[/]", $"[cyan2]{item.Name}[/]", $"[mediumspringgreen]{item.Email}[/]", $"[springgreen2_1]{item.Role}[/]");
            }
            AnsiConsole.Write(table);

        }
        #endregion

        #region Check Existance Of Account
        public bool CheckExistanceOfAccunt(int id)
        {
            foreach (var item in Accounts)
            {
                if (item.Id == id)
                {

                    return true;
                }

            }
            return false;

        }
        #endregion

        #region Accounts approval methods for instructors and stutends 
        // Approve for account of instructor
        public string ApproveAccount(int id, int salary, List<int> trackCode, List<int> courseCode)
        {


            Account account = new Account();
            foreach (var item in Accounts)
            {
                if (item.Id == id)
                {
                    account = item; // pending

                    break;
                }

            }


            // if role is instructor
            if (account.Role == "instructor")
            {
                bool flag = true;
                foreach (var item in Instructors)
                {
                    // Validations on email if it is exists
                    if (item.Email == account.Email)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    account.Approvement = true;

                    Instructor instructor = new Instructor();

                    instructor.Id = account.Id;
                    instructor.Name = account.Name;
                    instructor.Email = account.Email;
                    instructor.Specialization = account.Specialization;
                    instructor.Password = account.Password;
                    instructor.Role = account.Role;
                    instructor.Salary = salary;
                    instructor.Tracks = new List<Track>();
                    instructor.Courses = new List<Course>();
                    foreach (var ListTracksEntered in trackCode)
                    {
                        foreach (var item in Tracks)
                        {
                            if (item.TrackCode == ListTracksEntered)
                            {

                                instructor.Tracks.Add(item);
                            }
                        }
                    }
                    foreach (var ListCoursesEntered in courseCode)
                    {
                        foreach (var item2 in Courses)
                        {
                            if (item2.CourseCode == ListCoursesEntered)
                            {
                                instructor.Courses.Add(item2);
                            }
                        }
                    }
                    instructor.Approvement = account.Approvement;


                    Instructors.Add(instructor);

                    SaveDataToJson("InstructorsData.json", Instructors, "add", "Instructor");

                    Accounts.Remove(account);
                    SaveDataToJson("PendingAccounts.json", Accounts, "remov", "Instructor pending account");

                    return "Addition process Successed";
                }
                else
                {
                    return ("Email of instructor already exists !!");
                }

            }
            else
            {
                return ("Invalid Input of Data");
            }


        }
        // Approve for account of student
        public void ApproveAccount(int id, int trackCode)
        {
            Account account = new Account();
            foreach (var item in Accounts)
            {
                if (item.Id == id)
                {
                    account = item; // pending
                }
            }


            if (account.Role == "student")
            {
                bool flag = true;
                foreach (var item in Students)
                {
                    // Validations on email if it is exists
                    if (item.Email == account.Email)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    account.Approvement = true;

                    Student student = new Student();

                    student.Id = account.Id;
                    student.Name = account.Name;
                    student.Email = account.Email;
                    student.Password = account.Password;
                    student.Role = account.Role;
                    student.Courses = new List<Course>();

                    foreach (var item in Tracks)
                    {
                        if (item.TrackCode == trackCode)
                        {
                            student.Track = item;
                            foreach (var item2 in Courses)
                            {
                                if (item2.TrackCode == item.TrackCode)
                                {
                                    student.Courses.Add(item2);
                                }
                            }
                        }
                    }
                    //student.Courses = courses;

                    student.Specialization = account.Specialization;
                    student.EnrollmentDate = DateTime.Now.ToString("MM/dd/yyyy");
                    student.Approvement = account.Approvement;
                    // Add instructor data to previous data in list 
                    Students.Add(student);

                    SaveDataToJson("StudentsData.json", Students, "add", "Student");

                    //Remove data from pending accounts
                    Accounts.Remove(account);
                    SaveDataToJson("PendingAccounts.json", Accounts, "remov", "Student pending account");

                }
                else
                {
                    Console.WriteLine("Email of student already exists !!");
                }

            }
            else
            {
                Console.WriteLine("Invalid Input of Data");
            }


        }
        #endregion

        #region Instructor CRUD operation (Done) 

        public bool AddInstructor(int id, string name, string email, string password, string specilization, int salary, List<int> tracksCode, List<int> coursesCode)
        {
            bool flag = false;
            if (id > 100 && id < 1000 && name.Contains(' ') && email.Contains('@') && email.Contains('.') && password.Length >= 8 && specilization != null && salary != null && tracksCode != null && coursesCode != null)
            {

                foreach (var item in Instructors)
                {
                    if (item.Id == id || item.Email == email)
                    {
                        flag = false; break;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    Instructor instructor = new Instructor();
                    instructor.Id = id;
                    instructor.Name = name;
                    instructor.Email = email;
                    instructor.Password = password;
                    instructor.Salary = salary;
                    instructor.Specialization = specilization;
                    instructor.Role = "instructor";
                    instructor.Tracks = new List<Track>();
                    instructor.Courses = new List<Course>();
                    foreach (var ListTracksEntered in tracksCode)//102,110
                    {
                        foreach (var item in Tracks)
                        {
                            if (item.TrackCode == ListTracksEntered)
                            {
                                instructor.Tracks.Add(item);
                            }
                        }
                    }
                    foreach (var ListCoursesEntered in coursesCode)
                    {
                        foreach (var item in Courses)
                        {
                            if (item.CourseCode == ListCoursesEntered)
                            {
                                instructor.Courses.Add(item);
                            }
                        }
                    }
                    instructor.Approvement = true;
                    Instructors.Add(instructor);
                    SaveDataToJson("InstructorsData.json", Instructors, "add", "Instructor");

                }
            }
            else
            {
                return flag;
            }
            return flag;
        }

        public void ViewInstructors()
        {
            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Instructor Id[/]");
            table.AddColumn(new TableColumn("[grey58]Instructor Name[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Email[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Password[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Specialization[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Salary[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Tracks[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Courses[/]").Centered());

            foreach (var instructor in Instructors)
            {
                string courses = "";
                string tracks = "";
                foreach (var track in instructor.Tracks)
                {
                    tracks = track.TrackName + " ";
                }
                foreach (var course in instructor.Courses)
                {
                    courses= course.CourseName + " ";
                }
                table.AddRow($"[cyan1]{instructor.Id}[/]", $"[cyan2]{instructor.Name}[/]", $"[mediumspringgreen]{instructor.Email}[/]", $"[springgreen2_1]{instructor.Password}[/]", $"[green1]{instructor.Specialization}[/]", $"[lightgreen_1]{instructor.Salary}[/]", $"[lightgreen]{tracks}[/]", $"[lightgreen]{courses}[/]");
            }
            AnsiConsole.Write(table);
        }

        public bool ViewSpecificInstructor(int id)
        {
            bool flag = false;
            foreach (var instructor in Instructors)
            {
                if (instructor.Id == id)
                {
                    string[] instructorDataToEdit = { "ID", "Name", "Email", "Password", "Specilization", "Salary", "Track Name", "Track code", "Course Name", "Course Code" };

                    Console.WriteLine($"Data of Instructor of id {id}");
                    Console.WriteLine("--------------------------------");
                    for (int i = 0; i < instructorDataToEdit.Length; i++)
                    {
                        Console.Write(instructorDataToEdit[i] + "\t");
                    }
                    Console.WriteLine();

                    Console.Write(instructor.Id + "\t");
                    Console.Write(instructor.Name + "\t");
                    Console.Write(instructor.Email + "\t");
                    Console.Write(instructor.Password + "\t");
                    Console.Write(instructor.Specialization + "\t");
                    Console.Write(instructor.Salary + "\t");
                    foreach (var track in instructor.Tracks)
                    {
                        Console.Write(track.TrackName + " ");
                        Console.WriteLine(track.TrackCode + " ");
                    }
                    foreach (var course in instructor.Courses)
                    {
                        Console.Write(course.CourseName + " ");
                        Console.WriteLine(course.CourseCode + " ");
                    }

                    Console.WriteLine();
                    flag = true;
                    break;
                }

            }
            return flag;
        }

        public bool EditInstructor<T>(int EditChoice, int id, T value)
        {
            bool flag = false;
            if (value != null && id != null)
            {

                foreach (var item in Instructors)
                {
                    if (item.Id == id)
                    {
                        switch (EditChoice)
                        {
                            case 1:
                                item.Id = Convert.ToInt32(value);
                                SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                flag = true;

                                break;
                            case 2:
                                item.Name = Convert.ToString(value);
                                SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                flag = true;
                                break;
                            case 3:
                                item.Email = Convert.ToString(value);
                                SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                flag = true;
                                break;
                            case 4:
                                item.Password = Convert.ToString(value);
                                SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                flag = true;
                                break;
                            case 5:
                                item.Specialization = Convert.ToString(value);
                                SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                flag = true;
                                break;
                            case 6:
                                item.Salary = Convert.ToInt32(value);
                                SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                flag = true;
                                break;
                            case 7:
                                if (value is List<int> tracksToAdd)//101,102,103
                                {
                                    foreach (var ListTracksEntered in tracksToAdd)//101
                                    {
                                        foreach (var item2 in Tracks)
                                        {
                                            if (item2.TrackCode == ListTracksEntered)
                                            {
                                                if (!item.Tracks.Any(instructorTrack => instructorTrack.TrackCode == ListTracksEntered))
                                                {
                                                    item.Tracks.Add(item2);

                                                    SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");

                                                    flag = true;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Track Code Already exists");
                                                    flag = false;
                                                }
                                                break;
                                            }
                                        }
                                    }

                                }
                                break;
                            case 8:
                                if (value is List<int> tracksToRemove)//101,102,103
                                {
                                    foreach (var ListTracksEntered in tracksToRemove)//101
                                    {
                                        item.Tracks.RemoveAll(track => track.TrackCode == ListTracksEntered);
                                        item.Courses.RemoveAll(course => course.TrackCode == ListTracksEntered);


                                    }
                                    SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                    flag = true;
                                }
                                break;
                            case 9:
                                if (value is List<int> coursesToAdd)
                                {
                                    foreach (var ListCoursesEntered in coursesToAdd)//101
                                    {
                                        foreach (var item2 in Courses)
                                        {
                                            if (item2.CourseCode == ListCoursesEntered)
                                            {
                                                if (!item.Courses.Any(instructorCourse => instructorCourse.CourseCode == ListCoursesEntered))
                                                {
                                                    item.Courses.Add(item2);
                                                    SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                                    flag = true;

                                                }
                                                else
                                                {
                                                    Console.WriteLine("Course Code Already exists");
                                                    flag = false;
                                                }
                                                break;
                                            }
                                        }
                                    }


                                }
                                break;
                            case 10:
                                if (value is List<int> coursesToRemove)
                                {
                                    foreach (var ListCoursesEntered in coursesToRemove)//101
                                    {
                                        item.Courses.RemoveAll(course => course.CourseCode == ListCoursesEntered);
                                    }

                                    SaveDataToJson("InstructorsData.json", Instructors, "edit", "Instructor");
                                    flag = true;
                                }
                                break;
                            default:
                                break;



                        }

                        break;

                    }

                }

            }
            else
            {
                return flag;
            }
            return flag;
        }

        public bool DeleteInstructor(int id)
        {
            bool flag = false;
            foreach (var item in Instructors)
            {
                if (item.Id == id)
                {
                    Instructors.Remove(item);
                    SaveDataToJson("InstructorsData.json", Instructors, "delet", "Instructor");
                    flag = true;
                    break;
                }

            }
            return flag;
        }

        #endregion

        #region Student CRUD operations (Done) 

        public bool AddStudent(int id, string name, string email, string password, string specilization, string enrollmentdate, int trackCode)
        {
            bool flag = false;
            if (id > 100 && id < 1000 && name.Contains(' ') && email.Contains('@') && email.Contains('.') && password.Length >= 8 && specilization != null && enrollmentdate != null && trackCode != null)
            {

                foreach (var item in Students)
                {
                    if (item.Id == id || item.Email == email)
                    {
                        flag = false; break;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    Student student = new Student();
                    student.Id = id;
                    student.Name = name;
                    student.Email = email;
                    student.Password = password;
                    student.EnrollmentDate = DateTime.Now.ToString("MM/dd/yyyy"); ;
                    student.Specialization = specilization;
                    student.Role = "instructor";
                    student.Courses = new List<Course>();

                    foreach (var item in Tracks)
                    {
                        if (item.TrackCode == trackCode)
                        {
                            student.Track = item;
                            foreach (var item2 in Courses)
                            {
                                if (item2.TrackCode == item.TrackCode)
                                {
                                    student.Courses.Add(item2);
                                }
                            }
                        }
                    }

                    student.Approvement = true;
                    Students.Add(student);
                    SaveDataToJson("StudentsData.json", Students, "add", "student");

                }
            }
            else
            {
                return flag;
            }
            return flag;
        }

        public void ViewStudents()
        {
            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Student Id[/]");
            table.AddColumn(new TableColumn("[grey58]Student Name[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Email[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Password[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Specialization[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Track Name[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Enrollment Date[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Courses[/]").Centered());

            foreach (var student in Students)
            {
                string courses = "";

                foreach (var course in student.Courses)
                {
                    courses+= course.CourseName + " ";
                }
                table.AddRow($"[cyan1]{student.Id}[/]", $"[cyan2]{student.Name}[/]", $"[mediumspringgreen]{student.Email}[/]", $"[springgreen2_1]{student.Password}[/]", $"[green1]{student.Specialization}[/]", $"[lightgreen_1]{student.Track.TrackName}[/]", $"[lightgreen]{student.EnrollmentDate}[/]", $"[lightgreen]{courses}[/]");
            }
            AnsiConsole.Write(table);

        }

        public bool ViewSpecificStudent(int id)
        {
            bool flag = false;
            foreach (var student in Students)
            {
                if (student.Id == id)
                {
                    string[] studentDataToEdit = { "ID", "Name", "Email", "Password", "Specilization", "Track Name", "Enrollment Date", "Courses" };

                    Console.WriteLine();
                    Console.WriteLine($"Data of Student of id {id}");
                    Console.WriteLine("--------------------------------");
                    for (int i = 0; i < studentDataToEdit.Length; i++)
                    {
                        Console.Write(studentDataToEdit[i] + "\t");
                    }
                    Console.WriteLine();
                    Console.Write(student.Id + "\t");
                    Console.Write(student.Name + "\t");
                    Console.Write(student.Email + "\t");
                    Console.Write(student.Password + "\t");
                    Console.Write(student.Specialization + "\t");
                    Console.Write(student.Track.TrackName + "\t");
                    Console.Write(student.EnrollmentDate + "\t");

                    foreach (var course in student.Courses)
                    {
                        Console.Write(course.CourseName + " ");
                    }

                    Console.WriteLine();
                    flag = true;
                    break;
                }

            }
            return flag;
        }

        public bool EditStudent<T>(int EditChoice, int id, T value)
        {
            bool flag = false;
            if (value != null && id != null)
            {

                foreach (var item in Students)
                {
                    if (item.Id == id)
                    {
                        switch (EditChoice)
                        {
                            case 1:
                                item.Id = Convert.ToInt32(value);
                                SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                flag = true;

                                break;
                            case 2:
                                item.Name = Convert.ToString(value);
                                SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                flag = true;
                                break;
                            case 3:
                                item.Email = Convert.ToString(value);
                                SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                flag = true;
                                break;
                            case 4:
                                item.Password = Convert.ToString(value);
                                SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                flag = true;
                                break;
                            case 5:
                                item.Specialization = Convert.ToString(value);
                                SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                flag = true;
                                break;
                            case 6:
                                int trackCode = Convert.ToInt32(value);
                                item.Track = null;
                                item.Courses.Clear();
                                foreach (var track in Tracks)
                                {

                                    item.Track = track;

                                    foreach (var item2 in Courses)
                                    {
                                        if (item2.TrackCode == item.Track.TrackCode)
                                        {
                                            item.Courses.Add(item2);
                                            SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                            flag = true;
                                        }
                                    }

                                    break;
                                }


                                break;
                            case 7:
                                item.EnrollmentDate = Convert.ToString(value);
                                SaveDataToJson("StudentsData.json", Students, "edit", "student");
                                flag = true;
                                break;

                            default:
                                break;

                        }

                        break;

                    }

                }

            }
            else
            {
                return flag;
            }
            return flag;
        }

        public bool DeleteStudent(int id)
        {
            bool flag = false;
            foreach (var item in Students)
            {
                if (item.Id == id)
                {
                    Students.Remove(item);
                    SaveDataToJson("StudentsData.json", Students, "delet", "student");
                    flag = true;
                    break;
                }

            }
            return flag;
        }

        #endregion

        #region Track CRUD operations (Done) 
        public void ViewTracks()
        {
            var table = new Table();
            table.AddColumn("[grey58]Track Name[/]");
            table.AddColumn(new TableColumn("[grey58]Track Code[/]").Centered());
            foreach (var item in Tracks)
            {
                table.AddRow($"[cyan1]{item.TrackName}[/]", $"[cyan2]{item.TrackCode}[/]");
            }
            AnsiConsole.Write(table);
        }
        public bool ViewSpecificTrack(int trackCode)
        {
            bool flag = false;
            foreach (var track in Tracks)
            {
                if (track.TrackCode == trackCode)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Data of Track of code {trackCode}");
                    Console.WriteLine("--------------------------------");
                    Console.Write("Track Name" + "\t");
                    Console.Write("Track Code" + "\t");
                    Console.WriteLine();
                    Console.Write(track.TrackName + "\t");
                    Console.Write(track.TrackCode + "\t");

                    Console.WriteLine();
                    flag = true;
                    break;
                }

            }
            return flag;
        }

        public bool EditTrack<T>(int EditChoice, int trackCode, T value)
        {
            bool flag = false;
            if (value != null && trackCode != null)
            {

                foreach (var item in Tracks)
                {
                    if (item.TrackCode == trackCode)
                    {
                        switch (EditChoice)
                        {
                            case 1:
                                item.TrackName = Convert.ToString(value);
                                SaveDataToJson("TracksData.json", Tracks, "edit", "track");
                                flag = true;
                                break;
                            case 2:
                                item.TrackCode = Convert.ToInt32(value);
                                SaveDataToJson("TracksData.json", Tracks, "edit", "track");
                                flag = true;

                                break;
                            default:
                                flag = false;
                                break;

                        }
                    }
                }
            }
            return flag;
        }
        public bool AddTrack(int TrackCode, string TrackName)
        {
            bool flag = true;
            foreach (var item in Tracks)
            {
                if (item.TrackCode == TrackCode)
                {
                    flag = false; break;
                }
            }
            if (flag)
            {
                Track track = new Track();
                track.TrackCode = TrackCode;
                track.TrackName = TrackName;
                Tracks.Add(track);
                SaveDataToJson("TracksData.json", Tracks, "Add", "Track");
            }

            return flag;
        }

        public bool DeleteTrack(int TrackCode)
        {
            bool flag = false;
            foreach (var item in Tracks.ToList())
            {
                if (item.TrackCode == TrackCode)
                {
                    Tracks.Remove(item);
                    flag = true;
                    SaveDataToJson("TracksData.json", Tracks, "delet", "Track");
                }
            }
            return flag;
        }





        #endregion


        #region Course CRUD operations 
        public void ViewCourses()
        {
            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Course Name[/]");
            table.AddColumn(new TableColumn("[grey58]Course Code[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Track Code[/]").Centered());
            foreach (var item in Courses)
            {
                
                table.AddRow($"[cyan1]{item.CourseName}[/]", $"[cyan2]{item.CourseCode}[/]", $"[mediumspringgreen]{item.TrackCode}[/]");

            }
            AnsiConsole.Write(table);
        }
        public bool ViewSpecificCourse(int CourseCode)
        {
            bool flag = false;
            foreach (var Course in Courses)
            {
                if (Course.CourseCode == CourseCode)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Data of Course of code {CourseCode}");
                    Console.WriteLine("--------------------------------");
                    Console.Write("Course Name" + "\t");
                    Console.Write("Course Code" + "\t");
                    Console.Write("Track Code" + "\t");
                    Console.WriteLine();
                    Console.Write(Course.CourseName + "\t");
                    Console.Write(Course.CourseCode + "\t");
                    Console.Write(Course.TrackCode + "\t");
                    Console.WriteLine();
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        public bool EditCourse<T>(int EditChoice, int CourseCode, T value)
        {
            bool flag = false;
            if (value != null && CourseCode != null)
            {
                foreach (var item in Courses)
                {
                    if (item.CourseCode == CourseCode)
                    {
                        switch (EditChoice)
                        {
                            case 1:
                                item.CourseName = Convert.ToString(value);
                                SaveDataToJson("CoursesData.json", Courses, "edit", "Courses Name");
                                flag = true;
                                break;
                            case 2:
                                item.CourseCode = Convert.ToInt32(value);
                                SaveDataToJson("CoursesData.json", Courses, "edit", "Courses Code");
                                flag = true;
                                break;
                            case 3:
                                foreach (var Track in Tracks)
                                {
                                    if (Convert.ToInt32(value) == Track.TrackCode)
                                    {
                                        item.TrackCode = Convert.ToInt32(value);
                                        SaveDataToJson("CoursesData.json", Courses, "edit", "Track Code");
                                        flag = true;
                                        break;
                                    }
                                }
                                break;
                            default:
                                flag = false;
                                break;
                        }
                    }
                }
            }
            return flag;
        }
        public bool AddCourse (int CourseCode, string CourseName ,int TrackCode)
        {
            bool flag = true;
            foreach (var item in Courses)
            {
                if (item.CourseCode == CourseCode)
                {
                    flag = false; 
                    break;
                }
            }
            if (flag)
            {
                Course course = new Course();
                course.CourseCode = CourseCode;
                course.CourseName = CourseName;
                foreach (var Track in Tracks)
                {
                    if (TrackCode == Track.TrackCode)
                    {
                        course.TrackCode = TrackCode;
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                Courses.Add(course);
                SaveDataToJson("CoursesData.json",Courses, "Add", "Course");
            }

            return flag;
        }
        public bool DeleteCourse(int CourseCode)
        {
            bool flag = false;
            foreach (var item in Courses.ToList())
            {
                if (item.CourseCode == CourseCode)
                {
                    Courses.Remove(item);
                    flag = true;
                    SaveDataToJson("CoursesData.json", Courses, "delet", "Course");
                    break;
                }
            }
            return flag;
        }
        #endregion

        #region  TimeTable CRUD operations
        public void ViewTimeTable()
        {            

            var table = new Table();

            // Add some columns
            table.AddColumn("[grey58]Course Name[/]");
            table.AddColumn(new TableColumn("[grey58]Course Code[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Track Name[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Instructor Name[/]").Centered());
            table.AddColumn(new TableColumn("[grey58]Date[/]").Centered());
            table.AddColumn(new TableColumn("From").Centered());
            table.AddColumn(new TableColumn("To").Centered());

            foreach (var timetable in Timetables)
            {
                table.AddRow($"[cyan1]{timetable.CourseName}[/]", $"[cyan2]{timetable.CourseCode}[/]",$"[mediumspringgreen]{ timetable.TrackName}[/]",$"[springgreen2_1]{timetable.InstructorName}[/]", $"[green1]{timetable.Date}[/]", $"[red]{timetable.From}[/]", $"[red]{timetable.To}[/]");

                // Write centered cell grid contents to Console
            }
            AnsiConsole.Write(table);
        }
        public bool ViewSpecificTimeTable(int CourseCode,int InstructorID)
        {
            bool flag = false;
            foreach (var timetable in Timetables)
            {
                if (timetable.CourseCode == CourseCode && timetable.InstructorID == InstructorID)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Data of TimeTable for CourseCode = {CourseCode} & InstructorID = {InstructorID}");
                    Console.WriteLine("--------------------------------");
                    Console.Write("Course Name" + "\t");
                    Console.Write("Course Code" + "\t");
                    Console.Write("Track Name" + "\t");
                    Console.Write("Instructor Name" + "\t");
                    Console.Write("Instructor ID" + "\t");
                    Console.Write("Date" + "\t");
                    Console.Write("From" + "\t");
                    Console.Write("To" + "\t");
                    Console.WriteLine();
                    Console.Write(timetable.CourseName + "\t");
                    Console.Write(timetable.CourseCode + "\t");
                    Console.Write(timetable.TrackName + "\t");
                    Console.Write(timetable.InstructorName + "\t");
                    Console.Write(timetable.InstructorID + "\t");
                    Console.Write(timetable.Date + "\t");
                    Console.Write(timetable.From + "\t");
                    Console.Write(timetable.To + "\t");
                    Console.WriteLine();
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        public bool  SetTimetable(int CourseCode,int InstructorID,string Date,string From,string To)
        {
            bool flag = false;
            foreach (var instructor in Instructors)
            {
                if (instructor.Id == InstructorID)
                {
                    for (int  i = 0; i < instructor.Courses.Count; i++)
                    {
                        if (instructor.Courses[i].CourseCode == CourseCode)
                        {
                            Timetable timetable = new Timetable();
                            foreach (var course in Courses)
                            {
                                if (course.CourseCode == CourseCode)
                                {
                                    timetable.CourseCode = CourseCode;
                                    timetable.CourseName = course.CourseName;
                                }
                            }
                            foreach (var course in Courses)
                            {
                                if (course.CourseCode == CourseCode)
                                {
                                    foreach (var track in Tracks)
                                    {
                                        if (track.TrackCode == course.TrackCode)
                                        {
                                            timetable.TrackName = track.TrackName;
                                        }
                                    }
                                }
                            }
                            foreach (var inst in Instructors)
                            {
                                if (inst.Id == InstructorID)
                                {
                                    timetable.InstructorID = InstructorID;
                                    timetable.InstructorName = inst.Name;
                                }
                            }
                            timetable.Date = Convert.ToDateTime(Date).ToString("dd/MM/yyyy");
                            timetable.From = Convert.ToDateTime(From).ToString("hh:mm tt");
                            timetable.To = Convert.ToDateTime(To).ToString("hh:mm tt");
                            Timetables.Add(timetable);
                            SaveDataToJson("TimetablesData.json", Timetables, "Add", "TimeTable");
                            flag = true;
                        }
                    }
                }
            }
            
            return flag;
        }
        public bool EditTimeTable<T>(string EditChoice,int InstructorID, int CourseCode, T value)
        {
            bool flag = false;
            if (value != null && CourseCode != null && InstructorID != null)
            {
                foreach (var timetable in Timetables)
                {
                    if (timetable.InstructorID == InstructorID && timetable.CourseCode == CourseCode)
                    {
                        switch (EditChoice)
                        {
                            case "1. Course code":
                                foreach (var course in Courses)
                                {
                                    if (Convert.ToInt32(value) == course.CourseCode)
                                    {
                                        timetable.CourseCode = Convert.ToInt32(value);
                                        timetable.CourseName = course.CourseName;
                                        SaveDataToJson("TimetablesData.json",Timetables, "edit", "Course Name & Course Id ");
                                        flag = true;
                                        break;
                                    }
                                }
                                break;
                            case "2. Instructor ID":
                                foreach (var instructor in Instructors)
                                {
                                    if (Convert.ToInt32(value) == instructor.Id)
                                    {
                                        timetable.InstructorID = Convert.ToInt32(value);
                                        timetable.InstructorName = instructor.Name;
                                        SaveDataToJson("TimetablesData.json", Timetables, "edit", "Instructor Name & Course Id ");
                                        flag = true;
                                        break;
                                    }
                                }
                                break;
                            case "3. Track Name":
                                foreach (var track in Tracks)
                                {
                                    if (Convert.ToString(value) == track.TrackName)
                                    {
                                        timetable.TrackName = Convert.ToString(value);
                                        SaveDataToJson("TimetablesData.json", Timetables, "edit", "Track Name ");
                                        flag = true;
                                        break;
                                    }
                                }
                                break;
                            case "4. Date":
                                timetable.Date = Convert.ToDateTime(value).ToString("dd/MM/yyyy");
                                SaveDataToJson("TimetablesData.json", Timetables, "edit", "Lecture Date ");
                                flag = true;
                                break;
                            case "5. From":
                                timetable.From = Convert.ToDateTime(value).ToString("hh:mm tt");
                                SaveDataToJson("TimetablesData.json", Timetables, "edit", "Lecture Start Time ");
                                flag = true;
                                break;
                            case "6. To":
                                timetable.To = Convert.ToDateTime(value).ToString("hh:mm tt");
                                SaveDataToJson("TimetablesData.json", Timetables, "edit", "Lecture End Time ");
                                flag = true;
                                break;
                            default:
                                flag = false;
                                break;
                        }
                    }
                }
            }
            return flag;
        }
        public bool DeleteTimetable(int InstructorID, int CourseCode)
        {
            bool flag = false;
            if ( CourseCode != null && InstructorID != null)
            {
                foreach (var timetable in Timetables.ToList())
                {
                    if (timetable.CourseCode == CourseCode && timetable.InstructorID == InstructorID)
                    {
                        Timetables.Remove(timetable);
                        SaveDataToJson("TimetablesData.json", Timetables, "delet", "TimeTable");
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
        #endregion

        #region  Feedback CRUD operations
        public bool ViewFeedBack( int InstructorID)
        {
            bool flag = false;
            var table = new Table();
            table.AddColumn("[grey58]Student ID[/]");
            table.AddColumn(new TableColumn("[grey58]Feedback Content[/]").Centered());
            Console.WriteLine($"Feedbacks for Instructor ID= {InstructorID}");
            foreach (var feedback in Feedbacks)
            {
                
                if (feedback.InstructorId == InstructorID)
                {
                    table.AddRow($"[cyan1]{feedback.StudentId}[/]", $"[cyan2]{feedback.ReportText}[/]");
 
                    flag = true;
                    
                }
            }
            AnsiConsole.Write(table);
            return flag;
        }
        public bool ViewReport(int StudentId)
        {
            bool flag = false;
            var table = new Table();
            table.AddColumn("[grey58]Instructor ID[/]");
            table.AddColumn(new TableColumn("[grey58]Report Content[/]").Centered());
            Console.WriteLine($"Reports for Student ID= {StudentId}");
            foreach (var report in Reports)
            {
                if (report.StudentId == StudentId)
                {
                    table.AddRow($"[cyan1]{report.InstructorId}[/]", $"[cyan2]{report.ReportText}[/]");
                    flag = true;
                    
                }
            }
            AnsiConsole.Write(table);
            return flag;
        }
        #endregion





    }
}
