using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace employeemanagment.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("الاسم:")]
        [Required (ErrorMessage ="الاسم مطلوب")]
        public string Name { get; set; }
        [DisplayName("التخصص:")]
        [Required(ErrorMessage = "التخصص مطلوب")]
        public string Position { get; set; }
        [DisplayName("الراتب:")]
        [Required(ErrorMessage = "الراتب مطلوب")]
        public decimal Salary { get; set; }

            
    }
}