using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Wpf_AeroSphere_test_task
{
    class Drives_list : IDirChangeable
    {
        struct Hardware_ico_and_info
        {
            public ImageSource hardware_ico;// картинка носителя информации диска или флешки
            public DriveInfo drive_info;//информация о носителе
        }

        public Drives_list(ListView list_view_devices)
        {
            Get_all_drives(list_view_devices);
        }
        private string currentDirName;//имя текущей директории
        private string prev_DirName;
        private string next_DirName;
        private bool is_disk_choosen = false;
        private const string default_root_dir = "💻MyComputer";//имя корневой папки
        private const string divide_symbol = "❯";//символ разделения каталогов
        private const int disks_interrogation_delay = 3000;
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
        public DriveInfo[] AllDrives { get; set; }//массив всех дисков 

        public void Choose_disk(ListView list_view_disks, ListView list_view_files, ListView list_volumes, TextBox txt_box_Path, DataGrid data_grid_meta_data)//Переход из списка дисков к файлам на этом диске
        {
            is_disk_choosen = true;
            data_grid_meta_data.Items.Clear();
            data_grid_meta_data.Visibility = Visibility.Collapsed;
            currentDirName = list_volumes.SelectedItem.GetType().GetProperty("Name").GetValue(list_volumes.SelectedItem,null).ToString();
            txt_box_Path.Text = $"{default_root_dir} {divide_symbol} {currentDirName}";
            Switch_btw_files_and_disks_listviews(list_view_disks, list_view_files);
            list_view_files.ItemsSource = (from filepath in GetAllFiles() select Path.GetFileName(filepath)).ToList();
        }

        public void Return_to_disk_choosing(ListView list_view_disks, ListView list_view_files, TextBox txt_box_Path, DataGrid data_grid_meta_data)//возврат к каталогу со всеми дисками
        {
            if (is_disk_choosen)
            {
                is_disk_choosen = false;
                data_grid_meta_data.Visibility = Visibility.Visible;
                txt_box_Path.Text = default_root_dir;
                Switch_btw_files_and_disks_listviews(list_view_disks, list_view_files);
            }
            else;//мы итак в директории выборе диска находимся
        }

        public string[] GetAllFiles()
        {
            return Directory.GetFiles(currentDirName);
        }
        private async void Get_all_drives(ListView list_view_disks)
        {
            while (true)
            {
                list_view_disks.Items.Clear();
                AllDrives = DriveInfo.GetDrives();
                Hardware_ico_and_info img_and_info;
                for (int i = 0; i < AllDrives.Length; i++)//Добавим по картинке к каждому устройству
                {
                    img_and_info.drive_info = AllDrives[i];

                    switch (AllDrives[i].DriveType)
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
                        {
                            VolumeLabel = img_and_info.drive_info.VolumeLabel,
                            Name = img_and_info.drive_info.Name,
                            AvailableFreeSpace = img_and_info.drive_info.AvailableFreeSpace,
                            TotalSize = img_and_info.drive_info.TotalSize,
                            Img = img_and_info.hardware_ico
                        });
                    }
                    else
                    {
                        list_view_disks.Items.Add(new
                        {
                            VolumeLabel = "Uknown",
                            Name = img_and_info.drive_info.Name,
                            Img = img_and_info.hardware_ico
                        });
                    }

                }
                if (AllDrives.Length > 0)
                {
                    //Значит все хорошо, так как в системе есть хоть одно запоминающее устройство
                    await Task.Delay(disks_interrogation_delay);
                }
                else
                {
                    throw new Exception("На данном устройстве нет доступных драйверов!!!");
                }

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
