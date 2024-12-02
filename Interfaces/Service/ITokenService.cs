using ModernLibrary.Models;

namespace ModernLibrary.Interfaces.Service
{
    public interface ITokenService
    {
        Task<string?> CreateToken(AppUser user);
    }
}
