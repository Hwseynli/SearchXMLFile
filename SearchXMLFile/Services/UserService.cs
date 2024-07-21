using System;
using SearchXMLFile.Data;
using SearchXMLFile.DTOs;
using SearchXMLFile.Models;
using System.Xml.Linq;

namespace SearchXMLFile.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public bool IsUserNameUnique(string userFullName)
        {
            string url = "http://www.hms.gov.az/frq-content/nov_snk_v1/DOMESTIC.xml";
            XDocument xmlDoc = XDocument.Load(url);
            var fullNames = xmlDoc.Descendants("NAME_ORIGINAL_SCRIPT")
                                  .Select(x => x.Value.Replace(" ", "").Substring(0, x.Value.Length - 7))
                                  .ToList();
            return !fullNames.Any(fn => fn.ToLower() == userFullName.ToLower());
        }

        public async Task AddUserAsync(UserDTO userDto)
        {
            
            User user = new User
            {
                Name = Capitalize(userDto.Name),
                Surname = Capitalize(userDto.Surname),
                FatherName = Capitalize(userDto.FatherName),
                Birthday = userDto.Birthday,
                FullName=userDto.Name+userDto.Surname+userDto.FatherName
            };
            if (_context.Users.Any(x=>x.FullName==user.FullName)) return;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        private string Capitalize(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
        }


        public bool IsValidBirthday(DateTime birthday)
        {
            // Validate BirthDate
            if (birthday >= DateTime.Now.AddDays(-1))
            {
                return false;
            }
            if (birthday < DateTime.Now.AddYears(-1000))
            {
                return false;
            }

            // Add other validations if needed
            return true;
        }
    }

}

