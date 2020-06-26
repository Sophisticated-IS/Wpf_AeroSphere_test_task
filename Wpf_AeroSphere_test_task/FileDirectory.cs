using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AeroSphere_test_task
{
    class FileDirectory : IDirChangeable
    {
        public FileDirectory()
        {
            CurrentDir = Set_default_dir();
        }
        public string CurrentDir { get;}        

        public void Move_dir_up()
        {
            throw new NotImplementedException();
        }

        public void Mov_dir_down()
        {
            throw new NotImplementedException();
        }

        public DriveInfo[] Get_disks_list()
        {
           return DriveInfo.GetDrives();
        }
        private string Set_default_dir()//устанавливает первый драйвер по умолчанию
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            if (allDrives.Length > 0)
            {
                return allDrives[0].Name;
            }
            else
            {
                throw new Exception("На данном устройстве нет доступных драйверов!!!");
            }
            
        }
    }
}
