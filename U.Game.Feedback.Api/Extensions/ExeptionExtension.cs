using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using U.Game.Feedback.Api.Models;

namespace U.Game.Feedback.Api.Extensions
{
    public static class ExeptionExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var handlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(handlerFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails() { 
                            StatusCode = context.Response.StatusCode,
                            Message = handlerFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
