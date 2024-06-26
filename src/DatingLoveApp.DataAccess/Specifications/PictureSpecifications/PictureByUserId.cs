using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class PictureByUserId : Specification<Picture>
{
    public PictureByUserId(string id)
        : base(p => p.AppUserId == id)
    {
    }
}
