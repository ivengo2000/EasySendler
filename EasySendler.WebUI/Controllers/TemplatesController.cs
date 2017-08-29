using System;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Extensions;
using EasySendler.Models.BusinessLogic;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    public class TemplatesController : Controller
    {
        private MySmtpEntities db = new MySmtpEntities();


        public TemplatesController()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Template, TemplateViewModel>());
        }
        // GET: Templates
        public ActionResult Index()
        {
            return View(db.Templates.ProjectTo<TemplateViewModel>().ToList());
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetTemplates()
        {
            var result = db.Templates.ProjectTo<TemplateViewModel>().ToList();

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: Templates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var template = Mapper.Map<TemplateViewModel>(db.Templates.Find(id));
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // GET: Templates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TemplateId,Name,Description,Body")] Template template, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            //string path = Path.Combine(Server.MapPath("~/UploadImages"), fileName);
                            //file.SaveAs(path);
                            //template.Thumbnail = file.InputStream;


                            byte[] uploadedFile = new byte[file.InputStream.Length];
                            file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                            template.Thumbnail = uploadedFile;
                        }
                    }
                    catch (Exception ex)
                    {
                        //ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }

                db.Templates.Add(template);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Mapper.Map<TemplateViewModel>(template));
        }

        // GET: Templates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var template = Mapper.Map<TemplateViewModel>(db.Templates.Find(id));
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TemplateId,Name,Description,Body")] Template template, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        if (!string.IsNullOrEmpty(fileName))
                        {

                            Stream newFileStream = new MemoryStream();
                            string fileExtention = file.ContentType;
                            int fileLenght = file.ContentLength;
                            if (fileExtention == "image/png" || fileExtention == "image/jpeg" || fileExtention == "image/x-png")
                            {
                                if (fileLenght <= 1048576)
                                {
                                    Bitmap bmpPostedImage = new Bitmap(file.InputStream);
                                    Image objImage = Scale(bmpPostedImage);
                                    byte[] uploadedFile = ImageToByteArray(objImage);
                                    file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                                    template.Thumbnail = uploadedFile;
                                }
                                else
                                {
                                    //lblmsg.Text = "Image size cannot be more then 1 MB.";
                                    //lblmsg.Style.Add("Color", "Red");
                                }
                            }
                            else
                            {
                                //lblmsg.Text = "Invaild Format!";
                                //lblmsg.Style.Add("Color", "Red");
                            }
 
                           // byte[] uploadedFile = new byte[newFileStream.Length];
                            //byte[] uploadedFile = new byte[file.InputStream.Length];
                            //file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                            //template.Thumbnail = uploadedFile;
                            //newFileStream.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        //ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }

                db.Entry(template).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Mapper.Map<TemplateViewModel>(template));
        }

        // GET: Templates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var template = Mapper.Map<TemplateViewModel>(db.Templates.Find(id));
            if (template == null)
            {
                return HttpNotFound();
            }

            return View(template);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Template template = db.Templates.Find(id);
            if (template != null)
            {
                db.Templates.Remove(template);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Image Scale(Image imgPhoto)
        {
            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            int Width = 300;
            int Height = 300;

            // force resize, might distort image
            if (Width != 0 && Height != 0)
            {
                destWidth = Width;
                destHeight = Height;
            }
            // change size proportially depending on width or height
            else if (Height != 0)
            {
                destWidth = (float)(Height * sourceWidth) / sourceHeight;
                destHeight = Height;
            }
            else
            {
                destWidth = Width;
                destHeight = (float)(sourceHeight * Width / sourceWidth);
            }

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        public byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
