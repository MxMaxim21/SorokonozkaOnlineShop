using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SorokonozkaApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.ShoesDBEntities Context
        {  get; set; } = new Entities.ShoesDBEntities();

        public static Entities.Users CurrentUser = null;
    }
}
