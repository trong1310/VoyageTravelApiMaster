namespace TravelMasterApi.Base.BaseMessages
{
    public class BaseRequestMessage
    {
        public int Limit { get; set; }
        public int Page { get; set; }
    }

    public class BaseRequestMessageKeyword : BaseRequestMessage
    {
        public string? Keyword { get; set; }
    }
	public class BaseRequestMessageKeywordTime : BaseRequestMessageKeyword
	{
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
	}
}
