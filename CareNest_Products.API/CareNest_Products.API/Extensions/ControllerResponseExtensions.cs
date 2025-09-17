using CareNest_Products.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Products.API.Extensions
{
    /// <summary>
    /// Controller response extensions
    /// </summary>
    public static class ControllerResponseExtensions
    {
        /// <summary>
        /// Trả về response thành công với data
        /// </summary>
        public static IActionResult OkResponse<T>(this ControllerBase controller, T data, string message = "Success")
        {
            var response = ApiResponse<T>.SuccessResult(data, message);
            return controller.Ok(response);
        }

        /// <summary>
        /// Trả về response thành công không có data
        /// </summary>
        public static IActionResult OkResponse(this ControllerBase controller, string message = "Success")
        {
            var response = ApiResponse.SuccessResult(message);
            return controller.Ok(response);
        }

        /// <summary>
        /// Trả về response lỗi
        /// </summary>
        public static IActionResult ErrorResponse(this ControllerBase controller, string message, int statusCode = 400, List<string>? errors = null)
        {
            var response = ApiResponse.ErrorResult(message, errors);
            return statusCode switch
            {
                400 => controller.BadRequest(response),
                404 => controller.NotFound(response),
                500 => controller.StatusCode(500, response),
                _ => controller.BadRequest(response)
            };
        }
    }
}
