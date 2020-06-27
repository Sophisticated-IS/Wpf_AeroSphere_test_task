using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;


namespace Wpf_AeroSphere_test_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Drives_list volumes; 

        public MainWindow()
        {
            InitializeComponent();
            
            txt_box_Path.Text = "💻MyComputer";
            volumes  = new Drives_list(list_view_disks);//экземпляр нашей файловой системы                            
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
                        volumes.Choose_disk(list_view_disks, list_view_files, list_volumes, txt_box_Path, data_grid_disks_meta_data);
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
            data_grid_disks_meta_data.Visibility = Visibility.Visible;
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

        private void List_view_files_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list_folders_and_files = (ListView)sender;

            if (list_folders_and_files != null && list_folders_and_files.Items.Count > 0 && list_folders_and_files.SelectedIndex >= 0)
            {
                string doc_or_folder_name = (string)list_folders_and_files.SelectedValue;
                if (!string.IsNullOrEmpty(doc_or_folder_name) && string.IsNullOrEmpty(Path.GetExtension(doc_or_folder_name)) )
                {
                    volumes.CurrentDirName = Path.Combine(volumes.CurrentDirName, doc_or_folder_name);
                    volumes.Directory_down(list_view_files,txt_box_Path,doc_or_folder_name);
                }
                else//значит это файл так как у него есть расширение
                {
                    var full_path = Path.Combine(volumes.CurrentDirName, doc_or_folder_name);
                    try
                    {
                        Process.Start(full_path);
                    }
                    catch (System.ComponentModel.Win32Exception)//не удалось найти приложение для данного файла
                    {
                        Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = full_path });//вызовем открыть с помощью и пусть пользователь выберет
                        
                    }
                   
                    
                }
            }
            else;//элемент не выбран или его нет
        }

        private void Left_arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            volumes.Return_to_disk_choosing(list_view_disks, list_view_files, txt_box_Path, data_grid_disks_meta_data);
        }
    }
}
