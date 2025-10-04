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
            
            // Chạy RegisterForm để tạo tài khoản Admin ban đầu
            // Sau khi tạo xong, người dùng có thể chuyển sang LoginForm
            Application.Run(new RegisterForm());
        }
    }
}
