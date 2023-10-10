using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        [SecurityCritical]
        [DllImport("ntdll.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVERSIONINFOEX
        {
            // The OSVersionInfoSize field must be set to Marshal.SizeOf(typeof(OSVERSIONINFOEX))
            internal int OSVersionInfoSize;
            internal int MajorVersion;
            internal int MinorVersion;
            internal int BuildNumber;
            internal int PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string CSDVersion;
            internal ushort ServicePackMajor;
            internal ushort ServicePackMinor;
            internal short SuiteMask;
            internal byte ProductType;
            internal byte Reserved;
        }
        public Form3()
        {
            InitializeComponent();
        }
        public static long GetTotalPhysicalMemory()
        {
            long capacity = 0;
            try
            {
                foreach (ManagementObject mo1 in new ManagementClass("Win32_PhysicalMemory").GetInstances())
                    capacity += long.Parse(mo1.Properties["Capacity"].Value.ToString());
            }
            catch
            {
                capacity = -1;
            }
            return capacity;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            string osv;
            var osVersionInfo = new OSVERSIONINFOEX { OSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
            if (RtlGetVersion(ref osVersionInfo) != 0)
            {
                // 错误处理
                osv = "RtlGetVersion Error!";
            }
            else {
                if (osVersionInfo.BuildNumber >= 22000)
                {
                    osv = "Microsoft Windows 11";
                }
                else
                {
                    osv = "Microsoft Windows " + osVersionInfo.MajorVersion;
                }
            }
            string osb = osVersionInfo.MajorVersion.ToString()+"."+osVersionInfo.MinorVersion.ToString()+"."+osVersionInfo.BuildNumber.ToString();
            string s = null;
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            if (moc.Count != 0)
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    s = mo["Manufacturer"].ToString();
                }
            }
            ManagementClass mcbs = new ManagementClass("Win32_BIOS");
            ManagementObjectCollection mocbs = mcbs.GetInstances();
            string strID = null;
            string bsname = null;
            foreach (ManagementObject mo in mocbs)
            {
                strID = mo.Properties["SMBIOSBIOSVersion"].Value.ToString();
                bsname = mo.Properties["Name"].Value.ToString();
                break;
            }
            string CPUName = "";
            ManagementObjectSearcher moscpu = new ManagementObjectSearcher("Select * from Win32_Processor");//Win32_Processor  CPU处理器
            foreach (ManagementObject mo in moscpu.Get())
            {
                CPUName = mo["Name"].ToString();
            }
            moscpu.Dispose();
            listView1.Items.Add("ComPE工具箱版本：7.0.4.2");
            listView1.Items.Add("OS系统：" + osv);
            listView1.Items.Add("OS版本号：" + osb);
            listView1.Items.Add("计算机名：" + Environment.MachineName);
            listView1.Items.Add("系统登录用户名：" + Environment.UserName);
            listView1.Items.Add("主板厂商：" + s);
            listView1.Items.Add("BIOS版本：" + strID);
            listView1.Items.Add("BIOS名称：" + bsname);
            listView1.Items.Add("物理内存大小：" + (GetTotalPhysicalMemory()/1024/1024/1024).ToString()+"GB");
            listView1.Items.Add("CPU型号：" + CPUName);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("start https://win-compe.top");
        }

        
    }
}
