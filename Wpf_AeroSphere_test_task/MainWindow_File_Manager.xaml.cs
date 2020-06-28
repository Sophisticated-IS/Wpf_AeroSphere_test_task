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

            txt_box_Path.Text = "💻MyComputer ❯ ";
            volumes = new Drives_list(list_view_disks);//экземпляр нашей файловой системы                            
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
                        volumes.Choose_disk(Grid_drives, Grid_files_and_folders, list_view_files, list_volumes, txt_box_Path, data_grid_disks_meta_data);
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
                FileAttributes fileAttr = File.GetAttributes(full_path);

                bool isFile;
                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    isFile = false;
                }
                else
                {
                    isFile = true;
                }

                if (!string.IsNullOrEmpty(doc_or_folder_name) && !isFile)
                {
                    try
                    {
                        volumes.Directory_down(list_view_files, txt_box_Path, full_path);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Вам отказано в доступе к данной папке!");
                    }
                }
                else//значит это файл так как у него есть расширение TODO: есть папки с расширениями как их обрабатывать тоже в винде открывать?
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
            }
            else;//элемент не выбран или его нет
        }

        private void Left_arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            volumes.Directory_up(list_view_files, txt_box_Path);
        }

        private void List_view_files_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView list_files = (ListView)sender;

            if (list_files.SelectedValue != null)
            {
                data_grid_files_meta_data.Visibility = Visibility.Visible;
                data_grid_files_meta_data.Items.Clear();

                File_ico_and_name file_ico_name = (File_ico_and_name)list_files.SelectedValue;
                var path_dir = Path.Combine(volumes.CurrentDirName, file_ico_name.Name);
                FileInfo fileinfo = new FileInfo(path_dir);


                if ((fileinfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                 //   var files = Directory.GetFiles(path_dir,"*.*",SearchOption.TopDirectoryOnly);// TODO:System.UnauthorizedAccessException: 'Отказано в доступе по пути "C:\Program Files\windows nt\Стандартные".'
                  //  var ff = new List<string>(100);
                    int files_counter = 0;
                    long size_folder_in_byte = 0;


                    var files = new List<string>();
                    var pattern = "*.*";
                    AddFiles(path_dir, files);


                    foreach (var file in files)
                    {

                        FileInfo get_info = new FileInfo(file);
                        size_folder_in_byte += get_info.Length;//System Input outp
                        files_counter++;
#if DEBUG
                        Debug.WriteLine($"file - {file.Length}");
#endif
                    }

                    data_grid_files_meta_data.Items.Add(new { Name = "Количество файлов ", Value = files_counter });
                    if (size_folder_in_byte>>10 > 0)//значит файл весит хотя бы 1 кб
                    {
                        if (size_folder_in_byte >>20 > 0)//значит размер файла весит хотя бы 1 МБ
                        {
                            if (size_folder_in_byte >> 30 > 0)//значит размер файла весит хотя бы 1 ГБ
                            {
                                double size_in_GB =(double)size_folder_in_byte /( 1024 * 1024 * 1024);
                                size_in_GB = Math.Round(size_in_GB,2);
                                data_grid_files_meta_data.Items.Add(new { Name = "Размер (Гб)", Value = size_in_GB });
                                
                            }
                            else//Выводим в мегабайтах
                            {
                                double size_in_MB = (double)size_folder_in_byte / (1024 * 1024);
                                size_in_MB = Math.Round(size_in_MB, 2);
                                data_grid_files_meta_data.Items.Add(new { Name = "Размер (Мб)", Value = size_in_MB });
                            }
                        }
                        else//выводим в КБ
                        {
                            double size_in_KB = (double)size_folder_in_byte / 1024 ;
                            size_in_KB = Math.Round(size_in_KB, 2);
                            data_grid_files_meta_data.Items.Add(new { Name = "Размер (Кб)", Value = size_in_KB });
                        }
                        
                    }
                    else//размер файла меньше 1 кб
                    {
                        data_grid_files_meta_data.Items.Add(new { Name = "Размер (байт)", Value = size_folder_in_byte });
                    }

                }
                else//это не директория
                {
                    var dir_info = new DirectoryInfo(path_dir);

                    var dir = Path.GetDirectoryName(path_dir);
                    var file = Path.GetFileName(path_dir);

                    var shellAppType = Type.GetTypeFromProgID("Shell.Application");
                    dynamic shell = Activator.CreateInstance(shellAppType);
                    var folder = shell.NameSpace(dir);
                    var folderItem = folder.ParseName(file);

                    var names =
                        (from idx in Enumerable.Range(0, short.MaxValue)
                         let key = (string)folder.GetDetailsOf(null, idx) // пришлось вставить cast
                         where !string.IsNullOrEmpty(key)
                         select (idx, key)).ToDictionary(p => p.idx, p => p.key);

                    var properties =
                        (from idx in names.Keys
                         orderby idx
                         let value = (string)folder.GetDetailsOf(folderItem, idx) // пришлось вставить cast
                         where !string.IsNullOrEmpty(value)
                         select (name: names[idx], value)).ToList();

                    foreach (var (name, value) in properties)
                    {
                        data_grid_files_meta_data.Items.Add(new { Name = name, Value = value });
                    }

                }



            }
            else;//клик был не на элемент

        }

        private static void AddFiles(string path, IList<string> files)
        {
            try
            {
                Directory.GetFiles(path)
                    .ToList()
                    .ForEach(s => files.Add(s));

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => AddFiles(s, files));
            }
            catch (UnauthorizedAccessException ex)
            {
                // ok, so we are not allowed to dig into that directory. Move on.
            }
        }
    }
}
