using Microsoft.AspNetCore.Mvc;
using NLog;
using eshop_orderapi.API.Filters;
using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.ViewModels;
using eshop_orderapi.Business.ViewModels.General;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace eshop_orderapi.API.Controllers
{
    [JwtAuthenticationFilter]
    public class BaseApiController : Controller
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ApplicationUserApiRequestModel ApplicationUserApiRequest { get; set; }

        public BaseApiController()
        {
            ApplicationUserApiRequest = new ApplicationUserApiRequestModel();
        }

        protected async Task<ResponseDetail<T>> GetDataWithMessage<T>(Func<Task<Tuple<T, string, DropMessageType>>> getDataFunc)
        {
            var output = new ResponseDetail<T>();
            try
            {
                var result = await getDataFunc();
                output.Data = result.Item1;
                output.Message = result.Item2;
                output.MessageType = result.Item3;
                return await Task.FromResult(output);
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);

                // Get the parent frame (i.e. the method that directly called the method where the exception occurred)
                var parentFrame = stackTrace.GetFrame(0);

                // Get the file name, line number, and method name of the parent frame
                var lineNumber = parentFrame.GetFileLineNumber();

                output.MessageType = DropMessageType.Error;
                output.Error = new Error
                {
                    Code = ErrorCode.SERVICE_EXECUTION_FAILED,
                    Message = ex.Message
                };
                output.Message = "Something went wrong!! Please Try again later";
                Logger.Error($"An error has occuerd on {Convert.ToString(ControllerContext.RouteData.Values["controller"]) + " controller & " + Convert.ToString(ControllerContext.RouteData.Values["action"]) + " Method & Line Number:" + lineNumber} & Message:{ex.Message}");
                return await Task.FromResult(output);
            }
        }

        protected new Tuple<T, string, DropMessageType> Response<T>(T data, string message, DropMessageType messageType = DropMessageType.Success)
        {
            return new Tuple<T, string, DropMessageType>(data, message, messageType);
        }
    }
}