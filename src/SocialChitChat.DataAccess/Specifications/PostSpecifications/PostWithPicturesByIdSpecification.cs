using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.PostSpecifications;

public class PostWithPicturesByIdSpecification : Specification<Post>
{
    public PostWithPicturesByIdSpecification(Guid id)
        : base(p => p.Id == id)
    {
        AddIncludes(p => p.Pictures);
    }
}
