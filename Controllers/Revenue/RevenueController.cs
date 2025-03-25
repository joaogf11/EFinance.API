using EFinnance.API;
using EFinnance.API.ViewModels.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/revenue")]
[ApiController]
[Authorize] 
public class RevenueController : ControllerBase
{
    private readonly AppDbContext _context;

    public RevenueController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("get-revenue")]
        public async Task<ActionResult<IEnumerable<RevenueViewModel>>> GetRevenues()
    {
        var revenues = await _context.Revenues.Include(r => r.Category).Include(r => r.User).ToListAsync();
        
        if (revenues == null || revenues.Count == 0)
        {
            return NotFound("Nenhuma receita encontrada.");
        }

        return Ok(revenues);
    }

    [HttpPost("new-revenue")]
        public async Task<ActionResult<RevenueViewModel>> CreateRevenue(RevenueViewModel revenue)
    {
        var revenueEntity = new RevenueViewModel
        {
            
        };

        _context.Revenues.Add(revenueEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRevenues), new { id = revenueEntity.Id }, revenueEntity);
    }
}
