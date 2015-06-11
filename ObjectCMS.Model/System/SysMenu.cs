using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;


namespace ObjectCMS.Model.System
{
    [ModelAttribute(TableName = "SysMenu", Fields = new string[] { "ParentId", "Title", "Url", "Ico", "Target", "Sort", "Enable" })]
    public class SysMenu : BaseModel<SysMenu>
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Ico { get; set; }
        public string Target { get; set; }
        public int Sort { get; set; }
        public bool Enable { get; set; }
    }
}
