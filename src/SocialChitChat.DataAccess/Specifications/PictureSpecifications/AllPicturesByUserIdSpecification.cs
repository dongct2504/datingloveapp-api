using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class AllPicturesByUserIdSpecification : Specification<Picture>
{
    public AllPicturesByUserIdSpecification(Guid userId)
        : base(p => p.AppUserId == userId)
    {
    }
}
