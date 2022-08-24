using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using UniversalAPI.Extensions;
using UniversalAPI.Interfaces;
using UniversalAPI.Logger.Models;

namespace UniversalAPI.Controllers.Core
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class MyControllerBase : ControllerBase
    {
        ILogger _logger;
        IControllerService _service;
        public MyControllerBase(ILogger logger, IControllerService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Controller base for data loading based controller
        /// </summary>
        /// <returns>IActionResult</returns>
        protected async Task<IActionResult> GetActionResultAndLoadDataAsync<T>(Func<Task<T>> loadFunction)
        {
            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                T data = await loadFunction();
                sw.Stop();
                if (data == null)
                {
                    _logger.LogInformation(GetLogContent(_service.HttpContext.Request.Path, (int)HttpStatusCode.NotFound, 0, sw.ElapsedMilliseconds, null));
                    return NotFound();
                }

                int length = 1;
                // get number of items in data if it is from collaction type
                if (data.IsGenericEnumerable())
                {
                    var val = data.GetType()?.GetProperty("Count")?.GetValue(data);
                    length = Convert.ToInt32(val);
                }

                _logger.LogInformation(GetLogContent(_service.HttpContext.Request.Path, (int)HttpStatusCode.OK, length, sw.ElapsedMilliseconds, null));
                return Ok(data);

            }
            catch (SqlException ex)
            {
                sw.Stop();
                if (ex.Number == -2)
                {
                    _logger.LogWarning(GetLogContent(_service.HttpContext.Request.Path, (int)HttpStatusCode.RequestTimeout, 0, sw.ElapsedMilliseconds, ex.Message));
                    return StatusCode((int)HttpStatusCode.RequestTimeout);
                }
                else
                {
                    _logger.LogError(GetLogContent(_service.HttpContext.Request.Path, (int)HttpStatusCode.InternalServerError, 0, sw.ElapsedMilliseconds, ex.Message));
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(GetLogContent(_service.HttpContext.Request.Path, (int)HttpStatusCode.InternalServerError, 0, sw.ElapsedMilliseconds, ex.Message));
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Creates a log model and serializes it into a json object
        /// </summary>
        /// <param name="service">Controller interface</param>
        /// <param name="statusCode">Status code</param>
        /// <param name="responseLength">Size of the response (list.Count)</param>
        /// <param name="latency">Application time passed for the process</param>
        /// <param name="message">Additional message</param>
        /// <returns>Json</returns>
        protected string GetLogContent(string service, int statusCode, int responseLength, long latency, string? message)
        {
            LogModel model = new LogModel
            {
                latency = latency,
                service = service,
                statusCode = statusCode,
                responseLength = responseLength
            };

            if (!string.IsNullOrEmpty(message))
                model.message = message;

            return JsonSerializer.Serialize(model);
        }
    }
}
