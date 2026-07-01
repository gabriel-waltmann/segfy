using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Root;

[ApiController]
[Tags("Root")]
[Route("/")]
public class RootController : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult> ExecuteAsync()
  {
    return StatusCode(200, new { message = "API running!" });
  }
}