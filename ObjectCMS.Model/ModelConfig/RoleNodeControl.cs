using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "RoleNodeControl", Fields = new string[] { "RoleId", "NodeControlMark" })]
    public class RoleNodeControl : BaseModel<RoleNodeControl>
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string NodeControlMark { get; set; }
    }
}