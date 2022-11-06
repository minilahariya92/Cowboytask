namespace Cowboytask.Database.Models;

public class Firearm
{
    public int Id { get; set; }
    public string GunName { get; set; }
    public int MaxNumOfBullets { get; set; }
    public int RemainedNumOfBullets { get; set; }
}
