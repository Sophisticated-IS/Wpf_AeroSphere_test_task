using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_AeroSphere_test_task
{
    class Drives_list : IDirChangeable
    {
        public Drives_list()
        {
            AllDrives = Get_all_drives();
        }
        private string currentDirName;//имя текущей директории
        private const string root_dir = "💻MyComputer";
        private const string divide_symbol = "❯";
        public string CurrentDirName
        {
            get
            {
                return currentDirName;
            }
            set
            {
                currentDirName = value;
                Directory.SetCurrentDirectory(currentDirName);
            }
        }
        public DriveInfo[] AllDrives { get; }

        public void Move_dir_down(ListView list_view_disks, ListView list_view_files, ListView list_volumes, TextBox txt_box_Path, DataGrid data_grid_meta_data)
        {
                data_grid_meta_data.Items.Clear();
                data_grid_meta_data.Visibility = Visibility.Collapsed;
                currentDirName = list_volumes.SelectedItem.ToString();
                txt_box_Path.Text = $"{root_dir} {divide_symbol} {currentDirName}";
                Switch_btw_files_and_disks_listviews(list_view_disks, list_view_files);
                list_view_files.ItemsSource = GetAllFiles();
        }

        public void Mov_dir_up()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllFiles()
        {            
            return Directory.GetFiles(currentDirName);
        }
        private DriveInfo[] Get_all_drives()
        {
            DriveInfo[] AllDrives = DriveInfo.GetDrives();

            if (AllDrives.Length > 0)
            {
                return AllDrives;
            }
            else
            {
                throw new Exception("На данном устройстве нет доступных драйверов!!!");
            }
        }
        private void Switch_btw_files_and_disks_listviews(ListView list_view_disks, ListView list_view_files)
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
    }
}
