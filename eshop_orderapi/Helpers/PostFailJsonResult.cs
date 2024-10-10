using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.ViewModels.General;
using System.Linq;

namespace eshop_orderapi.Helpers
{
    public class PostFailJsonResult : PostJsonResult<PostFailJsonResultData>
    {
        public PostFailJsonResult()
        {
            StatusCode = 400;
        }

        public PostFailJsonResult(string message) : this()
        {
            Data.Message = message;
        }

        public PostFailJsonResult(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState) : this()
        {
            Data.ModelErrors = modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToArray();
        }

        //public PostFailJsonResult(Microsoft.AspNetCore.Mvc.ModelStateDictionary modelState) : this()
        //{
        //    Data.ModelErrors = modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToArray();
        //}

        public PostFailJsonResult WithDropMessage(string message, string description, DropMessageType dropMessageType)
        {
            Data.DropMessage = new DropMessageModel { Message = message, Description = description, DropMessageType = dropMessageType };
            return this;
        }

        public PostFailJsonResult WithDropMessage(string message, string description = null)
        {
            return WithDropMessage(message, description, DropMessageType.Error);
        }
    }

    public class PostFailJsonResultData : PostJsonResultData
    {
        public string[] ModelErrors { get; set; }

        public string Message { get; set; }
    }
}