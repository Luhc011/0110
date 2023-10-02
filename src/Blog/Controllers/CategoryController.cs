﻿using Blog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] DataContext context)
            => Ok(context.Categories.ToList());
    }
}
