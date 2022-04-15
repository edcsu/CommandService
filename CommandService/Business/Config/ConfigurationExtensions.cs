namespace CommandService.Business.Config
{
    public static class ConfigurationExtensions
    {
        public static SeqConfig GetSeqSettings(this IConfiguration configuration)
        {
            return configuration.GetSection("Seq").Get<SeqConfig>();
        }
    }
}
