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
        readonly Drives_list volumes = new Drives_list();

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            foreach (var disk in volumes.AllDrives)
            {
                Debug.WriteLine(disk.Name);
            }
#endif
            list_view_disks.ItemsSource = volumes.AllDrives;
        }
        private void Switch_btw_files_and_disks_listviews()
        {
            if (list_view_disks.Visibility == Visibility.Collapsed)
            {
                list_view_disks.Visibility = Visibility.Visible;
                list_view_files.Visibility = Visibility.Collapsed;
            }
            else
            {
                list_view_disks.Visibility = Visibility.Collapsed;
                list_view_files.Visibility = Visibility.Visible;
            }
            
        }


        private void List_view_disks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list_volumes = (ListView)sender;
            if (list_volumes != null && list_volumes.Items.Count > 0)
            {
                if (list_volumes.SelectedIndex >= 0)
                {
                    if (volumes.AllDrives[list_volumes.SelectedIndex].IsReady)
                    {
                        Switch_btw_files_and_disks_listviews();
                        volumes.CurrentDirName = list_volumes.SelectedItem.ToString();
                        list_view_files.ItemsSource = volumes.GetAllFiles();
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

        private void List_view_disks_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            data_grid_disks_meta_data.Items.Clear();
            var list_volumes = (ListView)sender;
            if (list_volumes != null && list_volumes.Items.Count > 0 && list_volumes.SelectedIndex >= 0)
            {
                if (volumes.AllDrives[list_volumes.SelectedIndex].IsReady)
                {
                    foreach (var prop in volumes.AllDrives[list_volumes.SelectedIndex].GetType().GetProperties())
                    {
                        string tmp = prop.GetValue(volumes.AllDrives[list_volumes.SelectedIndex], null).ToString();
                        data_grid_disks_meta_data.Items.Add(new { Name = prop.Name, Value = tmp });
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
