using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace SpeedTestTool
{
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            lblPrompt.Text = "";
            //获得系统盘符,并将其显示在combobox里, 以供选择
            DriveInfo[] allDrivers = DriveInfo.GetDrives();
            foreach(DriveInfo d in allDrivers)
            {
                cmbPartition.Items.Add(d.Name);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string surPath, desPath,ts,ts_h,ts_m,ts_s,strPartition;
            double t,speed;
            DateTime dt1, dt2;

            btnStart.Enabled = false;
          
            if (cmbPartition.SelectedIndex== -1)
            {
                //如果没有选择盘符, 则给出错误信息
                MessageBox.Show("Please Select Target Partition firstly!","Error Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {               
                strPartition = cmbPartition.SelectedItem.ToString();//获得选择的系统盘符
                //MessageBox.Show(strPartition);

                //定义源文件的路径
                surPath = Directory.GetCurrentDirectory() + "\\1.zip";
                //MessageBox.Show(surPath);

                //定义目标文件夹的路径
                desPath = strPartition + "test"; 
                //MessageBox.Show(desPath);

                //判断目标文件夹是否存在,如果不存在,就创建目标文件夹
                if (!Directory.Exists(desPath))
                {
                    try
                    {
                        Directory.CreateDirectory(desPath);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }

                //判断目标文件是否存大,如果存在,就删除.
                if (File.Exists(desPath + "\\1.zip"))
                {
                    try
                    {
                        File.Delete(desPath + "\\1.zip");
                    }
                    catch(Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }


                try
                {
                    //开始复制文件之前,获得系统当前时间,将在txtBeginTime里显示出来.
                    dt1 = DateTime.Now;
                    txtBeginTime.Text = dt1.ToLongTimeString();

                    //复制文件
                    File.Copy(surPath, desPath + "\\1.zip");
                   
                    //复制文件成功后,获得系统当前时间,将在txtEndTime里显示出来.
                    dt2 = DateTime.Now;
                    txtEndTime.Text = dt2.ToLongTimeString();

                    //计算间隔时间,并在txtInterval中显示出来
                    ts = (dt2 - dt1).ToString();
                    string[] strs = ts.Split(':');
                    ts_h = strs[0];
                    ts_m = strs[1];
                    ts_s = strs[2];
                    t = Convert.ToDouble(ts_s) + Convert.ToDouble(ts_m) * 60 + Convert.ToDouble(ts_h) * 3600;
                    txtInterval.Text = (Convert.ToInt32(t)).ToString() + " s";

                    //得到文件的大小, 并在txtFileSize中显示出来
                    FileInfo fi = new FileInfo(desPath + "\\1.zip");
                    txtFileSize.Text = ((fi.Length) / (1024 * 1024)).ToString()+ " MB";

                    //计算速度并显示在txtSpeed中.
                    speed = ((fi.Length) / (1024 * 1024)) / t;
                    txtSpeed.Text = (Convert.ToInt32(speed)).ToString()+" MB/s";

                    //没完成的时候,给出提示
                    lblPrompt.ForeColor = Color.Green;
                    lblPrompt.Text = "Complete!";

                    //让start按钮可以用
                    btnStart.Enabled = true;

                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message,"Error Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }               
                
            }
            
            
        }
    }
}
