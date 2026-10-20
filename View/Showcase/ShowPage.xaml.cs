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

namespace SorokonozkaApp.View.Showcase
{
    /// <summary>
    /// Логика взаимодействия для ShowPage.xaml
    /// </summary>
    public partial class ShowPage : Page
    {
        public ShowPage()
        {
            this.DataContext = this;

            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
