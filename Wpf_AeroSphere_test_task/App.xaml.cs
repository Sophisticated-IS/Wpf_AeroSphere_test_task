using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_AeroSphere_test_task
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

    }

    public class Volumes_list
    {
        public string VolumeName { get; set; }
        public int MyProperty { get; set; }
        public int TotalSize { get; set; }
    }
}
