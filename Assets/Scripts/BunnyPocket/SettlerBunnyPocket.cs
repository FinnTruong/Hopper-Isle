using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SettlerBunnyPocket : Pocket, IPointerClickHandler
{
    public GamePhase CurrentGamePhase => GameManager.Instance.CurrentGamePhase;
    public int foodPrice => GameManager.Instance.priceConfig.GetPriceConfig(CurrentGamePhase).settlerPrice;

    [SerializeField] private GameObject facesRoot;

    [SerializeField] protected GameObject bunnyPrefab;
    [SerializeField] private GameObject amountLabel;
    [SerializeField] private TMP_Text foodPriceText;
    [SerializeField] private GameObject blockIcon;

    [SerializeField] private TMP_Text limitText;

    [SerializeField] private Button button;

    protected override bool IsBlocked
    {
        get => !(GameManager.Instance.SettlerBunny > 0 || GameManager.Instance.Food >= foodPrice);
    }

    public int SettlerLimit => Mathf.Clamp(Mathf.RoundToInt(HexGridGenerator.Instance.UnlockedTiles * 0.2f + 6) , 6, HexGridGenerator.Instance.TotalTiles);

    public int SettlerOnMap => HexGridGenerator.Instance.tileList.Count(x => x.hasWorker);

    public bool LimitReached => SettlerOnMap >= SettlerLimit;

    private void Start()
    {
        GameManager.Instance.OnFoodChanged += UpdateState;
        GameManager.Instance.OnSettlerBunnyChanged += UpdateState;
        GameManager.Instance.OnRemoveSettler += UpdateState;
        GameManager.Instance.OnGamePhaseChanged += UpdateState;
        UpdateState();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFoodChanged -= UpdateState;
        GameManager.Instance.OnSettlerBunnyChanged -= UpdateState;
        GameManager.Instance.OnRemoveSettler -= UpdateState;
        GameManager.Instance.OnGamePhaseChanged -= UpdateState;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //if (LimitReached)
        //    return;
        base.OnBeginDrag(eventData);
        GameManager.Instance.IsDraggingWorkerBunny = true;
        GameManager.Instance.IsDraggingExplorerBunny = false;
    }
    public override void OnRelease()
    {
        GameManager.Instance.IsDraggingWorkerBunny = false;
        if (Utility.IsPointerOverTile(out Tile tile))
        {
            if (GameManager.Instance.SettlerBunny <= 0 && GameManager.Instance.Food < foodPrice)
                return;

            if (tile.hasWorker || tile.state != TileState.HasUnlocked)
                return;

            tile.AssignWorker(bunnyPrefab);
            if (GameManager.Instance.SettlerBunny > 0)
            {
                GameManager.Instance.SettlerBunny--;
            }
            else
            {
                GameManager.Instance.Food -= foodPrice;
            }
        }
    }

    private void UpdatePrice()
    {
        //Update New Price
        UpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        button.interactable = !IsBlocked;
        foodPriceText.SetText(foodPrice.ToString());
        //foodPriceText.color = GameManager.Instance.Food >= foodPrice ? Color.black : Color.red;
        blockIcon.SetActive(IsBlocked);
        amountLabel.SetActive(GameManager.Instance.SettlerBunny > 0);
        //limitText.SetText($"{SettlerOnMap}/{SettlerLimit}");
        //limitText.color = LimitReached ? Color.green : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.Food >= foodPrice)
        {
            enterTime = Time.time;
            GameManager.Instance.SettlerBunny++;
            GameManager.Instance.Food -= foodPrice;
            AudioManager.Instance.PlaySFX("Purchase");
        }
    }
}
