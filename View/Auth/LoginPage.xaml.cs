using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SorokonozkaApp.View;

namespace SorokonozkaApp.View
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TBoxLogin.Text;

            try
            {
                if (string.IsNullOrWhiteSpace(login))
                {
                    MessageBox.Show("Логин не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var currentUser = App.Context.Users.FirstOrDefault(p => p.Login == login);

                if (currentUser != null)
                {
                    MessageBox.Show($"Вы вошли как: {currentUser.Login}!", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    //NavigationService.Navigate(new Page);
                    return;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль! \nАдрес службы поддержки: admin@mail.com", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new RegistrationPage());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
