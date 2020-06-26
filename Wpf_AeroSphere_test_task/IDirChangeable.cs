using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AeroSphere_test_task
{
    interface IDirChangeable
    {
        string CurrentDirName  { get;}//текущая директория
        void Move_dir_up();//подняться по директории
        void Mov_dir_down();//опуститься по директории
    }
}
