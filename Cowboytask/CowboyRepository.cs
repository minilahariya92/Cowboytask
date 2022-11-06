﻿using Cowboytask.Database.Models;
using System.Device.Location;

namespace Cowboytask;

public class CowboyRepository : ICowboyRepository
{
    static ApiContext context = new ApiContext();

    public List<Cowboy> GetAllCowboys()
    {
        return context.Cowboy.ToList();
    }

    public void CreateCowboy(Cowboy cowboy)
    {
        context.Cowboy.Add(cowboy);
        context.SaveChanges();
    }

    public bool UpdateCowboy(Cowboy cowboy)
    {
        bool isUpdated = false;
        var cowboyToBeUpdated = context.Cowboy.FirstOrDefault(x => x.Id == cowboy.Id);
        if (cowboyToBeUpdated != null)
        {
            
            cowboyToBeUpdated.Name = cowboy.Name;
            cowboyToBeUpdated.Height = cowboy.Height;
            cowboyToBeUpdated.Hair = cowboy.Hair;
            cowboyToBeUpdated.Longitude = cowboy.Longitude;
            cowboyToBeUpdated.Latitude = cowboy.Latitude;
            cowboyToBeUpdated.Firearms = cowboy.Firearms;
            context.Cowboy.Update(cowboyToBeUpdated);
            context.SaveChanges();
            isUpdated = true;
        }
        return isUpdated;
    }

    public bool DeleteCowboy(Cowboy cowboy)
    {
        bool isDeleted = false;
        var itemToBeDeleted = context.Cowboy.ToList().FirstOrDefault(x => x.Id == cowboy.Id);
        if(itemToBeDeleted != null)
        {
            context.Cowboy.Remove(itemToBeDeleted);
            context.SaveChanges();
            isDeleted = true;
        }
        return isDeleted; 
    }

    public bool ReloadFirearm(int cowboyId, int firearmId, int bulletsToBeAdded)
    {
        bool isReloaded = false;
        var cowboy = context.Cowboy.ToList().FirstOrDefault(x => x.Id == cowboyId);
        if (cowboy != null)
        {
            var firearmToReload = cowboy.Firearms.FirstOrDefault(x => x.Id == firearmId);
            if (firearmToReload != null
                && bulletsToBeAdded + firearmToReload.RemainedNumOfBullets < firearmToReload.MaxNumOfBullets)
            {
                firearmToReload.RemainedNumOfBullets += bulletsToBeAdded;
                isReloaded = true;
            }
        }
        context.SaveChanges();
        return isReloaded;
    }

    public bool ShootFirearm(int cowboyId, int firearmId)
    {
        bool isShooted= false;
        var cowboy = context.Cowboy.ToList().FirstOrDefault(x => x.Id == cowboyId);
        if (cowboy != null)
        {
            var firearmToShoot = cowboy.Firearms.FirstOrDefault(x => x.Id == firearmId);
            if (firearmToShoot != null && firearmToShoot.RemainedNumOfBullets > 0)
            {
                firearmToShoot.RemainedNumOfBullets -= 1;
                isShooted = true;
            }
        }
        context.SaveChanges();
        return isShooted;
    }

    public double? CalculateDistance(int firstCowboyId, int secondCowboyId)
    {
        var firstCowboy = context.Cowboy.ToList().FirstOrDefault(x => x.Id == firstCowboyId);
        var secondCowboy = context.Cowboy.ToList().FirstOrDefault(x => x.Id == secondCowboyId);

        if(firstCowboy != null && secondCowboy != null)
        {
            var sCoord = new GeoCoordinate(firstCowboy.Latitude, firstCowboy.Longitude);
            var eCoord = new GeoCoordinate(secondCowboy.Latitude, secondCowboy.Longitude);

            return sCoord.GetDistanceTo(eCoord);
        }
        return null;
    }

    public Cowboy FindComabatWinner(int firstCowboyId, int secondCowboyId)
    {
        var firstCowboy = context.Cowboy.ToList().FirstOrDefault(x => x.Id == firstCowboyId);
        var secondCowboy = context.Cowboy.ToList().FirstOrDefault(x => x.Id == secondCowboyId);

        if (firstCowboy != null && secondCowboy != null)
        {
            var hitsByFirstCowboy = GetAchievableTarget(firstCowboy);
            var hitsBySecondCowboy = GetAchievableTarget(secondCowboy);
            return hitsByFirstCowboy > hitsBySecondCowboy ? firstCowboy : secondCowboy;
        }
        return null;
    }

    public int GetAchievableTarget(Cowboy cowboy)
    {
        int remainedBullets = cowboy.Firearms.Sum(x => x.RemainedNumOfBullets);
        var targets = (remainedBullets*cowboy.HitRate)/100;
        return targets;
    }
}
