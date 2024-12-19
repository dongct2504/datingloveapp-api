using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Extensions;

public static class GroupChatExtensions
{
    public static string GetGroupName(this GroupChat groupChat)
    {
        List<string> nickNames = groupChat.Participants
            .Select(p => p.AppUser.Nickname)
            .ToList();

        if (!nickNames.Any())
        {
            return "Unnamed group chat";
        }
        else if (nickNames.Count <= 4)
        {
            return string.Join(", ", nickNames);
        }
        else
        {
            return string.Join(",", nickNames.Take(4) + "...");
        }
    }
}
