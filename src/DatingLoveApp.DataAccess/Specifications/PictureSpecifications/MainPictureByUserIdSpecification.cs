using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class MainPictureByUserIdSpecification : Specification<Picture>
{
    public MainPictureByUserIdSpecification(string id)
        : base(p => p.AppUserId == id && p.IsMain)
    {
    }
}
