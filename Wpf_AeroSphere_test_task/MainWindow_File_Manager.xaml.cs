﻿using System;
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
using System.Windows.Shapes;

namespace Wpf_AeroSphere_test_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Drives_list volumes = new Drives_list();//экземпляр нашей файловой системы
        struct Hardware_ico_and_info
        {
           public ImageSource hardware_ico;// картинка носителя информации диска или флешки
           public DriveInfo drive_info;//информация о носителе
        }
        public MainWindow()
        {
            InitializeComponent();
            txt_box_Path.Text = "💻MyComputer";
           
            var drives = volumes.AllDrives;
            
            Hardware_ico_and_info img_and_info;
            for (int i = 0; i < drives.Length; i++)//Добавим по картинке к каждому устройству
            {
                img_and_info.drive_info = drives[i];
                
                switch (drives[i].DriveType)
                {
                    case DriveType.Fixed: 
                        img_and_info.hardware_ico = Convert_images.Convert_to_ImageSource(Properties.Resources.hp_hdd.ToBitmap());
                        break;

                    case DriveType.Removable:
                        img_and_info.hardware_ico = Convert_images.Convert_to_ImageSource(Properties.Resources.hp_flash_drive.ToBitmap());
                        break;
                    default:
                        img_and_info.hardware_ico = Convert_images.Convert_to_ImageSource(Properties.Resources.question_shield.ToBitmap());
                        break;
                }
                if (img_and_info.drive_info.IsReady)
                {
                    list_view_disks.Items.Add(new 
                    { VolumeLabel = img_and_info.drive_info.VolumeLabel, Name = img_and_info.drive_info.Name, 
                        AvailableFreeSpace = img_and_info.drive_info.AvailableFreeSpace, TotalSize = img_and_info.drive_info.TotalSize,
                        Img = img_and_info.hardware_ico 
                    });
                }
                else
                {
                    list_view_disks.Items.Add(new {
                       VolumeLabel = "Uknown",
                        Name = img_and_info.drive_info.Name,
                        Img = img_and_info.hardware_ico
                    });
                }
                
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
                
            }
            else;//элемент не выбран или его нет
        }

        private void Left_arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            volumes.Return_to_disk_choosing(list_view_disks, list_view_files, txt_box_Path, data_grid_disks_meta_data);
        }
    }
}
