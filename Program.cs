using ITI_System;
using myiti;
using System.Security.Principal;

namespace ITI_system
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            DisplayHome();
        }
        static void DisplayHome()
        {

            Console.Clear();
            Console.WriteLine("Welcome to ITI System");
            Console.WriteLine("1. Register ");
            Console.WriteLine("2. Login ");
            Console.WriteLine("3. Close the program ");
            Console.Write("Choose option  : ");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    RegisterForm();
                    break;
                case 2:
                    LoginForm();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Choice option");
                    break;

            }


        }
        static void RegisterForm()
        {
            Console.Clear();
            Console.WriteLine("Registeration Form");
            Console.WriteLine("--------------------");
            Console.WriteLine();

            Console.WriteLine("Welcome User, please fill the following inforamtion");
            Console.WriteLine("-----------------------------------------------------------");
            Console.Write("Are you instructor or student ?  ");
            string role = Console.ReadLine().ToLower().Trim();
            Console.Write("Your Name : ");
            string name = Console.ReadLine();
            Console.Write("Your Specialization : ");
            string specialization = Console.ReadLine();
            Console.Write("Your Email : ");
            string email = Console.ReadLine();
            Console.Write("Your Password : "); // Password Masking is needed
            string password = Console.ReadLine();

            Account account1 = new Account();
            bool result = account1.CreateAccount(name, specialization, email, password, role);
            if (result)
            {
                Console.WriteLine("Account Created Succesfully Wait for Admin Approval :) ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to Return Home");
                Console.ReadKey();
                DisplayHome();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Invalid Data Inputs");
                Console.WriteLine("-------------------");
                Console.WriteLine("* Empty fields is not allowed");
                Console.WriteLine("* Email must be valid email");
                Console.WriteLine("* Password Length must be more than 8 ");
                Console.WriteLine();
                Console.WriteLine("Press any key to try again");
                Console.ReadKey();

                RegisterForm();

            }


        }
        static void LoginForm()
        {
            Console.Clear();
            Console.WriteLine("Login Form");
            Console.WriteLine("--------------------");
            Console.WriteLine();
            Console.WriteLine("1. Login as Admin");
            Console.WriteLine("2. Login as Instructor");
            Console.WriteLine("3. Login as Student");
            Console.WriteLine("4. Return Home");

            int choice = GetUserChoice();

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Welcome Admin");
                    Console.WriteLine();
                    int counter = 3;

                    while (counter > 0)
                    {
                        Console.Write("Your Email : ");
                        string email = Console.ReadLine().ToLower().Trim();
                        Console.Write("Your Password : "); // Password Masking is needed
                        string password = Console.ReadLine().ToLower().Trim();
                        Admin admin = new Admin();

                        bool LoginResult = admin.Login(email, password);
                    adminhome:
                        if (LoginResult)
                        {
                            // All implement of Admin 
                            Console.Clear();

                            Console.WriteLine("************************");
                            Console.WriteLine("* Welcome Admin, Ghada *");
                            Console.WriteLine("************************");
                            Console.WriteLine();
                            Console.WriteLine("1. Accounts Management");//done
                            Console.WriteLine("2. Instructors Management");//done
                            Console.WriteLine("3. Students Management");
                            Console.WriteLine("4. Tracks Management");
                            Console.WriteLine("5. Courses Management");
                            Console.WriteLine("6. Timetables Management");
                            Console.WriteLine("7. Feedback Management");


                            Console.WriteLine();

                            int AdminChoice = GetUserChoice();

                            switch (AdminChoice)
                            {

                                case 1:
                                AccountManagementHome:
                                    Console.Clear();
                                    Console.WriteLine("Accounts Management");
                                    Console.WriteLine("--------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View pending accounts");
                                    Console.WriteLine("2. Approve instructor account");
                                    Console.WriteLine("3. Approve student account");
                                    Console.WriteLine("4. Return Home");
                                    int accountChoice = GetUserChoice();

                                    switch (accountChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("Pending Accounts");
                                            Console.WriteLine("-----------------");
                                            admin.ViewPendingAccounts();
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto AccountManagementHome;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Approve instructor account");
                                            Console.WriteLine("--------------------------");
                                            Console.WriteLine();
                                            Console.Write("Instructor ID : ");
                                            int id = int.Parse(Console.ReadLine());
                                            bool ResultOfExistance = admin.CheckExistanceOfAccunt(id);
                                            if (ResultOfExistance)
                                            {
                                                Console.Write("Set Instructor Salary : ");
                                                int salary = int.Parse(Console.ReadLine());
                                                Console.WriteLine("Set Instructor Tracks (enter '-1' to finish) : ");
                                                List<int> tracks = new List<int>();
                                                int trackInput;
                                                do
                                                {
                                                    trackInput = int.Parse(Console.ReadLine());
                                                    if (trackInput != -1)
                                                    {
                                                        tracks.Add(trackInput);
                                                    }
                                                } while (trackInput != -1);

                                                Console.WriteLine("Set Instructor Courses (enter '-1' to finish) : ");
                                                List<int> courses = new List<int>();
                                                int coursesInput;
                                                do
                                                {
                                                    coursesInput = int.Parse(Console.ReadLine());
                                                    if (coursesInput != -1)
                                                    {
                                                        courses.Add(coursesInput);
                                                    }
                                                } while (coursesInput != -1);


                                                string result = admin.ApproveAccount(id, salary, tracks, courses);
                                                Console.WriteLine(result);
                                                PressAnyKeyToManageConsoleScreen("return home");
                                                goto AccountManagementHome;
                                            }
                                            else
                                            {
                                                Console.WriteLine("ID not exists in pending accounts ");
                                                PressAnyKeyToManageConsoleScreen("return home");
                                                goto AccountManagementHome;
                                            }
                                        case 3:

                                            Console.Clear();
                                            Console.WriteLine("Approve student account");
                                            Console.WriteLine("--------------------------");
                                            Console.WriteLine();
                                            Console.Write("Student ID : ");
                                            int studId = int.Parse(Console.ReadLine());
                                            Console.Write("Set Student Track Code : ");
                                            int trackCode = int.Parse(Console.ReadLine());

                                            admin.ApproveAccount(studId, trackCode);
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto AccountManagementHome;
                                        case 4:
                                            goto adminhome;
                                        default:
                                            Console.WriteLine();
                                            break;
                                    }

                                    break;
                                case 2:
                                InstructorManagementHome:
                                    Console.Clear();
                                    Console.WriteLine("Instructors Management");
                                    Console.WriteLine("----------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View Instructors");
                                    Console.WriteLine("2. Add Instructor");
                                    Console.WriteLine("3. Edit Instructor");
                                    Console.WriteLine("4. Delete Instructor");
                                    Console.WriteLine("5. Return Home");

                                    int instructorChoice = GetUserChoice();
                                    string[] instructorDataToEdit = { "ID", "Name", "Email", "Password", "Specilization", "Salary", "Track Code", "Course Code" };

                                    switch (instructorChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("All instructors in system");
                                            Console.WriteLine("-------------------------");
                                            for (int i = 0; i < instructorDataToEdit.Length; i++)
                                            {
                                                Console.Write(instructorDataToEdit[i] + "\t");
                                            }
                                            Console.WriteLine();
                                            admin.ViewInstructors();

                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto InstructorManagementHome;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Add Instructor in system");
                                            Console.WriteLine("------------------------");
                                            Console.Write("Instructor ID : ");
                                            int instid = int.Parse(Console.ReadLine());
                                            Console.Write("Instructor Name : ");
                                            string instName = Console.ReadLine();
                                            Console.Write("Instructor Email : ");
                                            string instEmail = Console.ReadLine();
                                            Console.Write("Set Instructor Password : ");
                                            string instPassword = Console.ReadLine();
                                            Console.Write("Instructor Salary : ");
                                            int instSalary = int.Parse(Console.ReadLine());
                                            Console.Write("Instructor Specilization : ");
                                            string instSpecialization = Console.ReadLine();
                                            Console.WriteLine("Set Instructor Tracks Codes (enter '-1' to finish) : ");
                                            List<int> instTracks = new List<int>();
                                            int trackInput;
                                            do
                                            {
                                                trackInput = int.Parse(Console.ReadLine());
                                                if (trackInput != -1)
                                                {
                                                    instTracks.Add(trackInput);
                                                }
                                            } while (trackInput != -1);
                                            Console.WriteLine("Set Instructor Courses (enter '-1' to finish ) : ");
                                            List<int> instCourses = new List<int>();
                                            int courseInput;
                                            do
                                            {
                                                courseInput = int.Parse(Console.ReadLine());
                                                if (courseInput != -1)
                                                {
                                                    instCourses.Add(courseInput);
                                                }
                                            } while (courseInput != -1);

                                            bool result = admin.AddInstructor(instid, instName, instEmail, instPassword, instSpecialization, instSalary, instTracks, instCourses);
                                            if (result == false)
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("Instructor Account Creation Failed !!");
                                            }
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto InstructorManagementHome;
                                        case 3:
                                        EditInstructor:
                                            Console.Clear();
                                            Console.WriteLine("Edit Data Of Instructor");
                                            Console.WriteLine("-------------------------");
                                            Console.WriteLine();
                                            Console.Write("ID of instructor : ");
                                            int id = int.Parse(Console.ReadLine());

                                            Console.WriteLine();

                                            bool ResultOfViewSpecificInstructor = admin.ViewSpecificInstructor(id);
                                            if (ResultOfViewSpecificInstructor)
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("What do you want to edit?");
                                                string[] instructorDataToEditOptions = { "Edit ID", "Edit Name", "Edit Email", "Edit Password", "Edit Specilization", "Edit Salary", "Add Track Code", "Remove Track Code", "Add Course Code", "Remove Course Code" };

                                                for (int i = 0; i < instructorDataToEditOptions.Length; i++)
                                                {
                                                    Console.WriteLine($"{i + 1}. {instructorDataToEditOptions[i]}");
                                                }

                                                Console.WriteLine();
                                                Console.WriteLine("0. Return Home");
                                                int editChoice = GetUserChoice();
                                                switch (editChoice)
                                                {
                                                    case 1:
                                                        int intValue = int.Parse(Console.ReadLine());
                                                        bool resultOfEdit = admin.EditInstructor(editChoice, id, intValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another instructor data");
                                                            goto EditInstructor;

                                                        }
                                                        break;
                                                    case 2:
                                                    case 3:
                                                    case 4:
                                                    case 5:
                                                        Console.WriteLine("Enter value:");
                                                        string stringValue = Console.ReadLine();
                                                        resultOfEdit = admin.EditInstructor(editChoice, id, stringValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another instructor data");
                                                            goto EditInstructor;

                                                        }
                                                        break;
                                                    case 6:
                                                        double doubleValue = double.Parse(Console.ReadLine());
                                                        resultOfEdit = admin.EditInstructor(editChoice, id, doubleValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another instructor data");
                                                            goto EditInstructor;

                                                        }
                                                        break;
                                                    case 7:
                                                    case 8:
                                                    case 9:
                                                    case 10:
                                                        Console.WriteLine("Edit Data (enter '-1' to finish)");

                                                        List<int> tracksEdit = new List<int>();
                                                        int trackEditInput;
                                                        do
                                                        {
                                                            trackEditInput = int.Parse(Console.ReadLine());
                                                            if (trackEditInput != -1)
                                                            {
                                                                tracksEdit.Add(trackEditInput);
                                                            }
                                                        } while (trackEditInput != -1);
                                                        resultOfEdit = admin.EditInstructor(editChoice, id, tracksEdit);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another instructor data");
                                                            goto EditInstructor;

                                                        }
                                                        break;
                                                    case 0:
                                                        goto InstructorManagementHome;
                                                    default:
                                                        Console.WriteLine("Invalid choice");
                                                        break;
                                                }


                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid ID ");
                                                PressAnyKeyToManageConsoleScreen("try another id");
                                                goto EditInstructor;
                                            }
                                            break;
                                        case 4:
                                            Console.Clear();
                                            Console.WriteLine("Delete data of instructor");
                                            Console.WriteLine("---------------------------");

                                            Console.WriteLine();
                                            Console.WriteLine("0. Return home");
                                            Console.Write("Enter the id of instructor : ");
                                            int delId = int.Parse(Console.ReadLine());
                                            switch (delId)
                                            {
                                                case 0:
                                                    goto InstructorManagementHome;
                                                default:
                                                    bool delResult = admin.DeleteInstructor(delId);
                                                    if (delResult == false)
                                                    {
                                                        Console.WriteLine("Invalid Id");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine();
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto InstructorManagementHome;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 5:
                                            goto adminhome;



                                    }

                                    break;

                                case 3:
                                StudentManagementHome:
                                    Console.Clear();
                                    Console.WriteLine("Students Management");
                                    Console.WriteLine("--------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View Students");
                                    Console.WriteLine("2. Add Student");
                                    Console.WriteLine("3. Edit Student");
                                    Console.WriteLine("4. Delete Student");
                                    Console.WriteLine("5. Return Home");
                                    int studentChoice = GetUserChoice();

                                    string[] studentDataToEdit = { "ID", "Name", "Email", "Password", "Specilization", "Track Name", "Enrollment Date", "Courses" };

                                    switch (studentChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("All students in system");
                                            Console.WriteLine("-------------------------");
                                            for (int i = 0; i < studentDataToEdit.Length; i++)
                                            {
                                                Console.Write(studentDataToEdit[i] + "\t");
                                            }
                                            Console.WriteLine();
                                            admin.ViewStudents();

                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto StudentManagementHome;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Add Student in system");
                                            Console.WriteLine("------------------------");
                                            Console.Write("Student ID : ");
                                            int studid = int.Parse(Console.ReadLine());
                                            Console.Write("Student Name : ");
                                            string studName = Console.ReadLine();
                                            Console.Write("Student Email : ");
                                            string studEmail = Console.ReadLine();
                                            Console.Write("Set Student Password : ");
                                            string studPassword = Console.ReadLine();
                                            Console.Write("Student Specilization : ");
                                            string studSpecialization = Console.ReadLine();
                                            Console.Write("Student Track Code : ");
                                            int studTrackCode = int.Parse(Console.ReadLine());
                                            Console.Write("Student Enrollment Date (mm/dd/yyyy) : ");
                                            string studEnrollmentDate = Console.ReadLine();
                                            bool result = admin.AddStudent(studid, studName, studEmail, studPassword, studSpecialization, studEnrollmentDate, studTrackCode);
                                            if (result == false)
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("Student Account Creation Failed !!");
                                            }
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto StudentManagementHome;
                                        case 3:
                                        EditStudent:
                                            Console.Clear();
                                            Console.WriteLine("Edit Data Of Student");
                                            Console.WriteLine("-------------------------");
                                            Console.WriteLine();
                                            Console.Write("ID of student : ");
                                            int id = int.Parse(Console.ReadLine());


                                            Console.WriteLine();
                                            bool ResultOfViewSpecificStudent = admin.ViewSpecificStudent(id);
                                            if (ResultOfViewSpecificStudent)
                                            {


                                                Console.WriteLine();
                                                Console.WriteLine("What do you want to edit?");
                                                string[] studentDataToEditOptions = { "ID", "Name", "Email", "Password", "Specilization", "Track Code", "Enrollment Date" };

                                                for (int i = 0; i < studentDataToEditOptions.Length; i++)
                                                {
                                                    Console.WriteLine($"{i + 1}. {studentDataToEditOptions[i]}");
                                                }

                                                Console.WriteLine();
                                                Console.WriteLine("0. Return Home");
                                                int editChoice = GetUserChoice();
                                                switch (editChoice)
                                                {
                                                    case 1:
                                                        int intValue = int.Parse(Console.ReadLine());
                                                        bool resultOfEdit = admin.EditStudent(editChoice, id, intValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another student data");
                                                            goto EditStudent;

                                                        }
                                                        break;
                                                    case 2:
                                                    case 3:
                                                    case 4:
                                                    case 5:


                                                        Console.WriteLine("Enter value:");
                                                        string stringValue = Console.ReadLine();
                                                        resultOfEdit = admin.EditStudent(editChoice, id, stringValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another student data");
                                                            goto EditStudent;

                                                        }
                                                        break;

                                                    case 6:

                                                        Console.WriteLine("Enter value:");
                                                        int trackCode = int.Parse(Console.ReadLine());
                                                        resultOfEdit = admin.EditStudent(editChoice, id, trackCode);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another student data");
                                                            goto EditStudent;

                                                        }
                                                        break;
                                                    case 0:
                                                        goto StudentManagementHome;
                                                    default:
                                                        Console.WriteLine("Invalid choice");
                                                        break;
                                                }


                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid ID ");
                                                PressAnyKeyToManageConsoleScreen("try another id");
                                                goto EditStudent;
                                            }
                                            break;
                                        case 4:
                                            Console.Clear();
                                            Console.WriteLine("Delete data of student");
                                            Console.WriteLine("---------------------------");
                                            Console.WriteLine();
                                            Console.WriteLine("0. Return home");

                                            Console.Write("Enter the id of student : ");
                                            int delId = int.Parse(Console.ReadLine());
                                            switch (delId)
                                            {
                                                case 0:
                                                    goto StudentManagementHome;
                                                default:
                                                    bool delResult = admin.DeleteStudent(delId);
                                                    if (delResult == false)
                                                    {
                                                        Console.WriteLine("Invalid Id");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine();
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto StudentManagementHome;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 5:
                                            goto adminhome;

                                    }

                                    break;

                                case 4:
                                TrackManagementHome:
                                    Console.Clear();
                                    Console.WriteLine("Tracks Management");
                                    Console.WriteLine("--------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View Tracks");
                                    Console.WriteLine("2. Add Track");
                                    Console.WriteLine("3. Edit Track");
                                    Console.WriteLine("4. Delete Track");
                                    Console.WriteLine("5. Return Home");
                                    int trackChoice = GetUserChoice();

                                    switch (trackChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("Avialable Tracks");
                                            Console.WriteLine("-----------------");
                                            Console.Write("Track Name" + "\t");
                                            Console.Write("Track Code" + "\t");
                                            Console.WriteLine();
                                            admin.ViewTracks();
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto TrackManagementHome;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Add Track");
                                            Console.Write("Track Name : ");
                                            string trackName = Console.ReadLine();
                                            Console.Write("Track Code : ");
                                            int trackCode = int.Parse(Console.ReadLine());
                                            bool result = admin.AddTrack(trackCode, trackName);
                                            if (result)
                                            {
                                                Console.WriteLine("Track Additon Completed Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Track Already Exists !!");

                                            }
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto TrackManagementHome;
                                        case 3:
                                        EditTrack:
                                            Console.Clear();
                                            Console.WriteLine("Edit Data Of Track");
                                            Console.WriteLine("-------------------------");
                                            Console.WriteLine();
                                            Console.Write("Code of track : ");
                                            int Tcode = int.Parse(Console.ReadLine());


                                            Console.WriteLine();
                                            bool ResultOfViewSpecificTrack = admin.ViewSpecificTrack(Tcode);
                                            if (ResultOfViewSpecificTrack)
                                            {


                                                Console.WriteLine();
                                                Console.WriteLine("What do you want to edit?");
                                                string[] trackDataToEditOptions = { "Track Name", "Track Code" };

                                                for (int i = 0; i < trackDataToEditOptions.Length; i++)
                                                {
                                                    Console.WriteLine($"{i + 1}. {trackDataToEditOptions[i]}");
                                                }

                                                Console.WriteLine();
                                                Console.WriteLine("0. Return Home");
                                                int editChoice = GetUserChoice();
                                                switch (editChoice)
                                                {
                                                    case 1:
                                                        Console.WriteLine("Track Name:");
                                                        string stringValue = Console.ReadLine();
                                                        bool resultOfEdit = admin.EditTrack(editChoice, Tcode, stringValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another track data");
                                                            goto EditTrack;

                                                        }
                                                        break;
                                                    case 2:
                                                        Console.WriteLine("Track Code:");
                                                        int intValue = int.Parse(Console.ReadLine());
                                                        resultOfEdit = admin.EditTrack(editChoice, Tcode, intValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another student data");
                                                            goto EditTrack;

                                                        }
                                                        break;

                                                    case 0:
                                                        goto TrackManagementHome;
                                                    default:
                                                        Console.WriteLine("Invalid choice");
                                                        break;
                                                }


                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Code ");
                                                PressAnyKeyToManageConsoleScreen("try another code");
                                                goto EditTrack;
                                            }
                                            break;
                                        case 4:
                                            Console.Clear();
                                            Console.WriteLine("Delete data of Track");
                                            Console.WriteLine("---------------------------");
                                            Console.WriteLine();
                                            Console.WriteLine("0. Return home");

                                            Console.Write("Enter the code of track : ");
                                            int delCode = int.Parse(Console.ReadLine());
                                            switch (delCode)
                                            {
                                                case 0:
                                                    goto TrackManagementHome;
                                                default:
                                                    bool delResult = admin.DeleteTrack(delCode);
                                                    if (delResult == false)
                                                    {
                                                        Console.WriteLine("Invalid Id");
                                                    }
                                                    else
                                                    {

                                                        Console.WriteLine();
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto TrackManagementHome;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 5:
                                            goto adminhome;

                                    }

                                    break;




                                // Mohamed Part 
                                case 5:
                                    Console.Clear();
                                    Console.WriteLine("Courses Management");
                                    Console.WriteLine("--------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View Courses");
                                    Console.WriteLine("2. Add Course");
                                    Console.WriteLine("3. Edit Course");
                                    Console.WriteLine("4. Delete Course");
                                    int coursesChoice = GetUserChoice();

                                    break;
                                case 6:
                                    Console.Clear();
                                    Console.WriteLine("Timetables Management");
                                    Console.WriteLine("--------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View Timetable");
                                    Console.WriteLine("2. Add Timetable");
                                    Console.WriteLine("3. Edit Timetable");
                                    Console.WriteLine("4. Delete Timetable");
                                    int timetableChoice = GetUserChoice();

                                    break;
                                case 7:
                                    Console.Clear();
                                    Console.WriteLine("Feedbacks Management");
                                    Console.WriteLine("--------------------");
                                    Console.WriteLine();
                                    Console.WriteLine("1. View Feedback");
                                    Console.WriteLine("2. Delete Feedback");
                                    int feedbackChoice = GetUserChoice();

                                    break;

                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid Email or Password, Number of tries left {counter - 1}");
                            counter--;
                        }
                    }
                    if (counter == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Too many wrong data please try again later :(");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return home");
                        Console.ReadKey();
                        DisplayHome();
                    }

                    break;
                // Instructor Implementation
                case 2:
                    Console.WriteLine("Welcome Instructor");
                    Console.WriteLine();
                    int counter1 = 3;
                    
                    while (counter1 > 0)
                    {
                        Console.Write("Your Email : ");
                        string email = Console.ReadLine().ToLower().Trim();
                        Console.Write("Your Password : "); // Password Masking is needed
                        string password = Console.ReadLine().ToLower().Trim();
                        Instructor InstructorAccount = new Instructor();
                        Instructor holder;

                        holder= InstructorAccount.Login(email, password);
                        if (holder != null)
                        {
                            // All implement of instructor 
                            Console.Clear();
                            Console.WriteLine("Instructor Login successfully");
                            
                            Console.WriteLine(holder.Id);

                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid Email or Password, Number of tries left {counter1 - 1}");
                            counter1--;
                        }
                    }
                    if (counter1 == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Too many wrong data please try again later :(");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return home");
                        Console.ReadKey();
                        DisplayHome();
                    }

                    break;
                // Student Implementation
                case 3:
                    Console.WriteLine("Welcome Student");
                    Console.WriteLine();
                    int counter2 = 3;
                    while (counter2 > 0)
                    {
                        Console.Write("Your Email : ");
                        string email = Console.ReadLine().ToLower().Trim();
                        Console.Write("Your Password : "); // Password Masking is needed
                        string password = Console.ReadLine().ToLower().Trim();
                        Student account1 = new Student();
                        Student holder;
                        holder = account1.Login(email, password);
                        if (holder != null)
                        {

                            // All implement of student 
                            Console.Clear();
                            Console.WriteLine("Student Login successfully");
                            Console.WriteLine(holder.Id);
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid Password, Number of tries left {counter2 - 1}");
                            counter2--;
                        }
                    }
                    if (counter2 == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Too many wrong data please try again later :(");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return home");
                        Console.ReadKey();
                        DisplayHome();

                    }
                    break;
                case 4:
                    DisplayHome();
                    break;


            }

        }
        static int GetUserChoice()
        {
            Console.WriteLine();
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine();
            return choice;
        }
        static void PressAnyKeyToManageConsoleScreen(string operation)
        {
            Console.WriteLine();
            Console.WriteLine($"Press any key to {operation}");
            Console.ReadKey();
        }

    }
}
