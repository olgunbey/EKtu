using System.Text.Json;

namespace EKtu.WEBAPI.Logging
{
    public class MyLogging
    {
        private readonly ILogger<MyLogging> _logger;
        public MyLogging(ILogger<MyLogging> logger)
        {
            _logger = logger;   
        }
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }
}
