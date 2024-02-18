using BitBenderGames;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public bool IsOverUI => Utility.IsPointerOverUIElement();

    public bool IsDragging = false;
    private bool CanDrag => !IsOverUI && !IsDragging;

    [SerializeField] private TouchInputController touchInputController;

    private Vector3 startPos;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        startPos = transform.position;
    }

    private void Update()
    {
        touchInputController.IsInputOnLockedArea = !CanDrag;
    }

    public void CenterCamera()
    {
        transform.DOKill();
        transform.DOMove(startPos, 0.5f).SetEase(Ease.InOutSine);
    }
}
