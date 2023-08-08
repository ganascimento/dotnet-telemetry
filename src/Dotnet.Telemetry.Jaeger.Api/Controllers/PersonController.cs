using Bogus;
using Dotnet.Telemetry.Jaeger.Api.Context;
using Dotnet.Telemetry.Jaeger.Api.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Telemetry.Jaeger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PersonController> _logger;

    public PersonController(AppDbContext context, ILogger<PersonController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation($"{nameof(PersonController)}: Try get all Persons | {DateTime.Now.ToString("dd/MM/yyyy")}");

            var result = await _context.Person.ToListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(PersonController)}: {ex.Message} | {DateTime.Now.ToString("dd/MM/yyyy")}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        try
        {
            _logger.LogInformation($"{nameof(PersonController)}: Try create person | {DateTime.Now.ToString("dd/MM/yyyy")}");

            var person = new Faker<PersonEntity>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.MotherName, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Female))
                .RuleFor(u => u.BirthDate, f => f.Date.Between(new DateTime(1950, 01, 01), new DateTime(2010, 12, 31)))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## #####-####"))
                .Generate();

            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();

            return Ok(person);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(PersonController)}: {ex.Message} | {DateTime.Now.ToString("dd/MM/yyyy")}");
            return BadRequest(ex.Message);
        }
    }
}