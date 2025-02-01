using System;

namespace homework20.Models;


public class UserProfile
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    // Флаг, показывающий, что предупреждение за неделю до удаления уже отправлено.
    public bool DeletionWarningSent { get; set; }

}
