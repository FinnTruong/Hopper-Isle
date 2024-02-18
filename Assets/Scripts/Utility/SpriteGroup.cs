using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteGroup : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] sprite;
    [Range(0,1)]
    public float alpha = 1;

    
    private void OnValidate()
    {
        SetAlpha(alpha);
    }

    private void GetSprite()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetAlpha(float alpha)
    {
        for (int i = 0; i < sprite.Length; i++)
        {
            var tempColor = sprite[i].color;
            tempColor.a = alpha;
            sprite[i].color = tempColor;
        }
    }

    public void FadeIn(float duration = 1f)
    {
        gameObject.SetActive(true);
        DOVirtual.Float(0, 1, duration, x =>
        {
            SetAlpha(x);
        });
    }

    public void FadeOut(float duration = 1f)
    {
        DOVirtual.Float(1, 0, 1f, x =>
        {
            SetAlpha(x);
        }).OnComplete(() => gameObject.SetActive(false));
    }
}
