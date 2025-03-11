using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Models
{
    public partial class Menu
    {
        [NotMapped]
        public bool IS_EXIST_CHILD { get; set; }
        [NotMapped]
        public string MenuNameView { get; set; }
    }
}
