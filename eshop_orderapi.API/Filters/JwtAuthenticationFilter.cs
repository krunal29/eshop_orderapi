using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Business.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace eshop_orderapi.API.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JwtAuthenticationFilter : Attribute, IAuthorizationFilter, IActionFilter
    {
        public static ApplicationUserApiRequestModel ApplicationUserApiRequest { get; set; }
        private string requestModel = "";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
                ApplicationUserApiRequest = null;
                if (IsAuthenticated)
                {
                    var token = Convert.ToString(context.HttpContext.Request.Headers["Authorization"]).Substring("bearer ".Length);
                    var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;
                    ApplicationUserApiRequest = new ApplicationUserApiRequestModel
                    {
                        Id = Convert.ToInt32(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value),
                        Name = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Name")?.Value,
                        RoleId = Convert.ToInt32(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "RoleId")?.Value),
                        AuthToken = token
                    };
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (ApplicationUserApiRequest != null)
                {
                    dynamic valuesCntlr = context.Controller as dynamic;
                    valuesCntlr.ApplicationUserApiRequest = ApplicationUserApiRequest;
                }

                //Action log to file
                var model = ((System.Collections.Generic.Dictionary<string, object>)(context.ActionArguments)).Values;
                requestModel = JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Result == null)
                {
                    return;
                }
                if (context.Result != null && ((ObjectResult)context.Result).Value != null && ((dynamic)((ObjectResult)context.Result).Value).MessageType.ToString() == "Error")
                {
                    var controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                    var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                    var fileName = controllerName + ' ' + actionName + ' ' + DateTime.UtcNow.Ticks.ToString() + ".txt";
                    var filePath = Directory.GetCurrentDirectory() + "/uploads/RequestLog/" + DateTime.Now.Date.Year + "_" + DateTime.Now.Date.Month.ToString("00") + "_" + DateTime.Now.Day.ToString("00");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    fileName = Path.Combine(filePath, fileName);
                    //FileHelper.WriteLine(fileName, requestModel);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}