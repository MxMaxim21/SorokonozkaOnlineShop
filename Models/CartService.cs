using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SorokonozkaApp.Entities;

namespace SorokonozkaApp.Models
{
    public static class CartService
    {
        private static readonly Dictionary<int, ObservableCollection<Products>>
            UserCarts = new Dictionary<int, ObservableCollection<Products>>();

        public static ObservableCollection<Products> GetItems(int userId)
        {
            if (!UserCarts.ContainsKey(userId))
                UserCarts[userId] = new ObservableCollection<Products>();

            return UserCarts[userId];
        }

        public static bool Add(int userId, Products product)
        {
            var items = GetItems(userId);

            if (items.Any(x => x.ProductID == product.ProductID))
                return false;

            items.Add(product);
            return true;
        }

        public static void Remove(int userId, Products product)
        {
            GetItems(userId).Remove(product);
        }

        public static void Clear(int userId)
        {
            GetItems(userId).Clear();
        }
    }
}