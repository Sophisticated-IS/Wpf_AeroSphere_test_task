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

        private static Drives_list instance;
        
        private Drives_list(ListView list_view_devices)
        {
            Get_all_drives(list_view_devices);
        }
        public static Drives_list get_instance(ListView list_view_devices)//реализация сиглтон паттерна
        {
            if (instance == null)
            {
                instance = new Drives_list(list_view_devices);
            }
            return instance;
        }
        private string currentDirName;//имя текущей директории
        private string choosen_disk;//имя выбранного диска
        private bool is_disk_choosen = false;//был ли выбран диск
        private const int disks_interrogation_delay = 3000;//интервал опроса всех приводов в системе
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

        public void Choose_disk(Grid grid_files_and_folders, Grid grid_drives, ListView list_view_folders, ListView list_volumes, ListView list_view_path_frames, DataGrid data_grid_meta_data)//Переход из списка дисков к файлам на этом диске
        {
            is_disk_choosen = true;
            data_grid_meta_data.Items.Clear();
            data_grid_meta_data.Visibility = Visibility.Collapsed;
            currentDirName = list_volumes.SelectedItem.GetType().GetProperty("Name").GetValue(list_volumes.SelectedItem, null).ToString();
            choosen_disk = currentDirName;
            PathBuilder.Dir_down(list_view_path_frames, currentDirName);
            Switch_btw_grid_files_and_disks(grid_files_and_folders, grid_drives);
            Update_listview_folders(list_view_folders);
        }

        public void Return_to_disk_choosing(Grid grid_files_and_folders, Grid grid_drives, ListView list_view_path_frames)//возврат к каталогу со всеми дисками
        {
            if (is_disk_choosen)
            {
                is_disk_choosen = false;
                list_view_path_frames.Items.Clear();
                currentDirName = null;
                Switch_btw_grid_files_and_disks(grid_files_and_folders, grid_drives);
            }
            else;//мы итак в директории выборе диска находимся
        }

        public void Directory_down(ListView list_view_folders, ListView list_view_path_frames, string new_dir_down)//переход внутрь катлога
        {
            if (Directory.Exists(new_dir_down))
            {
                currentDirName = new_dir_down;
                PathBuilder.Dir_down(list_view_path_frames, Path.GetFileName(new_dir_down));

            }
            else
            {
                MessageBox.Show($"Директории  не сущетсвует по пути {new_dir_down}");
            }
            Update_listview_folders(list_view_folders);
        }

        public void Directory_up(ListView list_view_folders, ListView list_view_path_frames)//выход из каталога на папку выше
        {
            if (currentDirName != null && currentDirName != choosen_disk)
            {
                PathBuilder.Dir_up(list_view_path_frames);//поднимемся по директории
                var full_path_up = PathBuilder.Get_path(list_view_path_frames);
                if (Directory.Exists(full_path_up))
                {
                    currentDirName = full_path_up;//установим новую                   
                }
                else
                {
                    Exit_to_existing_dir(list_view_path_frames);
                }
                Update_listview_folders(list_view_folders);

            }
            else; //мы уже итак в этой директории

        }

        public void Directory_move_to_folder(ListView list_paths, ListView list_files, int selectedIndex)//возвращаемся к конкретной папке
        {
            if (selectedIndex >= 0)
            {
                while (list_paths.Items.Count - 1 != selectedIndex)
                {
                    PathBuilder.Dir_up(list_paths);//поднимемся по директории
                }
                var new_path = PathBuilder.Get_path(list_paths);

                if (Directory.Exists(new_path))
                {
                    currentDirName = new_path;//установим новую
                }
                else
                {
                    Exit_to_existing_dir(list_paths);
                }
                Update_listview_folders(list_files);

            }
            else
            {
                throw new Exception("Индекс не может быть меньше нуля!");
            }

        }
        private void Exit_to_existing_dir(ListView list_paths)//переход к существующей директории от удаленной директории,в которой находился пользователь
        {
            Get_all_files();//дойдем до существующей папки
            while (PathBuilder.Get_path(list_paths) != currentDirName)
            {
                PathBuilder.Dir_up(list_paths);
            }
            MessageBox.Show("Часть пути была удалена, так как папки в которых вы находились были удалены!");
        }
        private List<string> Get_all_files()//возвращает список всех папок и файлов НЕ скрытых
        {
            if (Directory.Exists(currentDirName))
            {
                //все хорошо
            }
            else
            {
                while (!Directory.Exists(currentDirName))//поднимемся пока не встретим существующую директорию
                {

                    if (currentDirName == null)
                    {
                        return new List<string>();
                    }
                    else
                    {
                        currentDirName = Path.GetDirectoryName(currentDirName);
                    }
                }
            }

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
                            //у драйверов совпадает имя и тип значит они равны
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

                            case DriveType.Network:
                                img_and_info.hardware_ico = Convert_images.Convert_to_ImageSource(Properties.Resources.network_connection_control_panel.ToBitmap());
                                break;

                            case DriveType.CDRom:
                                img_and_info.hardware_ico = Convert_images.Convert_to_ImageSource(Properties.Resources.hd_cdrom.ToBitmap());
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

        public void Update_listview_folders(ListView list_view_files)//обновляет список с папками и файлами
        {
            list_view_files.Items.Clear();
            foreach (var file in from filepath in Get_all_files() select Path.GetFileName(filepath))
            {
                try
                {
                    Icon extractedIcon = Files_ico_Win32API.GetIcon(Path.Combine(currentDirName, file), true);
                    list_view_files.Items.Add(new File_ico_and_name { Name = file, Ico = Convert_images.Convert_to_ImageSource(extractedIcon.ToBitmap()) });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось загрузить папку так как в ней есть системные файлы - {ex}");
                    break;
                }

            }
        }
    }
}
