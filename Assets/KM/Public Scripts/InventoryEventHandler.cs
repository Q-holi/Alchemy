using System;

public static class InventoryEventHandler
{
    /// <summary>
    /// 아이템 정보 확인시 호출되는 이벤트
    /// </summary>
    public static Action<int> OnMouse;

    /// <summary>
    /// 아이템 드래그 시작, 끝 에 호출되는 이벤트
    /// </summary>
    public static Action<int, bool> OnItemDragging;

    /// <summary>
    /// 인벤토리 필터 사용시 호출되는 이벤트
    /// </summary>
    public static Action<int, bool> OnUseFilter;
}
