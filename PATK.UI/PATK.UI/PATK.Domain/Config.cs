namespace PATK.Domain
{
    public static class Config
    {
        public static string Identifier => System.Configuration.ConfigurationManager.AppSettings["Subscription.Id"];

        public static string Key => System.Configuration.ConfigurationManager.AppSettings["Subscription.Key"];

        public static string Version => System.Configuration.ConfigurationManager.AppSettings["Subscription.Version"];
    }
}