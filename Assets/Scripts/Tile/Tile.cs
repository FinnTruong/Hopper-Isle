using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public List<Tile> neighborTiles;
    [HideInInspector]
    public Vector2Int coordinate;

    [SerializeField] private GameObject hoverHighlight;
    [SerializeField] private GameObject clickHighlight;
    [SerializeField] private TileInfoPopup tileInfoPopup;

    public CollectRewardEffect collectRewardEffect;

    public TileType tileType;
    public TileState state;

    [SerializeField] SpriteGroup spriteGroup;
    [SerializeField] Transform facesRoot;

    public System.Action OnUnlock;
    public System.Action OnAssignWorker;

    public System.Action OnUpdateState;


    [SerializeField] private ProgressWheel progressWheel;
    [SerializeField] private ProgressWheel cooldownWheel;
    [SerializeField] private GameObject notiBubble;

    public float BaseDuration => GameManager.Instance.tileConfig.GetTileConfig(tileType).baseDuration;
    public int BaseFoodOutput => GameManager.Instance.tileConfig.GetTileConfig(tileType).baseFoodOutput;

    public float baseCooldownDuration = 2f;

    public int FinalFoodOutput => BaseFoodOutput + grassAdjacencyFoodBonus + desertAdjacencyFoodBonus + oceanAdjacencyFoodBonus;
    public float FinalDuration => Mathf.Clamp(BaseDuration - TotalDurationDeduction, 0, BaseDuration);

    public bool hasWorker;
    private bool isWorking;
    private bool isOnCooldown;
    private bool hasCompleteWork;

    [SerializeField] private GameObject bonusResourceIcon;
    public bool hasBonusResources;

    public bool CanAssignWorker => state == TileState.HasUnlocked && !hasWorker;

    public bool isSpecialTile;
    public GameObject specialTileBack;

    public void CreateTile(Vector2Int coord, bool forceBonusResources = false)
    {
        coordinate = coord;
        neighborTiles = new List<Tile>();
        state = TileState.Hidden;
        //state = TileState.HasUnlocked;
        UpdateState();

        hasBonusResources = Random.Range(0, 10) >= 6;
        if (forceBonusResources)
            hasBonusResources = true;

        if (bonusResourceIcon != null)
            bonusResourceIcon.SetActive(hasBonusResources);
    }

    Sequence unlockSequence;
    public void UnlockTile(bool hasReward = true)
    {
        if (state == TileState.HasUnlocked)
            return;
        OnUnlock?.Invoke();
        unlockSequence?.Complete();
        unlockSequence = DOTween.Sequence();
        unlockSequence.Append(facesRoot.transform.DOScaleX(1, 0.2f).SetEase(Ease.OutQuad));
        unlockSequence.AppendCallback(() =>
        {
            if (hasReward)
                CollectRevealReward(GameManager.Instance.tileRewardConfig.GetRandomReward());
            UpdateState();
        });
        state = TileState.HasUnlocked;
        foreach (var tile in neighborTiles)
        {
            if (tile.state == TileState.Hidden)
            {
                tile.ShowTile();  
            }                
        }

        if (GameManager.Instance != null)
            GameManager.Instance.OnUnlockNewTile?.Invoke();
    }

    public void UnlockNeighborTiles()
    {
        AudioManager.Instance.PlaySFX("RevealTile", 1f);
        foreach (var tile in neighborTiles)
        {
            if (tile.state == TileState.CanUnlock)
                tile.UnlockTile();
        }
        UnlockTile();
    }

    public void UnlockAllAdjacentTiles()
    {
        AudioManager.Instance.PlaySFX("RevealTile", 1f);
        UnlockTile();
        foreach (var tile in neighborTiles)
        {
            tile.UnlockTile();
            foreach (var tile2 in tile.neighborTiles)
            {
                tile2.UnlockTile();
            }

        }
    }



    Sequence appearSequence;
    public void ShowTile()
    {
        Vector2 startScale = transform.localScale;
        transform.localScale = Vector2.zero;
        state = TileState.CanUnlock;
        appearSequence?.Complete();
        appearSequence = DOTween.Sequence();
        appearSequence.AppendInterval(0.1f);
        appearSequence.AppendCallback(()=>spriteGroup.FadeIn(0.3f));
        appearSequence.Append(transform.DOScale(startScale, 0.2f).SetEase(Ease.OutBack));
        appearSequence.AppendCallback(() => UpdateState());


    }



    public void UpdateState()
    {
        switch (state)
        {
            case TileState.Hidden:
                gameObject.SetActive(false);
                break;
            case TileState.CanUnlock:
                gameObject.SetActive(true);
                facesRoot.localScale = new Vector2(-1,1);
                break;
            case TileState.HasUnlocked:
                gameObject.SetActive(true);
                facesRoot.localScale = Vector2.one;
                gameObject.transform.localScale = Vector3.one;
                break;
            default:
                break;
        }

        specialTileBack.SetActive(isSpecialTile);
    }
    
    public void ShowInfoPopup(bool flag)
    {
        if (tileInfoPopup == null)
            return;
        tileInfoPopup.ShowTileInfoPopup(flag);        
            
    }
    private void OnMouseEnter()
    {
        if(GameManager.Instance.IsDraggingWorkerBunny && CanAssignWorker)
        {
            ShowHoverHighlight(true);
            ShowInfoPopup(true);
        }

        if(GameManager.Instance.IsDraggingExplorerBunny && state == TileState.CanUnlock)
        {
            ShowHoverHighlight(true);
        }

        if(GameManager.Instance.IsDraggingShovel && hasWorker)
        {
            ShowHoverHighlight(true);
        }
    }

    bool hasClicked;
    float clickTime;
    private void Update()
    {
        if(hasClicked && state == TileState.HasUnlocked)
        {
            if(Time.time - clickTime >= 0.5f)
            {

                ShowInfoPopup(true);
                hasClicked = false;
            }
        }
    }

    private void OnMouseExit()
    {
        ShowHoverHighlight(false);
        ShowClickHighlight(false);
        ShowInfoPopup(false);
    }

    private void OnMouseDown()
    {
        hasClicked = true;
        clickTime = Time.time;
        ShowHoverHighlight(false);
        ShowClickHighlight(true);
    }
    private void OnMouseUp()
    {
        hasClicked = false;
        ShowInfoPopup(false);
    }
    private void OnMouseUpAsButton()
    {
        ShowHoverHighlight(false);
        ShowClickHighlight(false);

#if UNITY_EDITOR
        if (state == TileState.CanUnlock)
            UnlockAllAdjacentTiles();
#endif


        if (hasCompleteWork)
        {
            CollectFood();
        }

        ShowInfoPopup(false);
    }    

    public void ShowHoverHighlight(bool flag)
    {
        if (hoverHighlight == null)
            return;

        hoverHighlight.SetActive(flag);
    }

    public void ShowClickHighlight(bool flag)
    {
        if (clickHighlight == null) 
            return;

        clickHighlight.SetActive(flag);
    }

    GameObject worker;

    public AudioClip[] onAssignWorkerSFX;
    public void AssignWorker(GameObject bunnyPrefab)
    {
        if (hasWorker)
            return;
        if (state != TileState.HasUnlocked)
            return;

        AudioManager.Instance.PlaySFX(onAssignWorkerSFX.GetRandomElement(), 1f);
        hasWorker = true;
        worker = Instantiate(bunnyPrefab, transform);
        StartWorking();
        ShowHoverHighlight(false);
        ShowClickHighlight(false);
        ShowInfoPopup(false);
    }

    public void RemoveSettler()
    {
        if (!hasWorker)
            return;
        hasWorker = false;
        isWorking = false;
        isOnCooldown = false;
        hasCompleteWork = false;
        progressWheel.StopProgress();
        cooldownWheel.StopProgress();
        notiBubble.SetActive(false);
        ShowHoverHighlight(false);
        ShowClickHighlight(false);
        GameManager.Instance.SettlerBunny++;
        GameManager.Instance.OnRemoveSettler?.Invoke();
        if (worker != null)
            Destroy(worker);
        AudioManager.Instance.PlaySFX("RemoveSettler", 1f);
    }
    public virtual void StartWorking()
    {
        if (isWorking || isOnCooldown || !hasWorker)
            return;
        isWorking = true;
        progressWheel.gameObject.SetActive(true);
        progressWheel.StartProgress(FinalDuration, () =>
        {
            hasCompleteWork = true;
            isWorking = false;
            notiBubble.SetActive(true);
            notiBubble.transform.localScale = Vector3.zero;
            notiBubble.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        });
    }

    public virtual void StartCooldown()
    {
        if (!hasCompleteWork || !hasWorker)
            return;
        isOnCooldown = true;
        cooldownWheel.gameObject.SetActive(true);
        cooldownWheel.StartProgress(baseCooldownDuration, () =>
        {
            isOnCooldown = false;
            StartWorking();
        });
    }

    public int TotalBonusFood => grassAdjacencyFoodBonus + desertAdjacencyFoodBonus + oceanAdjacencyFoodBonus;
    public float TotalDurationDeduction => grassDurationDeduction + desertDurationDeduction + oceanDurationDeduction;

    public int grassAdjacencyFoodBonus;
    public int desertAdjacencyFoodBonus;
    public int oceanAdjacencyFoodBonus;

    public float grassDurationDeduction;
    public float desertDurationDeduction;
    public float oceanDurationDeduction;
    public void CalculateBonus()
    {
        ResetFoodBonus();
        CalculateGrassBonus();
        CalculateSandBonus();
        CalculateOceanBonus();
    }

    private void CalculateGrassBonus()
    {
        grassAdjacencyFoodBonus = 0;
        int doubleBonus = GameManager.Instance.DoubleAdjacencyBonus ? 2 : 1;

        if (tileType != TileType.Grass || !hasBonusResources)
            return;

        foreach (var tile in neighborTiles)
        {
            if (tile.hasWorker)
                grassAdjacencyFoodBonus += tile.BaseFoodOutput * doubleBonus;
        }
    }

    private void CalculateSandBonus()
    {
        int doubleBonus = GameManager.Instance.DoubleAdjacencyBonus ? 2 : 1;
        desertAdjacencyFoodBonus = 0;
        foreach (var tile in neighborTiles)
        {
            if (tile.hasWorker && tile.hasBonusResources && tile.tileType == TileType.Sand)
                desertAdjacencyFoodBonus += tile.BaseFoodOutput * doubleBonus;
        }
    }

    private void CalculateOceanBonus()
    {
        int doubleBonus = GameManager.Instance.DoubleAdjacencyBonus ? 2 : 1;
        oceanDurationDeduction = 0;
        foreach (var tile in neighborTiles)
        {
            if(tile.hasWorker && tile.hasBonusResources && tile.tileType == TileType.Ocean)
            {
                oceanDurationDeduction += BaseDuration * 0.1f * doubleBonus;
            }
        }

        oceanDurationDeduction = Mathf.Clamp(oceanDurationDeduction, 0, BaseDuration);
    }

    private void ResetFoodBonus()
    {
        grassAdjacencyFoodBonus = 0;
        desertAdjacencyFoodBonus = 0;
        oceanAdjacencyFoodBonus = 0;
    }
    public virtual void CollectFood()
    {
        //Play Effect
        CalculateBonus();
        GameManager.Instance.Food += FinalFoodOutput;
        StartCooldown();
        hasCompleteWork = false;
        notiBubble.transform.DOScale(0, 0.2f);
        collectRewardEffect.PlayEffect(ResourceType.Food, FinalFoodOutput);
        if(!GameManager.Instance.HasCollectedFood)
        {
            GameManager.Instance.HasCollectedFood = true;
            GameManager.Instance.OnFirstCollectFood?.Invoke();
        }
        AudioManager.Instance.PlaySFX("Harvest", 0.85f);
    }

    private void CollectRevealReward(TileRewardType rewardType)
    {
        if(isSpecialTile)
        {
            if(GameManager.Instance.DoubleAdjacencyBonus)
            {
                GameManager.Instance.DoubleRevealReward = true;
            }
            else if(GameManager.Instance.DoubleRevealReward)
            {
                GameManager.Instance.DoubleAdjacencyBonus = true;
            }
            else
            {
                var rand = Random.Range(0, 2);
                if (rand == 0)
                    GameManager.Instance.DoubleRevealReward = true;
                else
                    GameManager.Instance.DoubleAdjacencyBonus = true;
            }

            return;
        }
        CalculateBonus();
        var currentGamePhase = GameManager.Instance.CurrentGamePhase;
        int doubleRewardModifier = GameManager.Instance.DoubleRevealReward ? 2 : 1;
        switch (rewardType)
        {
            case TileRewardType.None:
                Debug.Log("None");
                break;
            case TileRewardType.SmallFoodRation:
                int smallFoodAmount = GameManager.Instance.priceConfig.GetPriceConfig(currentGamePhase).settlerPrice / 4;
                collectRewardEffect.PlayEffect(ResourceType.Food, smallFoodAmount);
                GameManager.Instance.Food += smallFoodAmount * doubleRewardModifier;
                Debug.Log("SmallFoodRation");
                break;
            case TileRewardType.LargeFoodRation:
                int largeFoodAmount = GameManager.Instance.priceConfig.GetPriceConfig(currentGamePhase).settlerPrice;
                collectRewardEffect.PlayEffect(ResourceType.Food, largeFoodAmount);
                GameManager.Instance.Food += largeFoodAmount * doubleRewardModifier;
                Debug.Log("LargeFoodRation");
                break;
            case TileRewardType.Settler:
                GameManager.Instance.SettlerBunny+= doubleRewardModifier;
                collectRewardEffect.PlayEffect(ResourceType.Settler, 1);
                Debug.Log("Settler");
                break;
            case TileRewardType.Explorer:
                GameManager.Instance.ExplorerBunny += doubleRewardModifier;
                collectRewardEffect.PlayEffect(ResourceType.Explorer, 1);
                Debug.Log("Explorer");
                break;
            case TileRewardType.Adventurer:
                //if(!hasUnlockedAdventurer) => Convert to explorer
                GameManager.Instance.AdventurerBunny += doubleRewardModifier;
                collectRewardEffect.PlayEffect(ResourceType.Adventurer, 1);
                Debug.Log("Adventurer");
                break;
            default:
                break;
        }

    }
}
