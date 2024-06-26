using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class MainPictureByUserIdAndSpecification : Specification<Picture>
{
    public MainPictureByUserIdAndSpecification(string id)
        : base(p => p.AppUserId == id && p.IsMain)
    {
    }
}
