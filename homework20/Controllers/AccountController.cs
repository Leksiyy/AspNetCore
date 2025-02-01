using homework20.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
    
namespace homework20.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        
        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpPost]
        public async Task<IActionResult> RequestDeletion(int userId)
        {
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            
            if (user.DeletedAt == null)
            {
                user.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }

            return Ok("Запрос на удаление принят. Ваш аккаунт будет удалён через 30 дней.");
        }
    }
}