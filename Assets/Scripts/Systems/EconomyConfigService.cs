using UnityEngine;

public static class EconomyConfigService
{
    private const string DefaultPath = "GameData/EconomyConfig";
    private static EconomyConfig cached;

    public static EconomyConfig Get()
    {
        if (cached != null) return cached;

        cached = Resources.Load<EconomyConfig>(DefaultPath);
        return cached;
    }
}
