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


        /// <summary>
        /// 获取风格定位首页列表
        /// </summary>
        /// <param name="companyId">登录用户分公司ID</param>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "StyleId":1,
        ///             "StyleName":"自然风格",
        ///             "StyleIndex":"/Images/data/201507/a8612312-2ec8-4b4e-9108-53f881bbb3a60.jpg"
        ///         },
        ///         {
        ///             "StyleId":2,
        ///             "StyleName":"中式风格",
        ///             "StyleIndex":"/Images/data/201507/1629faa7-c6bd-411f-974a-96e0258017960.jpg"
        ///         },
        ///         {
        ///             "StyleId":3,
        ///             "StyleName":"地中海风格",
        ///             "StyleIndex":"/Images/data/201507/45e76ec8-f7cc-43a9-ae6a-e1b4e81f19a00.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
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


        /// <summary>
        ///  获取二级风格列表
        /// </summary>
        /// <param name="styleId">一级风格ID</param>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "StyleSecondId":1
        ///             "StyleDSecondName":"美式乡村",
        ///             "StyleSecondIndex":"/Images/data/201508/b8bfa8bc-54f3-4450-9d43-5243e622397a0.png"
        ///         },
        ///         {
        ///             "StyleSecondId":16,
        ///             "StyleDSecondName":"英式田园",
        ///             "StyleSecondIndex":"/Images/data/201508/38ae5c2b-5fe6-475e-8179-348fffafde570.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        public ActionResult GetStyleSecList(int styleId)
        {
            var secStyles = db.StyleDetails.Where(item => item.StyleId == styleId);
            List<StyleDetailsData> styleDetailList = new List<StyleDetailsData>();
            foreach(StyleDetails styleDetail in secStyles)
            {
                StyleDetailsData styleDetailData = new StyleDetailsData();
                styleDetailData.StyleSecondId = styleDetail.Id;
                styleDetailData.StyleDSecondName = styleDetail.StyleDetailName;
                styleDetailData.StyleSecondIndex = styleDetail.StyleDetailIndex;
                styleDetailList.Add(styleDetailData);
            }
            return Json(new { data = styleDetailList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三级风格列表信息
        /// </summary>
        /// <param name="styleSecId">二级风格ID</param>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "StyleThirdId":22,
        ///             "StyleThirdName":"美式乡村160平米--客厅",
        ///             "StyleThirdIndex":"/Images/data/201508/9af34ee7-6675-41d4-9b61-eb18775ef6180.png"
        ///         },
        ///         {
        ///             "StyleThirdId":55,
        ///             "StyleThirdName":"美式乡村160平米--餐厅",
        ///             "StyleThirdIndex":"/Images/data/201508/fdafe0b6-fb9b-4ddf-96da-b847d744a1200.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        public ActionResult GetStyleThiList(int styleSecId)
        {
            List<StyleThird> styleThirds = db.StyleThird.Where(item => item.StyleDetailId == styleSecId).ToList();
            List<StyleThirdData> styleThirdList = new List<StyleThirdData>();
            foreach(StyleThird styleThird in styleThirds)
            {
                StyleThirdData styleThirdData = new StyleThirdData();
                styleThirdData.StyleThirdId = styleThird.Id;
                styleThirdData.StyleThirdName = styleThird.StyleThirdName;
                styleThirdData.StyleThirdIndex = styleThird.StyleThirdIndex;
                styleThirdList.Add(styleThirdData);
            }
            return Json(new { data = styleThirdList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三级风格图片详情
        /// </summary>
        /// <param name="styleThirdId"></param>
        /// <returns>
        /// {
        ///     "styleThirdPics":"/Images/data/201507/9f202d77-641f-45f2-9684-5e8d3199d59a0.jpg /Images/data/201507/f9886162-0ca8-4d44-b931-fe86c01fbed91.jpg /Images/data/201507/718708ca-d6d0-4390-bfe4-bf1460d353512.jpg /Images/data/201507/acd768d4-d1f2-4835-99de-7cb2a99ba90c3.jpg "
        /// }
        /// </returns>
        public ActionResult GetStyleThirdDetail(int styleThirdId)
        {
            StyleThird styleThird = db.StyleThird.Find(styleThirdId);
            string styleThirdPics = styleThird.StyleThirdPics;
            return Json(new { styleThirdPics = styleThirdPics }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三级风格720地址
        /// </summary>
        /// <param name="styleThirdId"></param>
        /// <returns>
        /// {
        ///     "styleThird720":"http://119.188.113.104:8081/Show/chuanTongOuShi/chuanTongOuShi.html"
        /// }
        /// </returns>
        public ActionResult GetStyleThird360(int styleThirdId)
        {
            StyleThird styleThird = db.StyleThird.Find(styleThirdId);
            string styleThird720 = styleThird.StyleThird720;
            return Json(new { styleThird720 = styleThird720 }, JsonRequestBehavior.AllowGet);
        }

    }
}