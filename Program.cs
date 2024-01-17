using ITI_System;
using myiti;
using Newtonsoft.Json;
using System.Security.Principal;
using Spectre.Console;
using System.Data;


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
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
           .Title("[green]Welcome to ITI System[/]")
           .PageSize(10)
           .AddChoices(new[] {
            "1. Register", "2. Login", "3. Close the program",
        }));
            switch (choice)
            {
                case "1. Register":
                    RegisterForm();
                    break;
                case "2. Login":
                    LoginForm();
                    break;
                case "3. Close the program":
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
            var roleChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
           .Title("[green]Registeration Form[/]\n--------------------\n[blue]Welcome User, please fill the following inforamtion[/]\n-----------------------------------------------------------\n[red]Are you instructor or student ?[/]")
           .PageSize(10)
           .AddChoices(new[] {
            "1. Instructor", "2. Student", "0. Return Home",
        }));
            string role = "";
            switch (roleChoice)
            {
                case "1. Instructor":
                    role = "instructor";
                    break;
                case "2. Student":
                    role = "student";
                    break;
                case "0. Return Home":
                    DisplayHome();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    PressAnyKeyToManageConsoleScreen("return home");
                    DisplayHome();
                    break;
            }
            Console.WriteLine($"Register as {role}");
            Console.WriteLine("----------------------");
            Console.WriteLine();
            Console.Write("Your Name : ");
            string name = Console.ReadLine();
            Console.Write("Your Specialization : ");
            string specialization = Console.ReadLine();
            Console.Write("Your Email : ");
            string email = Console.ReadLine();
            Console.Write("Your Password : "); // Password Masking is needed
            string password = GetMaskedPassword();
            Console.WriteLine();
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
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
           .Title("[green]Login Form[/]\n--------------------")
           .PageSize(10)
           .AddChoices(new[] {
            "1. Login as Admin", "2. Login as Instructor", "3. Login as Student","4. Return Home",
            }));

            switch (choice)
            {
                case "1. Login as Admin":
                    Console.WriteLine("Welcome Admin");
                    Console.WriteLine("-----------------");                    
                    Console.WriteLine();
                    int counter = 3;

                    while (counter > 0)
                    {
                        Console.Write("Your Email : ");
                        string email = Console.ReadLine().ToLower().Trim();
                        Console.Write("Your Password : "); // Password Masking is needed
                        string password = GetMaskedPassword();
                        Console.WriteLine();
                        Admin admin = new Admin();

                        bool LoginResult = admin.Login(email, password);
                    adminhome:
                        if (LoginResult)
                        {
                            // All implement of Admin 
                            Console.Clear();

                           

                            var AdminChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                           .Title("[green]* Welcome Admin, Ghada *[/]\n--------------------")
                           .PageSize(10)
                           .AddChoices(new[] {
                            "1. Accounts Management", "2. Instructors Management","3. Students Management","4. Tracks Management",
                            "5. Courses Management","6. Timetables Management","7. Feedback Management","8. Logout",
                            }));


                            switch (AdminChoice)
                            {

                                case "1. Accounts Management":
                                AccountManagementHome:
                                    Console.Clear();
                                   
                                    var accountChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                   .Title("[green]Accounts Management[/]\n--------------------")
                                   .PageSize(10)
                                   .AddChoices(new[] {
                                   "1. View pending accounts", "2. Approve instructor account","3. Approve student account","4. Return Home",
                                      }));
                                    switch (accountChoice)
                                    {
                                        case "1. View pending accounts":
                                            Console.Clear();
                                            Console.WriteLine("Pending Accounts");
                                            admin.ViewPendingAccounts();
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto AccountManagementHome;
                                        case "2. Approve instructor account":
                                            Console.Clear();
                                            Console.WriteLine("Approve instructor account");
                                            Console.WriteLine("--------------------------");
                                            Console.WriteLine();
                                            Console.Write("Instructor ID : ");
                                            int id = int.Parse(Console.ReadLine());
                                            bool ResultOfExistance = admin.CheckExistanceOfInstructorAccunt(id);
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
                                        case "3. Approve student account":

                                            Console.Clear();
                                            Console.WriteLine("Approve student account");
                                            Console.WriteLine("--------------------------");
                                            Console.WriteLine();
                                            Console.Write("Student ID : ");
                                            int studId = int.Parse(Console.ReadLine());
                                            bool ResultOfExistanceStudent = admin.CheckExistanceOfStudentAccunt(studId);
                                            if (ResultOfExistanceStudent)
                                            {
                                                Console.Write("Set Student Track Code : ");
                                                int trackCode = int.Parse(Console.ReadLine());

                                                admin.ApproveAccount(studId, trackCode);
                                                PressAnyKeyToManageConsoleScreen("return home");
                                                goto AccountManagementHome;
                                            }
                                            else
                                            {
                                                Console.WriteLine("ID of student not exists");
                                                PressAnyKeyToManageConsoleScreen("return home");
                                                goto AccountManagementHome;
                                            }
                                                
                                        case "4. Return Home":
                                            goto adminhome;
                                        default:
                                            Console.WriteLine();
                                            break;
                                    }

                                    break;
                                case "2. Instructors Management":
                                InstructorManagementHome:
                                    Console.Clear();
                                   
                                    var instructorChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                   .Title("[green]Instructors Management[/]\n--------------------")
                                   .PageSize(10)
                                   .AddChoices(new[] {
                                   "1. View Instructors", "2. Add Instructor","3. Edit Instructor","4. Delete Instructor","5. Return Home",
                                      }));

                                    string[] instructorDataToEdit = { "ID", "Name", "Email", "Password", "Specilization", "Salary", "Track Code", "Course Code" };

                                    switch (instructorChoice)
                                    {
                                        case "1. View Instructors":
                                            Console.Clear();
                                            Console.WriteLine("All instructors in system");                                           
                                            admin.ViewInstructors();

                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto InstructorManagementHome;
                                        case "2. Add Instructor":
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
                                            string instPassword = GetMaskedPassword();
                                            Console.WriteLine();
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
                                        case "3. Edit Instructor":
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
                                                PressAnyKeyToManageConsoleScreen("return home");
                                                goto InstructorManagementHome;
                                            }
                                            break;
                                        case "4. Delete Instructor":
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
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto InstructorManagementHome;

                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine();
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto InstructorManagementHome;
                                                    }
                                                    
                                            }
                                            break;
                                        case "5. Return Home":
                                            goto adminhome;



                                    }

                                    break;

                                case "3. Students Management":
                                StudentManagementHome:
                                    Console.Clear();
                                   
                                    var studentChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                   .Title("[green]Students Management[/]\n--------------------")
                                   .PageSize(10)
                                   .AddChoices(new[] {
                                   "1. View Students", "2. Add Student","3. Edit Student","4. Delete Student","5. Return Home",
                                      }));
                                    string[] studentDataToEdit = { "ID", "Name", "Email", "Password", "Specilization", "Track Name", "Enrollment Date", "Courses" };

                                    switch (studentChoice)
                                    {
                                        case "1. View Students":
                                            Console.Clear();
                                            Console.WriteLine("All students in system");                                            
                                            admin.ViewStudents();

                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto StudentManagementHome;
                                        case "2. Add Student":
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
                                            string studPassword = GetMaskedPassword();
                                            Console.WriteLine();
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
                                        case "3. Edit Student":
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
                                        case "4. Delete Student":
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
                                        case "5. Return Home":
                                            goto adminhome;

                                    }

                                    break;

                                case "4. Tracks Management":
                                TrackManagementHome:
                                    Console.Clear();
                                    
                                    var trackChoice = AnsiConsole.Prompt(
                                     new SelectionPrompt<string>()
                                     .Title("[green]Tracks Management[/]\n--------------------")
                                     .PageSize(10)
                                     .AddChoices(new[] {
                                     "1. View Tracks", "2. Add Track","3. Edit Track","4. Delete Track","5. Return Home",
                                     }));
                                    switch (trackChoice)
                                    {
                                        case "1. View Tracks":
                                            Console.Clear();
                                            Console.WriteLine("Avialable Tracks");
                                            Console.WriteLine("-----------------");
                                            Console.Write("Track Name" + "\t");
                                            Console.Write("Track Code" + "\t");
                                            Console.WriteLine();
                                            admin.ViewTracks();
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto TrackManagementHome;
                                        case "2. Add Track":
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
                                        case "3. Edit Track":
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
                                        case "4. Delete Track":
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
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto TrackManagementHome;
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
                                        case "5. Return Home":
                                            goto adminhome;

                                    }

                                    break;




                                // Mohamed Part 
                                case "5. Courses Management":
                                CourseManagementHome:
                                    Console.Clear();
                                    var coursesChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                    .Title("[green]Courses Management[/]\n--------------------")
                                    .PageSize(10)
                                    .AddChoices(new[] {
                                    "1. View Courses", "2. Add Course","3. Edit Course","4. Delete Course","5. Return Home",
                                        }));
                                    switch (coursesChoice)
                                    {
                                        case "1. View Courses":
                                            Console.Clear();
                                            Console.WriteLine("Avialable Courses");
                                            Console.WriteLine("-----------------");
                                            Console.Write("Course Name" + "\t");
                                            Console.Write("Course Code" + "\t");
                                            Console.Write("Track Code" + "\t");
                                            Console.WriteLine();
                                            admin.ViewCourses();
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto CourseManagementHome;
                                        case "2. Add Course":
                                            Console.Clear();
                                            Console.WriteLine("Add Course");
                                            Console.Write("Course Name : ");
                                            string CourseName = Console.ReadLine();
                                            Console.Write("Course Code : ");
                                            int CourseCode = int.Parse(Console.ReadLine());
                                            Console.Write("Track Code : ");
                                            int TrackCode = int.Parse(Console.ReadLine());
                                            bool result = admin.AddCourse(CourseCode, CourseName, TrackCode);
                                            if (result)
                                            {
                                                Console.WriteLine("Course Additon Completed Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Course Already Exists OR Enter a valid Track Code !!");

                                            }
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto CourseManagementHome;
                                        case "3. Edit Course":
                                        EditCourse:
                                            Console.Clear();
                                            Console.WriteLine("Edit Data Of Course");
                                            Console.WriteLine("-------------------------");
                                            Console.WriteLine();
                                            Console.Write("Code of Course : ");
                                            int Ccode = int.Parse(Console.ReadLine());


                                            Console.WriteLine();
                                            bool ResultOfViewSpecificCourse = admin.ViewSpecificCourse(Ccode);
                                            if (ResultOfViewSpecificCourse)
                                            {


                                                Console.WriteLine();
                                                Console.WriteLine("What do you want to edit?");
                                                string[] trackDataToEditOptions = { "Course Name","Course code","Track Code" };

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
                                                        Console.WriteLine("Course Name:");
                                                        string CourseNameValue = Console.ReadLine();
                                                        bool resultOfEdit = admin.EditCourse(editChoice, Ccode, CourseNameValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another course data");
                                                            goto EditCourse;

                                                        }
                                                        break;
                                                    case 2:
                                                        Console.WriteLine("Course Code:");
                                                        int CourseCodeValue = int.Parse(Console.ReadLine());
                                                        resultOfEdit = admin.EditCourse(editChoice, Ccode, CourseCodeValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another course data");
                                                            goto EditCourse;
                                                        }
                                                        break;
                                                    case 3:
                                                        Console.WriteLine("Track Code:");
                                                        int TrackCodeValue = int.Parse(Console.ReadLine());
                                                        resultOfEdit = admin.EditCourse(editChoice, Ccode, TrackCodeValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit the another course data");
                                                            goto EditCourse;

                                                        }
                                                        break;
                                                    case 0:
                                                        goto CourseManagementHome;
                                                    default:
                                                        Console.WriteLine("Invalid choice");
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Code ");
                                                PressAnyKeyToManageConsoleScreen("try another code");
                                                goto EditCourse;
                                            }
                                            break;
                                        case "4. Delete Course":
                                            Console.Clear();
                                            Console.WriteLine("Delete data of Course");
                                            Console.WriteLine("---------------------------");
                                            Console.WriteLine();
                                            Console.WriteLine("0. Return home");

                                            Console.Write("Enter the code of Course : ");
                                            int delCode = int.Parse(Console.ReadLine());
                                            switch (delCode)
                                            {
                                                case 0:
                                                    goto CourseManagementHome;
                                                default:
                                                    bool delResult = admin.DeleteCourse(delCode);
                                                    if (delResult == false)
                                                    {
                                                        Console.WriteLine("Invalid Course Code");
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto CourseManagementHome;
                                                    }
                                                    else
                                                    {

                                                        Console.WriteLine();
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto CourseManagementHome;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case "5. Return Home":
                                            goto adminhome;
                                    }
                                    break;



                                case "6. Timetables Management":
                                TimetablesManagementHome:
                                    Console.Clear();
                                    
                                    var timetableChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                   .Title("[green]Timetables Management[/]\n--------------------")
                                   .PageSize(10)
                                   .AddChoices(new[] {
                                    "1. View Timetable", "2. Add Timetable","3. Edit Timetable","4. Delete Timetable","0. Return Home",
                                       }));
                                    switch (timetableChoice)
                                    {
                                        case "1. View Timetable":
                                            Console.Clear();
                                            Console.WriteLine("Avialable TimeTables");
                                            
                                            admin.ViewTimeTable();
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto TimetablesManagementHome;
                                        case "2. Add Timetable":
                                            Console.Clear();
                                            Console.WriteLine("Add TimeTable");
                                            Console.Write("Course Code : ");
                                            int CourseCode = int.Parse(Console.ReadLine());
                                            Console.Write("Instructor ID : ");
                                            int InstructorID = int.Parse(Console.ReadLine());
                                            Console.Write("Date: ");
                                            string Date = Console.ReadLine();
                                            Console.Write("From : ");
                                            string From = Console.ReadLine();
                                            Console.Write("To : ");
                                            string To = Console.ReadLine();
                                            bool result = admin.SetTimetable(CourseCode, InstructorID,Date,From,To );
                                            if (result)
                                            {
                                                Console.WriteLine("Course Additon Completed Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine(" Enter a valid Course Code OR Instructor ID !!");

                                            }
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto TimetablesManagementHome;
                                        case "3. Edit Timetable":
                                        EditTimeTable:
                                            Console.Clear();
                                            Console.WriteLine("Edit Data Of TimeTable");
                                            Console.WriteLine("-------------------------");
                                            Console.WriteLine();
                                            Console.Write("Code of Course : ");
                                            int Ccode = int.Parse(Console.ReadLine());
                                            Console.Write("ID of Instrructor : ");
                                            int IId = int.Parse(Console.ReadLine());
                                            Console.WriteLine();
                                            bool ResultOfViewSpecificCourse = admin.ViewSpecificTimeTable(Ccode,IId);
                                            if (ResultOfViewSpecificCourse)
                                            {
                                                Console.WriteLine();
                                                
                                                var editChoice = AnsiConsole.Prompt(
                                                new SelectionPrompt<string>()
                                                .Title("[green]What do you want to edit?[/]\n--------------------")
                                                .PageSize(10)
                                                .AddChoices(new[] {
                                                 "1. Course code", "2. Instructor ID","3. Track Name","4. Date","5. From","6. To","0. Return Home",
                                                      }));
                                                switch (editChoice)
                                                {

                                                    case "1. Course code":
                                                        Console.WriteLine("Course Code:");
                                                        int CourseCodeValue = int.Parse(Console.ReadLine());
                                                        bool resultOfEdit = admin.EditTimeTable(editChoice,IId, Ccode, CourseCodeValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit another TimeTable data");
                                                            goto TimetablesManagementHome;
                                                        }
                                                        break;
                                                    case "2. Instructor ID":
                                                        Console.WriteLine("Instructor ID :");
                                                        int InstructorIDValue = int.Parse(Console.ReadLine());
                                                        resultOfEdit = admin.EditTimeTable(editChoice,IId, Ccode, InstructorIDValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit another TimeTable data");
                                                            goto TimetablesManagementHome;

                                                        }
                                                        break;
                                                    case "3. Track Name":
                                                        Console.WriteLine("Track Name:");
                                                        string TrackNameValue = Console.ReadLine();
                                                        resultOfEdit = admin.EditTimeTable(editChoice,IId, Ccode, TrackNameValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit another TimeTable data");
                                                            goto TimetablesManagementHome;

                                                        }
                                                        break;
                                                    case "4. Date":
                                                        Console.WriteLine("Date:");
                                                        string DateValue = Console.ReadLine();
                                                        resultOfEdit = admin.EditTimeTable(editChoice, IId, Ccode, DateValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit another TimeTable data");
                                                            goto TimetablesManagementHome;

                                                        }
                                                        break;
                                                    case "5. From":
                                                        Console.WriteLine("From:");
                                                        string FromValue = Console.ReadLine();
                                                        resultOfEdit = admin.EditTimeTable(editChoice, IId, Ccode, FromValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit another TimeTable data");
                                                            goto TimetablesManagementHome;

                                                        }
                                                        break;
                                                    case "6. To":
                                                        Console.WriteLine("To:");
                                                        string ToValue = Console.ReadLine();
                                                        resultOfEdit = admin.EditTimeTable(editChoice, IId, Ccode, ToValue);
                                                        if (!resultOfEdit)
                                                        {
                                                            Console.WriteLine("Invalid Data");
                                                        }
                                                        else
                                                        {
                                                            PressAnyKeyToManageConsoleScreen("edit another TimeTable data");
                                                            goto TimetablesManagementHome;

                                                        }
                                                        break;
                                                    case "0. Return Home":
                                                        goto TimetablesManagementHome;
                                                    default:
                                                        Console.WriteLine("Invalid choice");
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Code ");
                                                PressAnyKeyToManageConsoleScreen("try another code");
                                                goto TimetablesManagementHome;
                                            }
                                            break;
                                        case "4. Delete Timetable":
                                            Console.Clear();
                                            Console.WriteLine("Delete data of TimeTable");
                                            Console.WriteLine("---------------------------");
                                            Console.WriteLine();
                                            Console.WriteLine("0. Return home");

                                            Console.Write("Enter the code of Course : ");
                                            int delCode = int.Parse(Console.ReadLine());
                                            Console.Write("Enter the id of Instructor : ");
                                            int delid = int.Parse(Console.ReadLine());
                                            switch (delCode)
                                            {
                                                case 0:
                                                    goto TimetablesManagementHome;
                                                default:
                                                    bool delResult = admin.DeleteTimetable(delid,delCode);
                                                    if (delResult == false)
                                                    {
                                                        Console.WriteLine("Invalid Course Code");
                                                    }
                                                    else
                                                    {

                                                        Console.WriteLine();
                                                        Console.WriteLine("Press any key to return home");
                                                        Console.ReadKey();
                                                        goto TimetablesManagementHome;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case "0. Return Home":
                                            goto adminhome;
                                    }
                                    break;

                                    
                                case "7. Feedback Management":
                                   FeedbackManagementHome:
                                    Console.Clear();
                                    var feedbackChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                   .Title("[green]Timetables Management[/]\n--------------------")
                                   .PageSize(10)
                                   .AddChoices(new[] {
                                    "1. View Feedback For instructor", "2. View Reports For student ","3. Return Home",
                                       }));

                                    switch (feedbackChoice)
                                    {
                                        case "1. View Feedback For instructor":
                                            Console.WriteLine("Enter Id of instuctor");
                                            int instructorId = int.Parse(Console.ReadLine());

                                            admin.ViewFeedBack(instructorId);
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto FeedbackManagementHome; ;
                                        case "2. View Reports For student ":
                                            Console.WriteLine("Enter Id of student");
                                            int studentId = int.Parse(Console.ReadLine());

                                            admin.ViewReport(studentId);
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto FeedbackManagementHome;
                                        case "3. Return Home":
                                            goto adminhome;

                                    }
                                    break;
                                case "8. Logout":
                                    DisplayHome();
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
                case "2. Login as Instructor":
                    Console.WriteLine("Welcome Instructor");
                    Console.WriteLine("-------------------");
                    Console.WriteLine();
                    int counter1 = 3;
                    
                    while (counter1 > 0)
                    {
                        Console.Write("Your Email : ");
                        string email = Console.ReadLine().ToLower().Trim();
                        Console.Write("Your Password : "); // Password Masking is needed
                        string password = GetMaskedPassword();
                        Console.WriteLine();
                        Instructor InstructorAccount = new Instructor();
                        Instructor holder;

                        holder= InstructorAccount.Login(email, password);
                        if (holder != null)
                        {
                        // All implement of instructor 
                        Console.WriteLine("Instructor Login successfully");
                        InstrucctorsMangment:
                            Console.Clear();
                           
                            var choic = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("[green]Instrucctors Mangment[/]\n--------------------")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "1. View Information ", "2. View courses ","3. View Time Table ","4. View students ","5. Manage Grade ","6. Report Student ",
                            "7. Logout ",
                               }));
                            switch (choic)
                            {
                                case "1. View Information ":
                                    Console.Clear();
                                    Console.WriteLine("The Instructor Information is :");
                                    holder.ViewInformation();
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto InstrucctorsMangment;
                                case "2. View courses ":
                                    Console.Clear();
                                    holder.ViewCoursesByInstructor(holder.Id);
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto InstrucctorsMangment;
                                 case "3. View Time Table ":
                                    Console.Clear();
                                    holder.ViewTimetable(holder.Id);
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto InstrucctorsMangment;
                                case "4. View students ":
                                    Console.Clear();
                                    Console.WriteLine("Enter Track Code To View Student  : ");
                                    int trackcode = int.Parse(Console.ReadLine());
                                    holder.ViewStudents(trackcode);
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto InstrucctorsMangment;
                                case "5. Manage Grade ":
                                    GradesMangment:
                                    Console.Clear();
                                    
                                    var Gradeschoic = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                   .Title("[green]Grades Management[/]\n--------------------")
                                   .PageSize(10)
                                   .AddChoices(new[] {
                                   "1. Add Grade", "2. Edit Grade","3. Back to Main Instructor Mangment",
                                        }));
                                    switch (Gradeschoic)
                                    {
                                        case "1. Add Grade":
                                            Console.Clear();
                                            Console.WriteLine("Enter Course Code :");
                                            int c = int.Parse(Console.ReadLine());
                                            Course co = new Course();
                                            co.CourseCode = c;
                                            Console.WriteLine("Enter student id : ");
                                            int i = int.Parse(Console.ReadLine());
                                            Student ss = new Student();
                                            ss.Id = i;
                                            Console.WriteLine("Enter Grade : ");
                                            double grade = double.Parse(Console.ReadLine());
                                            holder.GiveGrade(ss, co, grade);
                                            PressAnyKeyToManageConsoleScreen("return grades management home");
                                            goto GradesMangment;
                                        case "2. Edit Grade":
                                            Console.Clear();
                                            Console.WriteLine("Enter Course Code :");
                                            int cc2 = int.Parse(Console.ReadLine());
                                            Course co2 = new Course();
                                            co2.CourseCode = cc2;
                                            Console.WriteLine("Enter student id : ");
                                            int i2 = int.Parse(Console.ReadLine());
                                            Student ss2 = new Student();
                                            ss2.Id = i2;
                                            Console.WriteLine("Enter Grade : ");
                                            double grade2 = double.Parse(Console.ReadLine());
                                            holder.EditGrade(ss2, co2, grade2);
                                            PressAnyKeyToManageConsoleScreen("return grades management home");
                                            goto GradesMangment;
                                        case "3. Back to Main Instructor Mangment":
                                            Console.ReadKey();
                                            goto InstrucctorsMangment;

                                        default:
                                            Console.WriteLine("Invalid choice");
                                            PressAnyKeyToManageConsoleScreen("return home");
                                            goto InstrucctorsMangment;
                                    }
                                   
                                case "6. Report Student ":
                                    Console.Clear();
                                    Console.WriteLine("Enter Student Id who you need to report : ");
                                    int StID = int.Parse(Console.ReadLine());

                                    Console.WriteLine("Enter your Feedback : ");
                                    string feed = Console.ReadLine();
                                    holder.ReportStudent(StID, holder.Id, feed);
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto InstrucctorsMangment;
                                case "7. Logout ":
                                    DisplayHome();
                                    break;
                                default:
                                    Console.WriteLine("Invalid Choice ");
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto InstrucctorsMangment;

                            }

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
                case "3. Login as Student":
                    Console.WriteLine("Welcome Student");
                    Console.WriteLine("-----------------");
                    Console.WriteLine();
                    int counter2 = 3;
                    while (counter2 > 0)
                    {
                        Console.Write("Your Email : ");
                        string email = Console.ReadLine().ToLower().Trim();
                        Console.Write("Your Password : "); // Password Masking is needed
                        string password = GetMaskedPassword();
                        Console.WriteLine();
                        Student account1 = new Student();
                        Student holder;
                        holder = account1.Login(email, password);
                        if (holder != null)
                        {

                            // All implement of student
                            StudentManagementHome:
                            Console.Clear();
                          
                            var studentChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title($"[green]Welcome [/][red]{holder.Name}[/]\n--------------------")
                            .PageSize(10)
                            .AddChoices(new[] {
                             "1. View Personal Information", "2. View Timetable","3. View Grades","4. Report Instructor","5. Logout",
                                 }));
                            switch (studentChoice)
                            {
                                case "1. View Personal Information":
                                    Console.Clear();
                                    holder.ViewData();
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto StudentManagementHome;
                                case "2. View Timetable":
                                    Console.Clear();
                                    holder.ViewTimeTable();
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto StudentManagementHome;
                                case "3.View Grades":
                                    Console.Clear();
                                    holder.ViewGrade();
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto StudentManagementHome;
                                case "4. Report Instructor":
                                
                                    Console.Clear();
                                    Console.WriteLine("Enter Instructor Id who you need to report : ");
                                    int InstructorID = int.Parse(Console.ReadLine());

                                    Console.WriteLine("Enter your Feedback : ");
                                    string feed = Console.ReadLine();
                                    holder.ReportInstructor(holder.Id, InstructorID, feed);
                                    PressAnyKeyToManageConsoleScreen("return home");
                                    goto StudentManagementHome;
                                case "5. Logout":
                                    DisplayHome();
                                    break;
                                default:
                                    Console.WriteLine("Invalid Choice ");
                                    PressAnyKeyToManageConsoleScreen("return Home");
                                    goto StudentManagementHome;


                            }

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
                case "4. Return Home":
                    DisplayHome();
                    break;


            }

        }

        static string GetMaskedPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Check if the key is a valid input (excluding Enter key)
                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    // Append the pressed key to the password
                    password += key.KeyChar;
                    Console.Write("*"); // Display a masking character
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // Handle backspace - remove the last character from the password
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b"); // Erase the last character on the console
                }

            } while (key.Key != ConsoleKey.Enter);

            return password;
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
