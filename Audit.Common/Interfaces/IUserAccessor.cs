namespace Audit.Common.Interfaces
{
    public interface IUserAccessor
    {
        int GetCurrentUserId { get; }
    }
}
