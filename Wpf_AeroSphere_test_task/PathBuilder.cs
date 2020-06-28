using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wpf_AeroSphere_test_task
{
    static class PathBuilder
    {
        private const string divided_symbol = "❯";
       static public string Get_path(ListView listview_path_parts)
        {
            string full_path = "";
            foreach (var path_part in listview_path_parts.Items)
            {
                var elt = (FragmentedPath)path_part;
                full_path = Path.Combine(full_path, elt.Path_frame);
            }
            
            return full_path;
        }

        static public void dir_up(ListView listview_path_parts)
        {
            if (listview_path_parts != null && listview_path_parts.Items.Count > 1)
            {
                listview_path_parts.Items.RemoveAt(listview_path_parts.Items.Count - 1);
            }
            else;//корень мы не удаляем
        }

        static public void dir_down(ListView listview_path_parts, string folder)
        {            
            if (listview_path_parts!= null)
            {
                listview_path_parts.Items.Add(new FragmentedPath{Divide_symbol = divided_symbol, Path_frame = folder });
            }
        }
    }
}
