using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrashCreater
{
    public partial class Form1 : Form
    {

        string folderPath;
        string[] fileNames;
        string[] fileSuffixes;
        FileStream fileStream;

        public Form1()
        {
            InitializeComponent();
            fileNames = new string[] { "学习资料", "小电影", "我的照片", "神秘文件", "电子书", "学习笔记" };
            fileSuffixes = new string[] {".jpg", ".pdf", ".exe", ".mp4", ".mp3", ".avi", ".docx", ".png" };
        }

        private void pathButton_Click(object sender, EventArgs e)
        {
            //选择文件
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            if(browserDialog.ShowDialog() == DialogResult.OK){
                Random random = new Random();
                folderPath = browserDialog.SelectedPath;
                pathBox.Text = folderPath;
                string guid = Guid.NewGuid().ToString().Substring(0, 5);
                string fullPath = folderPath + @"\" + fileNames[random.Next(0, fileNames.Length - 1)]
                    + guid + fileSuffixes[random.Next(0, fileSuffixes.Length - 1)];
                fileStream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write);
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if(fileStream != null && Regex.IsMatch(fileSizeBox.Text, @"\d+"))
            {
                Random random = new Random();
                int fileSize = Convert.ToInt32(fileSizeBox.Text);
                int bufferSize = 1024 * 1024 * 50;
                int haveWrite = 0;
                byte[] buffer = new byte[bufferSize];
                while(haveWrite < fileSize)
                {
                    random.NextBytes(buffer);
                    //尚未写入大小
                    if(fileSize - haveWrite < bufferSize)
                    {
                        fileStream.Write(buffer, 0, fileSize - haveWrite);
                        haveWrite = fileSize;
                    }
                    else
                    {
                        fileStream.Write(buffer, 0, bufferSize);
                        haveWrite += bufferSize;
                    }
                    fileStream.Flush();
                }
                MessageBox.Show("生成完成");
                pathBox.Text = "";
                fileStream.Dispose();
                fileStream = null;
            }
            else
            {
                MessageBox.Show("尚未选择文件");
            }
        }
    }
}
