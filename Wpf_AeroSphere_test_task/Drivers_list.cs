using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AeroSphere_test_task
{
    class Drivers_list : IDirChangeable
    {
        public Drivers_list()
        {
            AllDrivers = Get_all_drives();
            CurrentDirName = AllDrivers[0].Name;
            Directory.SetCurrentDirectory(CurrentDirName);
        }
        public string CurrentDirName { get;}
        public DriveInfo[] AllDrivers { get; }

        public void Move_dir_up()
        {
            throw new NotImplementedException();
        }

        public void Mov_dir_down()
        {
            throw new NotImplementedException();
        }

        private DriveInfo[] Get_all_drives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();           
            
            if (allDrives.Length > 0)
            {
                return allDrives;
            }
            else
            {
                throw new Exception("На данном устройстве нет доступных драйверов!!!");
            }            
        }
    }
}
