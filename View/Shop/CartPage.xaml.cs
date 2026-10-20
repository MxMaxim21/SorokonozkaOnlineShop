using System;
using System.Data;
using System.Data.Entity;
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
using SorokonozkaApp.Models;

namespace SorokonozkaApp.View.Shop
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null) return;

            LViewCart.ItemsSource = Models.CartService.GetItems(App.CurrentUser.UserID);

            UpdateTotal();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShowPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null) return;

            var button = sender as Button;
            var product = button?.Tag as Entities.Products;

            if (product == null) return;

            CartService.Remove(App.CurrentUser.UserID, product);

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            if (App.CurrentUser == null)
            {
                TxtTotal.Text = "Итого: 0 ₽";
                return;
            }

            var cart = CartService.GetItems(App.CurrentUser.UserID);

            decimal total = cart.Sum(p => Convert.ToDecimal(p.Price));

            TxtTotal.Text = $"Итого: {total:N2} ₽";
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                MessageBox.Show("Войдите в аккаунт.");
                return;
            }

            var cart = CartService.GetItems(App.CurrentUser.UserID);

            if (cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста.");
                return;
            }
                try
                {
                    decimal total = 0;

                    var orderProducts = new List<Tuple<Entities.Products, decimal>>();

                    foreach (var item in cart.ToList())
                    {
                        var product = App.Context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);

                        if (product == null)
                        {
                            throw new Exception($"Товар «{item.Title}» больше не существует.");
                        }

                        App.Context.Entry(product).Reload();

                        int stock = Convert.ToInt32(product.Stock);

                        if (stock <= 0)
                        {
                            throw new Exception($"Товара «{product.Title}» нет в наличии.");
                        }

                        decimal price = Convert.ToDecimal(product.Price);

                        if (price < 0)
                        {
                            throw new Exception($"Некорректная цена товара «{product.Title}».");
                        }

                        total += price;
                        orderProducts.Add(Tuple.Create(product, price));
                    }

                    var order = new Entities.Orders
                    {
                        OrderDate = DateTime.Today,
                        UserID = App.CurrentUser.UserID,
                        TotalAmount = total
                    };

                    App.Context.Orders.Add(order);
                    App.Context.SaveChanges();

                    int nextItemId = App.Context.OrderItems
                        .Select(x => x.OrderItemID)
                        .DefaultIfEmpty(0)
                        .Max() + 1;

                    foreach (var entry in orderProducts)
                    {
                        var product = entry.Item1;
                        decimal price = entry.Item2;

                        App.Context.OrderItems.Add(new Entities.OrderItems
                        {
                            OrderID = order.OrderID,
                            ProductID = product.ProductID,
                            Quantity = 1,
                            UnitPrice = price,
                            LineTotal = price
                        });

                        product.Stock = Convert.ToInt32(product.Stock) - 1;
                    }

                    App.Context.SaveChanges();

                    CartService.Clear(App.CurrentUser.UserID);

                    LViewCart.ItemsSource = null;
                    LViewCart.ItemsSource = cart;

                    UpdateTotal();

                    MessageBox.Show($"Заказ №{order.OrderID} успешно оформлен!\n" + $"Сумма заказа: {total:N2} ₽", "Заказ оформлен", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка оформления заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
