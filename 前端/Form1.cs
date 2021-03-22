using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 前端
{
	public partial class Form1 : Form
	{
        System.Timers.Timer atimer;//自定义timer 这个timer会在interval到期而触发事件还未执行完毕时，创建一个新的线程来执行新的触发事件
        FilterInfoCollection videoDevices;//设备集合
        VideoCaptureDevice videoSource;//设备源
        public Form1()
		{
			InitializeComponent();
            cameraOn();//打开摄像头
            InitTimer();//初始化timer
		}
        //麦克风图标 点击会有开麦和闭麦的图片切换
		int voiceclick = 0;
		private void button1_Click(object sender, EventArgs e)
		{
			if(voiceclick == 0)
			{
				voiceclick = 1;
				button1.BackgroundImage = Properties.Resources._1;
			}
			else if(voiceclick == 1)
			{
				voiceclick = 0;
				button1.BackgroundImage = Properties.Resources._0;
			}
		}
        //打开摄像头
        private void cameraOn() {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);//获取所有设备
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);//连接摄像头
            //多个摄像头时无法选择，默认使用程序识别出的第一个摄像头
            this.videoSourcePlayer1.VideoSource = videoSource;//videoSourcePlayer1是显示视频画面的一个Aforge控件
            this.videoSourcePlayer1.Start();//启动
        }
        //关闭摄像头
        private void cameraOff() {
            this.videoSourcePlayer1.SignalToStop();
        }
        //关闭窗口时关闭摄像头
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cameraOff();
        }
        //获取当前帧图片
        private void takePhoto()
        {
            //定义文件名
            string imgName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string path = @"D:\videocamera\WindowsFormsApp1\images\" + imgName + ".jpg";
            if (videoSourcePlayer1 == null)
            {
                return;
            }
            //获取当前帧图片
            Bitmap bitmap = this.videoSourcePlayer1.GetCurrentVideoFrame();
            //保存图片
            bitmap.Save(path);
        }
        //初始化自定义timer
        private void InitTimer()
        {
            //设置定时间隔(毫秒为单位)
            int interval = 100;
            atimer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            atimer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            atimer.Enabled = true;
            //绑定Elapsed事件
            atimer.Elapsed += new System.Timers.ElapsedEventHandler(callback);

        }
        //设置回调函数
        private void callback(object sender, System.Timers.ElapsedEventArgs e)
        {
            takePhoto();
        }
        //开始拍照----定时器启动
        private void button7_Click(object sender, EventArgs e)
        {
            atimer.Start();
        }
        //停止拍照----定时器关闭
        private void button8_Click(object sender, EventArgs e)
        {
            atimer.Stop();
        }
        //使视频镜像
        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            if (image != null)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }

        
    }
}
