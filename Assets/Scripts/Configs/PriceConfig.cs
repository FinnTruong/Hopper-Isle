using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PriceConfigData
{
    public GamePhase gamePhase;
    public int settlerPrice;
    public int explorerPrice;
    public int adventurerPrice;
}


[CreateAssetMenu(fileName = "PriceConfig", menuName = "Config/PriceConfig")]

public class PriceConfig : ScriptableObject
{
    public List<PriceConfigData> dictionary;

    public PriceConfigData GetPriceConfig(GamePhase phase)
    {
        for (int i = 0; i < dictionary.Count; i++)
        {
            if (dictionary[i].gamePhase == phase)
                return dictionary[i];
        }

        return dictionary[0];
    }
}
