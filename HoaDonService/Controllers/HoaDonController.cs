using HoaDonService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HoaDonService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly HoaDonDBContext _hoaDonDBContext;

        public HoaDonController(HoaDonDBContext context)
        {
            _hoaDonDBContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetAllHoaDon()
        {
            return await _hoaDonDBContext.HoaDons.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetHoaDonById(int id)
        {
            var hoaDon = await _hoaDonDBContext.HoaDons.FindAsync(id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return hoaDon;
        }

        [HttpGet("sinhvien/{mssv}")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDonByMSSV(int mssv)
        {
            var hoaDons = await _hoaDonDBContext.HoaDons.Where(h => h.MSSV == mssv).ToListAsync();
            if (hoaDons == null || hoaDons.Count == 0)
            {
                return NotFound();
            }
            return hoaDons;
        }

        [HttpPut("{id}/{user_id}")]
        public async Task<IActionResult> UpdateStatus(int id, string user_id)
        {
            var hoaDon = await _hoaDonDBContext.HoaDons.FindAsync(id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            hoaDon.Status = Status.DA_THANH_TOAN;
            hoaDon.PaidById = user_id;
            hoaDon.PaidDate = DateTime.Now;

            _hoaDonDBContext.Entry(hoaDon).State = EntityState.Modified;

            try
            {
                await _hoaDonDBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("user/{user_id}")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDonByPaidById(string user_id)
        {
            var hoaDons = await _hoaDonDBContext.HoaDons.Where(h => h.PaidById.Equals(user_id)).ToListAsync();

            if (hoaDons == null || hoaDons.Count == 0)
            {
                return NotFound();
            }

            return hoaDons;
        }

        private bool HoaDonExists(int id)
        {
            return _hoaDonDBContext.HoaDons.Any(e => e.Id == id);
        }
    }
}
