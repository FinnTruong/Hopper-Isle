using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimatedSpriteButton : MonoBehaviour
{
    [SerializeField] private bool interactableWhenPause;
    [SerializeField] Color highlightedColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1);
    [SerializeField] Color disabledColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.5f);
    public UnityEvent onClick;
    public SpriteRenderer image;
    public AudioClip[] sfxClick;
    public Vector3 highlightScale = Vector3.one * 0.9f;
    public Sprite highlightSprite;

    private Color baseColor = Color.white;
    private Sprite baseSprite;

    Vector3 onPointerDownScale = Vector3.one;
    public float animateDuration = 0.1f;
    public Ease animatedCurve = Ease.InBack;
    public bool enableVibration = false;
    private bool interactebleValue = true;
    public bool interactable
    {
        get => interactebleValue;

        set
        {
            if (value)
                EnableButton();
            else
                DisableButton();

            interactebleValue = value;
        }
    }

    public float shrinkTime = 0.3f;

    Vector3 startScale;

    public CustomTransitionType transition = CustomTransitionType.None;

    public bool playSound = true;

    public bool isIgnoreAnimate;
    [SerializeField] private bool oneTimeClick = false;

    private void Awake()
    {
        //SceneInstaller.Inject(this);
        if (image == null)
            image = GetComponent<SpriteRenderer>();
        if (image != null)
        {
            startScale = image.transform.localScale == Vector3.zero ? Vector3.one : image.transform.localScale;
        }
        onPointerDownScale = new Vector3(highlightScale.x * startScale.x, highlightScale.y * startScale.y, highlightScale.z * startScale.z);
        if (image != null && transition == CustomTransitionType.Color)
        {
            baseColor = image.color;
            baseSprite = image.sprite;
        }
    }

    private void Start()
    {

    }


    private void OnPauseUpdated(bool paused)
    {
        interactable = !paused;
    }

    private void OnEnable()
    {
        if (oneTimeClick)
        {
            image.color = baseColor;
            interactable = true;
        }
    }
    public void DisableButton()
    {
        if (image != null && transition == CustomTransitionType.Color)
            image.color = disabledColor;
    }

    public void EnableButton()
    {
        if (image != null && transition == CustomTransitionType.Color)
            image.color = baseColor;
    }

    public void OnMouseDown()
    {
        if (Utility.IsPointerOverUIElement())
            return;
        if (!interactable)
            return;
        HighlightButton();

        if (isIgnoreAnimate)
            return;
        transform.DOScale(onPointerDownScale, animateDuration).SetEase(animatedCurve);
    }

    public void OnMouseUp()
    {
        if (Utility.IsPointerOverUIElement())
            return;
        if (!interactable || isIgnoreAnimate)
            return;
        transform.DOScale(startScale, animateDuration)
            .SetEase(animatedCurve)
            .OnComplete(() => ResetButton());
    }

    public void OnMouseExit()
    {
        if (Utility.IsPointerOverUIElement())
            return;
        if (!interactable || isIgnoreAnimate)
            return;
        transform.DOScale(startScale, animateDuration)
            .SetEase(animatedCurve)
            .OnComplete(() => ResetButton());
    }

    private void OnMouseUpAsButton()
    {
        if (Utility.IsPointerOverUIElement())
            return;
        if (!interactable)
            return;
        onClick?.Invoke();
        if (oneTimeClick)
            interactable = false;

    }
    public void HighlightButton()
    {
        if (image != null)
        {
            switch (transition)
            {
                case CustomTransitionType.None:
                    break;
                case CustomTransitionType.Color:
                    image.color = highlightedColor;
                    break;
                case CustomTransitionType.SpriteSwap:
                    if (highlightSprite != null)
                        image.sprite = highlightSprite;
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetButton()
    {
        if (image == null)
            return;

        switch (transition)
        {
            case CustomTransitionType.None:
                break;
            case CustomTransitionType.Color:
                image.color = baseColor;
                break;
            case CustomTransitionType.SpriteSwap:
                image.sprite = baseSprite;
                break;
            default:
                break;
        }
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void Shrink()
    {
        transform.DOScale(Vector3.zero, shrinkTime).SetEase(Ease.InBack);
        //LeanTween.scale(gameObject, Vector3.zero, shrinkTime).setEaseInBack().setIgnoreTimeScale(true);
    }
}
