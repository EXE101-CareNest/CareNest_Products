namespace CareNest_Products.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Role { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}
