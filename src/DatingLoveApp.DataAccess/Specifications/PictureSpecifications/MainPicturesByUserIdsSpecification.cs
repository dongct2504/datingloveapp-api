using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Specifications.PictureSpecifications;

public class MainPicturesByUserIdsSpecification : Specification<Picture>
{
    public MainPicturesByUserIdsSpecification(string[] userIds)
        : base(p => userIds.Contains(p.AppUserId) && p.IsMain)
    {
    }
}
