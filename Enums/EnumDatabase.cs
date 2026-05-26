using System.ComponentModel;

namespace TravelMasterApi.Enums
{
	public enum edIsEnable
	{
		DISABLE,
		ENABLE
	}

	public enum edState
	{
		LOCKED,
		ACTIVE
	}
    public enum eCategories
    {
        Tours = 1,
        Hotels = 2,
        Car = 3,
    }
    public enum eBookingState
    {
        PendingProcessing = 1,
        Processed = 2,
        Cancelled = 3,
    }

}
