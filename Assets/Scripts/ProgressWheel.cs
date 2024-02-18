using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressWheel : MonoBehaviour
{
    [SerializeField] Image fillWheel;

    public float duration;

    private float timer;

    private bool hasStarted;

    public System.Action OnCompleteEvent;

    private Vector3 startScale;

    public bool stopProgress = false;

    public bool inverted = false;

    private void Awake()
    {
        startScale = transform.localScale;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (hasStarted && !stopProgress)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                fillWheel.fillAmount = inverted ? 1 - (timer / duration) : timer / duration;
                if(timer >= duration)
                {
                    OnComplete();
                }
            }
        }
    }

    public void StartProgress(float duration, System.Action onCompleteEvent = null)
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(startScale, 0.2f).SetEase(Ease.OutBack);
        hasStarted = true;
        stopProgress = false;
        timer = 0;
        this.duration = duration;
        this.OnCompleteEvent += onCompleteEvent;
    }

    public void StopProgress()
    {
        timer = 0;
        transform.DOComplete();
        gameObject.SetActive(false);
        stopProgress = true;
    }

    public void OnComplete()
    {
        hasStarted = false;
        timer = 0;
        transform.DOScale(0, 0.2f).SetDelay(0.1f).OnComplete(() =>
        {
            OnCompleteEvent?.Invoke();
            OnCompleteEvent = null;
            gameObject.SetActive(false);
        });
    }
}
