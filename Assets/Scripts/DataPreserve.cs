/// <summary>
/// <para>
/// This class use for store the global data. Such as total score, survival time, character selected number,...
/// </para>
///
/// <para>
/// All fields must be pulbic static for accessing directly from outside to read, change the properties
/// </para>
/// </summary>
public class DataPreserve
{
    public static int characterSelectedNumber = 1; // default is John
    public static int totalScore = 0;
    public static float survivedTime = 0;
    public static bool allowPickUpWeapon = false;
    public static int enemyKilled = 0;
    public static int gameRound = 1;
    public static int totalEnemiesOnMap = 0;
    public static int gunLevel = 0;
}