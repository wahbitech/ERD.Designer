using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drawer.Models
{
    public class DiagramModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<EntityModel> entities { get; set; }
    }
}