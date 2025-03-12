namespace QLTPApi.Service
{
    public interface ITestService
    {
        public List<string> IpTesting();
    }
    public class TestSerivce : ITestService
    {
        public TestSerivce()
        {

        }
        public List<string> IpTesting() => new List<string>();
    }
}
