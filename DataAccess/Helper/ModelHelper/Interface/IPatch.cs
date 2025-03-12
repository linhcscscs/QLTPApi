using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.ModelHelper.Interface
{
    public interface IPatch
    {
        public List<PatchItem> PATCHES { get; set; }
    }
    public class PatchItem
    {
        public string KEY { get; set; }
        public string? VALUE { get; set; }
    }
}
