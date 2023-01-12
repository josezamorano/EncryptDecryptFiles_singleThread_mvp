using FileEncryptDecrypt.DomainLayer;
using FileEncryptDecrypt.Utils.Enumerations;
using FileEncryptDecrypt.Utils.Messages;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace FileEncryptor
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog folderBrowserDialog;
        private CryptographyManager cryptographyManager;
        private static string WARNING_MESSAGE = "WARNING: No Files in Folder!";  
        private static string WARNING_PWDS_DISCREPANCY = "WARNING: Password and Confirm Password are discrepant!";
        public Form1()
        {
            InitializeComponent();
            folderBrowserDialog = new FolderBrowserDialog();
            cryptographyManager = new CryptographyManager();
            SetLabelWarning();
            SetFolderBrowserDialog();
        }
        private void SetFolderBrowserDialog()
        {
            folderBrowserDialog.ShowHiddenFiles = false;
            folderBrowserDialog.Description = Notification.SELECT_A_FOLDER;// "Select a Folder";
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.UseDescriptionForTitle = true;
            folderBrowserDialog.ShowHiddenFiles = true;
        }
        private void SetLabelWarning() 
        {
            lblWarningEncryptEmptyFolder.Text = string.Empty;
        }

        private void btnFolderBrowserEncrypt_Click(object sender, EventArgs e)
        {
            SetLabelWarning();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                string selectedEncryptFolderPath = folderBrowserDialog.SelectedPath;
                int totalEncryptFiles = cryptographyManager.GetAllFiles(selectedEncryptFolderPath);
                txtEncryptFolderName.Text = selectedEncryptFolderPath;
                lblTotalEncryptedFilesCount.Text = totalEncryptFiles.ToString();
                lblWarningEncryptEmptyFolder.Text = (totalEncryptFiles == 0) ? Notification.WARNING_MESSAGE : string.Empty;
                
            }
            folderBrowserDialog.Dispose();
        
        }

        private void btnFolderBrowserDecrypt_Click(object sender, EventArgs e)
        {
            SetLabelWarning();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string selecteddecryptFolderPath = folderBrowserDialog.SelectedPath;
                int totalDecryptFiles = cryptographyManager.GetAllFiles(selecteddecryptFolderPath);
                txtDecryptFolderName .Text = selecteddecryptFolderPath;
                lblTotalDecryptedFilesCount.Text = totalDecryptFiles.ToString();
                lblWarningDecryptEmptyFolder.Text = (totalDecryptFiles == 0) ? Notification.WARNING_MESSAGE : string.Empty;

            }
            folderBrowserDialog.Dispose();
        }

        private void txtEncryptionPwd_TextChanged(object sender, EventArgs e)
        {
            lblEncryptionPwdDiscrepancy.Text = string.Empty;
        }

        private void txtConfirmEncryptionPwd_TextChanged(object sender, EventArgs e)
        {
            lblEncryptionPwdDiscrepancy.Text += string.Empty;
        }

        private void txtDecryptionPwd_TextChanged(object sender, EventArgs e)
        {
            lblDecryptionPwdDiscrepancy.Text += string.Empty;
        }

        private void txtDecryptionConfirmPwd_TextChanged(object sender, EventArgs e)
        {
            lblDecryptionPwdDiscrepancy.Text += string.Empty;
        }

        private void btnEncryptFiles_Click(object sender, EventArgs e)
        {
            if(txtEncryptionPwd.Text.Trim() != txtEncryptionConfirmPwd.Text.Trim())
            {
                lblEncryptionPwdDiscrepancy.Text = Notification.WARNING_PWDS_DISCREPANCY;//  WARNING_PWDS_DISCREPANCY;
                return;
            }
            string report = cryptographyManager.EncryptAllFiles(txtEncryptionPwd.Text);
            MessageBox.Show(report);
        }

        private void btnDecryptFiles_Click(object sender, EventArgs e)
        {
            if (txtDecryptionPwd.Text.Trim() != txtDecryptionConfirmPwd.Text.Trim())
            {
                lblDecryptionPwdDiscrepancy.Text = Notification.WARNING_PWDS_DISCREPANCY;// WARNING_PWDS_DISCREPANCY;
                return;
            }
            string report = cryptographyManager.DecryptAllFiles(txtDecryptionPwd.Text);
            MessageBox.Show(report);
        } 
       
    }
}