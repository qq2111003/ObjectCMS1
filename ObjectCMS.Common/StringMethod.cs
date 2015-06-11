using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ObjectCMS.Common
{
    public static class StringMethod
    {
        /// <summary>
        /// 计算path2到path1的相对路径
        /// 如/Manage/Permissions/RoleManage.aspx（path1）要引用/Manage/plugin/jquery-easyui/jquery.easyui.min.js（path2）
        /// 则输出../plugin/jquery-easyui/jquery.easyui.min.js
        /// </summary>
        /// <param name="path1">当前文件路径</param>
        /// <param name="path2">被引用文件路径</param>
        /// <returns></returns>
        public static string GetRelativePath(string path1, string path2)
        {
            string[] path1Array = path1.Split('/');
            string[] path2Array = path2.Split('/');
            //
            int s = path1Array.Length >= path2Array.Length ? path2Array.Length : path1Array.Length;
            //两个目录最底层的共用目录索引
            int closestRootIndex = -1;
            for (int i = 0; i < s; i++)
            {
                if (path1Array[i] == path2Array[i])
                {
                    closestRootIndex = i;
                }
                else
                {
                    break;
                }
            }
            //由path1计算 ‘../’部分
            string path1Depth = "";
            for (int i = 0; i < path1Array.Length; i++)
            {
                if (i > closestRootIndex + 1)
                {
                    path1Depth += "../";
                }
            }
            //由path2计算 ‘../’后面的目录
            string path2Depth = "";
            for (int i = closestRootIndex + 1; i < path2Array.Length; i++)
            {
                path2Depth += "/" + path2Array[i];
            }
            path2Depth = path2Depth.Substring(1);

            return path1Depth + path2Depth;
        }

       
    }
}
