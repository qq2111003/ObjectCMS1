using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectCMS.Model.Core;

namespace ObjectCMS.Model
{
    [ModelAttribute(TableName = "TeTags", Fields = new string[] {  "TagName", "TagHTML" })]
    public class TeTags : BaseModel<TeTags>
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string TagHTML { get; set; }

    }
}