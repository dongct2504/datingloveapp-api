using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Specifications;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class MainPicturesByUserIdsSpecification : Specification<Picture>
{
    public MainPicturesByUserIdsSpecification(Guid[] userIds)
        : base(p => userIds.Contains(p.AppUserId) && p.IsMain)
    {
    }
}
