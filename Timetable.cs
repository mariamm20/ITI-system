using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_System
{
    internal class Timetable
    {
        public string CourseName { get; set; }
        public int CourseCode { get; set; } 
        public string TrackName { get; set; }
        public string InstructorName { get; set; }
        public int InstructorID { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
