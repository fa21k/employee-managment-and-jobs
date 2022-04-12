using employeemanagment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
//using WebMvc.Models;
using System.Web.Configuration;

namespace employeemanagment.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        :base("ApplicationConnection")
    { 

      }
        public DbSet<Employee> Employees { get; set; }
    }
}