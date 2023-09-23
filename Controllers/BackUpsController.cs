using Acacia_Back_End.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Acacia_Back_End.Controllers
{
    public class BackUpsController : BaseApiController
    {
        private readonly Context _context;

        public BackUpsController(Context context)
        {
            _context = context;
        }

        public class BackupInfo
        {
            public string FileName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        //[HttpGet]
        //public async Task<IActionResult> ListBackups()
        //{
        //    try
        //    {
        //        string backupFolderPath = "wwwroot/backups"; // Adjust the folder path as needed
        //        DirectoryInfo directoryInfo = new DirectoryInfo(backupFolderPath);

        //        var backups = directoryInfo.GetFiles("*.sqlite")
        //            .OrderByDescending(f => f.CreationTime)
        //            .Select(f => new BackupInfo
        //            {
        //                FileName = f.Name,
        //                CreatedAt = f.CreationTime
        //            })
        //            .ToList();

        //        return Ok(backups);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Failed to list backups: {ex.Message}");
        //    }
        //}

        //[HttpPost("backup")]
        //public async Task<IActionResult> Backup()
        //{
        //    try
        //    {
        //        string backupFolderPath = "wwwroot/backups"; // Adjust the folder path as needed
        //        if (!Directory.Exists(backupFolderPath))
        //        {
        //            Directory.CreateDirectory(backupFolderPath);
        //        }

        //        string backupFileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.sqlite";
        //        string backupFullPath = Path.Combine(backupFolderPath, backupFileName);

        //        using (var source = new SqliteConnection(_context.Database.GetConnectionString()))
        //        {
        //            source.Open();
        //            using (var destination = new SqliteConnection($"Data Source={backupFullPath}"))
        //            {
        //                destination.Open();
        //                source.BackupDatabase(destination);
        //            }
        //        }

        //        return Ok($"Backup completed successfully. Backup file: {backupFileName}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Backup failed: {ex.Message}");
        //    }
        //}

        //[HttpPost("restore/{backupFileName}")]
        //public IActionResult Restore(string backupFileName)
        //{
        //    try
        //    {
        //        string backupFolderPath = "wwwroot/backups"; // Adjust the folder path as needed
        //        string restoreFileName = Path.Combine(backupFolderPath, backupFileName);

        //        if (!System.IO.File.Exists(restoreFileName))
        //        {
        //            // The specified backup file does not exist in the backup folder
        //            return BadRequest("The specified backup file does not exist.");
        //        }

        //        using (var source = new SqliteConnection($"Data Source={restoreFileName}"))
        //        {
        //            source.Open();
        //            using (var destination = new SqliteConnection(_context.Database.GetConnectionString()))
        //            {
        //                destination.Open();
        //                source.BackupDatabase(destination);
        //            }
        //        }

        //        return Ok("Restore completed successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Restore failed: {ex.Message}");
        //    }
        //}

        //public async Task<IActionResult> CreateBackup()
        //{
        //    try
        //    {
        //        string connectionString = "your_connection_string_here";
        //        string databaseName = "your_database_name";
        //        string backupFileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.bacpac";

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            await connection.OpenAsync();

        //            string backupQuery = $"BACKUP DATABASE [{databaseName}] TO URL = 'https://your-storage-account.blob.core.windows.net/your-container/{backupFileName}' WITH FORMAT;";
        //            using (SqlCommand command = new SqlCommand(backupQuery, connection))
        //            {
        //                await command.ExecuteNonQueryAsync();
        //            }
        //        }

        //        return Ok($"Backup completed successfully. Backup file: {backupFileName}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Backup failed: {ex.Message}");
        //    }
        //}

        //public async Task<IActionResult> RestoreDatabase(string backupUrl)
        //{
        //    try
        //    {
        //        string connectionString = "your_connection_string_here";
        //        string databaseName = "your_database_name";

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            await connection.OpenAsync();

        //            string restoreQuery = $"RESTORE DATABASE [{databaseName}] FROM URL = '{backupUrl}' WITH CREDENTIAL = 'your-storage-credential';";
        //            using (SqlCommand command = new SqlCommand(restoreQuery, connection))
        //            {
        //                await command.ExecuteNonQueryAsync();
        //            }
        //        }

        //        return Ok("Restore completed successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Restore failed: {ex.Message}");
        //    }
        //}
    }
}
