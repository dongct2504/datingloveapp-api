using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class MainPictureByUserIdSpecification : Specification<Picture>
{
    public MainPictureByUserIdSpecification(Guid id)
        : base(p => p.AppUserId == id && p.IsMain)
    {
    }
}
