using System.Collections.Generic;

public enum CatchRate
{
    Common,
    Uncommon,
    Rare,
    UltraRare
}

[System.Serializable]
public class CatchEntry
{
    public CatchRate CatchRate;
    public List<CritterData> Critters;
}