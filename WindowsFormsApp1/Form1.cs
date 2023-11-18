using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscUtils;
using DiscUtils.Iso9660;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Xml.Linq;
using WindowsFormsApp1.Properties;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Threading;
using System.ComponentModel.Composition;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        string boot;
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (boot == "UEFI")
            {
                if (MessageBox.Show("请确认重启前已保存所有的项目以免数据丢失\n重启后将进入UEFI固件管理界面，请确认完毕后选择“确认”，否则请选择“取消”", "警告：", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Process.Start("shutdown.exe", "/r /fw /t 00");
                }
            }
            else
            {
                if (MessageBox.Show("请确认重启前已保存所有的项目以免数据丢失\n请确认完毕后选择“确认”，否则请选择“取消”", "警告：", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Process.Start("shutdown.exe", "/r /t 00");
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本功能尚未完成，敬请期待...", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择您要保存到的路径：";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = dialog.SelectedPath;
                textBox1.Text = savePath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox1.Text) || textBox5.Text.Replace(" ", "").Equals(""))
            {
                MessageBox.Show("请输入正确的路径和文件名！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                button7.Text = "正在保存...";
                button7.Enabled = false;
                try
                {
                    progressBar1.Maximum = 11;
                    SaveISOFile(textBox5.Text, textBox1.Text);
                    MessageBox.Show("保存成功，感谢您使用ComPE！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("保存失败！请检查程序是否完整，以及目录是否仍然存在。\n错误原因：\n" + exp, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                button7.Text = "保存文件";
                button7.Enabled = true;
                progressBar1.Value = 0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            f.UpdateLabel("检查更新...");
            CheckUpdate(7042);
            f.UpdateLabel("初始化窗口...");
            comboBox2.SelectedIndex = 0;
            button2.Enabled = false;
            pictureBox3.Location = new Point(pictureBox2.Location.X + pictureBox1.Width - pictureBox3.Width / 2 * 3 + 1, button2.Location.Y + flowLayoutPanel1.Location.Y);
            pictureBox2.Location = new Point(pictureBox2.Location.X, button2.Location.Y + flowLayoutPanel1.Location.Y);
            f.UpdateLabel("获取磁盘列表...");
            textBox1.Text = Environment.CurrentDirectory;
            tabControl1.ItemSize = new Size(0, 1);
            comboBox1.Items.AddRange(GetDiskList());
            f.UpdateLabel("获取启动方案...");
            if (IsUEFI())
            {
                label3.Text = "当前计算机启动方案：UEFI";
                boot = "UEFI";
            }
            else
            {
                label3.Text = "当前计算机启动方案：BIOS";
                boot = "BIOS";
            }
            comboBox1.SelectedIndex = 0;
            f.Hide();
        }
        public string[] GetDiskList() //获取硬盘列表
        {
            List<string> list = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject disk_drive in searcher.Get())
            {
                if (disk_drive["DeviceID"].ToString().Substring(disk_drive["DeviceID"].ToString().Length - 1).Equals(GetDiskNumber(Environment.SystemDirectory.Substring(0, 2)).ToString()) || int.Parse(disk_drive["DeviceID"].ToString().Substring(disk_drive["DeviceID"].ToString().Length - 1)) < 0)
                {

                }
                else
                {
                    list.Add(disk_drive["Caption"].ToString() + ",Size:" + (long.Parse(disk_drive["Size"].ToString()) / 1024 / 1024 / 1024).ToString() + "GB");
                }
            }
            return list.ToArray();
        }

        public static int GetDiskNumber(string partition_letter) // 盘符取磁盘序号
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject disk_drive in searcher.Get())
            {
                foreach (ManagementObject partition in disk_drive.GetRelated("Win32_DiskPartition"))
                {
                    foreach (ManagementObject logical_disk in partition.GetRelated("Win32_LogicalDisk"))
                    {
                        if (logical_disk["Caption"].ToString() == partition_letter)
                        {
                            return Convert.ToInt32(disk_drive["DeviceID"].ToString().Substring(disk_drive["DeviceID"].ToString().Length - 1));
                        }
                    }
                }
            }
            return -1; // not found
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            Regex regex2 = new Regex("Size:.*");
            Match match2 = regex2.Match(comboBox1.Text);
            string result2 = match2.Value;
            label19.Text = result2.Replace("Size:", "");
            Regex regex = new Regex("^(.*?),Size:");
            Match match = regex.Match(comboBox1.Text);
            string result = match.Groups[1].Value;
            var disks = DriveInfo.GetDrives();
            foreach (var disk in disks)
            {
                string par = disk.Name.Substring(0, 2);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject disk_drive in searcher.Get())
                {
                    if (result.Equals(disk_drive["Caption"]))
                    {
                        string dn = disk_drive["DeviceID"].ToString().Substring(disk_drive["DeviceID"].ToString().Length - 1);
                        if (GetDiskNumber(par).ToString().Equals(dn))
                        {
                            comboBox3.Items.Add(par);
                        }
                    }
                    //
                }
            }
            comboBox3.SelectedIndex = 0;
        }


        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("再次警告！写入前请保存好您的U盘或可移动磁盘里的所有内容，以免数据丢失！\n程序运行图中可能出现未响应状态，属于正常现象\n继续请选择“是”，否则请选择“否”", "警告：", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                progressBar1.Maximum = 22;
                button8.Text = "正在制作...";
                button8.Enabled = false;
                string disk = comboBox3.Text.Substring(0, 2);
                string disklabel = textBox2.Text;
                string fmt;
                string cov;
                string active;
                string createpar;
                string formatfs;
                long totalsize = 0;
                progressBar1.Value += 1;

                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (GetDiskNumber(drive.Name.Substring(0, 2)) == GetDiskNumber(disk))
                    {
                        totalsize += drive.TotalSize;
                    }
                }
                long togb = totalsize / 1024 / 1024 / 1024 - 1;
                if (togb <= 32)
                {
                    fmt = "FAT32";
                }
                else
                {
                    fmt = "NTFS";
                }
                progressBar1.Value += 1;
                cov = "CONVERT MBR";
                active = "ACTIVE";
                createpar = "CREATE PAR PRI";
                formatfs = $"FORMAT FS={fmt} QUICK";

                progressBar1.Value += 1;

                string[] ids = { "06", "5a" };
                progressBar1.Value += 1;
                string[] lines = {"SELECT DISK "+GetDiskNumber(disk).ToString(),
                        "CLEAN",
                        cov,
                        createpar,
                        formatfs,
                        active,
                        "ASSIGN LETTER="+disk};
                string scrPath = Environment.CurrentDirectory;
                progressBar1.Value += 1;
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(scrPath, "script.txt"), false))
                {
                    foreach (string line in lines)
                        outputFile.WriteLine(line);
                }
                progressBar1.Value += 1;
                Process p = new Process();
                p.StartInfo.FileName = "Diskpart.exe";
                p.StartInfo.Arguments = "-s script.txt";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    MessageBox.Show("安装失败！请确认磁盘是否仍然存在！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Value = 0;
                    button8.Text = "制作USB启动盘";
                    button8.Enabled = true;
                    p.Close();
                }
                else
                {
                    progressBar1.Value += 1;
                    string[] lines2 = new string[] { };
                    lines2.Append("SELECT DISK " + GetDiskNumber(disk).ToString());
                    lines2.Append("SELECT PAR 1");
                    lines2.Append("SET ID=" + ids[comboBox2.SelectedIndex] + " OVERRIDE");
                    SaveISOFile("Tempfile.iso", Environment.CurrentDirectory);
                    progressBar1.Value += 1;
                    string isoPath = Environment.CurrentDirectory + "\\Tempfile.iso";
                    ExtractISO(isoPath, disk + "\\");
                    progressBar1.Value += 1;
                    string scrPath2 = Environment.CurrentDirectory;
                    progressBar1.Value += 1;
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(scrPath2, "script.txt"), false))
                    {
                        foreach (string line in lines2)
                            outputFile.WriteLine(line);
                    }
                    progressBar1.Value += 1;
                    Process p1 = new Process();
                    p1.StartInfo.FileName = "Diskpart.exe";
                    p1.StartInfo.Arguments = "-s script.txt";
                    p1.StartInfo.UseShellExecute = false;
                    p1.StartInfo.CreateNoWindow = true;
                    p1.Start();
                    p1.WaitForExit();
                    p1.Close();
                    Process po = new Process();
                    po.StartInfo.FileName = "label.exe";
                    po.StartInfo.Arguments = disk + " " + disklabel;
                    po.StartInfo.UseShellExecute = false;
                    po.StartInfo.CreateNoWindow = true;
                    po.Start();
                    po.WaitForExit();
                    po.Close();
                    File.Delete(isoPath);
                    File.Delete(Path.Combine(scrPath2, "script.txt"));
                    MessageBox.Show("安装完成！感谢您使用ComPE！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Value = 0;
                    button8.Text = "制作USB启动盘";
                    button8.Enabled = true;
                }
            }
        }

        //检查更新
        void CheckUpdate(int version)
        {
            try
            {
                string url = "http://update.win-compe.top/update.php";

                // 创建一个新的WebClient实例
                WebClient client = new WebClient();

                // 设置请求头
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                string postString = "number=" + version.ToString();
                byte[] postData = Encoding.ASCII.GetBytes(postString);

                // 发送POST请求，并将字符串作为请求体发送
                byte[] responseData = client.UploadData(url, "POST", postData);
                string result = Encoding.UTF8.GetString(responseData);

                // 将返回的JSON字符串解析为对象
                dynamic jsonObj = JsonConvert.DeserializeObject(result);

                // 获取update值
                string updateValue = jsonObj.update;

                if (updateValue == "1")
                {
                    if (MessageBox.Show("检测到新版本：" + jsonObj.version + "\n是否前往官网下载？", "发现新版本：", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start("https://win-compe.top");
                        Environment.Exit(0);
                    }
                }
            }
            catch
            {
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                label10.Text = "当前模式选择：硬盘仿真模式，启动速度较快";
            }
            if (comboBox2.SelectedIndex == 1)
            {
                label10.Text = "当前模式选择：大容量软盘仿真模式，兼容性较好";
            }
        }
        [DllImport("kernel32.dll")]
        public static extern uint GetFirmwareEnvironmentVariableA(string lpName, string lpGuid, IntPtr pBuffer, uint nSize);

        public static bool IsUEFI()
        {
            uint result = GetFirmwareEnvironmentVariableA("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);
            return result != 1; // 1 means ERROR_INVALID_FUNCTION
        }


        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }
        //保存ISO的函数
        void SaveISOFile(string filename, string dir)
        {
            // 假设合并后的文件名是file1.jpg
            string name = Path.GetFileName(Application.ExecutablePath);
            progressBar1.Value += 1;
            FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
            progressBar1.Value += 1;
            BinaryReader br = new BinaryReader(fs);
            progressBar1.Value += 1;
            // 读取ISO的数据
            _ = br.ReadBytes(1217024);
            byte[] data2 = br.ReadBytes((int)(fs.Length - fs.Position));
            progressBar1.Value += 1;
            // 关闭流
            br.Close();
            progressBar1.Value += 1;
            fs.Close();
            progressBar1.Value += 1;
            // 创建新的文件流和写入器
            FileStream fs1 = new FileStream(dir + "\\" + filename, FileMode.Create, FileAccess.Write);
            progressBar1.Value += 1;
            BinaryWriter bw1 = new BinaryWriter(fs1);
            progressBar1.Value += 1;
            // 写入数据
            bw1.Write(data2);
            progressBar1.Value += 1;
            // 关闭流
            bw1.Close();
            progressBar1.Value += 1;
            fs1.Close();
            progressBar1.Value += 1;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Process.Start("https://win-compe.top");
        }
        // 这个方法用于解压ISO文件
        private void ExtractISO(string toExtract, string folderName)
        {
            // 读取ISO文件
            CDReader Reader = new CDReader(File.Open(toExtract, FileMode.Open), true);
            // 传递根目录，文件夹名称和要解压的文件夹
            ExtractDirectory(Reader.Root, folderName /*+ Path.GetFileNameWithoutExtension(toExtract)*/ + "\\", "");
            // 清除Reader并释放内存
            Reader.Dispose();
        }

        // 这个方法用于递归地解压ISO文件中的每个目录
        private void ExtractDirectory(DiscDirectoryInfo Dinfo, string RootPath, string PathinISO)
        {
            if (!string.IsNullOrWhiteSpace(PathinISO))
            {
                PathinISO += "\\" + Dinfo.Name;
            }
            RootPath += "\\" + Dinfo.Name;
            // 创建目录
            AppendDirectory(RootPath);
            // 遍历每个子目录，并递归地调用该方法
            foreach (DiscDirectoryInfo dinfo in Dinfo.GetDirectories())
            {
                ExtractDirectory(dinfo, RootPath, PathinISO);
            }
            // 遍历每个文件，并将其解压到目标路径
            foreach (DiscFileInfo finfo in Dinfo.GetFiles())
            {
                using (Stream FileStr = finfo.OpenRead())
                {
                    using (FileStream Fs = File.Create(RootPath + "\\" + finfo.Name))
                    {
                        // 将FileStr中的数据复制到Fs中，缓冲区大小为4 * 1024，您可以根据需要修改
                        FileStr.CopyTo(Fs, 4 * 1024);
                    }
                }
            }
        }

        // 这个方法用于创建目录，如果目录不存在或路径过长，则递归地调用该方法
        static void AppendDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (DirectoryNotFoundException)
            {
                AppendDirectory(Path.GetDirectoryName(path));
            }
            catch (PathTooLongException)
            {
                AppendDirectory(Path.GetDirectoryName(path));
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("警告！如果未关闭反病毒软件，程序可能执行失败，建议关闭反病毒软件。\n程序可能无响应，是正常现象，不要强制退出。\n继续请选择“确定”，否则请选择“取消”", "提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    string time;
                    string title;
                    button9.Text = "正在安装...";
                    button9.Enabled = false;
                    if (textBox4.Text.Replace(" ", "") == "")
                    {
                        time = "5";
                    }
                    else
                    {
                        time = textBox4.Text;
                    }
                    if (textBox2.Text.Replace(" ", "") == "")
                    {
                        title = "进入ComPE维护系统";
                    }
                    else
                    {
                        title = textBox2.Text;
                    }
                    string guid1 = "{BEAAB3B9-80C3-13A8-6CF2-F5CC87F4006E}";
                    string guid2 = "{40D2B668-D94D-7EE2-0C07-7A796BB7A1D1}";
                    string systempar = Environment.SystemDirectory.Substring(0, 2);
                    var cmds = new List<string>();
                    if (boot.Equals("BIOS"))
                    {
                        cmds.Add("/create " + guid1 + " /d \"" + title + "\" /application osloader");
                        cmds.Add("/create " + guid2 + " /device");
                        cmds.Add("/set " + guid2 + " ramdisksdidevice partition=\"" + systempar + "\"");
                        cmds.Add("/set " + guid2 + " ramdisksdipath \\sources\\boot.sdi");
                        cmds.Add("/set " + guid1 + " device ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                        cmds.Add("/set " + guid1 + " osdevice ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                        cmds.Add("/set " + guid1 + " path \\windows\\system32\\boot\\winload.exe");
                        cmds.Add("/set " + guid1 + " systemroot \\windows");
                        cmds.Add("/set " + guid1 + " detecthal yes");
                        cmds.Add("/set " + guid1 + " winpe yes");
                        cmds.Add("/displayorder " + guid1 + " /addlast");
                        cmds.Add("/timeout " + time);
                    }
                    else
                    {
                        cmds.Add("/create " + guid1 + " /d \"" + title + "\" /application osloader");
                        cmds.Add("/create " + guid2 + " /device");
                        cmds.Add("/set " + guid2 + " ramdisksdidevice partition=\"" + systempar + "\"");
                        cmds.Add("/set " + guid2 + " ramdisksdipath \\boot\\boot.sdi");
                        cmds.Add("/set " + guid1 + " device ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                        cmds.Add("/set " + guid1 + " osdevice ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                        cmds.Add("/set " + guid1 + " path \\windows\\system32\\boot\\winload.efi");
                        cmds.Add("/set " + guid1 + " systemroot \\windows");
                        cmds.Add("/set " + guid1 + " detecthal yes");
                        cmds.Add("/set " + guid1 + " winpe yes");
                        cmds.Add("/displayorder " + guid1 + " /addlast");
                        cmds.Add("/timeout " + time);
                    }

                    progressBar1.Maximum = 4 + cmds.Count;
                    foreach (string cmd in cmds)
                    {
                        Process p = new Process();
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.FileName = "bcdedit.exe";
                        p.StartInfo.Arguments = cmd;
                        p.Start();
                        p.WaitForExit();
                        if (p.ExitCode != 0)
                        {
                            throw new Exception();//手动抛出异常以终止操作
                        }
                        else
                        {
                            progressBar1.Value += 1;
                        }

                    }
                    SaveISOFile("Tempfile.iso", Environment.CurrentDirectory);
                    string isoPath = Environment.CurrentDirectory + "\\Tempfile.iso";
                    progressBar1.Value += 1;
                    ExtractISO(isoPath, systempar + "\\");
                    progressBar1.Value += 1;
                    string[] uninstall = { "bcdedit >nul",
                                        "if not ERRORLEVEL 1 goto uacOK",
                                        "%1 powershell -Command \"Start-Process -FilePath %~n0 -Verb runAs\"&exit",
                                        ":uacOK",
                                        "@echo off",
                                        "set name =%~n0",
                                        ":1",
                                        "cls",
                                        "set /p var = \"确定要卸载ComPE吗？(Y/N):\"",
                                        "if /i %var%==y goto y",
                                        "if /i %var%==n goto n",
                                        ":2",
                                        "goto 1",
                                        ":y",
                                        "bcdedit /delete "+guid1+" /cleanup",
                                        "bcdedit /delete "+guid2,
                                        "del /s /f /q %systemdrive%\\sources",
                                        "@echo 卸载完成！感谢您使用ComPE，程序将自动退出...",
                                        "start \"cmd /c del /s /f /q %name%\"",
                                        "exit",
                                        ":n",
                                        "exit" }; //创建卸载的批处理程序，并且添加调用Powershell的命令申请管理员运行
                    string scrPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    progressBar1.Value += 1;
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(scrPath, "卸载ComPE.bat")))
                    {
                        foreach (string line in uninstall)
                            outputFile.WriteLine(line);
                    }
                    MessageBox.Show("安装成功！感谢您使用ComPE！。", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Value = 0;
                    button9.Text = "安装到系统磁盘";
                    button9.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("安装失败！请检查是否还有空间。", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Value = 0;
                    button9.Text = "安装到系统磁盘";
                    button9.Enabled = true;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(GetDiskList());
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show("未找到可移动磁盘，请确认是否正确连接后再次刷新。", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Form f3 = new Form3();
            f3.ShowDialog();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                MoveBlock(button2.Location.Y + flowLayoutPanel1.Location.Y);
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                MoveBlock(button1.Location.Y + flowLayoutPanel1.Location.Y);
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                //pictureBox2.Location = new Point(pictureBox2.Location.X, button3.Location.Y + flowLayoutPanel1.Location.Y);
                //pictureBox3.Location = new Point(pictureBox2.Location.X + pictureBox1.Width - pictureBox3.Width / 2 * 3 + 1, button3.Location.Y + flowLayoutPanel1.Location.Y);
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = false;;
                MoveBlock(button3.Location.Y + flowLayoutPanel1.Location.Y);
            }
        }
        int v;
        int ya;
        int a = 0;
        void MoveBlock(int y)
        {
            int y0 = pictureBox2.Top;
            int dy = y - y0;
            double vb = dy / 60;
            int v0 = (int)(2 * vb);
            v = v0;
            if (v > 0)
            {
                a = 1;
            }
            else if (v < 0)
            {
                a = -1;
            }
            timer1.Enabled = true;
            timer2.Enabled = true;
            ya = y;
        }

        void toMove()
        {
            v += a;
            pictureBox2.Top += v;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            toMove();
            pictureBox3.Top = pictureBox2.Top;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            pictureBox2.Top = ya;
            pictureBox3.Top = pictureBox2.Top;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
