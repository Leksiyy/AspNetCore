using ASPViewComponent.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPViewComponent.ViewComponents;

public class PersonViewComponent : ViewComponent
{
    public string Invoke(User user)
    {
        return $"Name - {user.Name}, age - {user.Age}";
    }
}