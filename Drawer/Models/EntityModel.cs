using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drawer.Models
{
    public class EntityModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<AttributeModel> childAttributes { get; set; }
        public List<RelationshipModel> chilRelationships { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public int xAxis { get; set; }
        public int yAxis { get; set; }
                
    }
}