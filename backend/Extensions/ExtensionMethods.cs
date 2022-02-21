using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend.Extensions
{
    public static class ExtensionMethods
    {
        public static async Task TruncateTable(this ApplicationDbContext _context, string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync("Delete from " + tableName);
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('" + tableName + "', RESEED, 0)");
        }

        public static List<string> GetDirectorNamesFromString(this string directorString)
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
