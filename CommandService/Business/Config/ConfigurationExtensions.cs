namespace CommandService.Business.Config
{
    public static class ConfigurationExtensions
    {
        public static SeqConfig GetSeqSettings(this IConfiguration configuration)
        {
            return configuration.GetSection("Seq").Get<SeqConfig>();
        }

        public static RabbitMQConfig GetRabbitMQConfig(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMQConfig").Get<RabbitMQConfig>();
        }
    }
}
