using TravelMasterApi.Databases;

namespace TravelMasterApi.Services
{

    public class UserServices
    {
        private readonly MasterDBContext _context;
        public UserServices(MasterDBContext context)
        {
            _context = context;
        }
        public async Task LoginWithGoogle()
        {

        }
    }
}
