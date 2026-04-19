using demo_exam.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace lab2.Views
{
    public partial class Authorization : Window
    {
        DatabaseContext _db;
        public Authorization()
        {
            _db = new DatabaseContext();
            InitializeComponent();
        }

        private void ButtonLogin(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxLogin.Text) && !string.IsNullOrWhiteSpace(BoxPassword.Text))
            {
                var user = _db.Users
                    .Include(x => x.Role)
                    .FirstOrDefault(x => x.Login == BoxLogin.Text && x.Password == BoxPassword.Text);

                if (user != null)
                {
                    Main w = new Main(user);
                    w.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля.");
            }
        }

        private void ButtonLoginGuest(object sender, RoutedEventArgs e)
        {
            Main w = new Main();
            w.Show();
            this.Close();
        }
    }
}
