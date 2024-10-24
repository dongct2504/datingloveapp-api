using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IPictureRepository : IRepository<Picture>
{
    Task UpdateAsync(Picture picture);
}
