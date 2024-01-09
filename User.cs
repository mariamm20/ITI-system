using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_System
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Approvement { get; set; }

        public User() { }

        #region Save data method to help in add , edit and delete operations
        public void SaveDataToJson<T>(string relativePath, List<T> ListName, string OperationName, string NameInMessage)
        {
            string solutionPath = @"C:\Users\Lenov\source\repos\ITI-system\Database\";

            string fullPath = Path.Combine(solutionPath, relativePath);

            string Data = JsonConvert.SerializeObject(ListName, Formatting.Indented);
            File.WriteAllText(fullPath, Data);
            Console.WriteLine($"{NameInMessage} {OperationName}ed successfully");
            Console.WriteLine();
        }

        #endregion

    }
}
