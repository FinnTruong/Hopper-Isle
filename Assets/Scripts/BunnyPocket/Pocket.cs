using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pocket : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] protected GameObject preview;
    //[SerializeField] protected GameObject toolTip;
    //[SerializeField] private float toolTipDelay;

    protected bool isDragging;
    protected bool hasEnter;
    protected float enterTime;

    public bool hasUnlocked;

    protected virtual bool IsBlocked { get; set; }

    private void Update()
    {
        //if(Time.time - enterTime > toolTipDelay && hasEnter && !isDragging)
        //{
        //    ShowTooltip();
        //}
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!hasUnlocked)
            return;
        if (IsBlocked)
            return;
        isDragging = true;
        preview.SetActive(true);
        preview.transform.position = Input.mousePosition;
        CameraController.Instance.IsDragging = true;
        //toolTip.SetActive(false);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!hasUnlocked)
            return;
        if (IsBlocked)
            return;
        preview.transform.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!hasUnlocked)
            return;
        if (IsBlocked)
            return;
        isDragging = false;
        preview.SetActive(false);
        CameraController.Instance.IsDragging = false;
        OnRelease();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasUnlocked)
            return;
        enterTime = Time.time;
        hasEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hasUnlocked)
            return;
        //if (toolTip != null)
        //    toolTip.SetActive(false);
        hasEnter = false;
    }

    private void ShowTooltip()
    {
        //if (toolTip != null)
        //    toolTip.SetActive(true);
    }


    public virtual void OnRelease() { }

    public virtual void UpdateState() { }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!hasUnlocked || IsBlocked)
            return;
        AudioManager.Instance.PlaySFX("StartDrag", 1f);
    }
}
