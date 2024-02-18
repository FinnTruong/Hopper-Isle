using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public System.Action OnFoodChanged;
    public System.Action OnSettlerBunnyChanged;
    public System.Action OnExplorerBunnyChanged;
    public System.Action OnAdventurerBunnyChanged;
    public System.Action OnUnlockNewTile;
    public System.Action OnClearedFirstMilestone;
    public System.Action OnClearedSecondMilestone;
    public System.Action OnClearedThirdMilestone;
    public System.Action OnFirstCollectFood;

    public System.Action OnRemoveSettler;

    public System.Action OnUnlockDoubleAdjacencyBonus;
    public System.Action OnUnlockDoublRevealRewardBonus;


    private int food;
    public int Food
    {
        get => food;
        set
        {
            food = Mathf.Clamp(value, 0, int.MaxValue);
            OnFoodChanged?.Invoke();
        }
    }


    private int settlerBunny;
    public int SettlerBunny 
    { 
        get => settlerBunny;
        set
        {
            settlerBunny = Mathf.Clamp(value, 0, int.MaxValue);
            OnSettlerBunnyChanged?.Invoke();
        }
    }


    private int explorerBunny;
    public int ExplorerBunny
    {
        get => explorerBunny;
        set
        {
            explorerBunny = Mathf.Clamp(value, 0, int.MaxValue);
            OnExplorerBunnyChanged?.Invoke();
        }
    }

    private int adventurerBunny;
    public int AdventurerBunny
    {
        get => adventurerBunny;
        set
        {
            adventurerBunny = Mathf.Clamp(value, 0, int.MaxValue);
            OnAdventurerBunnyChanged?.Invoke();
        }
    }



    public bool IsDraggingWorkerBunny;

    public bool IsDraggingExplorerBunny;

    public bool IsDraggingShovel;

    public bool IsDragging => IsDraggingWorkerBunny || IsDraggingExplorerBunny || IsDraggingShovel;


    public bool HasCollectedFood;




    [Space]
    [Header("Configs")]

    public RevealRewardConfig tileRewardConfig;
    public PriceConfig priceConfig;
    public TileConfig tileConfig;

    public System.Action OnGamePhaseChanged;
    private GamePhase currentGamePhase = GamePhase.PhaseOne;
    public GamePhase CurrentGamePhase 
    { 
        get => currentGamePhase; 
        set
        {
            currentGamePhase = value;
            OnGamePhaseChanged?.Invoke();
        }
        
    }

    private bool doubleAdjacencyBonus;

    public bool DoubleAdjacencyBonus
    {
        get => doubleAdjacencyBonus;
        set
        {
            doubleAdjacencyBonus = value;
            OnUnlockDoubleAdjacencyBonus?.Invoke();
        }
    }



    private bool doubleRevealReward;
    public bool DoubleRevealReward 
    {
        get => doubleRevealReward; 
        set
        {
            doubleRevealReward = value;
            OnUnlockDoublRevealRewardBonus?.Invoke();
        }
            
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        food = 0;
        settlerBunny = 6;
        explorerBunny = 1;
        adventurerBunny = 1;
    }


}
