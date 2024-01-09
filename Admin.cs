using ITI_System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace myiti
{
    internal class Admin : User
    {
        private List<Account> Accounts { get; set; }
        private List<Instructor> Instructors { get; set; }
        private List<Student> Students { get; set; }
        private List<Track> Tracks { get; set; }
        private List<Course> Courses { get; set; }
        // step 1

        public Admin()
        {
            LoadPendinAccountsData();
            LoadInstructorsData();
            LoadStudentsData();
            LoadTracksData();
            LoadCoursesData();
            // step 3
        }


        #region Loading Data from json to list
        private List<Account> LoadPendinAccountsData()
        {
            string PendingAccountsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\PendingAccounts.json");
            Accounts = JsonConvert.DeserializeObject<List<Account>>(PendingAccountsData);
            return Accounts;
        }

        private List<Instructor> LoadInstructorsData()
        {
            string InstructorsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\InstructorsData.json");
            Instructors = JsonConvert.DeserializeObject<List<Instructor>>(InstructorsData);
            return Instructors;
        }

        private List<Student> LoadStudentsData()
        {
            string StudentsData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\StudentsData.json");
            Students = JsonConvert.DeserializeObject<List<Student>>(StudentsData);
            return Students;
        }

        private List<Track> LoadTracksData()
        {
            string TracksData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\TracksData.json");
            Tracks = JsonConvert.DeserializeObject<List<Track>>(TracksData);
            return Tracks;
        }
        private List<Course> LoadCoursesData()
        {
            string CoursesData = File.ReadAllText(@"C:\Users\Lenov\source\repos\myiti\Database\CoursesData.json");
            Courses = JsonConvert.DeserializeObject<List<Course>>(CoursesData);
            return Courses;
        }

        // step 2
        #endregion

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
            Console.WriteLine("--------------------------");

            foreach (var item in Accounts)
            {
                Console.Write(item.Id + "\t\t");
                Console.Write(item.Name + "\t\t");
                Console.Write(item.Email + "\t\t");
                Console.Write(item.Role + "\t\t");
                Console.WriteLine();
            }

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

            foreach (var instructor in Instructors)
            {
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
            }
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
                                        item.Courses.RemoveAll(course =>  course.TrackCode == ListTracksEntered);
                                        

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

            foreach (var student in Students)
            {
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
            }
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
            foreach (var item in Tracks)
            {
                Console.Write(item.TrackName + "\t");
                Console.WriteLine(item.TrackCode + "\t");
            }
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
        public bool AddTrack(int TrackCode ,string TrackName)
        {
            bool flag = true;
            foreach(var item in Tracks)
            {
                if(item.TrackCode== TrackCode)
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
                SaveDataToJson("TracksData.json", Tracks  ,"Add", "Track");
            }

            return flag;
        }

        public bool DeleteTrack(int TrackCode)
        {
            bool flag = false;
            foreach(var item in Tracks.ToList())
            {
                if(item.TrackCode == TrackCode)
                {
                    Tracks.Remove(item);
                    flag = true;
                    SaveDataToJson("TracksData.json", Tracks, "delet", "Track");
                }
            }
            return flag;
        }





        #endregion





    }
}
