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
using System.Windows.Shapes;

namespace SorokonozkaApp.View.Shop
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductWindows.xaml
    /// </summary>
    public partial class AddEditProductWindows : Window
    {
        private Entities.Products currentProducts;
        public AddEditProductWindows(Entities.Products products)
        {
            InitializeComponent();
            currentProducts = products;

            if (currentProducts != null)
            {
                TBoxTitle.Text = currentProducts.Title;
                TBoxDescription.Text = currentProducts.Description;
                TBoxPrice.Text = currentProducts.Price.ToString();
                TBoxStock.Text = currentProducts.Stock.ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TBoxTitle.Text) || string.IsNullOrWhiteSpace(TBoxDescription.Text) || string.IsNullOrWhiteSpace(TBoxPrice.Text) || string.IsNullOrWhiteSpace(TBoxStock.Text))
                {
                    MessageBox.Show("Заполните все основные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(TBoxPrice.Text, out decimal price) || !int.TryParse(TBoxStock.Text, out int stock))
                {
                    MessageBox.Show("Цена и количество на складе должны быть числами!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (currentProducts == null)
                {
                    App.Context.Products.Add(currentProducts);
                }

                currentProducts.Title = TBoxTitle.Text.Trim();
                currentProducts.Description = TBoxDescription.Text.Trim();
                currentProducts.Price = price;
                currentProducts.Stock = stock;

                App.Context.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
