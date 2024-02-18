using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TileConfigData
{
    public TileType tileType;
    public int baseFoodOutput;
    public float baseDuration;
    public int bonusFoodOutput;
    public float bonusDurationBoost;
}

[CreateAssetMenu(fileName = "TileConfig", menuName = "Config/TileConfig")]
public class TileConfig : ScriptableObject
{
    public List<TileConfigData> collection;

    public TileConfigData GetTileConfig(TileType type)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            if (collection[i].tileType == type)
                return collection[i];
        }

        return collection[0];
    }
}
