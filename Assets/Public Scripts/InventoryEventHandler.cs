using System;

public static class InventoryEventHandler
{
    // 아이템 정보 확인시 호출되는 이벤트
    public static Action<Item> OnMouse;

    // 아이템 드래그 시작, 끝 에 호출되는 이벤트
    public static Action<int, bool> OnItemDragging;

    // 아이템 사용시 호출되는 이벤트
    public static Action<int> OnUseItem;

    // 인벤토리 필터 사용시 호출되는 이벤트
    public static Action<int, bool> OnUseFilter;
}
