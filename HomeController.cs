using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCApp.Models;
using DdLibrary;
using static DdLibrary.BusinessLogic.SocieteProcessor;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace MVCApp.Controllers
{

    public class HomeController : Controller
    {
        

        public static int soc_id;
        public ActionResult ViewSociete()
        {
            ViewBag.Message = "Societe List";

            var data = LoadSociete();

            List<SocieteModel> societe = new List<SocieteModel>();

            foreach (var row in data)
            {
                societe.Add(new SocieteModel
                {
                    SocieteId       = row.SocieteId,     
                    DetailSociete   = row.DetailSociete, 
                    SocieteAdresse  = row.SocieteAdresse,
                    ContactPerson   = row.ContactPerson
                });
            }

            return View(societe);
        }

        public ActionResult CreateSocieteRec()
        {
            ViewBag.Message = "Société";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSocieteRec(SocieteModel model)
        {
            if (ModelState.IsValid)
            {
                int recordsCreated = CreateSociete(
                    model.SocieteId,     
                    model.DetailSociete, 
                    model.SocieteAdresse,
                    model.ContactPerson
                    );
                return RedirectToAction("ViewSociete");
            }

            return View();
        }

        public ActionResult UpdateSocieteRec(int Id)
        {
            soc_id = Id;
            ViewBag.Message = "Update Societe";

            var data = LoadSociete();

            foreach (var row in data)
            {
                if (row.SocieteId == Id)
                {
                    SocieteModel societe = new SocieteModel
                    {
                        SocieteId       = row.SocieteId,
                        DetailSociete   = row.DetailSociete,
                        SocieteAdresse  = row.SocieteAdresse,
                        ContactPerson   = row.ContactPerson

                    };

                    return View(societe);
                }
            }

            return RedirectToAction("ViewSociete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSocieteRec(SocieteModel model)
        {
            if (ModelState.IsValid)
            {
                int recordsModified = UpdateSociete(
                    soc_id,
                    model.DetailSociete,
                    model.SocieteAdresse,
                    model.ContactPerson
                    );
                return RedirectToAction("ViewSociete");
            }
            soc_id = 0;
            return View();
        }

        public ActionResult DeleteSocieteRec(int Id)
        {
            soc_id = Id;
            ViewBag.Message = "Delete Société";

            var data = LoadSociete();


            foreach (var row in data)
            {
                if (row.SocieteId == Id)
                {
                    SocieteModel societe = new SocieteModel
                    {
                        SocieteId = row.SocieteId,
                        DetailSociete = row.DetailSociete,
                        SocieteAdresse = row.SocieteAdresse,
                        ContactPerson = row.ContactPerson

                    };

                    return View(societe);
                }
            }

            return RedirectToAction("ViewSociete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSocieteRec(SocieteModel model)
        {
           
            if (ModelState.IsValid)
            {
                int recordsDeleted = DeleteSociete(soc_id);
                return RedirectToAction("ViewSociete");
            }
            soc_id = 0;
            return View();
        }


        public void ExportSocieteListToCSV()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("\"Societe ID\",\"Détail de la Société\",\"Adresse de la Société\",\"Détail du Contact Person\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_Users.txt");
            Response.ContentType = "text/plain";
            //Response.ContentType = "text/csv";

            var data = LoadSociete();

            foreach (var line in data)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                      line.SocieteId,
                                      line.DetailSociete,
                                      line.SocieteAdresse,
                                      line.ContactPerson));
            }

            Response.Write(sw.ToString());

            Response.End();

        }

        public ActionResult ImportSocieteListFromCSV()
        {
            Thread nt = new Thread(new ThreadStart(ThreadMethod));
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();

            return RedirectToAction("ViewSociete");
        }

        public void ThreadMethod()
        {
            int lineNumber = 0;
            string filename;

            OpenFileDialog ofd = new OpenFileDialog();

            //ofd.Filter = "CSV|*.csv";
            ofd.Filter = "TXT|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;

                using (StreamReader reader = new StreamReader(@filename))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (lineNumber != 0)
                        {
                            var values = line.Split(',');

                            int Id = 0;

                            int recordsCreated = CreateSociete(Id, values[0], values[1], values[2]);

                        }
                        lineNumber++;
                    }
                }
            }

            RedirectToAction("ViewSociete");

        }

    }
}
