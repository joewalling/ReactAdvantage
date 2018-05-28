using System.Collections.Generic;

namespace ReactAdvantage.Domain.ViewModels.Dto
{
    public class DtoBase
    {
        internal List<ApiError> Errors = new List<ApiError>();

        public bool HasErrors()
        {
            return Errors.Count > 0;
        }

        public bool NoData { get; set; }
    

        public void AddError(string statusCode, string userMessage, string internalMessage, string moreInfo)
        {
            Errors.Add(new ApiError {StatusCode = statusCode, UserMessage = userMessage, InternalMessage = internalMessage, MoreInfo = moreInfo});
        }


        internal class ApiError
        {
            public string StatusCode { get; set; }
            public string UserMessage { get; set; }
            public string InternalMessage { get; set; }
            public string MoreInfo { get; set; }


        }
    }
}
