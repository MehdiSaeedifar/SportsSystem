using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUNSportsSystem.Web.ViewModels
{
    public class JqGridData
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Records { get; set; }
        public IList<JqGridRowData> Rows { get; set; }
        public object UserData { get; set; }
    }

    public class JqGridRowData
    {
        public int Id { set; get; }
        public IList<string> RowCells { set; get; }
    }
}