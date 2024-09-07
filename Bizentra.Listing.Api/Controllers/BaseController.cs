using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bizentra.Listing.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected Guid Id
        {
            get
            {
                try
                {
                    var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                    var userId = Guid.Parse(claimsIdentity.FindFirst("Id")?.Value);
                    return userId;
                }
                catch (NullReferenceException ex)
                {
                    return Guid.Empty;
                }
                catch (Exception)
                {
                    return Guid.Empty;
                }
            }

        }


        protected string Name
        {
            get
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                string Name = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
                return Name;
            }

        }

        protected string Email
        {
            get
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                string email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                return email;
            }

        }


        protected string Role
        {
            get
            {
                try
                {
                    var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                    string UserRole = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                    return UserRole;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }




    }
}
