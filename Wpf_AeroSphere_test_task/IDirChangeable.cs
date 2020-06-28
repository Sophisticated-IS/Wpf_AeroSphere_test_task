using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wpf_AeroSphere_test_task
{
    interface IDirChangeable
    {
        string CurrentDirName { get; }//текущая директория
        void Choose_disk(Grid grid_files_and_folders, Grid grid_drives, ListView list_view_folders, ListView list_volumes, TextBox txt_box_Path, DataGrid data_grid_meta_data);
        void Return_to_disk_choosing(Grid grid_files_and_folders, Grid grid_drives, TextBox txt_box_Path);
        void Directory_up(ListView list_view_folders, TextBox txt_box_Path);
        void Directory_down(ListView list_view_folders, TextBox txt_box_Path, string currentDirName);
    }
}
