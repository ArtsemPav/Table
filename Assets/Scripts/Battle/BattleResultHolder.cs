using System.Collections.Generic;
public static class BattleResultHolder
{
    public static bool IsWin;
    public static LevelConfig Level;
    public static int CoinsReward;
    public static int XpReward;
    public static int StarsEarned;
    public static List<TaskRecord> TaskHistory = new List<TaskRecord>();
}
