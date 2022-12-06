using System.Collections.Generic;

public class Constant
{
    public const string JoinKey = "j";
    public const string DifficultyKey = "d";
    public const string MapKey = "m";

    public static readonly List<string> MapKeys = new() { "A", "B", "C"};
    public static readonly List<string> Difficulties = new() { "Easy", "Medium", "Hard" };
}
