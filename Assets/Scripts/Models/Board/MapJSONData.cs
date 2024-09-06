using System.Collections.Generic;

[System.Serializable]
public class MapJSONData
{
    public Dictionary<string, CellData> map;
    public int rows;
    public int cols;
}