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
        string CurrentDirName  { get;}//текущая директория
        void Choose_disk(ListView list_view_disks, ListView list_view_files, ListView list_volumes,TextBox txt_box_Path, DataGrid data_grid_meta_data);//подняться по директории
        void Return_to_disk_choosing(ListView list_view_disks, ListView list_view_files, TextBox txt_box_Path, DataGrid data_grid_meta_data);//опуститься по директории
    }
}
