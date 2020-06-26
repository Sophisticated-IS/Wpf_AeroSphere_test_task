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
        void Move_dir_down(ListView list_view_disks, ListView list_view_files, ListView list_volumes,TextBox txt_box_Path, DataGrid data_grid_meta_data);//подняться по директории
        void Mov_dir_up();//опуститься по директории
    }
}
