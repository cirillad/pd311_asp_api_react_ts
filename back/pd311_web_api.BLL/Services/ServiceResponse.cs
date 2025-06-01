namespace pd311_web_api.BLL.Services
{
    public class ServiceResponse
    {
        public ServiceResponse() { }

        public ServiceResponse(string message, bool isSuccess = false, object? payload = null)
        {
            Message = message;
            IsSuccess = isSuccess; 
            Payload = payload;
        }

        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object? Payload { get; set; } = null;
    }
}
