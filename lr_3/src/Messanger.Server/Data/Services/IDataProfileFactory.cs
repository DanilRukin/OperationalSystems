using Messanger.Server.Data.DataProfiles.Base;

namespace Messanger.Server.Data.Services
{
    public interface IDataProfileFactory
    {
        DataProfile CreateProfile();
    }
}
