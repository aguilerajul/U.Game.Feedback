using Newtonsoft.Json;
using System.Net;

namespace U.Game.Feedback.Domain.Entities
{
    public class ActionResultMessage
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string Message { get; private set; }

        public ActionResultMessage(HttpStatusCode statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
