using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectCMS.Model.Core
{
    public class ModelAttribute:Attribute
    {
        public string TableName { get; set; }
        public string[] Fields { get; set; }
    }
}
