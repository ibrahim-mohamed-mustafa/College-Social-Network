//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace College_Social_Network_Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Course_Cat
    {
        public int Course_id { get; set; }
        public int Cat_id { get; set; }
        public string Note { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual tbl_categroy tbl_categroy { get; set; }
    }
}