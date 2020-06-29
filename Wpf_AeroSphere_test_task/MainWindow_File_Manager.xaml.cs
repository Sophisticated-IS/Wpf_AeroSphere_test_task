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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Wpf_AeroSphere_test_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Drives_list volumes;
        Thread thread_get_metadata_of_folders_files;//поток для получения данных о папках и файлаз
        int files_counter = 0;//для полсчета кол-ва файлов в папке
        long size_folder_in_byte = 0;//для подсчета размера папки с файлами
        string Previos_file_name;//имя предыдущего выбранного файла
        private const string search_message = "Search: 🔎";//подсказка для поиска файлов в текстбоксе
        public MainWindow()
        {
            InitializeComponent();
            volumes = new Drives_list(list_view_disks);//экземпляр нашей файловой системы  
            Check_current_drive_is_online(); //метод проверяющий включен ли драйвер
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
                        volumes.Choose_disk(Grid_drives, Grid_files_and_folders, list_view_files, list_volumes, list_view_path_frames, data_grid_disks_meta_data);
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
            var list_volumes = (ListView)sender;
            if (list_volumes != null && list_volumes.Items.Count > 0 && list_volumes.SelectedIndex >= 0)
            {
                data_grid_disks_meta_data.Visibility = Visibility.Visible;
                data_grid_disks_meta_data.Items.Clear();

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
                File_ico_and_name file_ico_name = (File_ico_and_name)list_folders_and_files.SelectedValue;
                string doc_or_folder_name = file_ico_name.Name;
                var full_path = Path.Combine(volumes.CurrentDirName, doc_or_folder_name);

                if (!string.IsNullOrEmpty(doc_or_folder_name) && Directory.Exists(full_path))
                {
                    if (Directory.Exists(full_path))
                    {
                        try
                        {
                            thread_get_metadata_of_folders_files.Abort();//остановим поток если пользователь решил перейти к другой папке
                            volumes.Directory_down(list_view_files, list_view_path_frames, full_path);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Вам отказано в доступе к данной папке!");
                        }
                    }
                    else
                    {
                        volumes.Update_listview_folders(list_view_files);
                        MessageBox.Show($"Директория не существует по пути {full_path}!");
                    }


                }
                else//значит это файл 
                {
                    if (File.Exists(full_path))
                    {
                        try
                        {
                            Process.Start(full_path);
                        }
                        catch (System.ComponentModel.Win32Exception)//не удалось найти приложение для данного файла
                        {
                            Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = full_path });//вызовем открыть с помощью и пусть пользователь выберет
                        }
                    }
                    else
                    {
                        volumes.Update_listview_folders(list_view_files);
                        MessageBox.Show($"Файл не существует по пути {full_path}!");
                    }


                }

            }
            else;//элемент не выбран или его нет
        }

        private async void Check_current_drive_is_online()//проверяет все ли приводы включены,и если текущий привод отключился, то возвращает пользователя к выбору привода
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (volumes.CurrentDirName != null)
                    {
                        DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(volumes.CurrentDirName));

                        if (driveInfo.IsReady == false)
                        {
                            Dispatcher.Invoke(() => //выкинем пользователя из удаленного устройства/драйвера
                            {
                                Return_to_disk_choosing_Button_Click(this, null);
                            });
                        }
                        else;//все в порядке 

                    }
                    else;//пользователь еще не выбрал диск

                    Thread.Sleep(100);
                }

            });
        }
        private void Left_arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            var curr_dir = volumes.CurrentDirName;
            volumes.Directory_up(list_view_files, list_view_path_frames);
            if (curr_dir != volumes.CurrentDirName)
            {
                thread_get_metadata_of_folders_files.Abort();
            }
            else;//мы в корневой директории
        }

        private void List_view_files_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView list_files = (ListView)sender;
            if (list_files.SelectedValue != null)
            {
                data_grid_files_meta_data.Visibility = Visibility.Visible;
                File_ico_and_name file_ico_name = (File_ico_and_name)list_files.SelectedValue;
                var path_dir = Path.Combine(volumes.CurrentDirName, file_ico_name.Name);
                FileInfo fileinfo = new FileInfo(path_dir);

                if (Previos_file_name != file_ico_name.Name)
                {
                    if (thread_get_metadata_of_folders_files != null)
                    {
                        thread_get_metadata_of_folders_files.Abort();
                    }                    
                    else;//поток еще ни разу не был создан и вызван

                    if ((fileinfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        data_grid_files_meta_data.Items.Clear();
                        //обнулим счетчики и вызовем поток для новой выбранной папки
                        files_counter = 0;
                        size_folder_in_byte = 0;
                        thread_get_metadata_of_folders_files = new Thread(new ParameterizedThreadStart(Count_folder_size));
                        thread_get_metadata_of_folders_files.Start(path_dir);
                    }
                    else//это не директория а файл
                    {
                        data_grid_files_meta_data.Items.Clear();

                        foreach (var file_prop in fileinfo.GetType().GetProperties())
                        {
                            if (file_prop.Name == "Length")
                            {
                                data_grid_files_meta_data.Items.Add(new { Name = file_prop.Name, Value = $"{(long)file_prop.GetValue(fileinfo) >> 10} Кб" });
                            }
                            else
                            {
                                data_grid_files_meta_data.Items.Add(new { Name = file_prop.Name, Value = file_prop.GetValue(fileinfo) });
                            }

                        }
                    }
                    Previos_file_name = file_ico_name.Name;
                }
                else
                {
                    //мы уже итак вычисляем или вычислили для этого файла метаданные 
                }

            }
            else;//клик был не на элемент

        }

        /// <summary>
        /// Получает размер папки в байтах, и конвертирует в Кб, Мб, Гб, если это возможно
        /// </summary>
        /// <param name="mesure_units">единицы измерения Гб,Мб, Кб,байт</param>
        /// <param name="size"> исходный размер в байтах</param>
        /// <param name="size_result"> сконвертированный в конкретные единицы размер</param>
        private void Get_folder_size_and_measure_units(out string mesure_units, long size, out double size_result)
        {
            if (size >> 10 > 0)//значит файл весит хотя бы 1 кб
            {
                if (size >> 20 > 0)//значит размер файла весит хотя бы 1 МБ
                {
                    if (size >> 30 > 0)//значит размер файла весит хотя бы 1 ГБ
                    {
                        size_result = (double)size / (1024 * 1024 * 1024);
                        size_result = Math.Round(size_result, 2);
                        mesure_units = "Размер (Гб)";
                    }
                    else//Выводим в мегабайтах
                    {
                        size_result = (double)size / (1024 * 1024);
                        size_result = Math.Round(size_result, 2);
                        mesure_units = "Размер (Мб)";
                    }
                }
                else//выводим в КБ
                {
                    size_result = (double)size_folder_in_byte / 1024;
                    size_result = Math.Round(size_result, 2);
                    mesure_units = "Размер (Кб)";
                }

            }
            else//размер файла меньше 1 кб
            {
                size_result = size;
                mesure_units = "Размер (байт)";
            }
        }
        private void Count_folder_size(object _path)//рекурсивный метод обхода всех файлов и подкаталогов папки
        {
            string path = (string)_path;
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.GetFiles(path)
                    .ToList()
                    .ForEach(s =>
                    {
                        FileInfo get_info = new FileInfo(s);
                        try
                        {
                            size_folder_in_byte += get_info.Length;//System.IO.FileNotFoundException
                            files_counter++;
                        }
                        catch (FileNotFoundException)
                        {
                            //пропустим файл если его удалили
                        }
                    });

                    Directory.GetDirectories(path)
                   .ToList()
                   .ForEach(s => Count_folder_size(s));
                }
                else;//директории нет
            }
            catch (UnauthorizedAccessException)
            {
                //пропустим папки к которым нет доступа
            }

            //TODO: рефакторинг и избавление от лишнего кода и повторяющегося
            //TODO: Проверки на удаление папок и файлов 
            Get_folder_size_and_measure_units(out string units_name, size_folder_in_byte, out double size_in_concrete_units);
            Dispatcher.Invoke(() =>
            {
                data_grid_files_meta_data.Items.Clear();
                data_grid_files_meta_data.Items.Add(new { Name = "Количество файлов ", Value = files_counter });
                data_grid_files_meta_data.Items.Add(new { Name = units_name, Value = size_in_concrete_units });
            }, DispatcherPriority.Background);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (thread_get_metadata_of_folders_files != null)
            {
                thread_get_metadata_of_folders_files.Abort();
            }
            else;//поток и не создавался

        }

        private void Return_to_disk_choosing_Button_Click(object sender, RoutedEventArgs e)
        {
            if (thread_get_metadata_of_folders_files != null)
            {
                thread_get_metadata_of_folders_files.Abort();
            }
            else;//поток не создан

            volumes.Return_to_disk_choosing(Grid_files_and_folders, Grid_drives, list_view_path_frames);
        }

        private void Search_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txt_box_search = (TextBox)sender;
            txt_box_search.Text = "";
            txt_box_search.Foreground = Brushes.Black;
        }

        private void Search_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt_box_search = (TextBox)sender;
            txt_box_search.Text = search_message;
            txt_box_search.Foreground = Brushes.Gray;
        }

        private void Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt_box_search = (TextBox)sender;
            var searching_filename = txt_box_search.Text;
            if (volumes != null && volumes.CurrentDirName != null && txt_box_search.IsFocused)
            {
                int i = 0;
                foreach (var file in list_view_files.Items)
                {

                    File_ico_and_name file_ico_name = (File_ico_and_name)file;
                    if (file_ico_name.Name.StartsWith(searching_filename))
                    {
                        file_ico_name.Visible_mode = Visibility.Visible;
                        //оставим нужные элементы
                    }
                    else//скроем
                    {
                        file_ico_name.Visible_mode = Visibility.Collapsed;
                    }

                    i++;
                }
                list_view_files.Items.Refresh();

            }
            else;//негде искать
        }

        private void List_view_path_frames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (thread_get_metadata_of_folders_files != null)
            {
                thread_get_metadata_of_folders_files.Abort();
            }
            else;//не был создан            
            var list_paths = (ListView)sender;
            volumes.Directory_move_to_folder(list_paths, list_view_files, list_paths.SelectedIndex);
        }
    }
}
