using backend.services.interfaces;
using System.Security.Claims;

namespace backend.services
{
    public class UserCurrent : IUserCurrent
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserCurrent(IHttpContextAccessor accessor) => _contextAccessor = accessor;

        public Guid Id
        {
            get
            {
                var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(userId, out var id) ? id : throw new UnauthorizedAccessException();
            }
        }
    }
}
