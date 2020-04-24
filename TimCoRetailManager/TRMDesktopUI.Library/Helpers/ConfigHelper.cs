using System.Configuration;

namespace TRMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public double GetTaxRate()
        {
            string taxRateText = ConfigurationManager.AppSettings["taxRate"];
            bool isValidTaxRate = double.TryParse(taxRateText, out double output);
            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The key for the tax rate in the configuration file is invalid");
            }
            return output / 100;
        }
    }
}
