using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.PictureSpecifications;

public class AllPicturesByPostIdSpecificaion : Specification<Picture>
{
    public AllPicturesByPostIdSpecificaion(Guid postId)
        : base(p => p.PostId == postId)
    {
    }
}
