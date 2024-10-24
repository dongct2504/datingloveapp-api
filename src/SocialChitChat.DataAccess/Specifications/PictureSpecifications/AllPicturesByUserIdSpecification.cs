using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Specifications;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class AllPicturesByUserIdSpecification : Specification<Picture>
{
    public AllPicturesByUserIdSpecification(string id)
        : base(p => p.AppUserId == id)
    {
    }
}
