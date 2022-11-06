using Cowboytask.Database.Models;
using Cowboytask.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Cowboytask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CowboyController : ControllerBase
{
    readonly ICowboyRepository _cowboyRepository;
    public CowboyController(ICowboyRepository _cowboyRepository)
    {
        this._cowboyRepository = _cowboyRepository;
    }

    [HttpGet]
    [Route("GetAllCowboys")]
    public ActionResult<List<Cowboy>> GetAllCowboys()
    {
        return Ok(_cowboyRepository.GetAllCowboys());
    }

    [HttpPost]
    [Route("CreateCowboy")]
    public ActionResult CreateCowboy()
    {
        var res = ExternalServiceHelper.GetFakeCowboy();
        if (res != null)
        {
            Cowboy cowboy = new()
            {
                Name = res.Name,
                Height = res.Height,
                Hair = res.Hair,
                Latitude = res.Latitude,
                Longitude = res.Longitude,
                HitRate = new Random().Next(1, 101), // Randomly choosing hit rate
                Firearms = ExternalServiceHelper.GetFirearms()
            };
            _cowboyRepository.CreateCowboy(cowboy);
            return Ok(cowboy.Name);
        }
        return Ok();
    }

    [HttpPatch]
    [Route("UpdateCowboy")]
    public ActionResult UpdateCowboy(Cowboy cowboy)
    {
        if (_cowboyRepository.UpdateCowboy(cowboy))
            return Ok($"Cowboy {cowboy.Name} updated successfully.");
        else
            return Ok($"Cowboy {cowboy.Name} is either not present or not updated.");
    }

    [HttpDelete]
    [Route("DeleteCowboy")]
    public ActionResult DeleteCowboy(Cowboy cowboy)
    {
        if (_cowboyRepository.DeleteCowboy(cowboy))
            return Ok($"Cowboy {cowboy.Name} deleted successfully.");
        else
            return Ok($"Cowboy {cowboy.Name} is either not present or not deleted.");
    }

    [HttpPost]
    [Route("ReloadFirearm")]
    public ActionResult ReloadFirearm(int cowboyId, int firearmId, int bulletsToBeAdded)
    {
        if (_cowboyRepository.ReloadFirearm(cowboyId, firearmId, bulletsToBeAdded))
            return Ok($"Firearm reloaded successfully.");
        else
            return Ok($"Firearm is either not present or not reloaded.");
    }

    [HttpPost]
    [Route("ShootFirearm")]
    public ActionResult ShootFirearm(int cowboyId, int firearmId)
    {
        if (_cowboyRepository.ShootFirearm(cowboyId, firearmId))
            return Ok($"Shooting done.");
        else
            return Ok($"Either the cowboy or arm is not present or the arm has no bullets.");
    }

    [HttpPost]
    [Route("CalculateDistance")]
    public ActionResult CalculateDistance(int firstCowboyId, int secondCowboyId)
    {
        var distance = _cowboyRepository.CalculateDistance(firstCowboyId, secondCowboyId);
        if (distance != null)
        {
            return Ok($"Distance between given cowboys is {distance} meters");
        }
        return Ok("Something went wrong while calculating distance.");
    }

    [HttpPost]
    [Route("FindComabatWinner")]
    public ActionResult FindComabatWinner(int firstCowboyId, int secondCowboyId)
    {
        var winner = _cowboyRepository.FindComabatWinner(firstCowboyId, secondCowboyId);
        if (winner != null)
        {
            return Ok($"Winner is {winner.Name}");
        }
        return Ok("Something went wrong while finding combat winner.");
    }
}