using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model
{
    [ModelAttribute(TableName = "NodeTemplates", Fields = new string[] { "NodeId", "TemplateId"})]
    public class NodeTemplates : BaseModel<NodeTemplates>
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public int TemplateId { get; set; }
    }
}