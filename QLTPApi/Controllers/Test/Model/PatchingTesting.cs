using DataAccess.Helper.ModelHelper.Interface;

namespace QLTPApi.Controllers.Test.Model
{
    public class PatchingTesting
    {
        public int PATCH_INT { get; set; }
        public string PATCH_STRING { get; set; }
        public bool PATCH_BOOL { get; set; }
        public DateTime PATCH_DATE_TIME { get; set; }
    }

    public class PatchModelTesting : IPatch
    {
        public List<PatchItem> PATCHES { get; set; }
    }
}
