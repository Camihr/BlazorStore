using System.Globalization;

namespace AdaStore.UI.Helpers
{
    public static class Extensions
    {
        public static string MoneyFormat(this double value)
        {
            double aux_value = Math.Pow(10, 2);
            double truncate = (Math.Truncate(value * aux_value) / aux_value);

            return truncate.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}
