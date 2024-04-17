using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLitftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDwon, bool idleLeft, bool idleRight);

public static class EventHandler
{

    //Drop selected item Event
    public static event Action DropSelectedItemEvent;
    public static void CallDropSelectedItemEvent()
    {
        if (DropSelectedItemEvent != null)
            DropSelectedItemEvent();
    }

    // Remove selected item from inventory
    public static event Action RemoveSelectedItemFromInventoryEvent;
    public static void CallRemoveSelectedItemFromInventoryEvent()
    {
        if (RemoveSelectedItemFromInventoryEvent != null)
        {
            RemoveSelectedItemFromInventoryEvent();
        }
    }

    public static event MovementDelegate movementEvent;
    public static void CallMovementEvent(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLitftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDwon, bool idleLeft, bool idleRight)
    {
        if (movementEvent != null)
        {
            movementEvent(inputX, inputY, isWalking, isRunning, isIdle, isCarrying, toolEffect,
                          isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                          isLiftingToolRight, isLiftingToolLeft, isLitftingToolUp, isLiftingToolDown,
                          isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                          isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                          idleUp, idleDwon, idleLeft, idleRight);
        }
    }

    #region GameTimeEvenet
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (InventoryUpdatedEvent != null)
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameMinuteEvent != null)
            AdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;

    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameHourEvent != null)
            AdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;

    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameDayEvent != null)
            AdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;

    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameSeasonEvent != null)
            AdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;

    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameYearEvent != null)
            AdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }
    #endregion

    public static event Action BeforeSceneUnloadFadeOutEvent;
    public static void CallBeforeSceneUnloadFadeOutEvent()
    {
        if (BeforeSceneUnloadFadeOutEvent != null)
            BeforeSceneUnloadFadeOutEvent();
    }
    // Before Scene Unload Event
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        if (BeforeSceneUnloadEvent != null)
            BeforeSceneUnloadEvent();
    }
    // After Scene Loaded Event
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        if (AfterSceneLoadEvent != null)
            AfterSceneLoadEvent();
    }

    // After Scene Load Fade In Event
    public static event Action AfterSceneLoadFadeInEvent;
    public static void CallAfterSceneLoadFadeInEvent()
    {
        if (AfterSceneLoadFadeInEvent != null)
            AfterSceneLoadFadeInEvent();
    }
}
