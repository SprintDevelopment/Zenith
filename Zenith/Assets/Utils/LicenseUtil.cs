using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Windows.Controls;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

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

            return processorIdObject is not null ?
                processorIdObject["ProcessorId"]
                    .ToString()
                    .Chunk(4)
                    .Select(chars => new string(chars))
                    .Select(hexString => (short)Math.Abs(Convert.ToInt16(hexString, 16) * 31))
                    .Select(value => value.ToString("X4"))
                    .Join("-") :
                "";
        }

        //licenseStringFormat => "serialNumber,startDate,endDate"
        public static AppLicenseDto GetLicense()
        {
            return (
                new ConfigurationRepository().Single($"{ConfigurationKeys.AppLicense}") ?? 
                new Models.Configuration())
                .Value.ToLicense();
        }

        public static AppLicenseDto ToLicense(this string licenseHashedString)
        {
            var license = new AppLicenseDto { State = AppLicenseStates.NotFound, SerialNumber = GetSerialNumber() };
            if (!licenseHashedString.IsNullOrWhiteSpace())
            {
                license.State = AppLicenseStates.Invalid;
                var licenseString = CryptoUtil.Decrypt(licenseHashedString);

                var licenseParts = licenseString.Split(',');
                if (licenseParts.Length == 3 && licenseParts[0] == license.SerialNumber && DateTime.TryParse(licenseParts[1], out DateTime startDate) && DateTime.TryParse(licenseParts[2], out DateTime endDate))
                {
                    license.StartDate = startDate;
                    license.EndDate = endDate;
                    license.State = DateTime.Today >= license.StartDate && DateTime.Today <= license.EndDate ?
                    AppLicenseStates.Valid :
                    AppLicenseStates.Expired;
                }
            }

            return license;
        }

        public static AppLicenseDto CheckAndApplyLicense(string licenseHashedString)
        {
            var license = licenseHashedString.ToLicense();
            if (license.State == AppLicenseStates.Valid)
                SetLicense(license);

            return license;
        }

        public static void SetLicense(AppLicenseDto license)
        {
            var licenseEncryptedString = CryptoUtil.Encrypt($"{license.SerialNumber},{license.StartDate:yyyy-MM-dd},{license.EndDate:yyyy-MM-dd}");
            var configurationRepository = new ConfigurationRepository();

            var licenseConfiguration = configurationRepository.Single($"{ConfigurationKeys.AppLicense}");

            if (licenseConfiguration is not null)
            {
                licenseConfiguration.Value = licenseEncryptedString;
                configurationRepository.Update(licenseConfiguration, $"{ConfigurationKeys.AppLicense}");
            }
            else
            {
                configurationRepository.Add(new Models.Configuration
                {
                    Key = $"{ConfigurationKeys.AppLicense}",
                    Value = licenseEncryptedString,
                });
            }
        }
    }
}
