using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Specifications;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class PictureByUserIdAndPictureIdSpecification : Specification<Picture>
{
    public PictureByUserIdAndPictureIdSpecification(string id, Guid pictureId)
        : base(p => p.AppUserId == id && p.PictureId == pictureId)
    {
    }
}
