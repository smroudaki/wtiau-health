using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_Form
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Model_Steps> Steps { get; set; }
    }
    public class Model_Steps
    {
        public string Name { get; set; }
        public List<Model_Questions> Questions { get; set; }

    }
    public class Model_Questions
    {
        public string Titel { get; set; }
        public List<Model_Responses> Responses { get; set; }
        public int type { get; set; }
        public string Name { get; set; }
        public string [] Response { get; set; }
    }
    public class Model_Responses
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}