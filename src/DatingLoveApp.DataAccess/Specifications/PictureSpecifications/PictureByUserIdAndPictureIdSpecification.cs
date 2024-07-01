using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class PictureByUserIdAndPictureIdSpecification : Specification<Picture>
{
    public PictureByUserIdAndPictureIdSpecification(string id, Guid pictureId)
        : base(p => p.AppUserId == id && p.PictureId == pictureId)
    {
    }
}
