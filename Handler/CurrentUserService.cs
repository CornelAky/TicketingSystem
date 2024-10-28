namespace TicketingSystem.Handler
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string AzureId => _httpContextAccessor.HttpContext?.User?.FindFirst("AzureID")?.Value;
        public string Name => _httpContextAccessor.HttpContext?.User?.FindFirst("Name")?.Value;
    }

}
