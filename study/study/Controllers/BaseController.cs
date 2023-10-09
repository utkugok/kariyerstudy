﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using study.DTOs;
using System.Net;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response)
        {
            if (response.Status is HttpStatusCode.NoContent)
            {
                return new ObjectResult(null) { StatusCode = response.Status.GetHashCode() };
            }

            return new ObjectResult(response) { StatusCode = response.Status.GetHashCode() };      
            
        }
    }
}
