using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model
{
    [ModelAttribute(TableName = "TeTemplates", Fields = new string[] { "TypeId", "Title", "Note", "TemplateHTML", "CreateTime", "UpdateTime" })]
    public class TeTemplates : BaseModel<TeTemplates>
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public string TemplateHTML { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}