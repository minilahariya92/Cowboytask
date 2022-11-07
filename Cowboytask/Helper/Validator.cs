using Cowboytask.Models;

namespace Cowboytask.Helper;

public class Validator
{
    public static bool ValidateInput(List<int> values)
    {
        return values.Any(x => x <= 0) ? false : true;
    }
}
