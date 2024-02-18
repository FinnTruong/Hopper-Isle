using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CollectRewardEffect : MonoBehaviour
{
    private Vector3 startPos;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text rewardAmountText;
    [SerializeField] GameObject foodIcon, settlerIcon, explorerIcon, adventurerIcon;


    private void Awake()
    {
        startPos = transform.localPosition;
    }
    public void PlayEffect(ResourceType reward, int amount)
    {
        foodIcon.SetActive(reward == ResourceType.Food);
        settlerIcon.SetActive(reward == ResourceType.Settler);
        explorerIcon.SetActive(reward == ResourceType.Explorer);
        adventurerIcon.SetActive(reward == ResourceType.Adventurer);
        rewardAmountText.SetText($"+{amount}");
        gameObject.SetActive(true);
        PlayMoveSequence();
        PlayFadeSequence();
    }

    Sequence moveSequence;
    private void PlayMoveSequence()
    {
        moveSequence?.Complete();
        transform.localPosition = startPos;
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOLocalMoveY(startPos.y + 250f, 3f));
    }

    Sequence fadeSequence;
    private void PlayFadeSequence()
    {
        fadeSequence?.Complete();
        canvasGroup.alpha = 0;
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(1, 0.25f));
        fadeSequence.AppendInterval(0.5f);
        fadeSequence.Append(canvasGroup.DOFade(0, 0.5f));
    }
}
