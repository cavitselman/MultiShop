using MS.DtoL.IdentityDtos.LoginDtos;

namespace MS.WebUI.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SignInDto signInDto);
        Task<bool> GetRefreshToken();
    }
}
