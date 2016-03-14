using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CityFamily.Controllers
{
    public class StylesController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        GetWebSession websession = new GetWebSession();
        public ActionResult List()
        {
            if (Session["userName"] != null)
            {
                int adminId = websession.AdminId;
                int companyId = websession.CompanyId;
                // var companyModel = db.T_CompanyInfo.Where(o => o.CompanyID == companyId).FirstOrDefault();
                var notshowdata = db.StylesID.Where(o => o.CompanyId == companyId).Select(o => o.StylesId).ToList();
                var styles = db.Styles.Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).OrderBy(item => item.Id);
                return View(styles);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }
        public ActionResult SecList(int id)
        {
            if (Session["userName"] != null)
            {
                Styles style = db.Styles.Find(id);
                ViewBag.styleName = style.StyleName;
                var secStyles = db.StyleDetails.Where(item => item.StyleId == id).OrderBy(item => item.Id);
                if (secStyles.Count() == 1)
                {
                    return RedirectToAction("ThirdList", new { id = secStyles.FirstOrDefault().Id });
                }
                return View(secStyles);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ThirdList(int id)
        {
            if (Session["userName"] != null)
            {
                var styleThird = db.StyleThird.Where(item => item.StyleDetailId == id).OrderBy(item => item.Id);
                StyleDetails styleDetail = db.StyleDetails.Find(id);
                ViewBag.StyleDetailName = styleDetail.StyleDetailName;
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View(styleThird);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult StyleThird(int id)
        {
            if (Session["userName"] != null)
            {
                StyleThird styleThird = db.StyleThird.Find(id);
                StyleDetails styleDetail = db.StyleDetails.Find(styleThird.StyleDetailId);
                ViewBag.StyleDetailName = styleDetail.StyleDetailName;
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View(styleThird);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Style720Show(int id)
        {
            if (Session["userName"] != null)
            {
                StyleThird styleThird = db.StyleThird.Find(id);
                StyleDetails styleDetail = db.StyleDetails.Find(styleThird.StyleDetailId);
                ViewBag.StyleDetailName = styleDetail.StyleDetailName;
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View(styleThird);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult GuestDIY(int id)
        {
            StyleThird styleThird = db.StyleThird.Find(id);
            StyleDetails styleDetail = db.StyleDetails.Find(styleThird.StyleDetailId);
            ViewBag.StyleDetailName = styleDetail.StyleDetailName;
            Styles style = db.Styles.Find(styleDetail.StyleId);
            ViewBag.StyleName = style.StyleName;
            ViewBag.StyleId = style.Id;
            return View(styleThird);

            //StyleDetails styleDetail = db.StyleDetails.Find(Convert.ToInt32(styleId));
            //ViewBag.styleDetailName = styleDetail.StyleDetailName;
            //ViewBag.styleDetailId = styleDetail.Id;
            //ViewBag.styleResource = styleDetail.StyleResource;
            //ViewBag.styleCode = styleDetail.StyleDetailCode;
            //ViewBag.styleName = db.Styles.Find(styleDetail.StyleId).StyleName;
            //ViewBag.styleId = db.Styles.Find(styleDetail.StyleId).Id;
            //return View(styleDetail);

        }

        [HttpPost]
        public ActionResult SaveDIY(string userName, string comment, string styleName, string styleDetail, int styleThirdId, string diyJson, string ReturnURL)
        {
            Session["diyResultUrl"] = ReturnURL;
            DIYResult diyResult = new DIYResult();
            diyResult.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
            diyResult.UserId = (int)Session["userId"];
            diyResult.UserName = (string)Session["userName"];
            diyResult.GuestName = userName;
            Session["GuestName"] = userName;
            diyResult.DIYJson = diyJson;
            diyResult.Remark = comment;
            diyResult.Style = styleName;
            diyResult.StyleDetail = styleDetail;
            diyResult.StyleDetailId = styleThirdId;
            diyResult.StyleUrl = ReturnURL;
            db.DIYResult.Add(diyResult);
            db.SaveChanges();

            return RedirectToAction("Questionnaire", "Function");
            //return RedirectToAction("StyleThird", new { id = styleThirdId });
        }
        public void DownLoad(string filePath)
        {
            if (Session["userName"] != null)
            {
                //还原文件路径
                filePath = "/3DResource/data/" + filePath;

                //获取文件大小
                filePath = Server.MapPath(filePath);
                FileInfo info = new FileInfo(filePath);
                long fileSize = info.Length;

                //设置下载文件名
                int intStart = filePath.LastIndexOf("\\") + 1;
                string saveFileName = filePath.Substring(intStart, filePath.Length - intStart);
                HttpContext.Response.Clear();

                //指定Http Mime格式为压缩包
                HttpContext.Response.ContentType = "application/x-zip-compressed";

                // Http 协议中有专门的指令来告知浏览器, 本次响应的是一个需要下载的文件. 格式如下:
                // Content-Disposition: attachment;filename=filename.ext
                HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + saveFileName);

                //不指明Content-Length用Flush的话不会显示下载进度   
                HttpContext.Response.AddHeader("Content-Length", fileSize.ToString());
                HttpContext.Response.TransmitFile(filePath, 0, fileSize);
                HttpContext.Response.Flush();
                HttpContext.Response.Close();
            }
        }
        [HttpPost]
        public ActionResult GetData(string id)
        {

            string targetUrl = TargetUrl;
            if (!string.IsNullOrEmpty(targetUrl))
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(targetUrl);
                req.Method = "Post";

                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"request\"");
                sb.Append(":");
                sb.Append("\"getSampleRoom\"");
                sb.Append(",");
                sb.Append("\"sampleRoom\"");
                sb.Append(":{");
                sb.Append("\"requestCode\"");
                sb.Append(":");
                sb.Append("\"" + id + "\"");
                sb.Append("}");
                sb.Append("}");

                string reqJson = sb.ToString();

                //return Content(reqJson);

                byte[] postBytes = Encoding.UTF8.GetBytes(reqJson);
                req.ContentType = "application/json;charset=utf-8";
                req.ContentLength = Encoding.UTF8.GetByteCount(reqJson);
                req.Accept = "*";
                Stream stream = req.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                //Response.AddHeader("Access-Control-Allow-Origin", "*");
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();


                StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string respHtml = sr.ReadToEnd();
                return Content(respHtml);
            }
            else
            {
                return Content("");
            }
        }


        private string TargetUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["TargetUrl"];
                if (!string.IsNullOrEmpty(url))
                {
                    return url;
                }
                return "";
            }

        }

        public ActionResult GetStyleList(int companyId)
        {
            var notshowdata = db.StylesID.Where(o => o.CompanyId == companyId).Select(o => o.StylesId).ToList();
            var styles = db.Styles.Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).OrderBy(item => item.Id);
            List<StylesData> styleDataList = new List<StylesData>();
            foreach(Styles style in styles)
            {
                StylesData stylesData = new StylesData();
                stylesData.StyleId = style.Id;
                stylesData.StyleName = style.StyleName;
                stylesData.StyleIndex = style.StyleIndex;
                styleDataList.Add(stylesData);
            }
            return Json(new { data = styleDataList }, JsonRequestBehavior.AllowGet);
        }

    }
}