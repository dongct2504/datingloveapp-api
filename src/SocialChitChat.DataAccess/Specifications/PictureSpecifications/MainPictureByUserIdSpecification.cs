using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Specifications;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class MainPictureByUserIdSpecification : Specification<Picture>
{
    public MainPictureByUserIdSpecification(string id)
        : base(p => p.AppUserId == id && p.IsMain)
    {
    }
}
