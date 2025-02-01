using homework20.Models;

namespace homework20.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.UserProfiles.Any())
            {
                return;
            }

            var users = new UserProfile[]
            {
                new UserProfile
                {
                    Username = "TestUser1",
                    Email = "testuser1@example.com",
                    DeletedAt = null,
                    DeletionWarningSent = false
                },
                new UserProfile
                {
                    Username = "TestUser2",
                    Email = "testuser2@example.com",
                    DeletedAt = null,
                    DeletionWarningSent = false
                }
            };

            foreach (var user in users)
            {
                context.UserProfiles.Add(user);
            }

            context.SaveChanges();
        }
    }
}