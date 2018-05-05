using System;
using Microsoft.AspNetCore.Mvc;

public class ApiController : Controller
{
    public IActionResult Item(int? id = null)
    {
        if(id != null)
            return Json(new { message = $"Here's your item: {id.Value}" });

        return Json(new object[]{
            new { message = "I am an object!" },
            new { message = "Me too!" }
        });
    }
}