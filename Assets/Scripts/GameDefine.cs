using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Grass = 1,
    Sand = 2,
    Ocean = 3,
}

public enum TileState
{
    Hidden = 0,
    CanUnlock = 1,
    HasUnlocked = 2,
}

public enum ResourceType
{
    None = 0,
    Food = 1,
    Settler = 2,
    Explorer = 3,
    Adventurer = 4,
}

public enum TileRewardType
{
    None = 0,
    SmallFoodRation = 1,
    LargeFoodRation = 2,
    Settler = 3,
    Explorer = 4,
    Adventurer = 5,
}

public enum GamePhase
{
    PhaseOne = 1,
    PhaseTwo = 2,
    PhaseThree = 3,
    PhaseFour =4,
}

public enum ModifierType
{
    DoubleAdjacencyBonus = 1,
    DoubleRevealReward = 2,
}
