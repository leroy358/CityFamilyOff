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
        [HttpPost]
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
                stylesData.StyleIndex = ConfigurationManager.AppSettings["ResourceUrl"] + style.StyleIndex;
                stylesData.UpdateTime = style.CreateTime;
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
        ///             "StyleName":"自然风格",
        ///             "StyleSecondId":1
        ///             "StyleDSecondName":"美式乡村",
        ///             "StyleSecondIndex":"/Images/data/201508/b8bfa8bc-54f3-4450-9d43-5243e622397a0.png"
        ///         },
        ///         {
        ///             "StyleName":"自然风格",
        ///             "StyleSecondId":16,
        ///             "StyleDSecondName":"英式田园",
        ///             "StyleSecondIndex":"/Images/data/201508/38ae5c2b-5fe6-475e-8179-348fffafde570.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetStyleSecList(int styleId)
        {
            Styles style = db.Styles.Find(styleId);
            var secStyles = db.StyleDetails.Where(item => item.StyleId == styleId);
            List<StyleDetailsData> styleDetailList = new List<StyleDetailsData>();
            foreach(StyleDetails styleDetail in secStyles)
            {
                StyleDetailsData styleDetailData = new StyleDetailsData();
                styleDetailData.StyleName = style.StyleName;
                styleDetailData.StyleSecondId = styleDetail.Id;
                styleDetailData.StyleDSecondName = styleDetail.StyleDetailName;
                styleDetailData.StyleSecondIndex = ConfigurationManager.AppSettings["ResourceUrl"] + styleDetail.StyleDetailIndex;
                styleDetailData.UpdateTime = styleDetail.CreateTime;
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
        ///              "StyleName”:"自然风格",
        ///              "StyleSecName":"英式田园",
        ///             "StyleThirdId":22,
        ///             "StyleThirdName":"美式乡村160平米--客厅",
        ///             "StyleThirdIndex":"/Images/data/201508/9af34ee7-6675-41d4-9b61-eb18775ef6180.png"
        ///         },
        ///         {
        ///              "StyleName”:"自然风格",
        ///              "StyleSecName":"英式田园",
        ///             "StyleThirdId":55,
        ///             "StyleThirdName":"美式乡村160平米--餐厅",
        ///             "StyleThirdIndex":"/Images/data/201508/fdafe0b6-fb9b-4ddf-96da-b847d744a1200.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetStyleThiList(int styleSecId)
        {
            StyleDetails styleDetail = db.StyleDetails.Find(styleSecId);
            Styles style = db.Styles.Find(styleDetail.StyleId);

            List<StyleThird> styleThirds = db.StyleThird.Where(item => item.StyleDetailId == styleSecId).ToList();
            List<StyleThirdData> styleThirdList = new List<StyleThirdData>();
            foreach(StyleThird styleThird in styleThirds)
            {
                StyleThirdData styleThirdData = new StyleThirdData();
                styleThirdData.StyleName = style.StyleName;
                styleThirdData.StyleSecName = styleDetail.StyleDetailName;
                styleThirdData.StyleThirdId = styleThird.Id;
                styleThirdData.StyleThirdName = styleThird.StyleThirdName;
                styleThirdData.StyleThirdIndex = ConfigurationManager.AppSettings["ResourceUrl"] + styleThird.StyleThirdIndex;
                styleThirdList.Add(styleThirdData);
            }
            return Json(new { data = styleThirdList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三级风格图片详情
        /// </summary>
        /// <param name="styleThirdId">三级风格ID</param>
        /// <returns>
        /// {
        ///     {"StyleThirdPics":[
        ///         "http://119.188.113.104:8085/Images/data/201507/6c150502-f7a8-4bff-bbaa-7b12df56386e0.jpg"
        ///         "http://119.188.113.104:8085/Images/data/201507/1ba1f162-0fc7-469d-8337-d215d60b3fea1.jpg",
        ///         "http://119.188.113.104:8085/Images/data/201508/2b2d1709-7294-40f7-93ad-22bb32b548b90.jpg",
        ///         "http://119.188.113.104:8085/Images/data/201508/2fd4fd2c-a095-4a78-94d8-096eb4096e2f0.jpg"
        ///     ]
        /// }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetStyleThirdDetail(int styleThirdId)
        {
            StyleThird styleThird = db.StyleThird.Find(styleThirdId);
            string[] styleThirdPics = styleThird.StyleThirdPics.Substring(0, styleThird.StyleThirdPics.Length - 1).Split(' ');
            for (int i = 0; i < styleThirdPics.Length; i++)
            {
                styleThirdPics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + styleThirdPics[i];
            }
            return Json(new { StyleThirdPics = styleThirdPics }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三级风格720地址
        /// </summary>
        /// <param name="styleThirdId">三级风格ID</param>
        /// <returns>
        /// {
        ///     "StyleThird720":"http://119.188.113.104:8081/Show/chuanTongOuShi/chuanTongOuShi.html"
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetStyleThird360(int styleThirdId)
        {
            StyleThird styleThird = db.StyleThird.Find(styleThirdId);
            string styleThird720 = styleThird.StyleThird720;
            return Json(new { StyleThird720 = styleThird720 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三级风格RequestCode
        /// </summary>
        /// <param name="styleThirdId">三级风格ID</param>
        /// <returns>
        /// {
        ///     "StyleThirdCode":"ded0065a-f46e-446a-8250-326418863f41"
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetStyleThirdCode(int styleThirdId)
        {
            StyleThird styleThird = db.StyleThird.Find(styleThirdId);
            string styleThirdCode = styleThird.StyleThirdCode;
            return Json(new { StyleThirdCode = styleThirdCode }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取DIY数据
        /// </summary>
        /// <param name="code">三级风格的RequestCode</param>
        /// <returns>
        /// {
        ///     "Result":{
        ///         "Success":true,
        ///         "Note":""
        ///     },
        ///     "SampleRoom":{
        ///         "id":58,
        ///         "name":"美式乡村160平米（客厅）",
        ///         "summary":"",
        ///         "itemPlaceholderList":[
        ///             {
        ///                 "id":341,
        ///                 "name":"造型",
        ///                 "replaceableItemList":[
        ///                     {
        ///                         "id":1855,
        ///                         "name":"造型06",
        ///                         "useDictionaryItem":{
        ///                             "id":1810,
        ///                             "name":"造型06",
        ///                             "thumbnailUrl":"/Images/201508/20150812141618676.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米06"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米06"
        ///                     },
        ///                     {
        ///                         "id":1854,
        ///                         "name":"造型05",
        ///                         "useDictionaryItem":{
        ///                             "id":1809,
        ///                             "name":"造型05",
        ///                             "thumbnailUrl":"/Images/201508/20150812141607693.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米05"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米05"
        ///                     },
        ///                     {
        ///                         "id":1853,
        ///                         "name":"造型04",
        ///                         "useDictionaryItem":{
        ///                             "id":1808,
        ///                             "name":"造型04",
        ///                             "thumbnailUrl":"/Images/201508/20150812141557990.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米04"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米04"
        ///                     },
        ///                     {
        ///                         "id":1852,
        ///                         "name":"造型03",
        ///                         "useDictionaryItem":{
        ///                             "id":1807,
        ///                             "name":"造型03",
        ///                             "thumbnailUrl":"/Images/201508/20150812141547351.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米03"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米03"
        ///                     },
        ///                     {
        ///                         "id":1851,
        ///                         "name":"造型02",
        ///                         "useDictionaryItem":{
        ///                             "id":1806,
        ///                             "name":"造型02",
        ///                             "thumbnailUrl":"/Images/201508/20150812141510581.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米02"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米02"
        ///                     },
        ///                     {
        ///                         "id":1850,
        ///                         "name":"造型01",
        ///                         "useDictionaryItem":{
        ///                             "id":1805,
        ///                             "name":"造型01",
        ///                             "thumbnailUrl":"/Images/201508/20150812141459583.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米01"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":true,
        ///                         "styleKeywords":"美式乡村风格160平米01"
        ///                     }
        ///                 ]
        ///             },
        ///             {"id":340,"name":"墙","replaceableItemList":[{"id":1849,"name":"墙06","useDictionaryItem":{"id":1804,"name":"墙06","thumbnailUrl":"/Images/201508/20150812141433157.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1848,"name":"墙05","useDictionaryItem":{"id":1803,"name":"墙05","thumbnailUrl":"/Images/201508/20150812141421473.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1847,"name":"墙04","useDictionaryItem":{"id":1802,"name":"墙04","thumbnailUrl":"/Images/201508/20150812141410287.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1846,"name":"墙03","useDictionaryItem":{"id":1801,"name":"墙03","thumbnailUrl":"/Images/201508/20150812141359086.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1845,"name":"墙02","useDictionaryItem":{"id":1800,"name":"墙02","thumbnailUrl":"/Images/201508/20150812141346669.jpg","unitPrice":11,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1844,"name":"墙01","useDictionaryItem":{"id":1799,"name":"墙01","thumbnailUrl":"/Images/201508/20150812141335218.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":339,"name":"门","replaceableItemList":[{"id":1843,"name":"门06","useDictionaryItem":{"id":1798,"name":"门06","thumbnailUrl":"/Images/201508/20150812141322536.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1842,"name":"门05","useDictionaryItem":{"id":1797,"name":"门05","thumbnailUrl":"/Images/201508/20150812141311101.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1841,"name":"门04","useDictionaryItem":{"id":1796,"name":"门04","thumbnailUrl":"/Images/201508/20150812141300150.jpg","unitPrice":11,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1840,"name":"门03","useDictionaryItem":{"id":1795,"name":"门03","thumbnailUrl":"/Images/201508/20150812141235065.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1839,"name":"门02","useDictionaryItem":{"id":1794,"name":"门02","thumbnailUrl":"/Images/201508/20150812141223521.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1838,"name":"门01","useDictionaryItem":{"id":1793,"name":"门01","thumbnailUrl":"/Images/201508/20150812141209247.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":338,"name":"家具","replaceableItemList":[{"id":1837,"name":"家具06","useDictionaryItem":{"id":1792,"name":"家具06","thumbnailUrl":"/Images/201508/20150814152645368.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1836,"name":"家具05","useDictionaryItem":{"id":1791,"name":"家具05","thumbnailUrl":"/Images/201508/20150814152637661.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1835,"name":"家具04","useDictionaryItem":{"id":1790,"name":"家具04","thumbnailUrl":"/Images/201508/20150814152630922.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1834,"name":"家具03","useDictionaryItem":{"id":1789,"name":"家具03","thumbnailUrl":"/Images/201508/20150814152623762.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1833,"name":"家具02","useDictionaryItem":{"id":1788,"name":"家具02","thumbnailUrl":"/Images/201508/20150814152616601.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1832,"name":"家具01","useDictionaryItem":{"id":1787,"name":"家具01","thumbnailUrl":"/Images/201508/20150814152604199.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":337,"name":"顶面","replaceableItemList":[{"id":1831,"name":"顶面06","useDictionaryItem":{"id":1786,"name":"顶面06","thumbnailUrl":"/Images/201508/20150814152512095.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1830,"name":"顶面05","useDictionaryItem":{"id":1785,"name":"顶面05","thumbnailUrl":"/Images/201508/20150814152522048.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1829,"name":"顶面04","useDictionaryItem":{"id":1784,"name":"顶面04","thumbnailUrl":"/Images/201508/20150814152530425.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1828,"name":"顶面03","useDictionaryItem":{"id":1783,"name":"顶面03","thumbnailUrl":"/Images/201508/20150814152537523.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1827,"name":"顶面02","useDictionaryItem":{"id":1782,"name":"顶面02","thumbnailUrl":"/Images/201508/20150814152544434.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1826,"name":"顶面01","useDictionaryItem":{"id":1781,"name":"顶面01","thumbnailUrl":"/Images/201508/20150814152551189.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":336,"name":"地面","replaceableItemList":[{"id":1825,"name":"地面06","useDictionaryItem":{"id":1780,"name":"地面06","thumbnailUrl":"/Images/201508/20150813102931754.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1824,"name":"地面05","useDictionaryItem":{"id":1779,"name":"地面05","thumbnailUrl":"/Images/201508/20150812140709789.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1823,"name":"地面04","useDictionaryItem":{"id":1778,"name":"地面04","thumbnailUrl":"/Images/201508/20150813102806937.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":0,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1822,"name":"地面03","useDictionaryItem":{"id":1777,"name":"地面03","thumbnailUrl":"/Images/201508/20150813103321667.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1821,"name":"地面02","useDictionaryItem":{"id":1776,"name":"地面02","thumbnailUrl":"/Images/201508/20150812140633113.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1820,"name":"地面01","useDictionaryItem":{"id":1775,"name":"地面01","thumbnailUrl":"/Images/201508/20150813103222028.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]}
        ///         ],
        ///         "perspectiveList":[
        ///             {
        ///                 "id":65,
        ///                 "name":"视角1",
        ///                 "backgroundImageUrl":"/Images/201508/20150812142430810.jpg",
        ///                 "visibleItemPlaceholderList":[
        ///                     {
        ///                         "id":337,
        ///                         "displayIndex":5
        ///                     },
        ///                     {
        ///                         "id":338,
        ///                         "displayIndex":4
        ///                     },
        ///                     {
        ///                         "id":341,
        ///                         "displayIndex":3
        ///                     },
        ///                     {
        ///                         "id":339
        ///                         "displayIndex":2
        ///                     },
        ///                     {
        ///                         "id":336,
        ///                         "displayIndex":1
        ///                     },
        ///                     {
        ///                         "id":340,
        ///                         "displayIndex":0
        ///                     }
        ///                 ]
        ///             }
        ///         ],
        ///         "materialList":[
        ///             {
        ///                 "id":2057,
        ///                 "forPerspective":{
        ///                     "id":65
        ///                 },
        ///                 "forReplaceableItem":{
        ///                     "id":1855
        ///                 },
        ///                 "imageUrl":"/Images/201508/20150812142502524.png"
        ///             },
        ///             {
        ///                 "id":2056,
        ///                 "forPerspective":{
        ///                     "id":65
        ///                 },"
        ///                 forReplaceableItem":
        ///                 {
        ///                     "id":1854
        ///                 },
        ///                 "imageUrl":"/Images/201508/20150812142507828.png"
        ///             },
        ///             {"id":2055,"forPerspective":{"id":65},"forReplaceableItem":{"id":1853},"imageUrl":"/Images/201508/20150812142512025.png"},
        ///             {"id":2054,"forPerspective":{"id":65},"forReplaceableItem":{"id":1852},"imageUrl":"/Images/201508/20150812142516596.png"},
        ///             {"id":2053,"forPerspective":{"id":65},"forReplaceableItem":{"id":1851},"imageUrl":"/Images/201508/20150812142520683.png"},
        ///             {"id":2052,"forPerspective":{"id":65},"forReplaceableItem":{"id":1850},"imageUrl":"/Images/201508/20150812142524474.png"},
        ///             {"id":2051,"forPerspective":{"id":65},"forReplaceableItem":{"id":1849},"imageUrl":"/Images/201508/20150812142528249.png"},
        ///             {"id":2050,"forPerspective":{"id":65},"forReplaceableItem":{"id":1848},"imageUrl":"/Images/201508/20150812142533366.png"},
        ///             {"id":2049,"forPerspective":{"id":65},"forReplaceableItem":{"id":1847},"imageUrl":"/Images/201508/20150812142540167.png"},
        ///             {"id":2048,"forPerspective":{"id":65},"forReplaceableItem":{"id":1846},"imageUrl":"/Images/201508/20150812142544660.png"},
        ///             {"id":2047,"forPerspective":{"id":65},"forReplaceableItem":{"id":1845},"imageUrl":"/Images/201508/20150812142550666.png"},
        ///             {"id":2046,"forPerspective":{"id":65},"forReplaceableItem":{"id":1844},"imageUrl":"/Images/201508/20150812142556594.png"},
        ///             {"id":2045,"forPerspective":{"id":65},"forReplaceableItem":{"id":1843},"imageUrl":"/Images/201508/20150812142600993.png"},
        ///             {"id":2044,"forPerspective":{"id":65},"forReplaceableItem":{"id":1842},"imageUrl":"/Images/201508/20150812142605970.png"},
        ///             {"id":2043,"forPerspective":{"id":65},"forReplaceableItem":{"id":1841},"imageUrl":"/Images/201508/20150812142611523.png"},
        ///             {"id":2042,"forPerspective":{"id":65},"forReplaceableItem":{"id":1840},"imageUrl":"/Images/201508/20150812142616469.png"},
        ///             {"id":2041,"forPerspective":{"id":65},"forReplaceableItem":{"id":1839},"imageUrl":"/Images/201508/20150812142620805.png"},
        ///             {"id":2040,"forPerspective":{"id":65},"forReplaceableItem":{"id":1838},"imageUrl":"/Images/201508/20150812142625485.png"},
        ///             {"id":2039,"forPerspective":{"id":65},"forReplaceableItem":{"id":1837},"imageUrl":"/Images/201508/20150813101602191.png"},
        ///             {"id":2038,"forPerspective":{"id":65},"forReplaceableItem":{"id":1836},"imageUrl":"/Images/201508/20150813101612674.png"},
        ///             {"id":2037,"forPerspective":{"id":65},"forReplaceableItem":{"id":1835},"imageUrl":"/Images/201508/20150813101621784.png"},
        ///             {"id":2036,"forPerspective":{"id":65},"forReplaceableItem":{"id":1834},"imageUrl":"/Images/201508/20150813101638710.png"},
        ///             {"id":2035,"forPerspective":{"id":65},"forReplaceableItem":{"id":1833},"imageUrl":"/Images/201508/20150813102433497.png"},
        ///             {"id":2034,"forPerspective":{"id":65},"forReplaceableItem":{"id":1832},"imageUrl":"/Images/201508/20150812162545539.png"},
        ///             {"id":2033,"forPerspective":{"id":65},"forReplaceableItem":{"id":1831},"imageUrl":"/Images/201508/20150813101654264.png"},
        ///             {"id":2032,"forPerspective":{"id":65},"forReplaceableItem":{"id":1830},"imageUrl":"/Images/201508/20150813101702968.png"},
        ///             {"id":2031,"forPerspective":{"id":65},"forReplaceableItem":{"id":1829},"imageUrl":"/Images/201508/20150813101710394.png"},
        ///             {"id":2030,"forPerspective":{"id":65},"forReplaceableItem":{"id":1828},"imageUrl":"/Images/201508/20150813101717866.png"},
        ///             {"id":2029,"forPerspective":{"id":65},"forReplaceableItem":{"id":1827},"imageUrl":"/Images/201508/20150813101725386.png"},
        ///             {"id":2028,"forPerspective":{"id":65},"forReplaceableItem":{"id":1826},"imageUrl":"/Images/201508/20150813101733201.png"},
        ///             {"id":2027,"forPerspective":{"id":65},"forReplaceableItem":{"id":1825},"imageUrl":"/Images/201508/20150812142727932.png"},
        ///             {"id":2026,"forPerspective":{"id":65},"forReplaceableItem":{"id":1824},"imageUrl":"/Images/201508/20150812142733127.png"},
        ///             {"id":2025,"forPerspective":{"id":65},"forReplaceableItem":{"id":1823},"imageUrl":"/Images/201508/20150812142738072.png"},
        ///             {"id":2024,"forPerspective":{"id":65},"forReplaceableItem":{"id":1822},"imageUrl":"/Images/201508/20150813103349373.png"},
        ///             {"id":2023,"forPerspective":{"id":65},"forReplaceableItem":{"id":1821},"imageUrl":"/Images/201508/20150812142748041.png"},
        ///             {"id":2022,"forPerspective":{"id":65},"forReplaceableItem":{"id":1820},"imageUrl":"/Images/201508/20150812142753813.png"}
        ///         ]
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetDIYData(int styleThirdId)
        {
            string targetUrl = TargetUrl + "/Normal/UserRequest";
            StyleThird style = db.StyleThird.Find(styleThirdId);
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
                sb.Append("\"" + style.StyleThirdCode + "\"");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "StyleThirdId":55,
        ///         "StyleThirdName":"美式乡村160平米--餐厅",
        ///         "StyleThirdIndex":"/Images/data/201508/fdafe0b6-fb9b-4ddf-96da-b847d744a1200.jpg",
        ///         "StyleThirdPics":[
        ///             "http://119.188.113.104:8085/Images/data/201508/f9d9e2a4-daf6-4f93-a7f0-5818f9a7bc750.jpg",
        ///             "http://119.188.113.104:8085/Images/data/201508/a597d8f4-72c8-4f66-85f7-e78abeebc7dc0.jpg",
        ///             "http://119.188.113.104:8085/Images/data/201508/bdb44945-24eb-4486-9642-6ce522df6edd0.jpg",
        ///             "http://119.188.113.104:8085/Images/data/201508/38a2cd56-85b0-4732-9d9d-8a3bfbb1f0f50.png"
        ///         ],
        ///         "UpdateTime":"2016-03-24 16:07:27",
        ///         "DiyData":""Result":{
        ///         "Success":true,
        ///         "Note":""
        ///     },
        ///     "SampleRoom":{
        ///         "id":58,
        ///         "name":"美式乡村160平米（客厅）",
        ///         "summary":"",
        ///         "itemPlaceholderList":[
        ///             {
        ///                 "id":341,
        ///                 "name":"造型",
        ///                 "replaceableItemList":[
        ///                     {
        ///                         "id":1855,
        ///                         "name":"造型06",
        ///                         "useDictionaryItem":{
        ///                             "id":1810,
        ///                             "name":"造型06",
        ///                             "thumbnailUrl":"/Images/201508/20150812141618676.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米06"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米06"
        ///                     },
        ///                     {
        ///                         "id":1854,
        ///                         "name":"造型05",
        ///                         "useDictionaryItem":{
        ///                             "id":1809,
        ///                             "name":"造型05",
        ///                             "thumbnailUrl":"/Images/201508/20150812141607693.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米05"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米05"
        ///                     },
        ///                     {
        ///                         "id":1853,
        ///                         "name":"造型04",
        ///                         "useDictionaryItem":{
        ///                             "id":1808,
        ///                             "name":"造型04",
        ///                             "thumbnailUrl":"/Images/201508/20150812141557990.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米04"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米04"
        ///                     },
        ///                     {
        ///                         "id":1852,
        ///                         "name":"造型03",
        ///                         "useDictionaryItem":{
        ///                             "id":1807,
        ///                             "name":"造型03",
        ///                             "thumbnailUrl":"/Images/201508/20150812141547351.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米03"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米03"
        ///                     },
        ///                     {
        ///                         "id":1851,
        ///                         "name":"造型02",
        ///                         "useDictionaryItem":{
        ///                             "id":1806,
        ///                             "name":"造型02",
        ///                             "thumbnailUrl":"/Images/201508/20150812141510581.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米02"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":false,
        ///                         "styleKeywords":"美式乡村风格160平米02"
        ///                     },
        ///                     {
        ///                         "id":1850,
        ///                         "name":"造型01",
        ///                         "useDictionaryItem":{
        ///                             "id":1805,
        ///                             "name":"造型01",
        ///                             "thumbnailUrl":"/Images/201508/20150812141459583.jpg",
        ///                             "unitPrice":1,
        ///                             "measurementUnits":"1",
        ///                             "styleKeywords":"美式乡村风格160平米01"
        ///                         },
        ///                         "count":1,
        ///                         "asDefaultItem":true,
        ///                         "styleKeywords":"美式乡村风格160平米01"
        ///                     }
        ///                 ]
        ///             },
        ///             {"id":340,"name":"墙","replaceableItemList":[{"id":1849,"name":"墙06","useDictionaryItem":{"id":1804,"name":"墙06","thumbnailUrl":"/Images/201508/20150812141433157.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1848,"name":"墙05","useDictionaryItem":{"id":1803,"name":"墙05","thumbnailUrl":"/Images/201508/20150812141421473.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1847,"name":"墙04","useDictionaryItem":{"id":1802,"name":"墙04","thumbnailUrl":"/Images/201508/20150812141410287.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1846,"name":"墙03","useDictionaryItem":{"id":1801,"name":"墙03","thumbnailUrl":"/Images/201508/20150812141359086.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1845,"name":"墙02","useDictionaryItem":{"id":1800,"name":"墙02","thumbnailUrl":"/Images/201508/20150812141346669.jpg","unitPrice":11,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1844,"name":"墙01","useDictionaryItem":{"id":1799,"name":"墙01","thumbnailUrl":"/Images/201508/20150812141335218.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":339,"name":"门","replaceableItemList":[{"id":1843,"name":"门06","useDictionaryItem":{"id":1798,"name":"门06","thumbnailUrl":"/Images/201508/20150812141322536.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1842,"name":"门05","useDictionaryItem":{"id":1797,"name":"门05","thumbnailUrl":"/Images/201508/20150812141311101.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1841,"name":"门04","useDictionaryItem":{"id":1796,"name":"门04","thumbnailUrl":"/Images/201508/20150812141300150.jpg","unitPrice":11,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1840,"name":"门03","useDictionaryItem":{"id":1795,"name":"门03","thumbnailUrl":"/Images/201508/20150812141235065.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1839,"name":"门02","useDictionaryItem":{"id":1794,"name":"门02","thumbnailUrl":"/Images/201508/20150812141223521.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1838,"name":"门01","useDictionaryItem":{"id":1793,"name":"门01","thumbnailUrl":"/Images/201508/20150812141209247.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":338,"name":"家具","replaceableItemList":[{"id":1837,"name":"家具06","useDictionaryItem":{"id":1792,"name":"家具06","thumbnailUrl":"/Images/201508/20150814152645368.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1836,"name":"家具05","useDictionaryItem":{"id":1791,"name":"家具05","thumbnailUrl":"/Images/201508/20150814152637661.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1835,"name":"家具04","useDictionaryItem":{"id":1790,"name":"家具04","thumbnailUrl":"/Images/201508/20150814152630922.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1834,"name":"家具03","useDictionaryItem":{"id":1789,"name":"家具03","thumbnailUrl":"/Images/201508/20150814152623762.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1833,"name":"家具02","useDictionaryItem":{"id":1788,"name":"家具02","thumbnailUrl":"/Images/201508/20150814152616601.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1832,"name":"家具01","useDictionaryItem":{"id":1787,"name":"家具01","thumbnailUrl":"/Images/201508/20150814152604199.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":337,"name":"顶面","replaceableItemList":[{"id":1831,"name":"顶面06","useDictionaryItem":{"id":1786,"name":"顶面06","thumbnailUrl":"/Images/201508/20150814152512095.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1830,"name":"顶面05","useDictionaryItem":{"id":1785,"name":"顶面05","thumbnailUrl":"/Images/201508/20150814152522048.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1829,"name":"顶面04","useDictionaryItem":{"id":1784,"name":"顶面04","thumbnailUrl":"/Images/201508/20150814152530425.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1828,"name":"顶面03","useDictionaryItem":{"id":1783,"name":"顶面03","thumbnailUrl":"/Images/201508/20150814152537523.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1827,"name":"顶面02","useDictionaryItem":{"id":1782,"name":"顶面02","thumbnailUrl":"/Images/201508/20150814152544434.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1826,"name":"顶面01","useDictionaryItem":{"id":1781,"name":"顶面01","thumbnailUrl":"/Images/201508/20150814152551189.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]},
        ///             {"id":336,"name":"地面","replaceableItemList":[{"id":1825,"name":"地面06","useDictionaryItem":{"id":1780,"name":"地面06","thumbnailUrl":"/Images/201508/20150813102931754.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米06"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米06"},{"id":1824,"name":"地面05","useDictionaryItem":{"id":1779,"name":"地面05","thumbnailUrl":"/Images/201508/20150812140709789.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米05"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米05"},{"id":1823,"name":"地面04","useDictionaryItem":{"id":1778,"name":"地面04","thumbnailUrl":"/Images/201508/20150813102806937.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米04"},"count":0,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米04"},{"id":1822,"name":"地面03","useDictionaryItem":{"id":1777,"name":"地面03","thumbnailUrl":"/Images/201508/20150813103321667.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米03"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米03"},{"id":1821,"name":"地面02","useDictionaryItem":{"id":1776,"name":"地面02","thumbnailUrl":"/Images/201508/20150812140633113.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米02"},"count":1,"asDefaultItem":false,"styleKeywords":"美式乡村风格160平米02"},{"id":1820,"name":"地面01","useDictionaryItem":{"id":1775,"name":"地面01","thumbnailUrl":"/Images/201508/20150813103222028.jpg","unitPrice":1,"measurementUnits":"1","styleKeywords":"美式乡村风格160平米01"},"count":1,"asDefaultItem":true,"styleKeywords":"美式乡村风格160平米01"}]}
        ///         ],
        ///         "perspectiveList":[
        ///             {
        ///                 "id":65,
        ///                 "name":"视角1",
        ///                 "backgroundImageUrl":"/Images/201508/20150812142430810.jpg",
        ///                 "visibleItemPlaceholderList":[
        ///                     {
        ///                         "id":337,
        ///                         "displayIndex":5
        ///                     },
        ///                     {
        ///                         "id":338,
        ///                         "displayIndex":4
        ///                     },
        ///                     {
        ///                         "id":341,
        ///                         "displayIndex":3
        ///                     },
        ///                     {
        ///                         "id":339
        ///                         "displayIndex":2
        ///                     },
        ///                     {
        ///                         "id":336,
        ///                         "displayIndex":1
        ///                     },
        ///                     {
        ///                         "id":340,
        ///                         "displayIndex":0
        ///                     }
        ///                 ]
        ///             }
        ///         ],
        ///         "materialList":[
        ///             {
        ///                 "id":2057,
        ///                 "forPerspective":{
        ///                     "id":65
        ///                 },
        ///                 "forReplaceableItem":{
        ///                     "id":1855
        ///                 },
        ///                 "imageUrl":"/Images/201508/20150812142502524.png"
        ///             },
        ///             {
        ///                 "id":2056,
        ///                 "forPerspective":{
        ///                     "id":65
        ///                 },"
        ///                 forReplaceableItem":
        ///                 {
        ///                     "id":1854
        ///                 },
        ///                 "imageUrl":"/Images/201508/20150812142507828.png"
        ///             },
        ///             {"id":2055,"forPerspective":{"id":65},"forReplaceableItem":{"id":1853},"imageUrl":"/Images/201508/20150812142512025.png"},
        ///             {"id":2054,"forPerspective":{"id":65},"forReplaceableItem":{"id":1852},"imageUrl":"/Images/201508/20150812142516596.png"},
        ///             {"id":2053,"forPerspective":{"id":65},"forReplaceableItem":{"id":1851},"imageUrl":"/Images/201508/20150812142520683.png"},
        ///             {"id":2052,"forPerspective":{"id":65},"forReplaceableItem":{"id":1850},"imageUrl":"/Images/201508/20150812142524474.png"},
        ///             {"id":2051,"forPerspective":{"id":65},"forReplaceableItem":{"id":1849},"imageUrl":"/Images/201508/20150812142528249.png"},
        ///             {"id":2050,"forPerspective":{"id":65},"forReplaceableItem":{"id":1848},"imageUrl":"/Images/201508/20150812142533366.png"},
        ///             {"id":2049,"forPerspective":{"id":65},"forReplaceableItem":{"id":1847},"imageUrl":"/Images/201508/20150812142540167.png"},
        ///             {"id":2048,"forPerspective":{"id":65},"forReplaceableItem":{"id":1846},"imageUrl":"/Images/201508/20150812142544660.png"},
        ///             {"id":2047,"forPerspective":{"id":65},"forReplaceableItem":{"id":1845},"imageUrl":"/Images/201508/20150812142550666.png"},
        ///             {"id":2046,"forPerspective":{"id":65},"forReplaceableItem":{"id":1844},"imageUrl":"/Images/201508/20150812142556594.png"},
        ///             {"id":2045,"forPerspective":{"id":65},"forReplaceableItem":{"id":1843},"imageUrl":"/Images/201508/20150812142600993.png"},
        ///             {"id":2044,"forPerspective":{"id":65},"forReplaceableItem":{"id":1842},"imageUrl":"/Images/201508/20150812142605970.png"},
        ///             {"id":2043,"forPerspective":{"id":65},"forReplaceableItem":{"id":1841},"imageUrl":"/Images/201508/20150812142611523.png"},
        ///             {"id":2042,"forPerspective":{"id":65},"forReplaceableItem":{"id":1840},"imageUrl":"/Images/201508/20150812142616469.png"},
        ///             {"id":2041,"forPerspective":{"id":65},"forReplaceableItem":{"id":1839},"imageUrl":"/Images/201508/20150812142620805.png"},
        ///             {"id":2040,"forPerspective":{"id":65},"forReplaceableItem":{"id":1838},"imageUrl":"/Images/201508/20150812142625485.png"},
        ///             {"id":2039,"forPerspective":{"id":65},"forReplaceableItem":{"id":1837},"imageUrl":"/Images/201508/20150813101602191.png"},
        ///             {"id":2038,"forPerspective":{"id":65},"forReplaceableItem":{"id":1836},"imageUrl":"/Images/201508/20150813101612674.png"},
        ///             {"id":2037,"forPerspective":{"id":65},"forReplaceableItem":{"id":1835},"imageUrl":"/Images/201508/20150813101621784.png"},
        ///             {"id":2036,"forPerspective":{"id":65},"forReplaceableItem":{"id":1834},"imageUrl":"/Images/201508/20150813101638710.png"},
        ///             {"id":2035,"forPerspective":{"id":65},"forReplaceableItem":{"id":1833},"imageUrl":"/Images/201508/20150813102433497.png"},
        ///             {"id":2034,"forPerspective":{"id":65},"forReplaceableItem":{"id":1832},"imageUrl":"/Images/201508/20150812162545539.png"},
        ///             {"id":2033,"forPerspective":{"id":65},"forReplaceableItem":{"id":1831},"imageUrl":"/Images/201508/20150813101654264.png"},
        ///             {"id":2032,"forPerspective":{"id":65},"forReplaceableItem":{"id":1830},"imageUrl":"/Images/201508/20150813101702968.png"},
        ///             {"id":2031,"forPerspective":{"id":65},"forReplaceableItem":{"id":1829},"imageUrl":"/Images/201508/20150813101710394.png"},
        ///             {"id":2030,"forPerspective":{"id":65},"forReplaceableItem":{"id":1828},"imageUrl":"/Images/201508/20150813101717866.png"},
        ///             {"id":2029,"forPerspective":{"id":65},"forReplaceableItem":{"id":1827},"imageUrl":"/Images/201508/20150813101725386.png"},
        ///             {"id":2028,"forPerspective":{"id":65},"forReplaceableItem":{"id":1826},"imageUrl":"/Images/201508/20150813101733201.png"},
        ///             {"id":2027,"forPerspective":{"id":65},"forReplaceableItem":{"id":1825},"imageUrl":"/Images/201508/20150812142727932.png"},
        ///             {"id":2026,"forPerspective":{"id":65},"forReplaceableItem":{"id":1824},"imageUrl":"/Images/201508/20150812142733127.png"},
        ///             {"id":2025,"forPerspective":{"id":65},"forReplaceableItem":{"id":1823},"imageUrl":"/Images/201508/20150812142738072.png"},
        ///             {"id":2024,"forPerspective":{"id":65},"forReplaceableItem":{"id":1822},"imageUrl":"/Images/201508/20150813103349373.png"},
        ///             {"id":2023,"forPerspective":{"id":65},"forReplaceableItem":{"id":1821},"imageUrl":"/Images/201508/20150812142748041.png"},
        ///             {"id":2022,"forPerspective":{"id":65},"forReplaceableItem":{"id":1820},"imageUrl":"/Images/201508/20150812142753813.png"}
        ///         ]
        ///     }"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult IsUpdate(int styleId,string updateTime)
        {
            StyleThird style = db.StyleThird.Find(styleId);
            DateTime time = Convert.ToDateTime(updateTime);
            string id = styleId.ToString();
            UpdateRecord record = db.UpdateRecord.Where(item => item.StyleId == id).FirstOrDefault();
            if (record.UpdateTime > time)
            {
                return Json(new { IsUpdate = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsUpdate = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetStyleData(int styleId)
        {
            StyleThird styleThird = db.StyleThird.Find(styleId);
            StyleThirdDownData downData = new StyleThirdDownData();
            string id = styleId.ToString();
            UpdateRecord record = db.UpdateRecord.Where(item => item.StyleId == id).FirstOrDefault();
            if (record == null)
            {
                record = new UpdateRecord();
                record.StyleId = id;
                record.UpdateTime = DateTime.Now;
                db.UpdateRecord.Add(record);
                db.SaveChanges();
            }
            downData.StyleThirdId = styleThird.Id;
            downData.StyleThirdName = styleThird.StyleThirdName;
            downData.StyleThirdIndex = ConfigurationManager.AppSettings["ResourceUrl"] + styleThird.StyleThirdIndex;
            downData.StyleThirdPics = styleThird.StyleThirdPics.Substring(0, styleThird.StyleThirdPics.Length - 1).Split(' ');
            for (int i = 0; i < downData.StyleThirdPics.Length; i++)
            {
                downData.StyleThirdPics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + downData.StyleThirdPics[i];
            }
            downData.UpdateTime = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");

            string targetUrl = TargetUrl + "/Normal/UserRequest";
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
            sb.Append("\"" + styleThird.StyleThirdCode + "\"");
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

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();


            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            string respHtml = sr.ReadToEnd();
            //return Content(respHtml);
            downData.DiyData = respHtml;

            return Json(new { data = downData }, JsonRequestBehavior.AllowGet);
        }

    }
}