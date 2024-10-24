using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class PictureExistInUserSpecification : Specification<Picture>
{
    public PictureExistInUserSpecification(Guid userId, Guid pictureId)
        : base(p => p.AppUserId == userId && p.Id == pictureId)
    {
    }
}
