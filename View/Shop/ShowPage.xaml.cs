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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SorokonozkaApp.View.Shop
{
    /// <summary>
    /// Логика взаимодействия для ShowPage.xaml
    /// </summary>
    public partial class ShowPage : Page
    {
        public ShowPage()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (App.Context == null)
            {
                return;
            }

            try
            {
                var shoes = App.Context.Products.ToList();

                LViewShoes.ItemsSource = shoes;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            var shoes = (sender as Button)?.Tag as Entities.Products;

            if (shoes == null)
            {
                MessageBox.Show("Не удалось определить выбранный товар.");
                return;
            }

            if (App.CurrentUser == null)
            {
                MessageBox.Show("Сначала войдите в аккаунт.");
                return;
            }

            if (shoes.Stock == null || shoes.Stock <= 0)
            {
                MessageBox.Show("Данный товар закончился.");
                return;
            }

            if (MessageBox.Show($"Добавить в корзину «{shoes.Title}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            bool added = Models.CartService.Add(App.CurrentUser.UserID, shoes);

            if (added)
            {
                MessageBox.Show($"Товар «{shoes.Title}» добавлен в корзину!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Этот товар уже находится в корзине.", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
