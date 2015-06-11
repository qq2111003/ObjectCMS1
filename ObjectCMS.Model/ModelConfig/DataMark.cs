using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model.ModelConfig
{
    [ModelAttribute(TableName = "DataMark", Fields = new string[] { "Title", "MarkName", "Ico" })]
    public class DataMark : BaseModel<DataMark>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MarkName { get; set; }
        public string Ico { get; set; }
    }
}
