using System;

namespace JwtTemplate.Controllers
{
    public class Response
    {
        public string message { get; }

        public Response(string message)
        {
            this.message = message;
        }

    }
}