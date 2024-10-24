using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IPictureRepository : IRepository<Picture>
{
    Task UpdateAsync(Picture picture);
}
