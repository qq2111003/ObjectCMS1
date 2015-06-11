using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "Node", Fields = new string[] {"ParentId","Title","UserModelId","PageType",
        "ListTemplateId","ListPageUrl","DetailTemplateId","DetailPageUrl"})]
    public class Node : BaseModel<Node>
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public int UserModelId { get; set; }
        public bool PageType { get; set; }
        public int ListTemplateId { get; set; }
        public string ListPageUrl { get; set; }
        public int DetailTemplateId { get; set; }
        public string DetailPageUrl { get; set; }
        public string Target { get; set; }
    }
}