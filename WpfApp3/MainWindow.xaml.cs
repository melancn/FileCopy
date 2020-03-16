using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.IO.Log;

namespace FileCopy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string SourcePath = source.Text;
            string DestinationPath = distinct.Text;
            string error_file = "";
            if (SourcePath.Length == 0)
            {
                System.Windows.MessageBox.Show("来源路径错误:"+ SourcePath);
                return;
            }
            if (DestinationPath.Length == 0)
            {
                System.Windows.MessageBox.Show("目标路径错误:"+ DestinationPath);
                return;
            }
            string files = fileList.Text;
            if (files.Length == 0)
            {
                System.Windows.MessageBox.Show(messageBoxText: "文件列表为空");
                return;
            }
            string[] list = files.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string path in list)
            {
                bool res = CopyDirectory(path, SourcePath, DestinationPath, delstart.Text);
                if (res == false)
                {
                    error_file += path+"\r\n";
                }
            }
            if (error_file.Length > 0)
            {
                System.Windows.MessageBox.Show("路径错误:" + error_file);
            }
            System.Windows.MessageBox.Show(messageBoxText: "复制完成！");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => fileList.Text = "";

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                source.Text = openFileDialog.SelectedPath;
            }
        }

        private void distinct_select_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                distinct.Text = openFileDialog.SelectedPath;
            }
        }

        /**
         * Source 相对文件路径
         * SourcePath 源目录
         * DestinationPath 目标目录
         *
         */
        private static bool CopyDirectory(string Source, string SourcePath, string DestinationPath, string delstart = "", bool overwriteexisting = true)
        {
            bool ret = false;
            try
            {
                Source = Source.StartsWith(delstart) ? Source.Substring(delstart.Length) : Source;
                Source = Source.StartsWith("/") ? Source.Substring(1) : Source;
                Source = Source.Replace("/", @"\");
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                var a  =DestinationPath.EndsWith(@"\");
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                
                if (Source.EndsWith(@"\"))
                {
                    throw new Exception("no file");
                }

                string source_file = SourcePath + Source;

                if (File.Exists(source_file))
                {
                    string des_file = DestinationPath + Source;
                    string des_dir = System.IO.Path.GetDirectoryName(des_file);
                    if (Directory.Exists(des_dir) == false)
                        Directory.CreateDirectory(des_dir);

                    FileInfo flinfo = new FileInfo(source_file);
                    flinfo.CopyTo(des_file, overwriteexisting);
                }
                else
                {
                    throw new Exception("no file "+ source_file);
                }
                ret = true;
            }
            catch (Exception ex)
            {
                // System.Windows.MessageBox.Show(ex.ToString());
                ret = false;
            }
            return ret;
        }
    }
}
