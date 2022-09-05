using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace College_Social_Network_Project.Models
{
    public class SelectOption
    {
        public SOpt StudentGender { get; set; }
    }

    public enum SOpt
    {
        A,
        B,
        C,
        D
    }
}