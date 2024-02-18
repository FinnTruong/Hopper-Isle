using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Pulse : MonoBehaviour
{
    public float duration = 0.3f;
    public float targetScale;

    public bool ignoreTimeScale = false;
    void Start()
    {
        transform.DOScale(transform.localScale * targetScale, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(UpdateType.Normal,ignoreTimeScale);
    }


}
