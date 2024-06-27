using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class PictureByUserIdSpecification : Specification<Picture>
{
    public PictureByUserIdSpecification(string id)
        : base(p => p.AppUserId == id)
    {
    }
}
