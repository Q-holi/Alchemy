using System;

public static class EventHandler
{
    public static Action<Item> OnMouse;
    public static Action<Item, bool> OnItemDragging;
    public static Action<Item> OnUseItem;
}
