using Microsoft.AspNetCore.Mvc;

namespace ASPViewComponent.ViewComponents;

public class UsersListViewComponent : ViewComponent
{
    private List<string> _users;

    public UsersListViewComponent()
    {
        _users = new List<string>
        {
            "Tom", "Tim", "Bob", "Sam", "Alice", "Kate"
        };
    }

    public IViewComponentResult Invoke()
    {
        int number = _users.Count;

        if (Request.Query.ContainsKey("number"))
        {
            Int32.TryParse(Request.Query["number"], out number);
        }
        
        return View(_users.Take(number));
    }
}