namespace SocialChitChat.DataAccess.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTime LocalVietnamDateTimeNow { get; }
}
