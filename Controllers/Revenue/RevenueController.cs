using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFinnance.API.Controllers.Revenue
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class RevenueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RevenueController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RevenueViewModel>>> GetRevenues()
        {
            return await _context.Revenues.Include(r => r.Category).Include(r => r.User).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<RevenueViewModel>> CreateRevenue(RevenueViewModel revenue)
        {
            _context.Revenues.Add(revenue);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRevenues), new { id = revenue.Id }, revenue);
        }
    }
}
