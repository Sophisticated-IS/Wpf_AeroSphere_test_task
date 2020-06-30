using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wpf_AeroSphere_test_task
{
    public static class PathBuilder//для перехода по директориям и правильного отображения пути в GUI
    {
        private const string divided_symbol = "❯";
        static public string Get_path(ListView listview_path_parts)//собирает путь из элементов листбокс
        {
            string full_path = "";
            foreach (var path_part in listview_path_parts.Items)
            {
                var elt = (FragmentedPath)path_part;
                full_path = Path.Combine(full_path, elt.Path_frame);
            }

            return full_path;
        }

        static public void Dir_up(ListView listview_path_parts)//продвигается вверх по пути в листбокс
        {
            if (listview_path_parts != null && listview_path_parts.Items.Count > 1)
            {
                listview_path_parts.Items.RemoveAt(listview_path_parts.Items.Count - 1);
            }
            else;//корень мы не удаляем
        }

        static public void Dir_down(ListView listview_path_parts, string folder)//продвигается вниз по пути в листбокс
        {
            if (listview_path_parts != null)
            {
                listview_path_parts.Items.Add(new FragmentedPath { Divide_symbol = divided_symbol, Path_frame = folder });
            }
        }


        /// <param name="index">
        /// Конец listview.Items до которого (включительно) надо копировать путь индекс должен быть больше 0 так как корень не учитывается
        /// </param>
        static public string Get_path(ListView listview_path_parts, int index)//берет путь от начала до указанного индекса
        {
            string full_path = "";
            if (listview_path_parts != null)
            {
                if (index >= 0)
                {
                    if (listview_path_parts.Items.Count > 0)
                    {
                        if (listview_path_parts.Items.Count - 1 >= index)
                        {
                            if (index == 0)
                            {
                                var elt = (FragmentedPath)listview_path_parts.Items[0];
                                full_path = Path.Combine(full_path, elt.Path_frame);
                            }
                            else
                            {
                                for (int i = 0; i < index; i++)
                                {
                                    var elt = (FragmentedPath)listview_path_parts.Items[i];
                                    full_path = Path.Combine(full_path, elt.Path_frame);
                                }
                            }

                        }
                        else
                        {
                            throw new Exception("Индекс превышает размер коллекции!");
                        }


                    }
                    else
                    {
                        throw new Exception("Переданная коллекия пуста!");
                    }



                }
                else
                {
                    throw new Exception("Индекс должен быть положительным!");
                }

            }
            else
            {
                throw new Exception("Listview ссылается на null!");
            }
            return full_path;
        }
    }
}
