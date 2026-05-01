using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProfileData
{
    public int schemaVersion = 1;
    public int softCurrency = 0;
    public int bestScore = 0;
    public string playerName = "Player";

    public string equippedShipId = "ship_default";
    public string equippedColorHex = "#FFFFFFFF";

    public List<string> unlockedShipIds = new() { "ship_default" };
    public List<string> unlockedColorHexes = new() { "#FFFFFFFF" };

    public List<RankEntryData> localRanking = new();

    public string missionDateIsoUtc = "";
    public int missionProgressScore = 0;
    public bool missionClaimed = false;

}

[Serializable]
public class RankEntryData
{
    public string playerName;
    public int score;
    public string createdAtIsoUtc;
}
