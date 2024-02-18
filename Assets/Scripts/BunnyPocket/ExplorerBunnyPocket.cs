using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ExplorerBunnyPocket : Pocket, IPointerClickHandler
{
    public GamePhase CurrentGamePhase => GameManager.Instance.CurrentGamePhase;
    public int foodPrice => GameManager.Instance.priceConfig.GetPriceConfig(CurrentGamePhase).explorerPrice;
    public int bunnyPrice = 1;

    [SerializeField] private GameObject facesRoot;

    [SerializeField] private GameObject amountLabel;
    [SerializeField] private TMP_Text foodPriceText;
    [SerializeField] private TMP_Text bunnyPriceText;
    [SerializeField] private GameObject blockIcon;
    [SerializeField] private Button button;

    private GameManager gameManager => GameManager.Instance;
    protected override bool IsBlocked => !(gameManager.ExplorerBunny > 0 
        || (GameManager.Instance.Food >= foodPrice && gameManager.SettlerBunny >= bunnyPrice));

    private void Start()
    {
        GameManager.Instance.OnFoodChanged += UpdateState;
        GameManager.Instance.OnSettlerBunnyChanged += UpdateState;
        GameManager.Instance.OnExplorerBunnyChanged += UpdateState;
        GameManager.Instance.OnGamePhaseChanged += UpdateState;
        GameManager.Instance.OnFirstCollectFood += Unlock;

        UpdateState();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFoodChanged -= UpdateState;
        GameManager.Instance.OnSettlerBunnyChanged -= UpdateState;
        GameManager.Instance.OnExplorerBunnyChanged -= UpdateState;
        GameManager.Instance.OnGamePhaseChanged -= UpdateState;
        GameManager.Instance.OnFirstCollectFood -= Unlock;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        GameManager.Instance.IsDraggingWorkerBunny = false;
        GameManager.Instance.IsDraggingExplorerBunny = true;
    }
    public override void OnRelease()
    {
        GameManager.Instance.IsDraggingExplorerBunny = false;
        base.OnRelease();
        if (Utility.IsPointerOverTile(out Tile tile))
        {
            if (IsBlocked)
                return;


            if (tile.state == TileState.CanUnlock)
            {
                tile.UnlockNeighborTiles();
                tile.ShowHoverHighlight(false);
                if (GameManager.Instance.ExplorerBunny > 0)
                {
                    GameManager.Instance.ExplorerBunny--;
                }
                else
                {
                    GameManager.Instance.Food -= foodPrice;
                    GameManager.Instance.SettlerBunny -= bunnyPrice;
                }

            }
        }
    }

    private void UpdatePrice()
    {
        UpdatePrice();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        button.interactable = !IsBlocked;
        foodPriceText.SetText(foodPrice.ToString());
        //foodPriceText.color = GameManager.Instance.Food >= foodPrice ? Color.black : Color.red;
        bunnyPriceText.SetText(bunnyPrice.ToString());
        //bunnyPriceText.color = GameManager.Instance.SettlerBunny >= bunnyPrice ? Color.black : Color.red;
        blockIcon.SetActive(IsBlocked);
        amountLabel.SetActive(GameManager.Instance.ExplorerBunny > 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!hasUnlocked)
            return;
        if (GameManager.Instance.Food >= foodPrice && GameManager.Instance.SettlerBunny >= bunnyPrice)
        {
            enterTime = Time.time;
            GameManager.Instance.ExplorerBunny++;
            GameManager.Instance.SettlerBunny -= bunnyPrice;
            GameManager.Instance.Food -= foodPrice;
            AudioManager.Instance.PlaySFX("Purchase");
        }
    }

    public void Unlock()
    {
        facesRoot.transform.DOScaleX(1, 0.25f).SetEase(Ease.OutQuad);
        hasUnlocked = true;
    }
}
