using backend.Data;
using Microsoft.EntityFrameworkCore;
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
    }
}
