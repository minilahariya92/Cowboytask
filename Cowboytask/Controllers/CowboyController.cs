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
            return Ok(cowboy);
        }
        return Ok();
    }

    [HttpPatch]
    [Route("UpdateCowboy")]
    public ActionResult UpdateCowboy(Cowboy cowboy)
    {
        if (_cowboyRepository.UpdateCowboy(cowboy))
            return Ok(cowboy);
        else
            return Ok($"Cowboy {cowboy.Name} is either not found or not updated.");
    }

    [HttpDelete]
    [Route("DeleteCowboy")]
    public ActionResult DeleteCowboy(int cowboyId)
    {
        if(!Validator.ValidateInput(new List<int> { cowboyId}))
            return Ok($"Please provide valid input parameter.");

        if (_cowboyRepository.DeleteCowboy(cowboyId))
            return Ok($"Cowboy deleted successfully.");
        else
            return Ok($"Cowboy is either not found or not deleted.");
    }

    [HttpPost]
    [Route("ReloadFirearm")]
    public ActionResult ReloadFirearm(int cowboyId, int firearmId, int bulletsToBeAdded)
    {
        if (!Validator.ValidateInput(new List<int> { cowboyId, firearmId, bulletsToBeAdded}))
            return Ok($"Please provide valid input parameter.");

        if (_cowboyRepository.ReloadFirearm(cowboyId, firearmId, bulletsToBeAdded))
            return Ok($"Firearm reloaded successfully.");
        else
            return Ok($"Firearm is either not present or not reloaded.");
    }

    [HttpPost]
    [Route("ShootFirearm")]
    public ActionResult ShootFirearm(int cowboyId, int firearmId)
    {
        if (!Validator.ValidateInput(new List<int> { cowboyId, firearmId }))
            return Ok($"Please provide valid input parameters.");

        if (_cowboyRepository.ShootFirearm(cowboyId, firearmId))
            return Ok($"Shooting done.");
        else
            return Ok($"Looks like cowboy or firearm is not found or the firearm has no bullets.");
    }

    [HttpPost]
    [Route("CalculateDistance")]
    public ActionResult CalculateDistance(int firstCowboyId, int secondCowboyId)
    {
        if (!Validator.ValidateInput(new List<int> { firstCowboyId, secondCowboyId }))
            return Ok($"Please provide valid input parameters.");

        if (firstCowboyId == secondCowboyId)
            return Ok($"Please provide two different cowboys Id.");

        var distance = _cowboyRepository.CalculateDistance(firstCowboyId, secondCowboyId);
        if (distance != null)
        {
            return Ok($"Distance between given cowboys is {distance} meters");
        }
        return Ok("Looks like cowboy(s) not found.");
    }

    [HttpPost]
    [Route("FindComabatWinner")]
    public ActionResult FindComabatWinner(int firstCowboyId, int secondCowboyId)
    {
        if (!Validator.ValidateInput(new List<int> { firstCowboyId, secondCowboyId }))
            return Ok($"Please provide valid input parameters.");

        if (firstCowboyId == secondCowboyId)
            return Ok($"Please provide two different cowboys Id.");

        var winner = _cowboyRepository.FindComabatWinner(firstCowboyId, secondCowboyId);
        if (winner != null)
        {
            return Ok($"Winner is cowboy having Id {winner.Id}");
        }
        return Ok("Looks like cowboy(s) not found.");
    }
}