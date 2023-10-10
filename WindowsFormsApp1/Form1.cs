using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscUtils.Iso9660;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            MessageBox.Show("敬请期待...", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                }catch(Exception exp)
                {
                    MessageBox.Show("保存失败！请检查程序是否完整，以及目录是否仍然存在。\n错误原因：\n"+exp, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                button7.Text = "保存文件";
                button7.Enabled = true;
                progressBar1.Value = 0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            var alldrives = DriveInfo.GetDrives();
            foreach (var drive in alldrives)
            {
                if (!GetDiskNumber(drive.Name.Substring(0,2)).Equals(GetDiskNumber(Environment.SystemDirectory.Substring(0, 2))) && !GetDiskNumber(drive.Name.Substring(0, 2)).Equals(-1)) {
                    comboBox1.Items.Add(drive.Name.Substring(0, 2) + "      磁盘序号："+GetDiskNumber(drive.Name.Substring(0, 2)));
                }
            }
            if (IsUEFI())
            {
                label3.Text = "当前计算机启动方案：UEFI";
                boot = "UEFI";
                label3.Location = new Point(475, 412);
            }
            else
            {
                label3.Text = "当前计算机启动方案：Legacy/BIOS";
                boot = "BIOS";
                label3.Location = new Point(435, 412);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = false;
            }
        }
        int GetDiskNumber(string driveLetter)
        {
            // 获取磁盘信息
            DriveInfo driveInfo = new DriveInfo(driveLetter);
            // 如果磁盘不存在，抛出异常
            if (!driveInfo.IsReady)
            {
                return -1;
            }
            // 获取磁盘的卷标
            string volumeLabel = driveInfo.VolumeLabel;
            // 遍历所有的磁盘，按序号排序
            DriveInfo[] drives = DriveInfo.GetDrives();
            Array.Sort(drives, (x, y) => x.Name.CompareTo(y.Name));
            // 从0开始计数，找到与卷标匹配的磁盘，返回其序号
            int diskNumber = 0;
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady && drive.VolumeLabel == volumeLabel)
                {
                    return diskNumber;
                }
                diskNumber++;
            }
            // 如果没有找到匹配的磁盘，抛出异常
            return -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("再次警告！写入前请保存好您的U盘或可移动磁盘里的所有内容，以免数据丢失！\n继续请选择“是”，否则请选择“否”", "警告：", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                progressBar1.Maximum = 12;
                button8.Text = "正在制作";
                button8.Enabled = false;
                string disk = comboBox1.Text.Substring(0, 2);
                string disklabel = textBox2.Text;
                string fmt;
                progressBar1.Value += 1;
                long totalsize = 0;
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (GetDiskNumber(drive.Name.Substring(0,2)) == GetDiskNumber(disk))
                    {
                        totalsize += drive.TotalSize;
                    }
                }
                progressBar1.Value += 1;
                long togb = totalsize / 1024 / 1024 / 1024;
                if(togb <= 32)
                {
                    fmt = "FAT32";
                }
                else
                {
                    fmt = "NTFS";
                }
                progressBar1.Value += 1;
                string[] ids = { "06", "5a" };
                progressBar1.Value += 1;
                string[] lines = {"SELECT DISK "+GetDiskNumber(disk).ToString(),
                        "CLEAN",
                        "CONVERT "+cov,
                        "CREATE PAR PRI",
                        "FORMAT FS=" + fmt + " LABLE="+disklabel+" QUICK",
                        "SET ID=" + ids[comboBox2.SelectedIndex] + " override",
                        "ACTIVE",
                        "ASSIGN LETTER="+disk};
                string scrPath = Environment.CurrentDirectory;
                progressBar1.Value += 1;
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(scrPath, "script.txt")))
                {
                    foreach (string line in lines)
                        outputFile.WriteLine(line);
                }
                progressBar1.Value += 1;
                Process p = Process.Start("Diskpart.exe", "-s script.txt");
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    MessageBox.Show("安装失败！请确认磁盘是否仍然存在！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Value = 0;
                    button8.Text = "制作USB启动盘";
                    button8.Enabled = true;
                }
                else
                {
                    progressBar1.Value += 1;
                    SaveISOFile("Tempfile.iso", Environment.CurrentDirectory);
                    string isoPath = Environment.CurrentDirectory+"\\Tempfile.iso";
                    progressBar1.Value += 1;
                    // 创建一个CDReader对象来读取ISO文件
                    using (CDReader cd = new CDReader(File.Open(isoPath, FileMode.Open), true))
                    {
                        // 获取ISO文件中的所有文件和目录
                        string[] files = cd.GetFiles("", "*", SearchOption.AllDirectories);
                        string[] dirs = cd.GetDirectories("", "*", SearchOption.AllDirectories);
                        progressBar1.Value += 1;
                        // 如果目标目录不存在，就创建它
                        // 在目标目录中创建子目录
                        foreach (string dir in dirs)
                        {
                            string subDir = Path.Combine(disk + "\\", dir);
                            if (!Directory.Exists(subDir))
                            {
                                Directory.CreateDirectory(subDir);
                            }
                        }
                        progressBar1.Value += 1;
                        // 将ISO文件中的文件复制到目标目录中
                        foreach (string file in files)
                        {
                            string srcFile = Path.Combine(isoPath, file);
                            string destFile = Path.Combine(disk + "\\", file);
                            using (Stream input = cd.OpenFile(file, FileMode.Open))
                            using (Stream output = File.Create(destFile))
                            {
                                input.CopyTo(output);
                            }
                        }
                    }
                    progressBar1.Value += 1;
                    File.Delete(isoPath);
                    MessageBox.Show("安装完成！感谢您使用ComPE！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Value = 0;
                    button8.Text = "制作USB启动盘";
                    button8.Enabled = true;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                label10.Text = "硬盘仿真模式，启动速度较快";
            }
            if (comboBox2.SelectedIndex == 1)
            {
                label10.Text = "大容量软盘仿真模式，兼容性较好";
            }
        }
        [DllImport("kernel32.dll")]
        public static extern uint GetFirmwareEnvironmentVariableA(string lpName, string lpGuid, IntPtr pBuffer, uint nSize);

        public static bool IsUEFI()
        {
            uint result = GetFirmwareEnvironmentVariableA("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);
            return result != 1; // 1 means ERROR_INVALID_FUNCTION
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }
        //保存ISO的函数
        void SaveISOFile(string filename,string dir)
        {
            // 假设合并后的文件名是file1.jpg，分界点是100KB
            string name = Path.GetFileName(Application.ExecutablePath);
            progressBar1.Value += 1;
            FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
            progressBar1.Value += 1;
            BinaryReader br = new BinaryReader(fs);
            progressBar1.Value += 1;
            // 读取ISO的数据
            _ = br.ReadBytes(49664);
            byte[] data2 = br.ReadBytes((int)(fs.Length - fs.Position));
            progressBar1.Value += 1;
            // 关闭流
            br.Close();
            progressBar1.Value += 1;
            fs.Close();
            progressBar1.Value += 1;
            // 创建新的文件流和写入器
            FileStream fs1 = new FileStream(dir+"\\"+filename, FileMode.Create, FileAccess.Write);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://win-compe.top");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("警告！如果未关闭反病毒软件，程序可能执行失败，建议关闭反病毒软件。继续请选择“确定”，否则请选择“取消”", "提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try {
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
                if(textBox2.Text.Replace(" ", "") == "")
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
                    cmds.Add("bcdedit /create " + guid1 + " /d \"" + title + "\" /application osloader");
                    cmds.Add("bcdedit /create " + guid2 + " /device");
                    cmds.Add("bcdedit /set " + guid2 + " ramdisksdidevice partition=\"" + systempar + "\"");
                    cmds.Add("bcdedit /set " + guid2 + " ramdisksdipath \\sources\\boot.sdi");
                    cmds.Add("bcdedit /set " + guid1 + " device ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                    cmds.Add("bcdedit /set " + guid1 + " osdevice ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                    cmds.Add("bcdedit /set " + guid1 + " path \\windows\\system32\\boot\\winload.exe");
                    cmds.Add("bcdedit /set " + guid1 + " systemroot \\windows");
                    cmds.Add("bcdedit /set " + guid1 + " detecthal yes");
                    cmds.Add("bcdedit /set " + guid1 + " winpe yes");
                    cmds.Add("bcdedit /displayorder " + guid1 + " /addlast");
                    cmds.Add("bcdedit /timeout " + time);
                }
                else
                {
                    cmds.Add("bcdboot %systemroot% /s " + systempar + " /f UEFI");
                    cmds.Add("bcdedit /create " + guid1 + " /d \""+title+"\" /application osloader");
                    cmds.Add("bcdedit /create " + guid2 + " /device");
                    cmds.Add("bcdedit /set " + guid2 + " ramdisksdidevice partition=\"" + systempar + "\"");
                    cmds.Add("bcdedit /set " + guid2 + " ramdisksdipath \\boot\\boot.sdi");
                    cmds.Add("bcdedit /set " + guid1 + " device ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                    cmds.Add("bcdedit /set " + guid1 + " osdevice ramdisk=\"[" + systempar + "]\\sources\\boot.wim," + guid2);
                    cmds.Add("bcdedit /set " + guid1 + " path \\windows\\system32\\boot\\winload.efi");
                    cmds.Add("bcdedit /set " + guid1 + " systemroot \\windows");
                    cmds.Add("bcdedit /set " + guid1 + " detecthal yes");
                    cmds.Add("bcdedit /set " + guid1 + " winpe yes");
                    cmds.Add("bcdedit /displayorder " + guid1 + " /addlast");
                    cmds.Add("bcdedit /timeout " +time);
                }
                progressBar1.Maximum = 4 + cmds.Count;
                foreach(string cmd in cmds)
                {
                    Process p = Process.Start("cmd", "/c " + cmd);
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                            throw new Exception();
                    }
                    else
                    {
                        progressBar1.Value += 1;
                    }
                    
                }
                        SaveISOFile("Tempfile.iso", Environment.CurrentDirectory);
                        string isoPath = Environment.CurrentDirectory + "\\Tempfile.iso";
                        progressBar1.Value += 1;
                        // 创建一个CDReader对象来读取ISO文件
                        using (CDReader cd = new CDReader(File.Open(isoPath, FileMode.Open), true))
                        {
                            // 获取ISO文件中的所有文件和目录
                            string[] files = cd.GetFiles("", "*", SearchOption.AllDirectories);
                            string[] dirs = cd.GetDirectories("", "*", SearchOption.AllDirectories);
                            progressBar1.Value += 1;
                            // 如果目标目录不存在，就创建它
                            // 在目标目录中创建子目录
                            foreach (string dir in dirs)
                            {
                                string subDir = Path.Combine(Environment.SystemDirectory.Substring(0, 3), dir);
                                if (!Directory.Exists(subDir))
                                {
                                    Directory.CreateDirectory(subDir);
                                }
                            }
                            progressBar1.Value += 1;
                            // 将ISO文件中的文件复制到目标目录中
                            foreach (string file in files)
                            {
                                string srcFile = Path.Combine(isoPath, file);
                                string destFile = Path.Combine(Environment.SystemDirectory.Substring(0, 3) + "\\", file);
                                using (Stream input = cd.OpenFile(file, FileMode.Open))
                                using (Stream output = File.Create(destFile))
                                {
                                    input.CopyTo(output);
                                }
                            }
                            progressBar1.Value += 1;
                        MessageBox.Show("安装完成！感谢您使用ComPE！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar1.Value = 0;
                        button9.Text = "安装到系统磁盘";
                        button9.Enabled = true;
                    }
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
    }
}
