using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Wpf_AeroSphere_test_task
{
    class File_ico_and_name
    {
        public ImageSource Ico { get; set; }//иконка файла или папки
        public string Name { get; set; }//название файла или папки
        public Visibility Visible_mode { get; set; } = Visibility.Visible;//виден ли элемент в UI
    }

}

