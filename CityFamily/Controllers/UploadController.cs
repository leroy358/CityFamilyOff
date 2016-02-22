using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vippiano.Controllers
{
    public class UploadController : Controller
    {
        public string Images()
        {
            string[] fileExtentsImg = new string[] { "jpg", "png", "jpeg" };
            string origion_file;
            string datetime_str;
            if (Request.Files.Count == 0) return null;
            int filesnums = Request.Files.Count;
            string filesname = string.Empty;
            for (int i = 0; i < filesnums; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                DateTime dateTime = System.DateTime.Now;
                string fileExtent = WXSSK.Common.DirectoryAndFile.GetFileExt(file.FileName);
                if (string.IsNullOrEmpty(fileExtent) || !fileExtentsImg.Contains(fileExtent.ToLower()))
                {
                    return "bad file type";
                }
                string directory = "Images\\data\\" + string.Format("{0:yyyyMM}", dateTime);//this.Request.PhysicalApplicationPath + 

                WXSSK.Common.DirectoryAndFile.CreateDirectory(directory);
                string guid = System.Guid.NewGuid().ToString();

                //datetime_str = dateTime.ToString("yyyyMMddHHmmssffff" + i);
                datetime_str = guid + i;
                // 文件名（时间前缀+扩展名）
                string filenewname = datetime_str + "." + fileExtent;
                string fullPath = this.Request.PhysicalApplicationPath + directory + "/";
                origion_file = fullPath + filenewname;
                // 保存位置（服务器地址+文件夹地址+文件名）
                file.SaveAs(origion_file);
                filesname = filesname + "/Images/data/" + string.Format("{0:yyyyMM}", dateTime) + "/" + filenewname + " ";
                if (i == filesnums - 1)
                {
                    filesname = filesname.Substring(0, filesname.Length - 1);
                }
            }
            return filesname;
        }
        public string CADFiles()
        {
            string[] fileExtentsVid = new string[] { "zip", "rar", "dwg"};
            string origion_file;
            if (Request.Files.Count == 0) return null;
            int filesnums = Request.Files.Count;
            string filesname = string.Empty;
            for (int i = 0; i < filesnums; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                DateTime dateTime = System.DateTime.Now;
                string fileExtent = WXSSK.Common.DirectoryAndFile.GetFileExt(file.FileName);
                if (string.IsNullOrEmpty(fileExtent) || !fileExtentsVid.Contains(fileExtent.ToLower()))
                {
                    return "bad file type";
                }

                string guid = System.Guid.NewGuid().ToString();
                string directory = "CADFiles\\data\\" + string.Format("{0:yyyyMM}", dateTime) + "\\" + guid;
                
                    WXSSK.Common.DirectoryAndFile.CreateDirectory(directory);
                
                //datetime_str = dateTime.ToString("yyyyMMddHHmmssfff" + i);
                // 文件名（时间前缀+扩展名)
                //string filenewname = datetime_str + "." + fileExtent;
                string filenewname = WXSSK.Common.DirectoryAndFile.GetFileName(file.FileName);
                string fullPath = this.Request.PhysicalApplicationPath + directory + "/";
                origion_file = fullPath + filenewname;

                // 保存位置（服务器地址+文件夹地址+文件名）
                file.SaveAs(origion_file);
                filesname = filesname + "/CADFiles/data/" + string.Format("{0:yyyyMM}", dateTime) + "/" + guid + "/" + filenewname + "|";
                if (i == filesnums - 1)
                {
                    filesname = filesname.Substring(0, filesname.Length - 1);
                }
            }
            return filesname;
        }
        public string ResourceFiles()
        {
            string[] fileExtentsVid = new string[] { "zip", "rar" };
            string origion_file;
            if (Request.Files.Count == 0) return null;
            int filesnums = Request.Files.Count;
            string filesname = string.Empty;
            for (int i = 0; i < filesnums; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                DateTime dateTime = System.DateTime.Now;
                string fileExtent = WXSSK.Common.DirectoryAndFile.GetFileExt(file.FileName);
                if (string.IsNullOrEmpty(fileExtent) || !fileExtentsVid.Contains(fileExtent.ToLower()))
                {
                    return "bad file type";
                }

                string guid = System.Guid.NewGuid().ToString();
                string directory = "3DResource\\data\\" + string.Format("{0:yyyyMM}", dateTime) + "\\" + guid;

                WXSSK.Common.DirectoryAndFile.CreateDirectory(directory);
                //datetime_str = dateTime.ToString("yyyyMMddHHmmssfff" + i);
                // 文件名（时间前缀+扩展名）
                //string filenewname = datetime_str + "." + fileExtent;
                string filenewname = WXSSK.Common.DirectoryAndFile.GetFileName(file.FileName);
                string fullPath = this.Request.PhysicalApplicationPath + directory + "/";
                origion_file = fullPath + filenewname;

                // 保存位置（服务器地址+文件夹地址+文件名）
                file.SaveAs(origion_file);
                filesname = filesname + "/3DResource/data/" + string.Format("{0:yyyyMM}", dateTime) + "/" + guid + "/" + filenewname + "|";
                if (i == filesnums - 1)
                {
                    filesname = filesname.Substring(0, filesname.Length - 1);
                }
            }
            return filesname;
        }
    }
}