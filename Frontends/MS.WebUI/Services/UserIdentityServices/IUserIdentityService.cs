using MS.DtoL.IdentityDtos.UserDtos;

namespace MS.WebUI.Services.UserIdentityServices
{
    public interface IUserIdentityService
    {
        Task<List<ResultUserDto>> GetAllUserListAsync();
    }
}
