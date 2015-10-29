using PATK.Domain.Azure;

namespace PATK.Common.XML
{
    public interface IPublishSettingsReader
    {
        Subscription ReadPublishSettings(byte[] publishSettingsContent);
    }
}