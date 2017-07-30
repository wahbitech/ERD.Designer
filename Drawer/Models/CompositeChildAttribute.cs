using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drawer.Models
{
    public class CompositeChildAttributeModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public string givenId { get; set; }

        public AttributeModel parentAttribute { get; set; }

        public string dataType { get; set; }

        public int length { get; set; }

        public bool isKey { get; set; }

        public string attributeType { get; set; }

        public string status { get; set; }
    }
}