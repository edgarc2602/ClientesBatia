using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SistemaClientesBatia.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
namespace SistemaClientesBatia.Middleware
{
    public class CustomErrorHandler
    {
        private readonly RequestDelegate _next;

        public CustomErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = ex switch
                {
                    CustomException _ => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException _ => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
                // var result = JsonSerializer.Create().Serialize(new { message = ex?.Message });
                var result = JsonConvert.SerializeObject(new { message = ex?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
