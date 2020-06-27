using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf_AeroSphere_test_task
{

    class Drives_list : IDirChangeable
    {
        struct Hardware_ico_and_info//хранит картинку и информацию о приводе
        {
            public ImageSource hardware_ico;// картинка носителя информации диска или флешки
            public DriveInfo drive_info;//информация о носителе
        }

        public Drives_list(ListView list_view_devices)
        {
            Get_all_drives(list_view_devices);
        }
        private string currentDirName;//имя текущей директории
        private string choosen_disk;
        private bool is_disk_choosen = false;
        private const string default_root_dir = "💻MyComputer ❯ ";//имя корневой папки
        //private const string divide_symbol = "❯";//символ разделения каталогов
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
        public DriveInfo[] AllDrives { get; set; }//массив всех приводов

        public void Choose_disk(Grid grid_files_and_folders, Grid grid_drives, ListView list_view_folders, ListView list_volumes, TextBox txt_box_Path, DataGrid data_grid_meta_data)//Переход из списка дисков к файлам на этом диске
        {
            is_disk_choosen = true;
            data_grid_meta_data.Items.Clear();
            data_grid_meta_data.Visibility = Visibility.Collapsed;
            currentDirName = list_volumes.SelectedItem.GetType().GetProperty("Name").GetValue(list_volumes.SelectedItem, null).ToString();
            choosen_disk = currentDirName;
            txt_box_Path.Text = $"{default_root_dir}{currentDirName}";
            Switch_btw_grid_files_and_disks(grid_files_and_folders, grid_drives);
            list_view_folders.Items.Clear();
            foreach (var item in from filepath in GetAllFiles() select Path.GetFileName(filepath))
            {
                Icon extractedIcon = Files_ico_Win32API.GetIcon(Path.Combine(currentDirName,item),true);
                list_view_folders.Items.Add(new File_ico_and_name {Name = item,Ico = Convert_images.Convert_to_ImageSource(extractedIcon.ToBitmap())});
            }
        }

        public void Return_to_disk_choosing(Grid grid_files_and_folders, Grid grid_drives, ListView list_view_files, TextBox txt_box_Path, DataGrid data_grid_meta_data)//возврат к каталогу со всеми дисками
        {
            if (is_disk_choosen)
            {
                is_disk_choosen = false;
                data_grid_meta_data.Visibility = Visibility.Visible;
                txt_box_Path.Text = default_root_dir;
                Switch_btw_grid_files_and_disks(grid_files_and_folders, grid_drives);
            }
            else;//мы итак в директории выборе диска находимся
        }

        public void Directory_down(ListView list_view_folders, TextBox txt_box_Path, string currentDirName)
        {
            this.currentDirName = currentDirName;
            txt_box_Path.Text = default_root_dir + currentDirName;
            list_view_folders.Items.Clear();
            foreach (var item in from filepath in GetAllFiles() select Path.GetFileName(filepath))
            {
                list_view_folders.Items.Add(item);
            }            
        }

        public void Directory_up(ListView list_view_folders, TextBox txt_box_Path)
        {
            if (currentDirName != null && currentDirName != choosen_disk)
            {
                var current_folder = Path.GetFileName(currentDirName);
                
                currentDirName = currentDirName.TrimEnd(current_folder.ToCharArray());
                if (CurrentDirName == choosen_disk)
                {
                    //если это корень тома,то мы не удаляем слэши
                }
                else
                {
                    currentDirName = currentDirName.TrimEnd('\\');
                }
                
                txt_box_Path.Text = default_root_dir + currentDirName;
                list_view_folders.Items.Clear();
                foreach (var item in from filepath in GetAllFiles() select Path.GetFileName(filepath))
                {
                    list_view_folders.Items.Add(item);
                }
            }
            else; //мы уже итак в этой директории

        }
        private List<string> GetAllFiles()//возвращает список всех папок и файлов НЕ скрытых
        {
            var all_files_and_folders = Directory.GetFileSystemEntries(currentDirName);//все файлы и папки
            List<string> not_hidden_folders_files = new List<string>();//только не скрытые Файлы и папки

            for (int i = 0; i < all_files_and_folders.Length; i++)
            {
                var dir_info = new DirectoryInfo(all_files_and_folders[i]);
                if ((dir_info.Attributes & FileAttributes.Hidden) == 0)
                {
                    not_hidden_folders_files.Add(all_files_and_folders[i]);
                }
            }
            return not_hidden_folders_files;
        }
        private async void Get_all_drives(ListView list_view_disks)//получает список всех доступных дисков и устройств динамически обновляя их
        {
            while (true)
            {
                var new_drives = DriveInfo.GetDrives();
                bool not_all_equal = false;//не все приводы/диски равны
                if (AllDrives != null && AllDrives.Length == new_drives.Length)
                {
                    for (int i = 0; i < new_drives.Length; i++)
                    {
                        if (new_drives[i].Name == AllDrives[i].Name && new_drives[i].DriveType == AllDrives[i].DriveType)
                        {

                        }
                        else
                        {
                            not_all_equal = true;
                            break;
                        }
                    }
                }
                else
                {
                    not_all_equal = true;
                }

                if (not_all_equal)
                {
                    list_view_disks.Items.Clear();
                    AllDrives = new_drives;//обновим список приводов на новый
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
                                AvailableFreeSpace = img_and_info.drive_info.AvailableFreeSpace >> 30,
                                TotalSize = img_and_info.drive_info.TotalSize >> 30,
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

                }
                else;

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
        private void Switch_btw_grid_files_and_disks(Grid grid_files_and_folders, Grid grid_drives)//меняет свойство Visibility для переключения между выбором устройства и файловой системы выбранного устройства 
        {
            if (grid_drives.Visibility == Visibility.Collapsed)
            {
                grid_drives.Visibility = Visibility.Visible;
                grid_files_and_folders.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_drives.Visibility = Visibility.Collapsed;
                grid_files_and_folders.Visibility = Visibility.Visible;
            }

        }
    }
}
