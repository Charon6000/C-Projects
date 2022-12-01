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
using System.IO;
using static System.Net.WebRequestMethods;

namespace Eksplorator
{
    public static class Data
    {
        public static string path;
        public static MainWindow window;
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.window = this;
        }

        public void PobierzDane(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;

            try
            {
                if (item.Items.Count != 0) return;

                foreach (string file in Directory.GetFiles(item.Tag.ToString()))
                {
                    TreeViewItem plik = new TreeViewItem();
                    plik.Header = file.Substring(file.LastIndexOf("\\") + 1);
                    plik.Tag = file;
                    item.FontWeight = FontWeights.Bold;
                    //plik.Expanded += new RoutedEventHandler(PobierzDane);
                    item.Expanded += new RoutedEventHandler(WypiszInfo);
                    item.Items.Add(plik);
                }

                foreach (string file in Directory.GetDirectories(item.Tag.ToString()))
                {
                    TreeViewItem plik = new TreeViewItem();
                    plik.Header = file.Substring(file.LastIndexOf("\\") + 1);
                    plik.Tag = file;
                    item.FontWeight = FontWeights.Normal;
                    plik.Expanded += new RoutedEventHandler(PobierzDane);
                    item.Expanded += new RoutedEventHandler(WypiszInfo);
                    item.Items.Add(plik);
                }

            }
            catch (Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Explorator.Items.Clear();
            foreach (string disc in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = disc;
                item.Tag = disc;
                item.FontWeight = FontWeights.Normal;
                item.Expanded += new RoutedEventHandler(PobierzDane);
                item.Expanded += new RoutedEventHandler(WypiszInfo);
                Explorator.Items.Add(item);
            }
        }

        public void WypiszInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem item = (TreeViewItem)sender;
                if (!System.IO.File.Exists(GetFullPath(Explorator.SelectedItem as TreeViewItem))) return;
                Zawartosc.Text = System.IO.File.ReadAllText(GetFullPath(Explorator.SelectedItem as TreeViewItem)).ToString();

                FileInfo fi = new FileInfo(GetFullPath(Explorator.SelectedItem as TreeViewItem));
                Information.Text = "Atrybut: " + fi.Attributes.ToString();
                Information.Text += "\nPełna Ścieżka: " + fi.FullName;
                Information.Text += "\nRozszerzenie: " + fi.Extension;
                Information.Text += "\nData utworzenia: " + fi.CreationTime;
                Information.Text += "\nReadOnly: " + fi.IsReadOnly;
                Information.Text += "\nOstatni czas dostępu: " + fi.LastAccessTime;
                Information.Text += "\nOstatni czas zmiany: " + fi.LastWriteTime;
            }
            catch (Exception) { }
        }

        private void Add_Folder_Click(object sender, RoutedEventArgs e)
        {
            string path = GetFullPath(Explorator.SelectedItem as TreeViewItem);
            if (path == ""||!Directory.Exists(path)) return;
            Directory.CreateDirectory(path + "\\hehe");
            Button_Click(sender, e);
        }

        private void Add_File_Click(object sender, RoutedEventArgs e)
        {
            Data.path = GetFullPath(Explorator.SelectedItem as TreeViewItem);
            if (Data.path == "" || !Directory.Exists(Data.path)) return;
            dodawanie dod = new dodawanie();
            dod.Show();
            Button_Click(sender, e);
        }

        public string GetFullPath(TreeViewItem node)
        {
            if (node == null)
                return "";

            var result = Convert.ToString(node.Header);

            for (var i = GetParentItem(node); i != null; i = GetParentItem(i))
                result = i.Header + "\\" + result;

            return result;
        }

        static TreeViewItem GetParentItem(TreeViewItem item)
        {
            for (var i = VisualTreeHelper.GetParent(item); i != null; i = VisualTreeHelper.GetParent(i))
                if (i is TreeViewItem)
                    return (TreeViewItem)i;

            return null;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            string path = GetFullPath(Explorator.SelectedItem as TreeViewItem);
            if(path == "") { Zawartosc.Text = "Brak pliku do usunięcia"; return; }
            if (!Directory.Exists(path) || Directory.GetDirectories(path).Length != 0) { Zawartosc.Text = "Folder nie jest pusty"; return; }
            Directory.Delete(path);
            Zawartosc.Text = "Usunieto " + path.Substring(path.LastIndexOf("\\") + 1);
            Button_Click(sender, e);
        }
    }
}
