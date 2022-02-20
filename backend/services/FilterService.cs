using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace backend.Services
{
    public class FilterService
    {
        public List<string> GetDirectorNamesFromString(string directorString)
        {
            directorString = Regex.Replace(directorString, "( and )|( , and)", ", ").Replace(",,", ",");

            return directorString
                .Split(", ")
                .Select(directorName => Regex.Replace(directorName.Trim(), @"\s+", " ")) // Convert any whitespace to a single space
                .Distinct()
                .ToList();
        }
    }
}
