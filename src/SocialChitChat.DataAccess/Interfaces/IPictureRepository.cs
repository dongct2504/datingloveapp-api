using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IPictureRepository : IRepository<Picture>
{
    void Update(Picture picture);
}
