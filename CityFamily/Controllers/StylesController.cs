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

                string id = style.Id.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.StyleId == id).FirstOrDefault();
                if (record == null)
                {
                    record = new UpdateRecord();
                    record.StyleId = id;
                    record.UpdateTime = DateTime.Now;
                    db.UpdateRecord.Add(record);
                    db.SaveChanges();
                }

                stylesData.UpdateTime = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            sb.Append("\"" + styleThird.StyleThirdCode.Trim() + "\"");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns>
        /// {
        ///     "data":
        ///         {
        ///             "Id":8,
        ///             "StyleName":"现代风格",
        ///             "StyleIndex":"http://119.188.113.104:8081/Images/data/201601/bb1f7149-d878-4bce-ae15-b69c8b8b917d0.jpg",
        ///             "UpdateTime":"2016-04-20 15:01:39",
        ///             "StyleSecondDataList":[
        ///                 {
        ///                     "StyleSecondId":6,
        ///                     "StyleSecondName":"现代风格120平米",
        ///                     "StyleSecondIndex":"http://119.188.113.104:8081/Images/data/201507/46d9fd29-9ea4-4ae5-b268-1257e05776440.jpg",
        ///                     "StyleThirdDatsList":[
        ///                         {
        ///                             "StyleThirdId":6,
        ///                             "StyleThirdName":"现代风格02--客厅",
        ///                             "StyleThirdIndex":"http://119.188.113.104:8081/Images/data/201507/5297ef68-240d-4b13-beb6-5b6512ac87560.jpg",
        ///                             "StyleThirdPics":[
        ///                                 "http://119.188.113.104:8081/Images/data/201507/8b37fe35-8724-485d-b045-44e179f09f270.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/f5aa7d79-2434-412e-9af5-5726870090b21.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/47d6d443-b580-45e8-8ba9-066cf808bd1c2.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/e64d5340-38f3-414e-861f-3d2034eca18f3.jpg"
        ///                             ],
        ///                             "UpdateTime":null,
        ///                             "DiyData":"{\"Result\":{\"Success\":false,\"Note\":\"invalid requestcode or room not ready\"}}"
        ///                         },
        ///                         {
        ///                             "StyleThirdId":40
        ///                             "StyleThirdName":"现代风格02--餐厅",
        ///                             "StyleThirdIndex":"http://119.188.113.104:8081/Images/data/201507/4b392fff-dc64-4bee-8742-605b20b86f7b0.jpg",
        ///                             "StyleThirdPics":[
        ///                                 "http://119.188.113.104:8081/Images/data/201507/3a255eb1-f41c-47a5-9b7a-e596c29d363d0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/523fc59c-624e-48ec-bbb6-f61fecfca9fc0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/f6177b10-fa01-4e5a-bb7e-bef78e6889cb0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/4be41db2-c52a-40ba-8f7e-e0233363cbf90.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/ae27183d-95e3-48a2-b401-73a68e7e9d9d0.jpg"
        ///                             ],
        ///                             "UpdateTime":null,
        ///                             "DiyData":"{\"Result\":{\"Success\":true,\"Note\":\"\"},\"SampleRoom\":{\"id\":29,\"name\":\"现代120平米（餐厅）\",\"summary\":\"\",\"itemPlaceholderList\":[{\"id\":144,\"name\":\"造型\",\"replaceableItemList\":[{\"id\":655,\"name\":\"造型06\",\"useDictionaryItem\":{\"id\":639,\"name\":\"造型06\",\"thumbnailUrl\":\"/Images/201508/20150803164248737.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米06\"},{\"id\":654,\"name\":\"造型05\",\"useDictionaryItem\":{\"id\":638,\"name\":\"造型05\",\"thumbnailUrl\":\"/Images/201508/20150803164229658.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米05\"},{\"id\":653,\"name\":\"造型04\",\"useDictionaryItem\":{\"id\":637,\"name\":\"造型04\",\"thumbnailUrl\":\"/Images/201508/20150803164211422.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米04\"},{\"id\":652,\"name\":\"造型03\",\"useDictionaryItem\":{\"id\":636,\"name\":\"造型03\",\"thumbnailUrl\":\"/Images/201508/20150803164152858.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米03\"},{\"id\":651,\"name\":\"造型02\",\"useDictionaryItem\":{\"id\":635,\"name\":\"造型02\",\"thumbnailUrl\":\"/Images/201508/20150803164138287.jpg\",\"unitPrice\":2,\"measurementUnits\":\"2\",\"styleKeywords\":\"现代120平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米02\"},{\"id\":650,\"name\":\"造型01\",\"useDictionaryItem\":{\"id\":634,\"name\":\"造型01\",\"thumbnailUrl\":\"/Images/201508/20150803164115979.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代120平米01\"}]},{\"id\":143,\"name\":\"墙\",\"replaceableItemList\":[{\"id\":661,\"name\":\"墙06\",\"useDictionaryItem\":{\"id\":627,\"name\":\"墙06\",\"thumbnailUrl\":\"/Images/201508/20150803163754208.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米06\"},{\"id\":660,\"name\":\"墙05\",\"useDictionaryItem\":{\"id\":626,\"name\":\"墙05\",\"thumbnailUrl\":\"/Images/201508/20150803163735629.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米05\"},{\"id\":659,\"name\":\"墙04\",\"useDictionaryItem\":{\"id\":625,\"name\":\"墙04\",\"thumbnailUrl\":\"/Images/201508/20150803163709764.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米04\"},{\"id\":658,\"name\":\"墙03\",\"useDictionaryItem\":{\"id\":624,\"name\":\"墙03\",\"thumbnailUrl\":\"/Images/201508/20150803163616693.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米03\"},{\"id\":657,\"name\":\"墙02\",\"useDictionaryItem\":{\"id\":623,\"name\":\"墙02\",\"thumbnailUrl\":\"/Images/201508/20150803163553074.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米02\"},{\"id\":656,\"name\":\"墙01\",\"useDictionaryItem\":{\"id\":622,\"name\":\"墙01\",\"thumbnailUrl\":\"/Images/201508/20150803163526382.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代120平米01\"}]},{\"id\":142,\"name\":\"家具\",\"replaceableItemList\":[{\"id\":667,\"name\":\"家具06\",\"useDictionaryItem\":{\"id\":633,\"name\":\"家具06\",\"thumbnailUrl\":\"/Images/201508/20150803164044233.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米06\"},{\"id\":666,\"name\":\"家具05\",\"useDictionaryItem\":{\"id\":632,\"name\":\"家具05\",\"thumbnailUrl\":\"/Images/201508/20150803164026667.jpg\",\"unitPrice\":4,\"measurementUnits\":\"4\",\"styleKeywords\":\"现代120平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米05\"},{\"id\":665,\"name\":\"家具04\",\"useDictionaryItem\":{\"id\":631,\"name\":\"家具04\",\"thumbnailUrl\":\"/Images/201508/20150803163950366.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米04\"},{\"id\":664,\"name\":\"家具03\",\"useDictionaryItem\":{\"id\":630,\"name\":\"家具03\",\"thumbnailUrl\":\"/Images/201508/20150803163931740.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米03\"},{\"id\":663,\"name\":\"家具02\",\"useDictionaryItem\":{\"id\":629,\"name\":\"家具02\",\"thumbnailUrl\":\"/Images/201508/20150803163839932.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米02\"},{\"id\":662,\"name\":\"家具01\",\"useDictionaryItem\":{\"id\":628,\"name\":\"家具01\",\"thumbnailUrl\":\"/Images/201508/20150803163820042.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代120平米01\"}]},{\"id\":141,\"name\":\"门\",\"replaceableItemList\":[{\"id\":673,\"name\":\"门06\",\"useDictionaryItem\":{\"id\":621,\"name\":\"门06\",\"thumbnailUrl\":\"/Images/201508/20150803163443498.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米06\"},{\"id\":672,\"name\":\"门05\",\"useDictionaryItem\":{\"id\":620,\"name\":\"门05\",\"thumbnailUrl\":\"/Images/201508/20150803163039576.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米05\"},{\"id\":671,\"name\":\"门04\",\"useDictionaryItem\":{\"id\":619,\"name\":\"门04\",\"thumbnailUrl\":\"/Images/201508/20150803163021168.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米04\"},{\"id\":670,\"name\":\"门03\",\"useDictionaryItem\":{\"id\":618,\"name\":\"门03\",\"thumbnailUrl\":\"/Images/201508/20150803163005864.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米03\"},{\"id\":669,\"name\":\"门02\",\"useDictionaryItem\":{\"id\":617,\"name\":\"门02\",\"thumbnailUrl\":\"/Images/201508/20150803162951169.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米02\"},{\"id\":668,\"name\":\"门01\",\"useDictionaryItem\":{\"id\":616,\"name\":\"门01\",\"thumbnailUrl\":\"/Images/201508/20150803162933588.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代120平米01\"}]},{\"id\":140,\"name\":\"顶面\",\"replaceableItemList\":[{\"id\":679,\"name\":\"顶面06\",\"useDictionaryItem\":{\"id\":615,\"name\":\"顶面06\",\"thumbnailUrl\":\"/Images/201508/20150803162906849.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米06\"},{\"id\":678,\"name\":\"顶面05\",\"useDictionaryItem\":{\"id\":614,\"name\":\"顶面05\",\"thumbnailUrl\":\"/Images/201508/20150803162848379.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米05\"},{\"id\":677,\"name\":\"顶面04\",\"useDictionaryItem\":{\"id\":613,\"name\":\"顶面04\",\"thumbnailUrl\":\"/Images/201508/20150803162820205.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米04\"},{\"id\":676,\"name\":\"顶面03\",\"useDictionaryItem\":{\"id\":612,\"name\":\"顶面03\",\"thumbnailUrl\":\"/Images/201508/20150803162803154.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米03\"},{\"id\":675,\"name\":\"顶面02\",\"useDictionaryItem\":{\"id\":611,\"name\":\"顶面02\",\"thumbnailUrl\":\"/Images/201508/20150803162716573.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米02\"},{\"id\":674,\"name\":\"顶面01\",\"useDictionaryItem\":{\"id\":610,\"name\":\"顶面01\",\"thumbnailUrl\":\"/Images/201508/20150803162609914.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代120平米01\"}]},{\"id\":139,\"name\":\"地面\",\"replaceableItemList\":[{\"id\":685,\"name\":\"地面06\",\"useDictionaryItem\":{\"id\":609,\"name\":\"地面06\",\"thumbnailUrl\":\"/Images/201508/20150803162523972.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米06\"},{\"id\":684,\"name\":\"地面05\",\"useDictionaryItem\":{\"id\":608,\"name\":\"地面05\",\"thumbnailUrl\":\"/Images/201508/20150803162504456.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米05\"},{\"id\":683,\"name\":\"地面04\",\"useDictionaryItem\":{\"id\":607,\"name\":\"地面04\",\"thumbnailUrl\":\"/Images/201508/20150803162424645.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米04\"},{\"id\":682,\"name\":\"地面03\",\"useDictionaryItem\":{\"id\":606,\"name\":\"地面03\",\"thumbnailUrl\":\"/Images/201508/20150803162324148.jpg\",\"unitPrice\":111,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米03\"},{\"id\":681,\"name\":\"地面02\",\"useDictionaryItem\":{\"id\":605,\"name\":\"地面02\",\"thumbnailUrl\":\"/Images/201508/20150803162259921.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代120平米02\"},{\"id\":680,\"name\":\"地面01\",\"useDictionaryItem\":{\"id\":604,\"name\":\"地面01\",\"thumbnailUrl\":\"/Images/201508/20150803162214619.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代120平米01\\r\\n\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代120平米01\"}]}],\"perspectiveList\":[{\"id\":35,\"name\":\"视角1\",\"backgroundImageUrl\":\"/Images/201508/20150811135807171.jpg\",\"visibleItemPlaceholderList\":[{\"id\":142,\"displayIndex\":5},{\"id\":144,\"displayIndex\":4},{\"id\":141,\"displayIndex\":3},{\"id\":140,\"displayIndex\":2},{\"id\":139,\"displayIndex\":1},{\"id\":143,\"displayIndex\":0}]}],\"materialList\":[{\"id\":845,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":685},\"imageUrl\":\"/Images/201508/20150803170338985.png\"},{\"id\":844,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":684},\"imageUrl\":\"/Images/201508/20150803170343525.png\"},{\"id\":843,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":683},\"imageUrl\":\"/Images/201508/20150803170347768.png\"},{\"id\":842,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":682},\"imageUrl\":\"/Images/201508/20150803170352401.png\"},{\"id\":841,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":681},\"imageUrl\":\"/Images/201508/20150803170356613.png\"},{\"id\":840,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":680},\"imageUrl\":\"/Images/201508/20150803170401340.png\"},{\"id\":839,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":679},\"imageUrl\":\"/Images/201508/20150803170405755.png\"},{\"id\":838,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":678},\"imageUrl\":\"/Images/201508/20150820114638882.png\"},{\"id\":837,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":677},\"imageUrl\":\"/Images/201508/20150803170415443.png\"},{\"id\":836,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":676},\"imageUrl\":\"/Images/201508/20150803170420279.png\"},{\"id\":835,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":675},\"imageUrl\":\"/Images/201508/20150803170424912.png\"},{\"id\":834,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":674},\"imageUrl\":\"/Images/201508/20150803170429374.png\"},{\"id\":833,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":673},\"imageUrl\":\"/Images/201508/20150804093436249.png\"},{\"id\":832,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":672},\"imageUrl\":\"/Images/201508/20150804093428902.png\"},{\"id\":831,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":671},\"imageUrl\":\"/Images/201508/20150804093421570.png\"},{\"id\":830,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":670},\"imageUrl\":\"/Images/201508/20150804093415220.png\"},{\"id\":829,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":669},\"imageUrl\":\"/Images/201508/20150804093407389.png\"},{\"id\":828,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":668},\"imageUrl\":\"/Images/201508/20150804093356953.png\"},{\"id\":827,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":667},\"imageUrl\":\"/Images/201508/20150803170517609.png\"},{\"id\":826,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":666},\"imageUrl\":\"/Images/201508/20150803170522133.png\"},{\"id\":825,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":665},\"imageUrl\":\"/Images/201511/20151124140020519.png\"},{\"id\":824,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":664},\"imageUrl\":\"/Images/201508/20150803170532616.png\"},{\"id\":823,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":663},\"imageUrl\":\"/Images/201508/20150803170541009.png\"},{\"id\":822,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":662},\"imageUrl\":\"/Images/201508/20150803170546032.png\"},{\"id\":821,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":661},\"imageUrl\":\"/Images/201508/20150803170551835.png\"},{\"id\":820,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":660},\"imageUrl\":\"/Images/201508/20150803170557935.png\"},{\"id\":819,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":659},\"imageUrl\":\"/Images/201508/20150803170604081.png\"},{\"id\":818,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":658},\"imageUrl\":\"/Images/201508/20150803170609697.png\"},{\"id\":817,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":657},\"imageUrl\":\"/Images/201508/20150804101246583.png\"},{\"id\":816,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":656},\"imageUrl\":\"/Images/201508/20150804101238767.png\"},{\"id\":815,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":655},\"imageUrl\":\"/Images/201508/20150803170625251.png\"},{\"id\":814,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":654},\"imageUrl\":\"/Images/201508/20150803170630118.png\"},{\"id\":813,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":653},\"imageUrl\":\"/Images/201508/20150803170635796.png\"},{\"id\":812,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":652},\"imageUrl\":\"/Images/201508/20150803170644220.png\"},{\"id\":811,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":651},\"imageUrl\":\"/Images/201508/20150803170649353.png\"},{\"id\":810,\"forPerspective\":{\"id\":35},\"forReplaceableItem\":{\"id\":650},\"imageUrl\":\"/Images/201508/20150803170654001.png\"}]}}"
        ///                         }
        ///                     ]
        ///                 },
        ///                 {
        ///                     "StyleSecondId":19,
        ///                     "StyleSecondName":"现代风格160平米",
        ///                     "StyleSecondIndex":"http://119.188.113.104:8081/Images/data/201507/92ed5e8b-e56e-4fdf-9b28-1d55b09443980.jpg",
        ///                     "StyleThirdDatsList":[
        ///                         {
        ///                             "StyleThirdId":34,
        ///                             "StyleThirdName":"现代风格01--客厅",
        ///                             "StyleThirdIndex":"http://119.188.113.104:8081/Images/data/201507/a4d1e19a-6121-42a0-b0c8-63df0e8b12cb0.jpg",
        ///                             "StyleThirdPics":[
        ///                                 "http://119.188.113.104:8081/Images/data/201507/fd044ca5-4535-47f8-a932-4c89542570280.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/ebc3f50d-c358-4b50-89a5-e8817f1b4e3a0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/2c35128b-5ba0-4187-9f16-44a9714656580.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/83b576ee-8a06-40eb-bbc2-3484680f45280.jpg"
        ///                             ],
        ///                             "UpdateTime":null,
        ///                             "DiyData":"{\"Result\":{\"Success\":true,\"Note\":\"\"},\"SampleRoom\":{\"id\":36,\"name\":\"现代风格160平米（客厅）\",\"summary\":\"\",\"itemPlaceholderList\":[{\"id\":192,\"name\":\"造型\",\"replaceableItemList\":[{\"id\":967,\"name\":\"造型06\",\"useDictionaryItem\":{\"id\":921,\"name\":\"造型06\",\"thumbnailUrl\":\"/Images/201508/20150806112830911.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":966,\"name\":\"造型05\",\"useDictionaryItem\":{\"id\":920,\"name\":\"造型05\",\"thumbnailUrl\":\"/Images/201508/20150806112633069.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":965,\"name\":\"造型04\",\"useDictionaryItem\":{\"id\":919,\"name\":\"造型04\",\"thumbnailUrl\":\"/Images/201508/20150806112617516.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":964,\"name\":\"造型03\",\"useDictionaryItem\":{\"id\":918,\"name\":\"造型03\",\"thumbnailUrl\":\"/Images/201508/20150806112602384.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":963,\"name\":\"造型02\",\"useDictionaryItem\":{\"id\":917,\"name\":\"造型02\",\"thumbnailUrl\":\"/Images/201508/20150806112548983.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":962,\"name\":\"造型01\",\"useDictionaryItem\":{\"id\":916,\"name\":\"造型01\",\"thumbnailUrl\":\"/Images/201508/20150806112534132.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":191,\"name\":\"门\",\"replaceableItemList\":[{\"id\":955,\"name\":\"门06\",\"useDictionaryItem\":{\"id\":909,\"name\":\"门06\",\"thumbnailUrl\":\"/Images/201508/20150806112138806.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":954,\"name\":\"门05\",\"useDictionaryItem\":{\"id\":908,\"name\":\"门05\",\"thumbnailUrl\":\"/Images/201508/20150806112124141.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":953,\"name\":\"门04\",\"useDictionaryItem\":{\"id\":907,\"name\":\"门04\",\"thumbnailUrl\":\"/Images/201508/20150806112109961.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":952,\"name\":\"门03\",\"useDictionaryItem\":{\"id\":906,\"name\":\"门03\",\"thumbnailUrl\":\"/Images/201508/20150806112056202.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":951,\"name\":\"门02\",\"useDictionaryItem\":{\"id\":905,\"name\":\"门02\",\"thumbnailUrl\":\"/Images/201508/20150806112020353.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":950,\"name\":\"门01\",\"useDictionaryItem\":{\"id\":904,\"name\":\"门01\",\"thumbnailUrl\":\"/Images/201508/20150806112004800.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":190,\"name\":\"墙\",\"replaceableItemList\":[{\"id\":961,\"name\":\"墙06\",\"useDictionaryItem\":{\"id\":915,\"name\":\"墙06\",\"thumbnailUrl\":\"/Images/201508/20150806112511075.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":960,\"name\":\"墙05\",\"useDictionaryItem\":{\"id\":914,\"name\":\"墙05\",\"thumbnailUrl\":\"/Images/201508/20150806112455693.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":959,\"name\":\"墙04\",\"useDictionaryItem\":{\"id\":913,\"name\":\"墙04\",\"thumbnailUrl\":\"/Images/201508/20150806112442745.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":958,\"name\":\"墙03\",\"useDictionaryItem\":{\"id\":912,\"name\":\"墙03\",\"thumbnailUrl\":\"/Images/201508/20150806112419314.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":957,\"name\":\"墙02\",\"useDictionaryItem\":{\"id\":911,\"name\":\"墙02\",\"thumbnailUrl\":\"/Images/201508/20150806112406117.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":956,\"name\":\"墙01\",\"useDictionaryItem\":{\"id\":910,\"name\":\"墙01\",\"thumbnailUrl\":\"/Images/201508/20150806112348707.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":189,\"name\":\"家具\",\"replaceableItemList\":[{\"id\":949,\"name\":\"家具06\",\"useDictionaryItem\":{\"id\":903,\"name\":\"家具06\",\"thumbnailUrl\":\"/Images/201508/20150806111945783.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":948,\"name\":\"家具05\",\"useDictionaryItem\":{\"id\":902,\"name\":\"家具05\",\"thumbnailUrl\":\"/Images/201508/20150806111932040.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":947,\"name\":\"家具04\",\"useDictionaryItem\":{\"id\":901,\"name\":\"家具04\",\"thumbnailUrl\":\"/Images/201508/20150806111916986.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":946,\"name\":\"家具03\",\"useDictionaryItem\":{\"id\":900,\"name\":\"家具03\",\"thumbnailUrl\":\"/Images/201508/20150806111902446.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":945,\"name\":\"家具02\",\"useDictionaryItem\":{\"id\":899,\"name\":\"家具02\",\"thumbnailUrl\":\"/Images/201508/20150806111846176.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":944,\"name\":\"家具01\",\"useDictionaryItem\":{\"id\":898,\"name\":\"家具01\",\"thumbnailUrl\":\"/Images/201508/20150806111830950.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":188,\"name\":\"顶面\",\"replaceableItemList\":[{\"id\":943,\"name\":\"顶面06\",\"useDictionaryItem\":{\"id\":897,\"name\":\"顶面06\",\"thumbnailUrl\":\"/Images/201508/20150806111804742.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":942,\"name\":\"顶面05\",\"useDictionaryItem\":{\"id\":896,\"name\":\"顶面05\",\"thumbnailUrl\":\"/Images/201508/20150806111736755.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":941,\"name\":\"顶面04\",\"useDictionaryItem\":{\"id\":895,\"name\":\"顶面04\",\"thumbnailUrl\":\"/Images/201508/20150806111715368.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":940,\"name\":\"顶面03\",\"useDictionaryItem\":{\"id\":894,\"name\":\"顶面03\",\"thumbnailUrl\":\"/Images/201508/20150806111702045.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":939,\"name\":\"顶面02\",\"useDictionaryItem\":{\"id\":893,\"name\":\"顶面02\",\"thumbnailUrl\":\"/Images/201508/20150806111648021.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":938,\"name\":\"顶面01\",\"useDictionaryItem\":{\"id\":892,\"name\":\"顶面01\",\"thumbnailUrl\":\"/Images/201508/20150806111631735.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":187,\"name\":\"地面\",\"replaceableItemList\":[{\"id\":937,\"name\":\"地面06\",\"useDictionaryItem\":{\"id\":891,\"name\":\"地面06\",\"thumbnailUrl\":\"/Images/201508/20150806111616946.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":936,\"name\":\"地面05\",\"useDictionaryItem\":{\"id\":890,\"name\":\"地面05\",\"thumbnailUrl\":\"/Images/201508/20150806111600769.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":935,\"name\":\"地面04\",\"useDictionaryItem\":{\"id\":889,\"name\":\"地面04\",\"thumbnailUrl\":\"/Images/201508/20150806111546136.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":934,\"name\":\"地面03\",\"useDictionaryItem\":{\"id\":888,\"name\":\"地面03\",\"thumbnailUrl\":\"/Images/201508/20150806111526932.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":933,\"name\":\"地面02\",\"useDictionaryItem\":{\"id\":887,\"name\":\"地面02\",\"thumbnailUrl\":\"/Images/201508/20150806111508274.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":932,\"name\":\"地面01\",\"useDictionaryItem\":{\"id\":886,\"name\":\"地面01\",\"thumbnailUrl\":\"/Images/201508/20150806111426092.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":186,\"name\":\"窗帘\",\"replaceableItemList\":[{\"id\":931,\"name\":\"窗帘06\",\"useDictionaryItem\":{\"id\":885,\"name\":\"窗帘06\",\"thumbnailUrl\":\"/Images/201508/20150806111406561.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":930,\"name\":\"窗帘05\",\"useDictionaryItem\":{\"id\":884,\"name\":\"窗帘05\",\"thumbnailUrl\":\"/Images/201508/20150806111330041.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":929,\"name\":\"窗帘04\",\"useDictionaryItem\":{\"id\":883,\"name\":\"窗帘04\",\"thumbnailUrl\":\"/Images/201508/20150806111308856.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":928,\"name\":\"窗帘03\",\"useDictionaryItem\":{\"id\":882,\"name\":\"窗帘03\",\"thumbnailUrl\":\"/Images/201508/20150806111234614.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":927,\"name\":\"窗帘02\",\"useDictionaryItem\":{\"id\":881,\"name\":\"窗帘02\",\"thumbnailUrl\":\"/Images/201508/20150806111201995.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":926,\"name\":\"窗帘01\",\"useDictionaryItem\":{\"id\":880,\"name\":\"窗帘01\",\"thumbnailUrl\":\"/Images/201508/20150806111108330.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]}],\"perspectiveList\":[{\"id\":42,\"name\":\"视角1\",\"backgroundImageUrl\":\"/Images/201508/20150811135620092.jpg\",\"visibleItemPlaceholderList\":[{\"id\":189,\"displayIndex\":6},{\"id\":192,\"displayIndex\":5},{\"id\":186,\"displayIndex\":4},{\"id\":191,\"displayIndex\":3},{\"id\":188,\"displayIndex\":2},{\"id\":187,\"displayIndex\":1},{\"id\":190,\"displayIndex\":0}]}],\"materialList\":[{\"id\":1127,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":967},\"imageUrl\":\"/Images/201508/20150806131059662.png\"},{\"id\":1126,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":966},\"imageUrl\":\"/Images/201508/20150806131054623.png\"},{\"id\":1125,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":965},\"imageUrl\":\"/Images/201508/20150806131049491.png\"},{\"id\":1124,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":964},\"imageUrl\":\"/Images/201508/20150806131248847.png\"},{\"id\":1123,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":963},\"imageUrl\":\"/Images/201508/20150806131036855.png\"},{\"id\":1122,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":962},\"imageUrl\":\"/Images/201508/20150806131028431.png\"},{\"id\":1121,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":961},\"imageUrl\":\"/Images/201508/20150806115122138.png\"},{\"id\":1120,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":960},\"imageUrl\":\"/Images/201508/20150806115127021.png\"},{\"id\":1119,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":959},\"imageUrl\":\"/Images/201508/20150806115132965.png\"},{\"id\":1118,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":958},\"imageUrl\":\"/Images/201508/20150806131042955.png\"},{\"id\":1117,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":957},\"imageUrl\":\"/Images/201508/20150806115145585.png\"},{\"id\":1116,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":956},\"imageUrl\":\"/Images/201508/20150806115150374.png\"},{\"id\":1115,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":955},\"imageUrl\":\"/Images/201508/20150806115154649.png\"},{\"id\":1114,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":954},\"imageUrl\":\"/Images/201508/20150806115202074.png\"},{\"id\":1113,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":953},\"imageUrl\":\"/Images/201508/20150806115214071.png\"},{\"id\":1112,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":952},\"imageUrl\":\"/Images/201508/20150806115218922.png\"},{\"id\":1111,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":951},\"imageUrl\":\"/Images/201508/20150806115224055.png\"},{\"id\":1110,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":950},\"imageUrl\":\"/Images/201508/20150806115229156.png\"},{\"id\":1109,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":949},\"imageUrl\":\"/Images/201508/20150806131152874.png\"},{\"id\":1108,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":948},\"imageUrl\":\"/Images/201508/20150806131146026.png\"},{\"id\":1107,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":947},\"imageUrl\":\"/Images/201508/20150806131138428.png\"},{\"id\":1106,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":946},\"imageUrl\":\"/Images/201508/20150806131130987.png\"},{\"id\":1105,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":945},\"imageUrl\":\"/Images/201508/20150806131125012.png\"},{\"id\":1104,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":944},\"imageUrl\":\"/Images/201508/20150806131118304.png\"},{\"id\":1103,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":943},\"imageUrl\":\"/Images/201508/20150806115348482.png\"},{\"id\":1102,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":942},\"imageUrl\":\"/Images/201508/20150806115356594.png\"},{\"id\":1101,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":941},\"imageUrl\":\"/Images/201508/20150806115401820.png\"},{\"id\":1100,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":940},\"imageUrl\":\"/Images/201508/20150806115407904.png\"},{\"id\":1099,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":939},\"imageUrl\":\"/Images/201508/20150806115413598.png\"},{\"id\":1098,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":938},\"imageUrl\":\"/Images/201508/20150806115418762.png\"},{\"id\":1097,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":937},\"imageUrl\":\"/Images/201508/20150806115429370.png\"},{\"id\":1096,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":936},\"imageUrl\":\"/Images/201508/20150806115434845.png\"},{\"id\":1095,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":935},\"imageUrl\":\"/Images/201508/20150806115440149.png\"},{\"id\":1094,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":934},\"imageUrl\":\"/Images/201508/20150806115446171.png\"},{\"id\":1093,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":933},\"imageUrl\":\"/Images/201508/20150806115451350.png\"},{\"id\":1092,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":932},\"imageUrl\":\"/Images/201508/20150806115457668.png\"},{\"id\":1091,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":931},\"imageUrl\":\"/Images/201508/20150806115502769.png\"},{\"id\":1090,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":930},\"imageUrl\":\"/Images/201508/20150806115507730.png\"},{\"id\":1089,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":929},\"imageUrl\":\"/Images/201508/20150806115512816.png\"},{\"id\":1088,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":928},\"imageUrl\":\"/Images/201508/20150806115518042.png\"},{\"id\":1087,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":927},\"imageUrl\":\"/Images/201508/20150806115523954.png\"},{\"id\":1086,\"forPerspective\":{\"id\":42},\"forReplaceableItem\":{\"id\":926},\"imageUrl\":\"/Images/201508/20150806115528946.png\"}]}}"
        ///                         },
        ///                         {
        ///                             "StyleThirdId":35,
        ///                             "StyleThirdName":"现代风格01--餐厅",
        ///                             "StyleThirdIndex":"http://119.188.113.104:8081/Images/data/201507/08cd1903-dbd9-4a07-9b48-427db8542b9d0.jpg",
        ///                             "StyleThirdPics":[
        ///                                 "http://119.188.113.104:8081/Images/data/201507/cb1f4cda-9c7d-461b-a769-2744b15cd3a80.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/ca9600ce-e83f-431d-a9b8-334f08acba6d0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/a3e0d97c-43ff-4691-9feb-536d2840137f0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/b0341d60-6d8a-4d33-a6ba-5ec19a4102050.jpg"
        ///                             ],
        ///                             "UpdateTime":null,
        ///                             "DiyData":"{\"Result\":{\"Success\":true,\"Note\":\"\"},\"SampleRoom\":{\"id\":37,\"name\":\"现代风格160平米（餐厅）\",\"summary\":\"\",\"itemPlaceholderList\":[{\"id\":199,\"name\":\"造型\",\"replaceableItemList\":[{\"id\":1009,\"name\":\"造型06\",\"useDictionaryItem\":{\"id\":963,\"name\":\"造型06\",\"thumbnailUrl\":\"/Images/201508/20150806134616758.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":1008,\"name\":\"造型05\",\"useDictionaryItem\":{\"id\":962,\"name\":\"造型05\",\"thumbnailUrl\":\"/Images/201508/20150806134545573.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":1007,\"name\":\"造型04\",\"useDictionaryItem\":{\"id\":961,\"name\":\"造型04\",\"thumbnailUrl\":\"/Images/201508/20150806134504311.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":1006,\"name\":\"造型03\",\"useDictionaryItem\":{\"id\":960,\"name\":\"造型03\",\"thumbnailUrl\":\"/Images/201508/20150806134441878.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":1005,\"name\":\"造型02\",\"useDictionaryItem\":{\"id\":959,\"name\":\"造型02\",\"thumbnailUrl\":\"/Images/201508/20150806134352925.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":1004,\"name\":\"造型01\",\"useDictionaryItem\":{\"id\":958,\"name\":\"造型01\",\"thumbnailUrl\":\"/Images/201508/20150806134315438.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":198,\"name\":\"墙 \",\"replaceableItemList\":[{\"id\":1003,\"name\":\"墙06\",\"useDictionaryItem\":{\"id\":957,\"name\":\"墙06\",\"thumbnailUrl\":\"/Images/201508/20150806134250728.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":1002,\"name\":\"墙05\",\"useDictionaryItem\":{\"id\":956,\"name\":\"墙05\",\"thumbnailUrl\":\"/Images/201508/20150806134234364.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":1001,\"name\":\"墙04\",\"useDictionaryItem\":{\"id\":955,\"name\":\"墙04\",\"thumbnailUrl\":\"/Images/201508/20150806134218467.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米0\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":1000,\"name\":\"墙03\",\"useDictionaryItem\":{\"id\":954,\"name\":\"墙03\",\"thumbnailUrl\":\"/Images/201508/20150806134154677.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":999,\"name\":\"墙02\",\"useDictionaryItem\":{\"id\":953,\"name\":\"墙02\",\"thumbnailUrl\":\"/Images/201508/20150806134207438.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":998,\"name\":\"墙01\",\"useDictionaryItem\":{\"id\":952,\"name\":\"墙01\",\"thumbnailUrl\":\"/Images/201508/20150806134053010.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":197,\"name\":\"门\",\"replaceableItemList\":[{\"id\":997,\"name\":\"门06\",\"useDictionaryItem\":{\"id\":951,\"name\":\"门06\",\"thumbnailUrl\":\"/Images/201508/20150806134025258.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":996,\"name\":\"门05\",\"useDictionaryItem\":{\"id\":950,\"name\":\"门05\",\"thumbnailUrl\":\"/Images/201508/20150806133953808.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":995,\"name\":\"门04\",\"useDictionaryItem\":{\"id\":949,\"name\":\"门04\",\"thumbnailUrl\":\"/Images/201508/20150806133937241.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":994,\"name\":\"门03\",\"useDictionaryItem\":{\"id\":948,\"name\":\"门03\",\"thumbnailUrl\":\"/Images/201508/20150806133910783.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":993,\"name\":\"门02\",\"useDictionaryItem\":{\"id\":947,\"name\":\"门02\",\"thumbnailUrl\":\"/Images/201508/20150806133856587.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":992,\"name\":\"门01\",\"useDictionaryItem\":{\"id\":946,\"name\":\"门01\",\"thumbnailUrl\":\"/Images/201508/20150806133840285.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":196,\"name\":\"家具\",\"replaceableItemList\":[{\"id\":991,\"name\":\"家具06\",\"useDictionaryItem\":{\"id\":945,\"name\":\"家具06\",\"thumbnailUrl\":\"/Images/201508/20150806133742705.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":990,\"name\":\"家具05\",\"useDictionaryItem\":{\"id\":944,\"name\":\"家具05\",\"thumbnailUrl\":\"/Images/201508/20150806133716794.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":989,\"name\":\"家具04\",\"useDictionaryItem\":{\"id\":943,\"name\":\"家具04\",\"thumbnailUrl\":\"/Images/201508/20150806133659743.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":988,\"name\":\"家具03\",\"useDictionaryItem\":{\"id\":942,\"name\":\"家具03\",\"thumbnailUrl\":\"/Images/201508/20150806133643223.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":987,\"name\":\"家具02\",\"useDictionaryItem\":{\"id\":941,\"name\":\"家具02\",\"thumbnailUrl\":\"/Images/201508/20150806133628215.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":986,\"name\":\"家具01\",\"useDictionaryItem\":{\"id\":940,\"name\":\"家具01\",\"thumbnailUrl\":\"/Images/201508/20150806133536969.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":195,\"name\":\"顶面\",\"replaceableItemList\":[{\"id\":985,\"name\":\"顶面06\",\"useDictionaryItem\":{\"id\":939,\"name\":\"顶面06\",\"thumbnailUrl\":\"/Images/201508/20150806133431948.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":984,\"name\":\"顶面05\",\"useDictionaryItem\":{\"id\":938,\"name\":\"顶面05\",\"thumbnailUrl\":\"/Images/201508/20150806133419359.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":983,\"name\":\"顶面04\",\"useDictionaryItem\":{\"id\":937,\"name\":\"顶面04\",\"thumbnailUrl\":\"/Images/201508/20150806133340499.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":982,\"name\":\"顶面03\",\"useDictionaryItem\":{\"id\":936,\"name\":\"顶面03\",\"thumbnailUrl\":\"/Images/201508/20150806133324416.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":981,\"name\":\"顶面02\",\"useDictionaryItem\":{\"id\":935,\"name\":\"顶面02\",\"thumbnailUrl\":\"/Images/201508/20150806133300127.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":980,\"name\":\"顶面01\",\"useDictionaryItem\":{\"id\":934,\"name\":\"顶面01\",\"thumbnailUrl\":\"/Images/201508/20150806133227975.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":194,\"name\":\"地面\",\"replaceableItemList\":[{\"id\":979,\"name\":\"地面06\",\"useDictionaryItem\":{\"id\":933,\"name\":\"地面06\",\"thumbnailUrl\":\"/Images/201508/20150806133207960.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":978,\"name\":\"地面05\",\"useDictionaryItem\":{\"id\":932,\"name\":\"地面05\",\"thumbnailUrl\":\"/Images/201508/20150806133012411.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":977,\"name\":\"地面04\",\"useDictionaryItem\":{\"id\":931,\"name\":\"地面04\",\"thumbnailUrl\":\"/Images/201508/20150806132926406.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":976,\"name\":\"地面03\",\"useDictionaryItem\":{\"id\":930,\"name\":\"地面03\",\"thumbnailUrl\":\"/Images/201508/20150806132906922.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":975,\"name\":\"地面02\",\"useDictionaryItem\":{\"id\":929,\"name\":\"地面02\",\"thumbnailUrl\":\"/Images/201508/20150806132850417.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":974,\"name\":\"地面01\",\"useDictionaryItem\":{\"id\":928,\"name\":\"地面01\",\"thumbnailUrl\":\"/Images/201508/20150806132820231.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]},{\"id\":193,\"name\":\"窗帘\",\"replaceableItemList\":[{\"id\":973,\"name\":\"窗帘06\",\"useDictionaryItem\":{\"id\":927,\"name\":\"窗帘06\",\"thumbnailUrl\":\"/Images/201508/20150806132759545.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米06\"},{\"id\":972,\"name\":\"窗帘05\",\"useDictionaryItem\":{\"id\":926,\"name\":\"窗帘05\",\"thumbnailUrl\":\"/Images/201508/20150806132743368.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米05\"},{\"id\":971,\"name\":\"窗帘04\",\"useDictionaryItem\":{\"id\":925,\"name\":\"窗帘04\",\"thumbnailUrl\":\"/Images/201508/20150806132730451.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米04\"},{\"id\":970,\"name\":\"窗帘03\",\"useDictionaryItem\":{\"id\":924,\"name\":\"窗帘03\",\"thumbnailUrl\":\"/Images/201508/20150806132714680.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米03\"},{\"id\":969,\"name\":\"窗帘02\",\"useDictionaryItem\":{\"id\":923,\"name\":\"窗帘02\",\"thumbnailUrl\":\"/Images/201508/20150806132700328.jpg\",\"unitPrice\":111,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代风格160平米02\"},{\"id\":968,\"name\":\"窗帘01\",\"useDictionaryItem\":{\"id\":922,\"name\":\"窗帘01\",\"thumbnailUrl\":\"/Images/201508/20150806132630984.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代风格160平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代风格160平米01\"}]}],\"perspectiveList\":[{\"id\":43,\"name\":\"视角1\",\"backgroundImageUrl\":\"/Images/201508/20150811135549376.jpg\",\"visibleItemPlaceholderList\":[{\"id\":196,\"displayIndex\":6},{\"id\":199,\"displayIndex\":5},{\"id\":197,\"displayIndex\":4},{\"id\":193,\"displayIndex\":3},{\"id\":195,\"displayIndex\":2},{\"id\":194,\"displayIndex\":1},{\"id\":198,\"displayIndex\":0}]}],\"materialList\":[{\"id\":1169,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1009},\"imageUrl\":\"/Images/201508/20150806140623248.png\"},{\"id\":1168,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1008},\"imageUrl\":\"/Images/201508/20150806140653060.png\"},{\"id\":1167,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1007},\"imageUrl\":\"/Images/201508/20150806140659081.png\"},{\"id\":1166,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1006},\"imageUrl\":\"/Images/201508/20150806140704323.png\"},{\"id\":1165,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1005},\"imageUrl\":\"/Images/201508/20150806140709253.png\"},{\"id\":1164,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1004},\"imageUrl\":\"/Images/201508/20150806140713995.png\"},{\"id\":1163,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1003},\"imageUrl\":\"/Images/201508/20150806140720001.png\"},{\"id\":1162,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1002},\"imageUrl\":\"/Images/201508/20150806140724821.png\"},{\"id\":1161,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1001},\"imageUrl\":\"/Images/201508/20150806140729985.png\"},{\"id\":1160,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":1000},\"imageUrl\":\"/Images/201508/20150806140734883.png\"},{\"id\":1159,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":999},\"imageUrl\":\"/Images/201508/20150806140839951.png\"},{\"id\":1158,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":998},\"imageUrl\":\"/Images/201508/20150806140845661.png\"},{\"id\":1157,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":997},\"imageUrl\":\"/Images/201508/20150806140850902.png\"},{\"id\":1156,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":996},\"imageUrl\":\"/Images/201508/20150806140924536.png\"},{\"id\":1155,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":995},\"imageUrl\":\"/Images/201508/20150806140929949.png\"},{\"id\":1154,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":994},\"imageUrl\":\"/Images/201508/20150806140935284.png\"},{\"id\":1153,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":993},\"imageUrl\":\"/Images/201508/20150806140940791.png\"},{\"id\":1152,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":992},\"imageUrl\":\"/Images/201508/20150806140945908.png\"},{\"id\":1151,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":991},\"imageUrl\":\"/Images/201508/20150806140952117.png\"},{\"id\":1150,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":990},\"imageUrl\":\"/Images/201508/20150806140956797.png\"},{\"id\":1149,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":989},\"imageUrl\":\"/Images/201508/20150806141001477.png\"},{\"id\":1148,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":988},\"imageUrl\":\"/Images/201508/20150806141006157.png\"},{\"id\":1147,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":987},\"imageUrl\":\"/Images/201508/20150806141011398.png\"},{\"id\":1146,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":986},\"imageUrl\":\"/Images/201508/20150806141017841.png\"},{\"id\":1145,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":985},\"imageUrl\":\"/Images/201508/20150806141023364.png\"},{\"id\":1144,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":984},\"imageUrl\":\"/Images/201508/20150806141028652.png\"},{\"id\":1143,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":983},\"imageUrl\":\"/Images/201508/20150806141033473.png\"},{\"id\":1142,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":982},\"imageUrl\":\"/Images/201508/20150806141038761.png\"},{\"id\":1141,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":981},\"imageUrl\":\"/Images/201508/20150820104349572.png\"},{\"id\":1140,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":980},\"imageUrl\":\"/Images/201508/20150818115317099.png\"},{\"id\":1139,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":979},\"imageUrl\":\"/Images/201508/20150806141054564.png\"},{\"id\":1138,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":978},\"imageUrl\":\"/Images/201508/20150806141059759.png\"},{\"id\":1137,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":977},\"imageUrl\":\"/Images/201508/20150806141106950.png\"},{\"id\":1136,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":976},\"imageUrl\":\"/Images/201508/20150806141112395.png\"},{\"id\":1135,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":975},\"imageUrl\":\"/Images/201508/20150806141117449.png\"},{\"id\":1134,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":974},\"imageUrl\":\"/Images/201508/20150806141124313.png\"},{\"id\":1133,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":973},\"imageUrl\":\"/Images/201508/20150806141131130.png\"},{\"id\":1132,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":972},\"imageUrl\":\"/Images/201508/20150806141135545.png\"},{\"id\":1131,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":971},\"imageUrl\":\"/Images/201508/20150806141140350.png\"},{\"id\":1130,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":970},\"imageUrl\":\"/Images/201508/20150806141144874.png\"},{\"id\":1129,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":969},\"imageUrl\":\"/Images/201508/20150806141149944.png\"},{\"id\":1128,\"forPerspective\":{\"id\":43},\"forReplaceableItem\":{\"id\":968},\"imageUrl\":\"/Images/201508/20150806141154156.png\"}]}}"
        ///                         }
        ///                     ]
        ///                 },
        ///                 {
        ///                     "StyleSecondId":20,
        ///                     "StyleSecondName":"现代风格140平米",
        ///                     "StyleSecondIndex":"http://119.188.113.104:8081/Images/data/201507/6aafb081-e970-43e1-9d2a-4efe95cfb36e0.jpg",
        ///                     "StyleThirdDatsList":[
        ///                         {
        ///                             "StyleThirdId":36,
        ///                             "StyleThirdName":"现代风格03--客厅",
        ///                             "StyleThirdIndex":"http://119.188.113.104:8081/Images/data/201507/c8347265-8b7c-4c9e-9e9b-9f2fff238ff40.jpg",
        ///                             "StyleThirdPics":[
        ///                                 "http://119.188.113.104:8081/Images/data/201507/5d2602db-f4f7-45ac-9663-7ad97ff33dbd0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/1933dced-d8b5-4b7f-ace0-18a248ed86a30.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/09a35ffc-e93a-46a9-a057-b7c8e772ed7d0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/c795474d-48f7-4429-b579-dea2a8ac89520.jpg"
        ///                             ],
        ///                             "UpdateTime":null,
        ///                             "DiyData":"{\"Result\":{\"Success\":true,\"Note\":\"\"},\"SampleRoom\":{\"id\":23,\"name\":\"现代140平米（客厅）\",\"summary\":\"\",\"itemPlaceholderList\":[{\"id\":103,\"name\":\"造型\",\"replaceableItemList\":[{\"id\":403,\"name\":\"造型06\",\"useDictionaryItem\":{\"id\":393,\"name\":\"造型06\",\"thumbnailUrl\":\"/Images/201508/20150817093555126.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":402,\"name\":\"造型05\",\"useDictionaryItem\":{\"id\":392,\"name\":\"造型05\",\"thumbnailUrl\":\"/Images/201508/20150817093654750.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":401,\"name\":\"造型04\",\"useDictionaryItem\":{\"id\":391,\"name\":\"造型04\",\"thumbnailUrl\":\"/Images/201508/20150817093638136.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":400,\"name\":\"造型03\",\"useDictionaryItem\":{\"id\":390,\"name\":\"造型03\",\"thumbnailUrl\":\"/Images/201508/20150817093707042.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":399,\"name\":\"造型02\",\"useDictionaryItem\":{\"id\":389,\"name\":\"造型02\",\"thumbnailUrl\":\"/Images/201508/20150817093721566.png\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":398,\"name\":\"造型01\",\"useDictionaryItem\":{\"id\":388,\"name\":\"造型01\",\"thumbnailUrl\":\"/Images/201508/20150817093745231.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":102,\"name\":\"墙\",\"replaceableItemList\":[{\"id\":409,\"name\":\"墙06\",\"useDictionaryItem\":{\"id\":387,\"name\":\"墙06\",\"thumbnailUrl\":\"/Images/201507/20150731110247082.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":408,\"name\":\"墙05\",\"useDictionaryItem\":{\"id\":386,\"name\":\"墙05\",\"thumbnailUrl\":\"/Images/201507/20150731110227910.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":407,\"name\":\"墙04\",\"useDictionaryItem\":{\"id\":385,\"name\":\"墙04\",\"thumbnailUrl\":\"/Images/201507/20150731110210859.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":406,\"name\":\"墙03\",\"useDictionaryItem\":{\"id\":384,\"name\":\"墙03\",\"thumbnailUrl\":\"/Images/201507/20150731110155259.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":405,\"name\":\"墙02\",\"useDictionaryItem\":{\"id\":383,\"name\":\"墙02\",\"thumbnailUrl\":\"/Images/201507/20150731110137272.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":404,\"name\":\"墙01\",\"useDictionaryItem\":{\"id\":382,\"name\":\"墙01\",\"thumbnailUrl\":\"/Images/201507/20150731110122031.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":101,\"name\":\"家具\",\"replaceableItemList\":[{\"id\":415,\"name\":\"家具06\",\"useDictionaryItem\":{\"id\":381,\"name\":\"家具06\",\"thumbnailUrl\":\"/Images/201508/20150817093919300.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":414,\"name\":\"家具05\",\"useDictionaryItem\":{\"id\":380,\"name\":\"家具05\",\"thumbnailUrl\":\"/Images/201508/20150817093902436.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":413,\"name\":\"家具04\",\"useDictionaryItem\":{\"id\":379,\"name\":\"家具04\",\"thumbnailUrl\":\"/Images/201508/20150817093849425.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":412,\"name\":\"家具03\",\"useDictionaryItem\":{\"id\":378,\"name\":\"家具03\",\"thumbnailUrl\":\"/Images/201508/20150817093834465.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":411,\"name\":\"家具02\",\"useDictionaryItem\":{\"id\":377,\"name\":\"家具02\",\"thumbnailUrl\":\"/Images/201508/20150817093819957.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":410,\"name\":\"家具01\",\"useDictionaryItem\":{\"id\":376,\"name\":\"家具01\",\"thumbnailUrl\":\"/Images/201508/20150817093801658.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":100,\"name\":\"顶面\",\"replaceableItemList\":[{\"id\":421,\"name\":\"顶面06\",\"useDictionaryItem\":{\"id\":375,\"name\":\"顶面06\",\"thumbnailUrl\":\"/Images/201507/20150731105916591.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":420,\"name\":\"顶面05\",\"useDictionaryItem\":{\"id\":374,\"name\":\"顶面05\",\"thumbnailUrl\":\"/Images/201507/20150731105858089.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":419,\"name\":\"顶面04\",\"useDictionaryItem\":{\"id\":373,\"name\":\"顶面04\",\"thumbnailUrl\":\"/Images/201507/20150731105839104.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":418,\"name\":\"顶面03\",\"useDictionaryItem\":{\"id\":372,\"name\":\"顶面03\",\"thumbnailUrl\":\"/Images/201507/20150731105820321.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":417,\"name\":\"顶面02\",\"useDictionaryItem\":{\"id\":371,\"name\":\"顶面02\",\"thumbnailUrl\":\"/Images/201507/20150731105801820.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":416,\"name\":\"顶面01\",\"useDictionaryItem\":{\"id\":370,\"name\":\"顶面01\",\"thumbnailUrl\":\"/Images/201507/20150731105733631.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":99,\"name\":\"地面\",\"replaceableItemList\":[{\"id\":427,\"name\":\"地面06\",\"useDictionaryItem\":{\"id\":369,\"name\":\"地面06\",\"thumbnailUrl\":\"/Images/201507/20150731105715847.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":426,\"name\":\"地面05\",\"useDictionaryItem\":{\"id\":368,\"name\":\"地面05\",\"thumbnailUrl\":\"/Images/201507/20150731105658125.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":425,\"name\":\"地面04\",\"useDictionaryItem\":{\"id\":367,\"name\":\"地面04\",\"thumbnailUrl\":\"/Images/201507/20150731105640903.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":424,\"name\":\"地面03\",\"useDictionaryItem\":{\"id\":366,\"name\":\"地面03\",\"thumbnailUrl\":\"/Images/201507/20150731105620732.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":423,\"name\":\"地面02\",\"useDictionaryItem\":{\"id\":365,\"name\":\"地面02\",\"thumbnailUrl\":\"/Images/201507/20150731105604274.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":422,\"name\":\"地面01\",\"useDictionaryItem\":{\"id\":364,\"name\":\"地面01\",\"thumbnailUrl\":\"/Images/201507/20150731105544867.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":98,\"name\":\"窗帘\",\"replaceableItemList\":[{\"id\":433,\"name\":\"窗帘06\",\"useDictionaryItem\":{\"id\":363,\"name\":\"窗帘06\",\"thumbnailUrl\":\"/Images/201507/20150731105521701.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":432,\"name\":\"窗帘05\",\"useDictionaryItem\":{\"id\":362,\"name\":\"窗帘05\",\"thumbnailUrl\":\"/Images/201507/20150731105504058.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":431,\"name\":\"窗帘04\",\"useDictionaryItem\":{\"id\":361,\"name\":\"窗帘04\",\"thumbnailUrl\":\"/Images/201507/20150731105447506.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":430,\"name\":\"窗帘03\",\"useDictionaryItem\":{\"id\":360,\"name\":\"窗帘03\",\"thumbnailUrl\":\"/Images/201507/20150731105423466.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":429,\"name\":\"窗帘02\",\"useDictionaryItem\":{\"id\":359,\"name\":\"窗帘02\",\"thumbnailUrl\":\"/Images/201507/20150731105403935.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":428,\"name\":\"窗帘01\",\"useDictionaryItem\":{\"id\":358,\"name\":\"窗帘01\",\"thumbnailUrl\":\"/Images/201507/20150731105343437.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":97,\"name\":\"窗\",\"replaceableItemList\":[{\"id\":439,\"name\":\"窗06\",\"useDictionaryItem\":{\"id\":357,\"name\":\"窗06\",\"thumbnailUrl\":\"/Images/201507/20150731105329334.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":438,\"name\":\"窗05\",\"useDictionaryItem\":{\"id\":356,\"name\":\"窗05\",\"thumbnailUrl\":\"/Images/201507/20150731105312174.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":437,\"name\":\"窗04\",\"useDictionaryItem\":{\"id\":355,\"name\":\"窗04\",\"thumbnailUrl\":\"/Images/201507/20150731105254453.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":436,\"name\":\"窗03\",\"useDictionaryItem\":{\"id\":354,\"name\":\"窗03\",\"thumbnailUrl\":\"/Images/201507/20150731105230522.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":435,\"name\":\"窗02\",\"useDictionaryItem\":{\"id\":353,\"name\":\"窗02\",\"thumbnailUrl\":\"/Images/201507/20150731105146202.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":434,\"name\":\"窗01\",\"useDictionaryItem\":{\"id\":352,\"name\":\"窗01\",\"thumbnailUrl\":\"/Images/201507/20150731105117405.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]}],\"perspectiveList\":[{\"id\":29,\"name\":\"视角1\",\"backgroundImageUrl\":\"/Images/201508/20150811141045051.jpg\",\"visibleItemPlaceholderList\":[{\"id\":101,\"displayIndex\":6},{\"id\":103,\"displayIndex\":5},{\"id\":98,\"displayIndex\":4},{\"id\":97,\"displayIndex\":3},{\"id\":102,\"displayIndex\":2},{\"id\":100,\"displayIndex\":1},{\"id\":99,\"displayIndex\":0}]}],\"materialList\":[{\"id\":599,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":439},\"imageUrl\":\"/Images/201507/20150731112214167.png\"},{\"id\":598,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":438},\"imageUrl\":\"/Images/201507/20150731112218862.png\"},{\"id\":597,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":437},\"imageUrl\":\"/Images/201507/20150731112223667.png\"},{\"id\":596,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":436},\"imageUrl\":\"/Images/201507/20150731112228815.png\"},{\"id\":595,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":435},\"imageUrl\":\"/Images/201507/20150731112253869.png\"},{\"id\":594,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":434},\"imageUrl\":\"/Images/201507/20150731112258923.png\"},{\"id\":593,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":433},\"imageUrl\":\"/Images/201507/20150731112305023.png\"},{\"id\":592,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":432},\"imageUrl\":\"/Images/201507/20150731112309656.png\"},{\"id\":591,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":431},\"imageUrl\":\"/Images/201507/20150731112322667.png\"},{\"id\":590,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":430},\"imageUrl\":\"/Images/201507/20150731112328142.png\"},{\"id\":589,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":429},\"imageUrl\":\"/Images/201507/20150731112332900.png\"},{\"id\":588,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":428},\"imageUrl\":\"/Images/201507/20150731112338173.png\"},{\"id\":587,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":427},\"imageUrl\":\"/Images/201507/20150731112342915.png\"},{\"id\":586,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":426},\"imageUrl\":\"/Images/201507/20150731112350637.png\"},{\"id\":585,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":425},\"imageUrl\":\"/Images/201507/20150731112359639.png\"},{\"id\":584,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":424},\"imageUrl\":\"/Images/201507/20150731112406081.png\"},{\"id\":583,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":423},\"imageUrl\":\"/Images/201507/20150731112411214.png\"},{\"id\":582,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":422},\"imageUrl\":\"/Images/201507/20150731112418296.png\"},{\"id\":581,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":421},\"imageUrl\":\"/Images/201507/20150731112424177.png\"},{\"id\":580,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":420},\"imageUrl\":\"/Images/201507/20150731112429279.png\"},{\"id\":579,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":419},\"imageUrl\":\"/Images/201507/20150731112437141.png\"},{\"id\":578,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":418},\"imageUrl\":\"/Images/201507/20150731112444379.png\"},{\"id\":577,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":417},\"imageUrl\":\"/Images/201507/20150731112508731.png\"},{\"id\":576,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":416},\"imageUrl\":\"/Images/201507/20150731112514862.png\"},{\"id\":575,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":415},\"imageUrl\":\"/Images/201508/20150817092230758.png\"},{\"id\":574,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":414},\"imageUrl\":\"/Images/201508/20150817092248245.png\"},{\"id\":573,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":413},\"imageUrl\":\"/Images/201508/20150817092303315.png\"},{\"id\":572,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":412},\"imageUrl\":\"/Images/201508/20150817092320007.png\"},{\"id\":571,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":411},\"imageUrl\":\"/Images/201508/20150817092336293.png\"},{\"id\":570,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":410},\"imageUrl\":\"/Images/201508/20150817092348524.png\"},{\"id\":569,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":409},\"imageUrl\":\"/Images/201507/20150731112613674.png\"},{\"id\":568,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":408},\"imageUrl\":\"/Images/201507/20150731112620054.png\"},{\"id\":567,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":407},\"imageUrl\":\"/Images/201507/20150731112625764.png\"},{\"id\":566,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":406},\"imageUrl\":\"/Images/201507/20150731112631068.png\"},{\"id\":565,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":405},\"imageUrl\":\"/Images/201507/20150731112636512.png\"},{\"id\":564,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":404},\"imageUrl\":\"/Images/201507/20150731112642690.png\"},{\"id\":563,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":403},\"imageUrl\":\"/Images/201508/20150817092404561.png\"},{\"id\":562,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":402},\"imageUrl\":\"/Images/201508/20150817092418117.png\"},{\"id\":561,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":401},\"imageUrl\":\"/Images/201508/20150817092431736.png\"},{\"id\":560,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":400},\"imageUrl\":\"/Images/201508/20150817092444590.png\"},{\"id\":559,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":399},\"imageUrl\":\"/Images/201508/20150817092457960.png\"},{\"id\":558,\"forPerspective\":{\"id\":29},\"forReplaceableItem\":{\"id\":398},\"imageUrl\":\"/Images/201508/20150817092511454.png\"}]}}"
        ///                         },
        ///                         {
        ///                             "StyleThirdId":37,
        ///                             "StyleThirdName":"现代风格03--餐厅",
        ///                             "StyleThirdIndex":"http://119.188.113.104:8081/Images/data/201507/fc83562c-12c9-4b45-ba25-4c32f47c181a0.jpg",
        ///                             "StyleThirdPics":[
        ///                                 "http://119.188.113.104:8081/Images/data/201507/894e2a75-4671-41dc-913e-cf2583380b860.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/ad008129-d401-44c2-a147-cf9ae523f3040.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/a0c20d29-803b-4d02-a500-ae07ee8e0d1a0.jpg",
        ///                                 "http://119.188.113.104:8081/Images/data/201507/4a67ec02-a4ae-47a7-9901-111f34364b160.jpg"
        ///                             ],
        ///                             "UpdateTime":null,
        ///                             "DiyData":"{\"Result\":{\"Success\":true,\"Note\":\"\"},\"SampleRoom\":{\"id\":26,\"name\":\"现代140平米（餐厅）\",\"summary\":\"\",\"itemPlaceholderList\":[{\"id\":124,\"name\":\"造型\",\"replaceableItemList\":[{\"id\":529,\"name\":\"造型06\",\"useDictionaryItem\":{\"id\":519,\"name\":\"造型06\",\"thumbnailUrl\":\"/Images/201508/20150803111204254.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":528,\"name\":\"造型05\",\"useDictionaryItem\":{\"id\":518,\"name\":\"造型05\",\"thumbnailUrl\":\"/Images/201508/20150803111157250.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":527,\"name\":\"造型04\",\"useDictionaryItem\":{\"id\":517,\"name\":\"造型04\",\"thumbnailUrl\":\"/Images/201508/20150803111146033.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":526,\"name\":\"造型03\",\"useDictionaryItem\":{\"id\":516,\"name\":\"造型03\",\"thumbnailUrl\":\"/Images/201508/20150803111135098.jpg\",\"unitPrice\":2,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":525,\"name\":\"造型02\",\"useDictionaryItem\":{\"id\":515,\"name\":\"造型02\",\"thumbnailUrl\":\"/Images/201508/20150803111124302.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":524,\"name\":\"造型01\",\"useDictionaryItem\":{\"id\":514,\"name\":\"造型01\",\"thumbnailUrl\":\"/Images/201508/20150803111115660.jpg\",\"unitPrice\":111,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":123,\"name\":\"墙\",\"replaceableItemList\":[{\"id\":535,\"name\":\"墙06\",\"useDictionaryItem\":{\"id\":513,\"name\":\"墙06\",\"thumbnailUrl\":\"/Images/201508/20150803100744838.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":534,\"name\":\"墙05\",\"useDictionaryItem\":{\"id\":512,\"name\":\"墙05\",\"thumbnailUrl\":\"/Images/201508/20150803100707086.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":533,\"name\":\"墙04\",\"useDictionaryItem\":{\"id\":511,\"name\":\"墙04\",\"thumbnailUrl\":\"/Images/201508/20150803100647462.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":532,\"name\":\"墙03\",\"useDictionaryItem\":{\"id\":510,\"name\":\"墙03\",\"thumbnailUrl\":\"/Images/201508/20150803100612439.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":531,\"name\":\"墙02\",\"useDictionaryItem\":{\"id\":509,\"name\":\"墙02\",\"thumbnailUrl\":\"/Images/201508/20150803100555061.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":530,\"name\":\"墙01\",\"useDictionaryItem\":{\"id\":508,\"name\":\"墙01\",\"thumbnailUrl\":\"/Images/201508/20150803100538509.jpg\",\"unitPrice\":2,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":122,\"name\":\"家具\",\"replaceableItemList\":[{\"id\":541,\"name\":\"家具06\",\"useDictionaryItem\":{\"id\":507,\"name\":\"家具06\",\"thumbnailUrl\":\"/Images/201508/20150820111344355.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":540,\"name\":\"家具05\",\"useDictionaryItem\":{\"id\":506,\"name\":\"家具05\",\"thumbnailUrl\":\"/Images/201508/20150820111326259.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":539,\"name\":\"家具04\",\"useDictionaryItem\":{\"id\":505,\"name\":\"家具04\",\"thumbnailUrl\":\"/Images/201508/20150820110842245.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":538,\"name\":\"家具03\",\"useDictionaryItem\":{\"id\":504,\"name\":\"家具03\",\"thumbnailUrl\":\"/Images/201508/20150820110824165.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":537,\"name\":\"家具02\",\"useDictionaryItem\":{\"id\":503,\"name\":\"家具02\",\"thumbnailUrl\":\"/Images/201508/20150820110756740.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":536,\"name\":\"家具01\",\"useDictionaryItem\":{\"id\":502,\"name\":\"家具01\",\"thumbnailUrl\":\"/Images/201508/20150820110646977.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":121,\"name\":\"顶面\",\"replaceableItemList\":[{\"id\":547,\"name\":\"顶面06\",\"useDictionaryItem\":{\"id\":501,\"name\":\"顶面06\",\"thumbnailUrl\":\"/Images/201508/20150803100032109.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":546,\"name\":\"顶面05\",\"useDictionaryItem\":{\"id\":500,\"name\":\"顶面05\",\"thumbnailUrl\":\"/Images/201508/20150803100008038.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":545,\"name\":\"顶面04\",\"useDictionaryItem\":{\"id\":499,\"name\":\"顶面04\",\"thumbnailUrl\":\"/Images/201508/20150803095826030.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":544,\"name\":\"顶面03\",\"useDictionaryItem\":{\"id\":498,\"name\":\"顶面03\",\"thumbnailUrl\":\"/Images/201508/20150803095807481.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":543,\"name\":\"顶面02\",\"useDictionaryItem\":{\"id\":497,\"name\":\"顶面02\",\"thumbnailUrl\":\"/Images/201508/20150803095747763.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":542,\"name\":\"顶面01\",\"useDictionaryItem\":{\"id\":496,\"name\":\"顶面01\",\"thumbnailUrl\":\"/Images/201508/20150803095727873.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":120,\"name\":\"地面\",\"replaceableItemList\":[{\"id\":553,\"name\":\"地面06\",\"useDictionaryItem\":{\"id\":495,\"name\":\"地面06\",\"thumbnailUrl\":\"/Images/201508/20150817095109506.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":552,\"name\":\"地面05\",\"useDictionaryItem\":{\"id\":494,\"name\":\"地面05\",\"thumbnailUrl\":\"/Images/201508/20150817095123609.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":551,\"name\":\"地面04\",\"useDictionaryItem\":{\"id\":493,\"name\":\"地面04\",\"thumbnailUrl\":\"/Images/201508/20150817095132454.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":550,\"name\":\"地面03\",\"useDictionaryItem\":{\"id\":492,\"name\":\"地面03\",\"thumbnailUrl\":\"/Images/201508/20150817095141674.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":549,\"name\":\"地面02\",\"useDictionaryItem\":{\"id\":491,\"name\":\"地面02\",\"thumbnailUrl\":\"/Images/201508/20150817095148912.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":548,\"name\":\"地面01\",\"useDictionaryItem\":{\"id\":490,\"name\":\"地面01\",\"thumbnailUrl\":\"/Images/201508/20150817095158132.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":119,\"name\":\"窗帘\",\"replaceableItemList\":[{\"id\":559,\"name\":\"窗帘06\",\"useDictionaryItem\":{\"id\":489,\"name\":\"窗帘06\",\"thumbnailUrl\":\"/Images/201508/20150803095419003.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":558,\"name\":\"窗帘05\",\"useDictionaryItem\":{\"id\":488,\"name\":\"窗帘05\",\"thumbnailUrl\":\"/Images/201508/20150803095400595.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":557,\"name\":\"窗帘04\",\"useDictionaryItem\":{\"id\":487,\"name\":\"窗帘04\",\"thumbnailUrl\":\"/Images/201508/20150803095341142.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":556,\"name\":\"窗帘03\",\"useDictionaryItem\":{\"id\":486,\"name\":\"窗帘03\",\"thumbnailUrl\":\"/Images/201508/20150803095314513.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":555,\"name\":\"窗帘02\",\"useDictionaryItem\":{\"id\":485,\"name\":\"窗帘02\",\"thumbnailUrl\":\"/Images/201508/20150803095257571.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":554,\"name\":\"窗帘01\",\"useDictionaryItem\":{\"id\":484,\"name\":\"窗帘01\",\"thumbnailUrl\":\"/Images/201508/20150803095230755.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]},{\"id\":118,\"name\":\"窗\",\"replaceableItemList\":[{\"id\":565,\"name\":\"窗06\",\"useDictionaryItem\":{\"id\":483,\"name\":\"窗06\",\"thumbnailUrl\":\"/Images/201508/20150803095207620.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米06\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米06\"},{\"id\":564,\"name\":\"窗05\",\"useDictionaryItem\":{\"id\":482,\"name\":\"窗05\",\"thumbnailUrl\":\"/Images/201508/20150803095128558.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米05\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米05\"},{\"id\":563,\"name\":\"窗04\",\"useDictionaryItem\":{\"id\":481,\"name\":\"窗04\",\"thumbnailUrl\":\"/Images/201508/20150803095048996.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米04\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米04\"},{\"id\":562,\"name\":\"窗03\",\"useDictionaryItem\":{\"id\":480,\"name\":\"窗03\",\"thumbnailUrl\":\"/Images/201508/20150803095003335.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米03\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米03\"},{\"id\":561,\"name\":\"窗02\",\"useDictionaryItem\":{\"id\":479,\"name\":\"窗02\",\"thumbnailUrl\":\"/Images/201508/20150803094832574.jpg\",\"unitPrice\":1,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米02\"},\"count\":1,\"asDefaultItem\":false,\"styleKeywords\":\"现代140平米02\"},{\"id\":560,\"name\":\"窗01\",\"useDictionaryItem\":{\"id\":478,\"name\":\"窗01\",\"thumbnailUrl\":\"/Images/201508/20150803094250995.jpg\",\"unitPrice\":11,\"measurementUnits\":\"1\",\"styleKeywords\":\"现代140平米01\"},\"count\":1,\"asDefaultItem\":true,\"styleKeywords\":\"现代140平米01\"}]}],\"perspectiveList\":[{\"id\":32,\"name\":\"视角1\",\"backgroundImageUrl\":\"/Images/201508/20150811135849275.jpg\",\"visibleItemPlaceholderList\":[{\"id\":122,\"displayIndex\":6},{\"id\":124,\"displayIndex\":5},{\"id\":119,\"displayIndex\":4},{\"id\":118,\"displayIndex\":3},{\"id\":121,\"displayIndex\":2},{\"id\":120,\"displayIndex\":1},{\"id\":123,\"displayIndex\":0}]}],\"materialList\":[{\"id\":725,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":565},\"imageUrl\":\"/Images/201508/20150803104142810.png\"},{\"id\":724,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":564},\"imageUrl\":\"/Images/201508/20150803104808958.png\"},{\"id\":723,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":563},\"imageUrl\":\"/Images/201508/20150803104815307.png\"},{\"id\":722,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":562},\"imageUrl\":\"/Images/201508/20150803104821641.png\"},{\"id\":721,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":561},\"imageUrl\":\"/Images/201508/20150803104908534.png\"},{\"id\":720,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":560},\"imageUrl\":\"/Images/201508/20150803105258276.png\"},{\"id\":719,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":559},\"imageUrl\":\"/Images/201508/20150803105646941.png\"},{\"id\":718,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":558},\"imageUrl\":\"/Images/201508/20150803105652058.png\"},{\"id\":717,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":557},\"imageUrl\":\"/Images/201508/20150803105815081.png\"},{\"id\":716,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":556},\"imageUrl\":\"/Images/201508/20150803105819917.png\"},{\"id\":715,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":555},\"imageUrl\":\"/Images/201508/20150803105824582.png\"},{\"id\":714,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":554},\"imageUrl\":\"/Images/201508/20150803105829558.png\"},{\"id\":713,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":553},\"imageUrl\":\"/Images/201508/20150803110003268.png\"},{\"id\":712,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":552},\"imageUrl\":\"/Images/201508/20150803110125792.png\"},{\"id\":711,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":551},\"imageUrl\":\"/Images/201508/20150803110243386.png\"},{\"id\":710,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":550},\"imageUrl\":\"/Images/201508/20150803110231624.png\"},{\"id\":709,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":549},\"imageUrl\":\"/Images/201508/20150803110250937.png\"},{\"id\":708,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":548},\"imageUrl\":\"/Images/201508/20150803110256911.png\"},{\"id\":707,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":547},\"imageUrl\":\"/Images/201508/20150803110301638.png\"},{\"id\":706,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":546},\"imageUrl\":\"/Images/201508/20150803110308034.png\"},{\"id\":705,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":545},\"imageUrl\":\"/Images/201508/20150803110313182.png\"},{\"id\":704,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":544},\"imageUrl\":\"/Images/201508/20150803110319095.png\"},{\"id\":703,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":543},\"imageUrl\":\"/Images/201508/20150803111331786.png\"},{\"id\":702,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":542},\"imageUrl\":\"/Images/201508/20150803110401839.png\"},{\"id\":701,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":541},\"imageUrl\":\"/Images/201508/20150803110410076.png\"},{\"id\":700,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":540},\"imageUrl\":\"/Images/201508/20150803110416113.png\"},{\"id\":699,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":539},\"imageUrl\":\"/Images/201508/20150803110437906.png\"},{\"id\":698,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":538},\"imageUrl\":\"/Images/201508/20150803110444255.png\"},{\"id\":697,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":537},\"imageUrl\":\"/Images/201508/20150803110449232.png\"},{\"id\":696,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":536},\"imageUrl\":\"/Images/201508/20150820105846808.png\"},{\"id\":695,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":535},\"imageUrl\":\"/Images/201508/20150803110500682.png\"},{\"id\":694,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":534},\"imageUrl\":\"/Images/201508/20150803110506875.png\"},{\"id\":693,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":533},\"imageUrl\":\"/Images/201508/20150803110513240.png\"},{\"id\":692,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":532},\"imageUrl\":\"/Images/201508/20150803110520650.png\"},{\"id\":691,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":531},\"imageUrl\":\"/Images/201508/20150803110554721.png\"},{\"id\":690,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":530},\"imageUrl\":\"/Images/201508/20150803110603129.png\"},{\"id\":689,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":529},\"imageUrl\":\"/Images/201508/20150803110611834.png\"},{\"id\":688,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":528},\"imageUrl\":\"/Images/201508/20150803110617762.png\"},{\"id\":687,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":527},\"imageUrl\":\"/Images/201508/20150803110623783.png\"},{\"id\":686,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":526},\"imageUrl\":\"/Images/201508/20150803110629977.png\"},{\"id\":685,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":525},\"imageUrl\":\"/Images/201508/20150820104950651.png\"},{\"id\":684,\"forPerspective\":{\"id\":32},\"forReplaceableItem\":{\"id\":524},\"imageUrl\":\"/Images/201508/20150803110655311.png\"}]}}"
        ///                         }
        ///                     ]
        ///                 }            
        ///             ]
        ///         }
        /// }
        /// </returns>
        //[HttpPost]
        public ActionResult GetStyleAll(int styleId)
        {
            Styles style = db.Styles.Find(styleId);
            List<StyleDetails> secStyles = db.StyleDetails.Where(item => item.StyleId == styleId).ToList();
            StyleAllData styleAllData = new StyleAllData();
            styleAllData.Id = styleId;
            styleAllData.StyleIndex = ConfigurationManager.AppSettings["ResourceUrl"] + style.StyleIndex;
            styleAllData.StyleName = style.StyleName;

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
            styleAllData.UpdateTime = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"); ;

            List<StyleSecondData> styleSecondDataList = new List<StyleSecondData>();

            foreach(StyleDetails styleDetail in secStyles)
            {
                StyleSecondData styleSecondData = new StyleSecondData();
                styleSecondData.StyleSecondId= styleDetail.Id;
                styleSecondData.StyleSecondName = styleDetail.StyleDetailName;
                styleSecondData.StyleSecondIndex = ConfigurationManager.AppSettings["ResourceUrl"] + styleDetail.StyleDetailIndex;

                List<StyleThirdDownData> styleThirdDownDataList = new List<StyleThirdDownData>();
                int styleDetailId = styleDetail.Id;
                List<StyleThird> styleThirdList = db.StyleThird.Where(item => item.StyleDetailId == styleDetailId).ToList();
                foreach(StyleThird styleThird in styleThirdList)
                {
                    StyleThirdDownData styleThirdDownData = new StyleThirdDownData();
                    styleThirdDownData.StyleThirdId = styleThird.Id;
                    styleThirdDownData.StyleThirdName = styleThird.StyleThirdName;
                    styleThirdDownData.StyleThird720 = styleThird.StyleThird720;
                    styleThirdDownData.StyleThirdIndex = styleThird.StyleThirdIndex;
                    styleThirdDownData.StyleThirdIndex = ConfigurationManager.AppSettings["ResourceUrl"] + styleThird.StyleThirdIndex;
                    styleThirdDownData.StyleThirdPics = styleThird.StyleThirdPics.Substring(0, styleThird.StyleThirdPics.Length - 1).Split(' ');
                    for (int i = 0; i < styleThirdDownData.StyleThirdPics.Length; i++)
                    {
                        styleThirdDownData.StyleThirdPics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + styleThirdDownData.StyleThirdPics[i];
                    }
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
                    sb.Append("\"" + styleThird.StyleThirdCode.Trim() + "\"");
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
                    styleThirdDownData.DiyData = respHtml;
                    styleThirdDownDataList.Add(styleThirdDownData);
                }
                styleSecondData.StyleThirdDatsList = styleThirdDownDataList;
                styleSecondDataList.Add(styleSecondData);
            }
            styleAllData.StyleSecondDataList = styleSecondDataList;

            return Json(new { data = styleAllData }, JsonRequestBehavior.AllowGet);
        }
    }
}