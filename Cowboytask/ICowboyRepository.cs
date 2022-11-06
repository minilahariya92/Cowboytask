using Cowboytask.Database.Models;

namespace Cowboytask;

public interface ICowboyRepository
{
    public List<Cowboy> GetAllCowboys();
    public void CreateCowboy(Cowboy cowboy);
    public bool UpdateCowboy(Cowboy cowboy);
    public bool DeleteCowboy(Cowboy cowboy);
    public bool ReloadFirearm(int cowboyId, int firearmId, int bulletsToBeAdded);
    public bool ShootFirearm(int cowboyId, int firearmId);
    public double? CalculateDistance(int firstCowboyId, int secondCowboyId);
    public Cowboy FindComabatWinner(int firstCowboyId, int secondCowboyId);
}
