using System;
using System.Windows.Forms;

namespace CinameAsset
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Khởi chạy với LoginForm
            // Người dùng có thể chuyển sang RegisterForm nếu cần đăng ký
            Application.Run(new LoginForm());
        }
    }
}
