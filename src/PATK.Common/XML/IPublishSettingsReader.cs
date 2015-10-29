using System.Collections.Generic;
using PATK.Domain.Azure;

namespace PATK.Common.XML
{
    public interface IPublishSettingsReader
    {
        IList<PublishSettings> ReadPublishSettings(byte[] publishSettingsContent);
    }
}