using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "NodeUserModelField", Fields = new string[] {"NodeId","UserModelFieldId","ReWriteTitle","ShowInList","ListWidth","Sort","DefVal","OtherAttr","Validator","Tip"})]
    public class NodeUserModelField : BaseModel<NodeUserModelField>
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public int UserModelFieldId { get; set; }
        public string ReWriteTitle { get; set; }
        public bool ShowInList { get; set; }
        public int ListWidth { get; set; }
        public int Sort { get; set; }
        public string DefVal { get; set; }
        public string OtherAttr { get; set; }
        public string Validator { get; set; }
        public string Tip { get; set; }
    }
}