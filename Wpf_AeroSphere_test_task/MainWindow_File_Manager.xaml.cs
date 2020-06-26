using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Wpf_AeroSphere_test_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Drivers_list  a = new Drivers_list();

        public MainWindow()
        {
            InitializeComponent();          

#if DEBUG
            foreach (var disk in a.AllDrivers)
            {
                Debug.WriteLine(disk.Name);                             
            }
#endif
            list_view_files.ItemsSource = a.AllDrivers;
        }

        private void List_view_files_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list_drivers = (ListView)sender;
            if (list_drivers != null && list_drivers.Items.Count > 0)
            {
                if (list_drivers.SelectedIndex > 0)
                {
                    if (a.AllDrivers[list_drivers.SelectedIndex].IsReady)
                    {
                        a.CurrentDirName = list_drivers.SelectedItem.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Диск не готов к работе!");
                    }
                }
                else;//элемент не выбран

            }
            else;//список пуст
        }

        private void list_view_files_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            data_grid_meta_data.Items.Clear();
            var list_drivers = (ListView)sender;
            if (list_drivers != null && list_drivers.Items.Count > 0 && list_drivers.SelectedIndex >= 0)
            {
                if (a.AllDrivers[list_drivers.SelectedIndex].IsReady)
                {
                    foreach (var prop in a.AllDrivers[list_drivers.SelectedIndex].GetType().GetProperties())
                    {
                        string tmp = prop.GetValue(a.AllDrivers[list_drivers.SelectedIndex], null).ToString();
                        data_grid_meta_data.Items.Add(new { Name = prop.Name, Value = tmp });
                    }
                }
                else
                {
                    MessageBox.Show("Диск не готов к работе!");
                }
            }
        }
    }
}
