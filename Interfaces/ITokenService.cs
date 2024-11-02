using ModernLibrary.Models;

namespace ModernLibrary.Interfaces
{
    public interface ITokenService
    {
        Task<string?> CreateToken(AppUser user);
    }
}
