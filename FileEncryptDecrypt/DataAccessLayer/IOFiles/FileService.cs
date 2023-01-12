using FileEncryptDecrypt.Utils.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptDecrypt.DataAccessLayer.IOFiles
{
    public class FileService
    {
        private string? ENCRYPTED;
        private string? DECRYPTED;
        public FileService() 
        {
            ENCRYPTED = Enum.GetName(typeof(CipherState), CipherState.Encrypted);
            DECRYPTED = Enum.GetName(typeof(CipherState), CipherState.Decrypted);
        }


        public List<string> GetAllFilesInDirectory(string directory)
        {            
            List<string> files = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(directory))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(directory))
                {
                    files.AddRange(GetAllFilesInDirectory(d));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return files;            
        }

       
        public string CreateCipherFileName(string originalDirectory, string fullPathOriginalFile, CipherState cipherState)
        {
            string outputDirectory = CreateCipherFolderName(originalDirectory , cipherState);
            string newPathFile = fullPathOriginalFile.Replace(originalDirectory, outputDirectory);
            string cleanNewPathFile = RemoveCipherSuffix(newPathFile);

            string newPathCipherFile = cleanNewPathFile + "_" + Enum.GetName(typeof(CipherState), cipherState);
            ResolveDirectoryIfNoExists(newPathCipherFile);
            return newPathCipherFile;
        }

        public string RemoveCipherSuffix(string fullPathFileName)
        {
            if (fullPathFileName.Contains(ENCRYPTED) || fullPathFileName.Contains(DECRYPTED))
            {            
                int index = fullPathFileName.LastIndexOf('_');

                if (index != -1)
                {
                    string result = fullPathFileName.Substring(0, index);
                    return result;
                }
            }
            return fullPathFileName;
        }

        public string CreateCipherFolderName(string originalDirectory, CipherState cipherState)
        {
            string cleanOriginalDirectory = RemoveCipherSuffix(originalDirectory);
            string outputDirectory = cleanOriginalDirectory + "_" + Enum.GetName(typeof(CipherState), cipherState);
            bool exists = System.IO.Directory.Exists(outputDirectory);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(outputDirectory);
            }
            return outputDirectory;
        }


        private static void ResolveDirectoryIfNoExists(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
        }

    }
}
