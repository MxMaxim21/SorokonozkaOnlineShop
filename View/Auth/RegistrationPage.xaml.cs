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

namespace SorokonozkaApp.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TBoxLastName.Text) || (string.IsNullOrWhiteSpace(TBoxFirstName.Text)))
                { 
                    MessageBox.Show("Укажите фамилию и имя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TBoxLogin.Text))
                {
                    MessageBox.Show("Заполните логин!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (TBoxLogin.Text.Trim().Length < 3)
                {
                    MessageBox.Show("Логин должен содержать минимум 3 символа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (App.Context.Users.Any(p => p.Login == TBoxLogin.Text))
                {
                    MessageBox.Show("Данный логин уже занят!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Entities.Users newUser = new Entities.Users()
                {
                    LastName = TBoxLastName.Text,
                    FirstName = TBoxFirstName.Text,
                    Patronymic = TBoxPatronymic.Text,
                    Login = TBoxLogin.Text,
                    RoleID = 3
                };

                App.Context.Users.Add(newUser);
                App.Context.SaveChanges();

                MessageBox.Show($"Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
