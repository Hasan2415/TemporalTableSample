using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemporalTableSample.Models;

namespace TemporalTableSample.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly EFDataContext _context;

    public PeopleController(EFDataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task Add(Person person)
    {
        _context.Add(person);
        await _context.SaveChangesAsync();
    }

    [HttpPut]
    public async Task Update(Person person)
    {
        var entity = await _context.People.FindAsync(person.Id);
        
        if (entity !=null)
        {
            entity.FirstName = person.FirstName;
            entity.LastName = person.LastName;
            await _context.SaveChangesAsync();    
        }
    }

    [HttpGet("{id}/histories")]
    public async Task<object> GetAllHistory(int id)
    {
        var result = await _context.People.TemporalAll()
            .Where(_=>_.Id == id)
            .Select(_ => new
            {
                _.Id,
                _.FirstName,
                _.LastName,
                Start = EF.Property<DateTime>(_, "PeriodStart"),
                End = EF.Property<DateTime>(_, "PeriodEnd")
            }).ToListAsync();
        
        return result;
    }
}