using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wpf_AeroSphere_test_task.Tests
{
    [TestClass]
    public class PathBuilderTests
    {
        [TestMethod]
        public void Test_path_root_Get_path()
        {
            //arrange
            ListView path_parts = new ListView() { };
            FragmentedPath fragmented_p = new FragmentedPath
            {
                Path_frame = @"C:\\"
            };
            path_parts.Items.Add(fragmented_p);
            string expected_path = @"C:\\";
           
            //act 
            string actual_path = PathBuilder.Get_path(path_parts);
            
            //assert
            Assert.AreEqual(expected_path, actual_path);
        }

        [TestMethod]
        public void Test_path_root_Dir_up()
        {
            //arrange
            ListView path_parts = new ListView() { };
            FragmentedPath fragmented_p = new FragmentedPath
            {
                Path_frame = @"C:\\"
            };
            path_parts.Items.Add(fragmented_p);
            int expected_capacity = 1;
            //act 
            PathBuilder.Dir_up(path_parts);
            int actual_capacity = path_parts.Items.Count;
            //assert
            Assert.AreEqual(expected_capacity,actual_capacity );
        }
    }
}
