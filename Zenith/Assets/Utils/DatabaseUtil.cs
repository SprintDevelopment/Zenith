using Microsoft.Data.SqlClient;
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

namespace Zenith.Assets.Utils
{
    public class DatabaseUtil
    {
        private const string DB_NAME = "Zenith";
        public static OperationResultDto Backup(string backupPath = "", bool isSilent = false)
        {
            var operationResult = new OperationResultDto();

            //if (string.IsNullOrWhiteSpace(backupPath) || !Directory.Exists(backupPath))
            //{
            //    var folderBrowserDialog = new FolderBrowserDialog();
            //    DialogResult result = folderBrowserDialog.ShowDialog();

            //    if (result == System.Windows.Forms.DialogResult.OK && Directory.Exists(folderBrowserDialog.SelectedPath))
            //        backupPath = folderBrowserDialog.SelectedPath;
            //    else
            //    {
            //        operationResult.OperationExecuteResultTypeEnum = OperationExecuteResultType.Canceled;
            //        operationResult.ResultTitle = "لغو عملیات";
            //        operationResult.ResultDescription = "عملیات پشتیبان گیری توسط کاربر لغو شد";

            //        return operationResult;
            //    }
            //}

            var backupFullName = Path.Combine(backupPath, $"ZenithBackup {DateTime.Now.ToString("yyyy-MM-dd HH-mm")}.bak");
            var sqlConnection = new SqlConnection(@"Data Source=.;Initial Catalog=master;Integrated Security=True;Encrypt=False");

            var backupCommandText = new SqlCommand(@"Backup Database @databaseName To Disk = @backupFullName With Init", sqlConnection);

            backupCommandText.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("@backupFullName", backupFullName),
                new SqlParameter("@databaseName", DB_NAME),
            });

            try
            {
                sqlConnection.Open();

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

        public static OperationResultDto Restore(string backupPath)
        {
            var operationResult = new OperationResultDto();

            var openFileDialog = new OpenFileDialog() { Filter = "Backup File (*.bak)|*.bak|All files (*.*)|*.*" };

            if (!string.IsNullOrWhiteSpace(backupPath) || Directory.Exists(backupPath))
                openFileDialog.InitialDirectory = backupPath;

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var sqlConnection = new SqlConnection(@"Data Source=.;Initial Catalog=master;Integrated Security=True;Encrypt=False");


                var restoreCommand = new SqlCommand("Restore Database @databaseName From disk = @backupPath With Replace", sqlConnection);

                restoreCommand.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@backupPath", openFileDialog.FileName),
                    new SqlParameter("@databaseName", DB_NAME),
                });


                try
                {
                    sqlConnection.Open();

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

            return null;
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
