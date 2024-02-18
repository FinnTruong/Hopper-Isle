using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoPopup : MonoBehaviour
{
    [SerializeField] private GameObject baseTitle;
    [SerializeField] private GameObject bonusResourceTitle;
    [SerializeField] private GameObject bonusResourceDescription;
    [SerializeField] private TMP_Text foodOutputText;
    [SerializeField] private TMP_Text foodBonusText;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text durationBonusText;

    public Tile parentTile;

    public RectTransform layoutRoot;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void ShowTileInfoPopup(bool flag)
    {
        if (parentTile == null)
            return;

        baseTitle.SetActive(!parentTile.hasBonusResources);
        bonusResourceTitle.SetActive(parentTile.hasBonusResources);

        bonusResourceDescription.SetActive(parentTile.hasBonusResources);

        //Calculate Reward
        parentTile.CalculateBonus();
        foodOutputText.SetText(parentTile.FinalFoodOutput.ToString());
        foodBonusText.gameObject.SetActive(parentTile.TotalBonusFood > 0);
        foodBonusText.SetText($"(+{parentTile.TotalBonusFood})");
        durationText.SetText($"{parentTile.FinalDuration}s");
        durationBonusText.gameObject.SetActive(parentTile.TotalDurationDeduction > 0);
        durationBonusText.SetText($"(-{parentTile.TotalDurationDeduction}s)");

        if (flag)
        {

            gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
            gameObject.transform.DOComplete();
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
}
