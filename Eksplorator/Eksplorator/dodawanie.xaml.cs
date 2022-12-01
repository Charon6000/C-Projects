using System;
using System.Collections.Generic;
using System.IO;
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

namespace Eksplorator
{
    /// <summary>
    /// Logika interakcji dla klasy dodawanie.xaml
    /// </summary>
    public partial class dodawanie : Window
    {
        public dodawanie()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string rozszerzenie = ".txt";
            if (_1.IsChecked == true) rozszerzenie = ".txt";
            else if (_2.IsChecked == true) rozszerzenie = ".docx";
            else if (_3.IsChecked == true) rozszerzenie = ".pptx";
            else if (_4.IsChecked == true) rozszerzenie = ".xlsx";
            File.Create(Data.path + "\\" + File_name.Text + rozszerzenie);
            this.Close();
        }
    }
}
