using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public struct RevealRewardConfigData
{
    public TileRewardType rewardType;
    public float weight;
}


[CreateAssetMenu(fileName = "RevealRewardConfig", menuName = "Config/RevealRewardConfig")]
public class RevealRewardConfig : ScriptableObject
{
    public List<RevealRewardConfigData> Collection = new();

    public float TotalWeight => Collection.Sum(x => x.weight);

    public TileRewardType GetRandomReward()
    {
        var targetReward = TileRewardType.None;
        var rand = UnityEngine.Random.Range(0, TotalWeight);


        var processedWeight = 0f;
        for (int i = 0; i < Collection.Count; i++)
        {
            processedWeight += Collection[i].weight;
            if (rand <= processedWeight)
            {
                targetReward = Collection[i].rewardType;
                break;
            }
        }
        return targetReward;
    }
}
