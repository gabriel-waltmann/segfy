using Microsoft.AspNetCore.Mvc;

namespace api.CarInsurancePolicy;

[ApiController]
[Tags("CarInsurancePolicy")]
[Route("api/car-insurance-policies")]
public class CarInsurancePolicyController(CarInsurancePolicyService service) : ControllerBase
{
    private readonly CarInsurancePolicyService _service = service;

    [HttpGet]
    public ActionResult<IEnumerable<CarInsurancePolicyEntity>> List()
    {
        return Ok(_service.List());
    }

    [HttpGet("expiring")]
    public ActionResult<IEnumerable<CarInsurancePolicyEntity>> ListExpiringSoon([FromQuery] int days = 30)
    {
        return Ok(_service.List(days));
    }

    [HttpGet("{number}")]
    public ActionResult<CarInsurancePolicyEntity> Get(string number)
    {
        var policy = _service.Get(number);
        if (policy is null)
        {
            return NotFound();
        }

        return Ok(policy);
    }

    [HttpPost]
    public ActionResult<CarInsurancePolicyEntity> Create([FromBody] CarInsurancePolicyEntity policy)
    {
        var created = _service.Create(policy);
        return CreatedAtAction(nameof(Get), new { number = created.Number }, created);
    }

    [HttpPut("{number}")]
    public ActionResult<CarInsurancePolicyEntity> Update(string number, [FromBody] CarInsurancePolicyEntity policy)
    {
        var updated = _service.Update(number, policy);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{number}")]
    public IActionResult Delete(string number)
    {
        var deleted = _service.Delete(number);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
