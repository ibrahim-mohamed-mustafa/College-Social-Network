using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using College_Social_Network_Project.Models;
using System.Net;

namespace College_Social_Network_Project.Controllers    
{
    public class ObjFileController : Controller
    {

        // GET: ObjFile
        public ActionResult Upload(string id)
        {
            TempData["ID"] = id;
            Session["ID"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ObjFile> ObjFiles = new List<ObjFile>();
            var filePath = Path.Combine(Server.MapPath("~/Files/" + id));
            if (Directory.Exists(filePath))
                {
                    foreach (string strfile in Directory.GetFiles(Server.MapPath("~/Files/" + id + "/")))
                    {
                    FileInfo fi = new FileInfo(strfile);
                    ObjFile obj = new ObjFile();
                    obj.File = fi.Name;
                    obj.Size = fi.Length;
                    obj.Type = GetFileTypeByExtension(fi.Extension);
                    ObjFiles.Add(obj);
                     }
                  }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
                
            return View(ObjFiles);
        }

        
        public FileResult Download(string fileName )
        {
           
            string ID = Session["ID"] as string;
            string fullPath = Path.Combine(Server.MapPath("~/Files/" + ID + "/"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult Delete(string fileName)
        {

            string ID = Session["ID"] as string;
            string fullPath = Path.Combine(Server.MapPath("~/Files/" + ID + "/"), fileName);
            FileInfo fi = new FileInfo(fullPath);
            if (fi != null)
            {
                System.IO.File.Delete(fullPath);
                fi.Delete();
            }
            return RedirectToAction("Upload", new { id = ID });
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string GetFileTypeByExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
               
                case ".docx":
                case ".doc":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
                    return "Image";
                case ".pdf":
                    return "Pdf File";

                default:
                    return "Unknown";
            }
        }
        [HttpPost]
        public ActionResult Upload(ObjFile doc , string id)
        {

        
            foreach (var file in doc.files)
             {

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Files/" + id));
                var filePath1 = Path.Combine(Server.MapPath("~/Files/" + id+"/"), fileName);
                if (file.ContentLength > 0)
                 {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                        foreach (string inputTagName in Request.Files)
                        {
                            HttpPostedFileBase Infile = Request.Files[inputTagName];
                            Infile.SaveAs(filePath1);
                        }
                       
                    }
                    else if (Directory.Exists(filePath))
                    {
                      //  Directory.CreateDirectory(filePath);
                        foreach (string inputTagName in Request.Files)
                        {
                            HttpPostedFileBase Infile = Request.Files[inputTagName];
                            Infile.SaveAs(filePath1);
                        }

                    }


                }
             }
             TempData["Message"] = "files uploaded successfully";
             TempData["ID"] = id;
            return RedirectToAction("Upload");
            

          

        }

    }
}
