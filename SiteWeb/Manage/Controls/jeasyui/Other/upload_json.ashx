﻿<%@ WebHandler Language="C#" Class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;

public class Upload : IHttpHandler
{
    private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

        #region 保存路径适应站群分目录

        #endregion


        //文件保存目录路径
        String savePath = "/Upload/";
        String siteSavePath = "/Publish/default/Upload/";
        //文件保存目录URL
        String saveUrl = "/Upload/";

        string sitepath = ObjectCMS.Common.Cookie.GetCookie("curdb");
        if (!string.IsNullOrEmpty(sitepath))
        {
            siteSavePath = "/Publish/" + sitepath + "/Upload/";
        }


        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2,swf,flv");

        //最大文件大小
        int maxSize = 1000000000;
        this.context = context;

        HttpPostedFile imgFile = context.Request.Files["imgFile"];
        if (imgFile == null)
        {
            showError("请选择文件。");
        }

        String dirPath = context.Server.MapPath(savePath);
        String siteDirPath = context.Server.MapPath(siteSavePath);
        if (!Directory.Exists(dirPath))
        {
            CreateFolder(savePath);
        }
        if (!Directory.Exists(siteDirPath))
        {
            CreateFolder(savePath);
        }

        String dirName = context.Request.QueryString["dir"];
        if (String.IsNullOrEmpty(dirName))
        {
            dirName = "image";
        }
        if (!extTable.ContainsKey(dirName))
        {
            showError("目录名不正确。");
        }

        String fileName = imgFile.FileName;
        String fileExt = Path.GetExtension(fileName).ToLower();

        //if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        //{
        //    showError("上传文件大小超过限制。");
        //}

        if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        }

        //创建文件夹
        dirPath += dirName + "/";
        siteDirPath += dirName + "/";

        saveUrl += dirName + "/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (!Directory.Exists(siteDirPath))
        {
            Directory.CreateDirectory(siteDirPath);
        }

        String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
        dirPath += ymd + "/";
        siteDirPath += ymd + "/";

        saveUrl += ymd + "/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (!Directory.Exists(siteDirPath))
        {
            Directory.CreateDirectory(siteDirPath);
        }

        String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
        String filePath = dirPath + newFileName;
        String siteFilePath = siteDirPath + newFileName;

        imgFile.SaveAs(filePath);
        imgFile.SaveAs(siteFilePath);

        String fileUrl = saveUrl + newFileName;

        Hashtable hash = new Hashtable();
        hash["error"] = 0;
        hash["url"] = fileUrl;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
    public void CreateFolder(string folder)
    {
        string[] folders = folder.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

        string f = "";
        for (int i = 0; i < folders.Length; i++)
        {
            f += "/" + folders[i];
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(f)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(f));
            }
        }
    }
}
