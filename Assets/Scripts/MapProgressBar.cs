using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class MapProgressBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] GameObject fillTip;
    [SerializeField] Transform startPos, endPos;
    [SerializeField] TMP_Text progressText;

    [Range(0, 1)] public float firstMilestone = 0.25f;
    [Range(0, 1)] public float secondMilestone = 0.5f;
    [Range(0, 1)] public float thirdMilestone = 0.75f;

    bool hasClearedFirstMilestone;
    bool hasClearedSecondMilestone;
    bool hasClearedThirdMilestone;
    private float currentProgress => HexGridGenerator.Instance.MapProgress;
    private float lastProgress = 0;
    private GameManager gameManager => GameManager.Instance;

    private void Start()
    {
        GameManager.Instance.OnUnlockNewTile += UpdateState;
        GameManager.Instance.OnClearedSecondMilestone += CreateSpecialTiles;
        lastProgress = HexGridGenerator.Instance.MapProgress;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnUnlockNewTile -= UpdateState;
        GameManager.Instance.OnClearedSecondMilestone -= CreateSpecialTiles;
    }
    Tweener percentageValue;
    private void UpdateState()
    {
        percentageValue?.Kill();
        percentageValue = DOVirtual.Float(lastProgress, currentProgress, 0.5f, v =>
        {
            var percentageValue = Mathf.RoundToInt(v * 1000f);
            progressText.SetText($"{percentageValue/10f}%");
        });
        //progressText.SetText(HexGridGenerator.Instance.MapProgress.ToString());
        fill.DOKill();
        fill.DOFillAmount(currentProgress, 1f).SetEase(Ease.OutBack);
        fillTip.transform.DOKill();
        fillTip.transform.DOMove(Vector2.Lerp(startPos.position, endPos.position, currentProgress), 1f).SetEase(Ease.OutBack);
        lastProgress = currentProgress;

        if(currentProgress >= firstMilestone && !hasClearedFirstMilestone)
        {
            Debug.Log("Cleared First");
            hasClearedFirstMilestone = true;
            GameManager.Instance.OnClearedFirstMilestone?.Invoke();
            GameManager.Instance.CurrentGamePhase = GamePhase.PhaseTwo;
        }

        if(currentProgress >= secondMilestone && !hasClearedSecondMilestone)
        {
            Debug.Log("Cleared Second");
            hasClearedSecondMilestone = true;
            gameManager.OnClearedSecondMilestone?.Invoke();
            GameManager.Instance.CurrentGamePhase = GamePhase.PhaseThree;
        }

        if(currentProgress >= thirdMilestone && !hasClearedThirdMilestone)
        {
            Debug.Log("Cleared Second");
            hasClearedThirdMilestone = true;
            gameManager.OnClearedThirdMilestone?.Invoke();
            GameManager.Instance.CurrentGamePhase = GamePhase.PhaseFour;
        }
    }

    public void CreateSpecialTiles()
    {
        for (int i = 0; i < 2; i++)
        {
            HexGridGenerator.Instance.CreateSpecialTile();
        }
    }
}
