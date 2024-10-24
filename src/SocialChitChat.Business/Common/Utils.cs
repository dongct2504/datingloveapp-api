namespace SocialChitChat.Business.Common;

public static class Utils
{
    public static string GetGroupName(Guid[] userIds)
    {
        //bool stringCompare = string.CompareOrdinal(callerId.ToString(), otherId.ToString()) < 0;
        //return stringCompare ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";

        Guid[] sortedUserIds = userIds.OrderBy(id => id).ToArray();
        return string.Join("-", sortedUserIds);
    }
}
