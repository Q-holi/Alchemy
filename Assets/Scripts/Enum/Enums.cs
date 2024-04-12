#region 인벤토리 위치(플레이어,가방,etc)
public enum InventoryLocation
{
    player,
    chest,
    count
}
#endregion
public enum ToolEffect
{
    none,
    watering
}
#region 플레이어 이동 방향
public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
}
#endregion

#region 아이템 타입
public enum ItemType
{
    Seed,
    Commodity,
    Watering_tool,
    Hoeing_tool,
    Chopping_tool,
    Breaking_tool,
    Reaping_tool,
    Collecting_tool,
    Reapable_scenary,
    Furniture,
    none,
    count
}
#endregion

#region AnimationName
public enum AnimationName
{
    idleDown,
    idleUp,
    idleRight,
    idleLeft,
    walkUp,
    walkDown,
    walkRight,
    walkLeft,
    runUp,
    runDown,
    runRight,
    runLeft,
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    swingToolUp,
    swingToolDown,
    swingToolRight,
    swingToolLeft,
    liftToolUp,
    liftToolDown,
    liftToolRight,
    liftToolLeft,
    holdToolUp,
    holdToolDown,
    holdToolRight,
    holdToolLeft,
    pickDown,
    pickUp,
    pickRight,
    pickLeft,
    count
}
#endregion

public enum CharacterPartAnimator
{
    Body,
    Arms,
    Hair,
    Tool,
    Hat,
    Count
}

public enum PartVariantColour
{ 
    none,
    count
}
public enum PartVariantType
{
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}
public enum GridBoolProperty
{
    diggable,
    canDropItem,
    canPlaceFurniture,
    isPath,
    isNPCObstacle
}
public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
    none,
    count
}

public enum SceneName
{
    Farm,
    Scene2_Field,
    Scene3_Cabin,
    Forest
}
public enum ItemRating
{
    Normal,
    Rare,
    Epic,
    Legend
}

public enum ItemFilterType
{
    Name,
    Capacity,
    Rating
}

public enum InventoryFilterType
{
    Potion,
    Collection,
    Tool
}