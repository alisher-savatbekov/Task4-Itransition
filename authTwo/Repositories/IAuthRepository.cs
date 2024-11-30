using authTwo.ModelDTO;

namespace authTwo.Repositories
{
    public interface IAuthRepository
    {
        Task RegisterUserAsync(RegisterDTO model);
        Task<string> LoginUserAsync(LoginDTO mdoel);
    }
}
