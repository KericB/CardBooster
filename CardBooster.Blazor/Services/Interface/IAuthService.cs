namespace CardBooster.Blazor.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> register(string email, string password);
        Task<string> login(string email, string password);
        Task Logout();
        bool IsAuthenticated();
    }
}
