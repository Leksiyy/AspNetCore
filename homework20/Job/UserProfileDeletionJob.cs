using homework20.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace homework20.Job
{
    [DisallowConcurrentExecution]
    public class UserProfileDeletionJob : IJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserProfileDeletionJob> _logger;
        
        private readonly TimeSpan _deletionDelay = TimeSpan.FromDays(30);
        private readonly TimeSpan _warningTime = TimeSpan.FromDays(23);

        public UserProfileDeletionJob(ApplicationDbContext dbContext, ILogger<UserProfileDeletionJob> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Запуск задачи UserProfileDeletionJob в {Time}", DateTime.UtcNow);

            var now = DateTime.UtcNow;
            
            var profiles = await _dbContext.UserProfiles
                .Where(u => u.DeletedAt.HasValue)
                .ToListAsync();

            foreach (var profile in profiles)
            {
                var timeSinceRequest = now - profile.DeletedAt.Value;
                
                if (timeSinceRequest >= _deletionDelay)
                {
                    _logger.LogInformation("Удаляем профиль {Username} (Id: {Id}). Прошло дней: {Days}.", profile.Username, profile.Id, timeSinceRequest.TotalDays);
                    _dbContext.UserProfiles.Remove(profile);
                }
                else if (timeSinceRequest >= _warningTime && !profile.DeletionWarningSent)
                {
                    Console.WriteLine("Отправляем предупреждение пользователю {Username} (Id: {Id}). Осталось дней до удаления: {RemainingDays}.", 
                        profile.Username, profile.Id, (_deletionDelay - timeSinceRequest).TotalDays);
                    
                    profile.DeletionWarningSent = true;
                }
            }
            
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Задача UserProfileDeletionJob завершена в {Time}", DateTime.UtcNow);
        }
    }
}
