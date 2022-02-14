using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_DropDown
    {


        public Model_DropDown()
        {


        }

        public Model_DropDown(int ID, string Name)
        {
            this.id = ID;
            this.text = Name;


        }


        public int id { get; set; }
        public string text { get; set; }

    }
}