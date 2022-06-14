using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace WindowManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool manageWindows;
        public MainWindow()
        {
            InitializeComponent();
            if(RegisterHotKey(1, 18, 81)) {
                Trace.WriteLine("Registered hotkey");
            } else {
                Trace.WriteLine("Unable to register hotkey");
            }
            
            while (manageWindows)
            {
                POINT cursorLocation = GetCursorPosition();
                Trace.WriteLine("Cursor position: " + cursorLocation);
                IntPtr currentWindow = GetAncestor(WindowFromPoint(cursorLocation), 2);

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void btn_Arrange_Click(object sender, RoutedEventArgs e)
        {
            if(manageWindows) {
                manageWindows = false;
            } else {
                manageWindows = true;
            }
        }

        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == 1)
            {
                a++;
                MessageBox.Show(a.ToString());
            }
            base.WndProc(ref m);
        }

        #region Imports
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr GetAncestor(IntPtr hwnd, int flag);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hwnd, int x, int y, int cx, int cy);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(int id, int fsModifiers, int vk);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        #endregion

    }


}
