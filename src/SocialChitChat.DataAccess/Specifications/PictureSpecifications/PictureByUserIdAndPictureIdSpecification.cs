using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class PictureByUserIdAndPictureIdSpecification : Specification<Picture>
{
    public PictureByUserIdAndPictureIdSpecification(Guid id, Guid pictureId)
        : base(p => p.AppUserId == id && p.Id == pictureId)
    {
    }
}
