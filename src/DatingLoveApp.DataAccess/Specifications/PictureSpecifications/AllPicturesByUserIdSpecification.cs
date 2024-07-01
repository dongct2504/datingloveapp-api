using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class AllPicturesByUserIdSpecification : Specification<Picture>
{
    public AllPicturesByUserIdSpecification(string id)
        : base(p => p.AppUserId == id)
    {
    }
}
