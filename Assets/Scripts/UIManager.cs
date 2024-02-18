using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PopupDoubleAdjacency;
    [SerializeField] GameObject PopupDoubleRevealReward;

    private void Start()
    {
        GameManager.Instance.OnUnlockDoubleAdjacencyBonus += ShowDoubleAdjacencyPopup;
        GameManager.Instance.OnUnlockDoublRevealRewardBonus += ShowDoubleRevealRewardPopup;
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.OnUnlockDoubleAdjacencyBonus -= ShowDoubleAdjacencyPopup;
        GameManager.Instance.OnUnlockDoublRevealRewardBonus -= ShowDoubleRevealRewardPopup;
    }

    public void ShowDoubleAdjacencyPopup()
    {
        PopupDoubleAdjacency.SetActive(true);
    }


    public void ShowDoubleRevealRewardPopup()
    {
        PopupDoubleRevealReward.SetActive(true);
    }
}
