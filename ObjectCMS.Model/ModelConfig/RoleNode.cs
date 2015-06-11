using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "RoleNode", Fields = new string[] {"RoleId","NodeId"})]
    public class RoleNode : BaseModel<RoleNode>
    {
        public int id { get; set; }
        public int RoleId { get; set; }
        public int NodeId { get; set; }
    }
}