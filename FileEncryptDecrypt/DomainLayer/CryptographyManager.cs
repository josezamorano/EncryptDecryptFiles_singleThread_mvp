using FileEncryptDecrypt.DataAccessLayer.IOFiles;
using FileEncryptDecrypt.Utils.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptDecrypt.DomainLayer
{
    public class CryptographyManager
    {
        private FileService _fileService;
        private Cipher _cryptographer;
        private List<string> _allSelectedFiles;
        private string _originalFolderName;
        public CryptographyManager()
        {
            _fileService = new FileService();
            _cryptographer = new Cipher();
            _allSelectedFiles = new List<string>();
            _originalFolderName = string.Empty;
        }


        public int GetAllFiles(string folder)
        {
            _originalFolderName = folder;
            _allSelectedFiles = _fileService.GetAllFilesInDirectory(folder);
            return _allSelectedFiles.Count();
        }

        public string EncryptAllFiles(string encryptionPassword)
        {
            Stopwatch  stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> encryptedFilesReport = new List<string>();
            foreach(string file in _allSelectedFiles)
            {
                string outputFile = _fileService.CreateCipherFileName(_originalFolderName, file , CipherState.Encrypted); 

                string enctryptedFileInfo =_cryptographer.EncryptFile(file, outputFile, encryptionPassword);
                encryptedFilesReport.Add(enctryptedFileInfo);
            }
            stopwatch.Stop();
            long timeInMilliseconds = stopwatch.ElapsedMilliseconds;
            string consolidatedReport = createReport(timeInMilliseconds, encryptedFilesReport, CipherState.Encrypted);
            return consolidatedReport;
        }

        public string DecryptAllFiles(string decryptionPassword)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> decryptedFilesReport = new List<string>();
            foreach (string file in _allSelectedFiles)
            {
                string outputFile = _fileService.CreateCipherFileName(_originalFolderName, file, CipherState.Decrypted);

                string dectryptedFileInfo = _cryptographer.DecryptFile(file, outputFile, decryptionPassword);
                decryptedFilesReport.Add(dectryptedFileInfo);
            }
            stopwatch.Stop();
            long timeInMilliseconds = stopwatch.ElapsedMilliseconds;
            string consolidatedReport = createReport(timeInMilliseconds, decryptedFilesReport,CipherState.Decrypted);
            return consolidatedReport;
        }

        private string createReport(long elapsedMilliseconds , List<string> encryptionReport, CipherState cipherState)
        {
            int totalFilesOk = 0;
            int totalFilesWithErrors = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var report in encryptionReport)
            {
                string[] info = report.Split('|');
                if (info.Length == 1)
                {
                    totalFilesOk++;
                }
                else
                {
                    totalFilesWithErrors++;
                    sb.AppendLine(report + Environment.NewLine);
                }
            }
            var reportType = Enum.GetName(typeof(CipherState), cipherState);
            string messageFilesWithErrors = (totalFilesWithErrors > 0) ? Environment.NewLine + sb.ToString() : string.Empty;
            string consolidatedReport = "Execution time:" + elapsedMilliseconds/1000 + " seconds."+ Environment.NewLine +
                                        $"Total {reportType} Files: " + totalFilesOk + Environment.NewLine +
                                        "Total Failed Files: " + totalFilesWithErrors +
                                        messageFilesWithErrors;

            return consolidatedReport;
        }


    }
}
