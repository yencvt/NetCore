using common.Models.Securities;

namespace common.Services.Interfaces
{
    public interface IUserService
    {
        CustomUser GetCurrentUser { get; }
        string GetCurrentUserId {get;}
    }
}

