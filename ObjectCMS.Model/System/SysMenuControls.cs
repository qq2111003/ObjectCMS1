using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.System
{
    [ModelAttribute(TableName = "SysMenuControls", Fields = new string[] { "MenuId", "Name", "CtrlID" })]
    public class SysMenuControls : BaseModel<SysMenuControls>
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string CtrlID { get; set; }
    }
}
