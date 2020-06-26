using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AeroSphere_test_task
{
    class Drives_list : IDirChangeable
    {
        public Drives_list()
        {
            AllDrives = Get_all_drives();
        }
        private string currentDirName;//имя текущей директории
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
        public DriveInfo[] AllDrives { get; }

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
            return Directory.GetDirectories(currentDirName); 
        }
        private DriveInfo[] Get_all_drives()
        {
            DriveInfo[] AllDrives = DriveInfo.GetDrives();

            if (AllDrives.Length > 0)
            {
                return AllDrives;
            }
            else
            {
                throw new Exception("На данном устройстве нет доступных драйверов!!!");
            }
        }
    }
}
