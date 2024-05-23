using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Data;

namespace Zenith.Assets.Utils
{
    public class DatabaseUtil
    {
        private const string DB_NAME = "Zenith";
        public static OperationResultDto Backup(string backupPath = "")
        {
            var operationResult = new OperationResultDto();

            var backupFullName = Path.Combine(backupPath, $"ZenithBackup {DateTime.Now.ToString("yyyy-MM-dd HH-mm")}.bak");

            var connectionString = "";
            using (var dbcontext = new DbContextFactory().CreateDbContext(null))
            {
                connectionString = dbcontext.Database.GetConnectionString();
            }

            var sqlConnection = new SqlConnection(connectionString);
            var backupCommandText = new SqlCommand(@"Backup Database @databaseName To Disk = @backupFullName With Init", sqlConnection);

            backupCommandText.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("@backupFullName", backupFullName),
                new SqlParameter("@databaseName", DB_NAME),
            });

            try
            {
                sqlConnection.Open();
                sqlConnection.ChangeDatabase("master");

                backupCommandText.ExecuteNonQuery();

                operationResult.OperationResultType = OperationResultTypes.Succeeded;
                operationResult.ResultTitle = "پشتیبان گیری موفق";
                operationResult.ResultDescription = "عملیات پشتیبان گیری با موفقیت انجام شد؛ برای مشاهده فایل پشتیبان، گزینه زیر را کلیک کنید.";
                operationResult.UsefulParameter = backupFullName;
            }
            catch (Exception exception)
            {
                operationResult.OperationResultType = OperationResultTypes.Failed;
                operationResult.ResultTitle = "شکست در انجام عملیات";
                operationResult.ResultDescription = "عملیات پشتیبان گیری به شکست انجامید؛ می توانید با استفاده از گزینه زیر، اطلاعات بیشتری در این مورد را مشاهده کنید.";

                // LOG THE EXCEPTION
                //operationResult.UsefulParameter = exception.Message;
            }
            finally
            {
                sqlConnection.Close();
            }

            return operationResult;
        }

        public static OperationResultDto Restore(string fileName)
        {
            var operationResult = new OperationResultDto();

            var connectionString = "";
            using (var dbcontext = new DbContextFactory().CreateDbContext(null))
            {
                connectionString = dbcontext.Database.GetConnectionString();
            }

            var sqlConnection = new SqlConnection(connectionString);

            var dataFileLocation = @"C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Zenith.mdf";
            var logFileLocation = @"C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Zenith_log.ldf";

            var restoreCommand = new SqlCommand($"Restore Database @databaseName From disk = @backupPath With REPLACE, MOVE 'Zenith' TO '{dataFileLocation}', MOVE 'Zenith_log' TO '{logFileLocation}'", sqlConnection);

            restoreCommand.Parameters.AddRange(new SqlParameter[]
            {
                    new SqlParameter("@backupPath", fileName),
                    new SqlParameter("@databaseName", DB_NAME),
            });


            try
            {
                sqlConnection.Open();
                sqlConnection.ChangeDatabase("master");

                KillRelatedProcesses(sqlConnection);

                restoreCommand.ExecuteNonQuery();

                operationResult.OperationResultType = OperationResultTypes.Succeeded;
                operationResult.ResultTitle = "بازیابی موفق";
                operationResult.ResultDescription = "عملیات بازیابی با موفقیت انجام شد.";
            }
            catch (Exception exception)
            {
                operationResult.OperationResultType = OperationResultTypes.Failed;
                operationResult.ResultTitle = "شکست در انجام عملیات";
                operationResult.ResultDescription = "عملیات بازیابی به شکست انجامید؛ می توانید با استفاده از گزینه زیر، اطلاعات بیشتری در این مورد را مشاهده کنید.";

                // LOG THE EXCEPTION
                //operationResult.UsefulParameter = exception.Message;
            }
            finally
            {
                sqlConnection.Close();
            }

            return operationResult;
        }

        private static void KillRelatedProcesses(SqlConnection sqlConnection)
        {
            var processListCommand = new SqlCommand("Exec sp_who;", sqlConnection);
            processListCommand.Parameters.Add(new SqlParameter("@databaseName", DB_NAME));

            var processListDataReader = processListCommand.ExecuteReader();

            var databaseProcesseIds = new List<short>();

            while (processListDataReader.Read())
                if (processListDataReader["dbname"].ToString() == DB_NAME)
                    databaseProcesseIds.Add((short)processListDataReader["spid"]);

            processListDataReader.Close();

            if (databaseProcesseIds.Any())
                new SqlCommand(databaseProcesseIds.Select(id => $"Kill {id};").Join(""), sqlConnection).ExecuteNonQuery();
        }
    }
}
