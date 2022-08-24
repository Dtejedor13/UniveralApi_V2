using UniversalAPI.Interfaces;

namespace UniversalAPI.Services
{
    public class ControllerService : IControllerService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public ControllerService(IHttpContextAccessor accessor)
        {
            contextAccessor = accessor;
        }

        public HttpContext HttpContext
        {
            get
            {
                return contextAccessor.HttpContext;
            }
        }
    }
}
