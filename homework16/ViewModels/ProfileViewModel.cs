using homework16.Models;
using Microsoft.AspNetCore.Identity;

namespace homework16.ViewModels;

public class ProfileViewModel
{
    public List<Articles> Articles { get; set; }
    public IdentityUser User { get; set; } 
}