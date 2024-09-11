using System.Collections.Generic;


[System.Serializable]
public class JSONMapData
{
    public Dictionary<string, JSONTileData> map { get; set; }
    public string rows { get; set; }
    public string cols { get; set; }
}
