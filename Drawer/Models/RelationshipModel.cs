using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drawer.Models
{
    public class RelationshipModel
    {
        public int id { get; set; }
        public int diagramId { get; set; }
        public string relationshipName { get; set; }
        public string primaryAttribute { get; set; }
        public string foriegnAttribute { get; set; }
        public string type { get; set; }
        public string relationtype { get; set; }
        public string status { get; set; }
    }
}