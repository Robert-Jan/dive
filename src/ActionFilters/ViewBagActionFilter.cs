using Dive.App.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dive.App.ActionFilters
{
    public class ViewBagActionFilter : ActionFilterAttribute
    {
        private readonly IUserRepository _userRepository;

        public ViewBagActionFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Controller is Controller)
            {
                var controller = context.Controller as Controller;

                controller.ViewBag.CurrentUser = context.HttpContext.User.Identity.IsAuthenticated
                    ? _userRepository.GetCurrentUser()
                    : null;
            }

            base.OnResultExecuting(context);
        }
    }
}