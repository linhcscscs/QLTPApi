using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class GroupUserMenuEntity
    {
        public long MenuID { get; set; }
        public Nullable<long> ParentID { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public decimal? GroupUserMenuID { get; set; }
        public long? GroupUserID { get; set; }
        public int? IsView { get; set; }
        public int? IsAdd { get; set; }
        public int? IsEdit { get; set; }
        public int? IsDelete { get; set; }
        public int? IsUpload { get; set; }
        public int? IsAuth { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string AuthBy { get; set; }
        public Nullable<System.DateTime> AuthAt { get; set; }
    }
}
