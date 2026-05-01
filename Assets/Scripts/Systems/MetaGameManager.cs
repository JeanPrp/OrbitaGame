using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetaGameManager : MonoBehaviour
{
    public static MetaGameManager Instance { get; private set; }

    [Header("Catalog")]
    [SerializeField] private ShipDefinition[] shipCatalog;
    [SerializeField] private string resourcesShipCatalogPath = "GameData/Ships";
    [SerializeField] private int maxLocalRankEntries = 20;

    private readonly Dictionary<string, ShipDefinition> shipById = new();
    private PlayerProfileData profile;

    public PlayerProfileData Profile => profile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AutoLoadCatalogIfNeeded();
        BuildShipIndex();
        profile = PlayerProfileService.LoadOrCreate();
        EnsureProfileConsistency();
        DailyMissionService.EnsureDailyState(profile);
        PlayerProfileService.Save(profile);
    }

    public IReadOnlyList<ShipDefinition> GetShipCatalog()
    {
        return shipCatalog;
    }


    public void RegisterRunForDailyMission(int score)
    {
        DailyMissionService.RegisterRun(profile, score);
        PlayerProfileService.Save(profile);
    }

    public bool TryClaimDailyMissionReward(out int reward)
    {
        bool claimed = DailyMissionService.TryClaim(profile, out reward);
        if (claimed)
        {
            profile.softCurrency += reward;
            PlayerProfileService.Save(profile);
        }

        return claimed;
    }

    public bool TryUnlockShip(string shipId)
    {
        if (!shipById.TryGetValue(shipId, out ShipDefinition ship)) return false;
        if (profile.unlockedShipIds.Contains(shipId)) return true;
        if (profile.softCurrency < ship.UnlockPrice) return false;

        profile.softCurrency -= ship.UnlockPrice;
        profile.unlockedShipIds.Add(shipId);
        PlayerProfileService.Save(profile);
        return true;
    }

    public bool EquipShip(string shipId)
    {
        if (!profile.unlockedShipIds.Contains(shipId)) return false;
        profile.equippedShipId = shipId;
        PlayerProfileService.Save(profile);
        return true;
    }

    public bool UnlockColor(Color color, int price)
    {
        string hex = ColorUtility.ToHtmlStringRGBA(color);
        string key = $"#{hex}";

        if (profile.unlockedColorHexes.Contains(key)) return true;
        if (profile.softCurrency < price) return false;

        profile.softCurrency -= price;
        profile.unlockedColorHexes.Add(key);
        PlayerProfileService.Save(profile);
        return true;
    }

    public bool EquipColor(Color color)
    {
        string key = $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        if (!profile.unlockedColorHexes.Contains(key)) return false;

        profile.equippedColorHex = key;
        PlayerProfileService.Save(profile);
        return true;
    }

    public void AddCurrency(int amount)
    {
        profile.softCurrency = Mathf.Max(0, profile.softCurrency + amount);
        PlayerProfileService.Save(profile);
    }


    public void SetPlayerName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) return;

        profile.playerName = newName.Trim();
        PlayerProfileService.Save(profile);
    }

    public void RegisterRunScore(string playerName, int score)
    {
        profile.bestScore = Mathf.Max(profile.bestScore, score);

        RankEntryData entry = new RankEntryData
        {
            playerName = string.IsNullOrWhiteSpace(playerName) ? profile.playerName : playerName,
            score = score,
            createdAtIsoUtc = DateTime.UtcNow.ToString("o")
        };

        profile.localRanking.Add(entry);
        profile.localRanking = profile.localRanking
            .OrderByDescending(r => r.score)
            .ThenBy(r => r.createdAtIsoUtc)
            .Take(Mathf.Max(1, maxLocalRankEntries))
            .ToList();

        PlayerProfileService.Save(profile);
    }


    private void AutoLoadCatalogIfNeeded()
    {
        if (shipCatalog != null && shipCatalog.Length > 0) return;

        ShipDefinition[] loaded = Resources.LoadAll<ShipDefinition>(resourcesShipCatalogPath);
        if (loaded != null && loaded.Length > 0)
            shipCatalog = loaded;
    }
    private void BuildShipIndex()
    {
        shipById.Clear();

        if (shipCatalog == null) return;

        foreach (ShipDefinition ship in shipCatalog)
        {
            if (ship == null || string.IsNullOrWhiteSpace(ship.ShipId)) continue;
            shipById[ship.ShipId] = ship;
        }
    }

    private void EnsureProfileConsistency()
    {
        if (profile.unlockedShipIds == null) profile.unlockedShipIds = new List<string>();
        if (profile.unlockedColorHexes == null) profile.unlockedColorHexes = new List<string>();
        if (profile.localRanking == null) profile.localRanking = new List<RankEntryData>();

        if (!profile.unlockedShipIds.Contains("ship_default"))
            profile.unlockedShipIds.Insert(0, "ship_default");

        if (!profile.unlockedColorHexes.Contains("#FFFFFFFF"))
            profile.unlockedColorHexes.Insert(0, "#FFFFFFFF");

        if (string.IsNullOrWhiteSpace(profile.playerName))
            profile.playerName = "Player";

        if (string.IsNullOrWhiteSpace(profile.equippedShipId))
            profile.equippedShipId = "ship_default";

        if (string.IsNullOrWhiteSpace(profile.equippedColorHex))
            profile.equippedColorHex = "#FFFFFFFF";
    }
}
