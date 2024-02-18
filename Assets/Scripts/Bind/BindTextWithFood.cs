using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class BindTextWithFood : MonoBehaviour
{
    private int oldValue;


    [SerializeField] TMP_Text foodText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnFoodChanged += Refresh;
        Refresh();

        oldValue = GameManager.Instance.Food;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFoodChanged -= Refresh;
    }

    private void Refresh()
    {
        PlayTextEffect(GameManager.Instance.Food);
    }

    Tweener animationTween;
    private void PlayTextEffect(int newValue)
    {
        animationTween?.Kill();
        int.TryParse(foodText.text, out oldValue);
        animationTween = DOVirtual.Int(oldValue, newValue, 0.5f, v =>
        {
            float lerpValue = Mathf.InverseLerp(oldValue, newValue, v);
            foodText.SetText($"{v}");
        }).OnComplete(() =>
        {
            oldValue = newValue;
        });
    }
}
