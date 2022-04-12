using ClosedXML.Excel;
using employeemanagment.DAL;
using employeemanagment.Models;
using PagedList;
//using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace employeemanagment.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string sortOrder,string searchstring ,string currentFilter,int ? page) 
        {
            ViewBag.CurrentSort = sortOrder;
            // sorting by name descinding , and by position ascinding with descinding and by salry asc and desc
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.positionSortParm = sortOrder=="position"? "position_desc" : "position";
            ViewBag.salarySortParm = sortOrder=="salary" ? "salary_desc" : "salary";


            if (searchstring!=null)
            {
                page = 1;
            }
            else
            {
                searchstring = currentFilter;
            }


            ViewBag.CurrentFilter = searchstring;

            var emp = from e in db.Employees select e;
            if(!string.IsNullOrEmpty(searchstring))
            {
                emp = emp.Where(e => e.Name.Contains(searchstring) || e.Position.Contains(searchstring));
            }
            switch(sortOrder)
            {
                case "name_desc":
                    emp = emp.OrderByDescending(e => e.Name);
                    break;

                case "position":
                    emp = emp.OrderBy(e => e.Position);
                    break;
                case "position_desc":
                    emp = emp.OrderByDescending(e => e.Position);
                    break;
                case "salary":
                    emp = emp.OrderBy(e => e.Salary);
                    break;
                case "salary_desc":
                    emp = emp.OrderByDescending(e => e.Salary);
                    break;

                default:
                    emp = emp.OrderBy(e => e.Name);
                    break;

            }
            int pageSize = 2;
            int pageNamber = (page ?? 1);

           // var employeelist = db.Employees.ToList();
            //return View(emp.ToList());
            return View(emp.ToPagedList(pageNamber,pageSize));

        }

        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            if(ModelState.IsValid)
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View();
        }
        public ActionResult Edit(int ? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = db.Employees.Find(id);
            if(emp==null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }
        [HttpPost]
        public ActionResult edit(Employee emp )
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");

            }
            return View(emp);
        }

        public ActionResult Delete( int ?id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = db.Employees.Find(id);
            if(emp==null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }
        [HttpPost]
        public ActionResult Delete (Employee emp)
        {
            if (ModelState.IsValid)

            {
                Employee _emp = db.Employees.Find(emp.Id);
                db.Employees.Remove(_emp);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emp);
        }

        public ActionResult Details(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                
                Employee emp = db.Employees.Find(id);
                if(emp==null)
                {
                    return HttpNotFound();

                }
               
            
            return View(emp);
        }
        //the action for make file excell to export 
        [HttpPost]
        public FileResult export()
        {
            DataTable dt = new DataTable("Grid"); // the file name is grid with the columns
            dt.Columns.AddRange(new DataColumn[3] {
                new DataColumn("Name"),
                new DataColumn("Position"),
                new DataColumn("Salary") });
            var emps = from emp in db.Employees.ToList() select emp;
            foreach(var emp in emps)
            {
                dt.Rows.Add(emp.Name, emp.Position, emp.Salary);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())//function for save the file
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");// type of file " excell type " with the name file (grid)
                }
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}