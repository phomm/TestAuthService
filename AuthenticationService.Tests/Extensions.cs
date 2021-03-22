using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Tests
{
    public static class Extensions
    {
        public static object ToValue(this IActionResult actionResult)
        {
            return (actionResult as ObjectResult)?.Value ?? (actionResult as JsonResult)?.Value;
        }
    }
}
