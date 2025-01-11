using Blog.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "Admin")]
public class MembershipController : Controller
{
    private readonly IMembership _membership;

    public MembershipController(IMembership membership)
    {
        this._membership = membership;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var allMemberships = await _membership.GetAllMembershipsAsync();
        return View(allMemberships);
    }

    [HttpGet]
    public async Task<IActionResult> Generate([FromServices] IHttpContextAccessor httpContextAccessor)
    {
        string code = Guid.NewGuid().ToString();
        string scheme = httpContextAccessor.HttpContext.Request.Scheme;
        string host = httpContextAccessor.HttpContext.Request.Host.Value;

        string link = $"{scheme}://{host}/register?code={code}";
        
        var membership = new Membership
        {
            CreatedDate = DateTime.Now,
            IsEnable = true,
            Code = code,
            Link = link,
        };
        _membership.AddMembershipAsync(membership);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int membershipId)
    {
        var currentMembership = await _membership.GetMembershipAsync(membershipId);
        if (currentMembership is not null)
        {
            await _membership.DeleteMembershipAsync(currentMembership);
        }

        return RedirectToAction("Index");
    }
}