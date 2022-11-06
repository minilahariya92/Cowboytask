using Cowboytask.Models;

namespace Cowboytask.Database.Models;

public class Cowboy
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public string Hair { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int HitRate { get; set; }
    public List<Firearm> Firearms { get; set; }
}
