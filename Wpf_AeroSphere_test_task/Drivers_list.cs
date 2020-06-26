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
        }
        private string currentDirName;
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
        public DriveInfo[] AllDrivers { get; }

        public void Move_dir_up()
        {
            throw new NotImplementedException();
        }

        public void Mov_dir_down()
        {
            throw new NotImplementedException();
        }

        public string[] GetAllFiles()
        {
            return Directory.GetFiles(CurrentDirName);
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
