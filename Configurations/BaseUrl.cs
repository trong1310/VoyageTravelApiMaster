using System.ComponentModel;

namespace TravelMasterApi.Configurations
{
    [Description("Định nghĩa đường dẫn")]
    public class BaseUrl
    {
        private string _url { get; }

        public BaseUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("url is empty");
            _url = url;
        }

        public override string ToString()
        {
            return _url;
        }
    }
}
