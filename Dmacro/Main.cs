using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dmacro
{
    public partial class Main : Form
    {
        #region DLL Import
        [System.Runtime.InteropServices.DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("User32", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr Parent, IntPtr Child, string lpszClass, string lpszWindows);

        #endregion

        System.Windows.Forms.Timer timer;
        string AppPlayerName = "";
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        int R = 0,G = 0,B=0;
        int pkCnt = 0;
        public Main()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(run);
        }
        private void run(object sender, EventArgs e)
        {
            Bitmap bmp = PrintScreenAndCheck();
            if (bmp == null)
                return;

            try
            {
                // 사각 잘라내서 보여주기, PK 감지 /* 이미지 서치 기능 동작 안해서 주석 처리
                /*
                Rectangle rects = new Rectangle(580, 290, 60, 60);
                var cloned = new Bitmap(bmp).Clone(rects, bmp.PixelFormat);
                pictureBox1.Image = cloned;
                */
                //  PK 감지 텔
                if (checkBox4.Checked)
                {
                    if(bmp.GetPixel(4, 40).R > 60 && bmp.GetPixel(4, 40).G < 10 && bmp.GetPixel(4, 40).B < 10 &&
                    bmp.GetPixel(4, 388).R > 60 && bmp.GetPixel(4, 388).G < 10 && bmp.GetPixel(4, 388).B < 10 &&
                    bmp.GetPixel(630, 388).R > 60 && bmp.GetPixel(630, 388).G < 10 && bmp.GetPixel(630, 388).B < 10 &&
                    bmp.GetPixel(630, 40).R > 60 && bmp.GetPixel(630, 40).G < 10 && bmp.GetPixel(630, 40).B < 10 )
                    {
                        playTel();
                    }

                    /* 이미지 서치 기능 동작 안해서 주석 처리
                    if (searchIMG(cloned, Resource.pk))
                    {
                        if (pkCnt==0)
                        {
                            playTel();
                        }
                        if (pkCnt == 3)
                            pkCnt = 0;
                        pkCnt++;
                    }*/
                }
                // hp20 귀환
                if (checkBox5.Checked)
                {
                    if (R < 175 && G > 10 && B > 10)
                    {
                        playGoHome();
                        checkBox5.Checked = false;
                    }
                }
                // 우편물 받기
                if (checkBox1.Checked)
                {
                    if (DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                    {
                        getPost();
                    }
                }
                // 혈맹 출첵
                if (checkBox2.Checked)
                {
                    if (DateTime.Now.Hour % 12 == 0 && DateTime.Now.Minute == 2 && DateTime.Now.Second == 0)
                    {
                        getGCheck();
                    }
                }
                // 계정 출첵
                if (checkBox3.Checked)
                {
                    if (DateTime.Now.Hour % 12 == 0 && DateTime.Now.Minute == 4 && DateTime.Now.Second == 0)
                    {
                        getACheck();
                    }
                }
                
                
                //lg(string.Format(@"{0} {1} {2}",R,G,B)); // 특정 지점 RGB 값 확인
            }
            catch (System.FormatException w)
            {
                lg(w.Message);
            }
        }

        // Pk시 텔
        private async void playTel()
        {
            lg("PK 감지, 텔 동작");
            await playTelTask();
        }
        private async Task playTelTask()
        {
            await Task.Delay(2000);
            //InClick_y(907, 511);    // 귀환
            InClick_y(561, 352);    // 텔
            await Task.Delay(3000);
        }

        // 저 체력 귀환
        private async void playGoHome()
        {
            lg("저 체력 귀환 시작");
            await getACheckTask();
            lg("저 체력 귀환 종료");
        }
        private async Task playGoHomeTask()
        {
            InClick_y(605, 353);    // 귀환
            //InClick_y(844, 511);    // 텔
            await Task.Delay(1500);
        }

        // 계정 출첵 받기
        private async void getACheck()
        {
            lg("계정 출첵 받기 시작");
            await getACheckTask();
            lg("계정 출첵 받기 종료");
        }
        private async Task getACheckTask()
        {
            InClick_y(619, 55);     // 메뉴
            await Task.Delay(1500);
            InClick_y(642, 398);    // 출첵
            await Task.Delay(3000);

            InClick_y(619, 55);     // 메뉴
            await Task.Delay(2000);
            InClick_y(619, 55);     // 메뉴
            await Task.Delay(1500);
        }

        // 혈맹 출첵 받기
        private async void getGCheck()
        {
            lg("혈맹 출첵 받기 시작");
            await getGCheckTask();
            lg("혈맹 출첵 받기 종료");
        }
        private async Task getGCheckTask()
        {
            InClick_y(619, 55);     // 메뉴
            await Task.Delay(1500);
            InClick_y(467, 163);    // 혈맹
            await Task.Delay(3000);

            InClick_y(606, 121);    // 출첵
            await Task.Delay(3000);

            InClick_y(619, 55);     // 메뉴
            await Task.Delay(2000);
            InClick_y(619, 55);     // 메뉴
            await Task.Delay(2000);
            InClick_y(619, 55);     // 메뉴
            await Task.Delay(1500);
        }

        // 우편물 받기
        private async void getPost()
        {
            lg("우편물 받기 시작");
            await getPostTask();
            lg("우편물 받기 종료");
        }
        private async Task getPostTask()
        {
            InClick_y(615, 55);     // 메뉴
            await Task.Delay(1500);
            InClick_y(500, 200);    // 우편함
            await Task.Delay(1500);
            InClick_y(564, 365);    // 받기
            await Task.Delay(1500);

            InClick_y(615, 55);     // 메뉴
            await Task.Delay(1500);
            InClick_y(615, 55);     // 메뉴
            await Task.Delay(1500);
        }

        //searchIMG에 스크린 이미지와 찾을 이미지를 넣어줄거에요
        public bool searchIMG(Bitmap screen_img, Bitmap find_img)
        {
            //스크린 이미지 선언
            using (Mat ScreenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screen_img))
            //찾을 이미지 선언
            using (Mat FindMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(find_img))
            //스크린 이미지에서 FindMat 이미지를 찾아라
            using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed))
            {
                //찾은 이미지의 유사도를 담을 더블형 최대 최소 값을 선언합니다.
                double minval, maxval = 0;
                //찾은 이미지의 위치를 담을 포인트형을 선업합니다.
                OpenCvSharp.Point minloc, maxloc;
                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                Debug.WriteLine("찾은 이미지의 유사도 : " + maxval);
                
                if (maxval >= 0.8)
                {
                    return true;
                }
            }
            return false;
        }
        public void searchIMG_Click(Bitmap screen_img, Bitmap find_img)
        {
            //스크린 이미지 선언
            using (Mat ScreenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screen_img))
            //찾을 이미지 선언
            using (Mat FindMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(find_img))
            //스크린 이미지에서 FindMat 이미지를 찾아라
            using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed))
            {
                //찾은 이미지의 유사도를 담을 더블형 최대 최소 값을 선언합니다.
                double minval, maxval = 0;
                //찾은 이미지의 위치를 담을 포인트형을 선업합니다.
                OpenCvSharp.Point minloc, maxloc;
                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                Debug.WriteLine("찾은 이미지의 유사도 : " + maxval);

                int offsetY = 35;
                //이미지를 찾았을 경우 클릭이벤트를 발생!!
                if (maxval >= 0.8)
                {
                    InClick(maxloc.X + FindMat.Width / 2, maxloc.Y + FindMat.Height / 2 - offsetY);
                }
            }
        }

        //x,y 값을 전달해주면 클릭이벤트를 발생합니다.
        public void InClick(int x, int y)
        {
            //클릭이벤트를 발생시킬 플레이어를 찾습니다.
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            if (findwindow != IntPtr.Zero)
            {
                //플레이어를 찾았을 경우 클릭이벤트를 발생시킬 핸들을 가져옵니다.
                IntPtr hwnd_child = FindWindowEx(findwindow, IntPtr.Zero, "RenderWindow", "TheRender");
                IntPtr lparam = new IntPtr(x | (y << 16));

                //플레이어 핸들에 클릭 이벤트를 전달합니다.
                SendMessage(hwnd_child, WM_LBUTTONDOWN, 1, lparam);
                SendMessage(hwnd_child, WM_LBUTTONUP, 0, lparam);
            }
        }
        public void InClick_y(int x, int y)
        {
            y -= 35;
            //클릭이벤트를 발생시킬 플레이어를 찾습니다.
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            if (findwindow != IntPtr.Zero)
            {
                //플레이어를 찾았을 경우 클릭이벤트를 발생시킬 핸들을 가져옵니다.
                IntPtr hwnd_child = FindWindowEx(findwindow, IntPtr.Zero, "RenderWindow", "TheRender");
                IntPtr lparam = new IntPtr(x | (y << 16));

                //플레이어 핸들에 클릭 이벤트를 전달합니다.
                SendMessage(hwnd_child, WM_LBUTTONDOWN, 1, lparam);
                SendMessage(hwnd_child, WM_LBUTTONUP, 0, lparam);
            }
        }
        private Bitmap PrintScreenAndCheck()
        {
            IntPtr findwindow = FindWindow(null, AppPlayerName);
            if (findwindow != IntPtr.Zero)
            {
                //찾은 플레이어를 바탕으로 Graphics 정보를 가져옵니다.
                Graphics Graphicsdata = Graphics.FromHwnd(findwindow);

                //찾은 플레이어 창 크기 및 위치를 가져옵니다. 
                Rectangle rect = Rectangle.Round(Graphicsdata.VisibleClipBounds);

                //플레이어 창 크기 만큼의 비트맵을 선언해줍니다.
                Bitmap bmp = new Bitmap(rect.Width, rect.Height);

                //비트맵을 바탕으로 그래픽스 함수로 선언해줍니다.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //찾은 플레이어의 크기만큼 화면을 캡쳐합니다.
                    IntPtr hdc = g.GetHdc();
                    PrintWindow(findwindow, hdc, 0x2);
                    g.ReleaseHdc(hdc);
                }

                // pictureBox1 이미지를 표시해줍니다.
                pictureBox.Image = bmp;

                // 특정 지점 RGB 컬러 저정
                Color clr = bmp.GetPixel(68, 49);
                R = clr.R;
                G = clr.G;
                B = clr.B;

                return bmp;
            }
            else
            {
                //플레이어를 못찾을경우
                lg("플레이어 찾을 수 없음");
                return null;
            }
        }
        private void lg(string str)
        {
            listBox.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss")+" - "+str);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            AppPlayerName = textBox.Text;
            if(AppPlayerName.Length == 0)
            {
                MessageBox.Show("플레이어 이름 입력");
                return;
            }
            lg("시작");
            timer.Start();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            lg("정지");
            timer.Stop();
        }
    }
}
