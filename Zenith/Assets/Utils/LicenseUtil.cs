using System.Linq;
using System.Management;
using Zenith.Assets.Extensions;

namespace Zenith.Assets.Utils
{
    public static class LicenseUtil
    {
        public static string GetSerialNumber()
        {
            var processorIdObject = new ManagementClass("Win32_Processor")
                .GetInstances().OfType<ManagementBaseObject>()
                .SkipWhile(mo => mo["ProcessorId"].ToString().IsNullOrWhiteSpace())
                .FirstOrDefault();

            return processorIdObject is not null ? $"{processorIdObject["ProcessorId"]:XXXX-XX-XXXX-XXXXXX}" : "";

            //if (processorId != "")
            //{
            //    var hashedProductKey = CryptographyUtil.GenerateSaltedHashBytes(processorId);
            //    return GetSeparatedValue(hashedProductKey);
            //}

            //return "";
        }
    }
}
