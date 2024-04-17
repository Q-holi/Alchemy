
using System;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    [ItemCodeDescription]
    public int seedItemCode; // this is the item code for the corresponding seed
    public int[] growthDays; // 단계별 필요한 성장 일수
    public int totalGrowthDays; // 최종 성장 일수 
    public GameObject[] growthPrefab;// 성장 단계를 인스턴스화할 때 사용할 프리팹
    public Sprite[] growthSprite; // growth sprite
    public Season[] seasons; // growth seasons
    public Sprite harvestedSprite; // sprite used once harvested

    [ItemCodeDescription]
    public int harvestedTransformItemCode; // 수확 시 수확한 작물의 형태의 아이템 코드로 변환
    public bool hideCropBeforeHarvestedAnimation; // if the crop should be disabled before the harvested animation
    public bool disableCropCollidersBeforeHarvestedAnimation;// 수확된 애니메이션이 다른 게임 오브젝트에 영향을 미치지 않도록 크롭의 충돌기를 비활성화해야 하는 경우
    public bool isHarvestedAnimation; // 수확된 애니메이션이 최종 성장 단계 프리팹에서 재생되는 경우 
    public bool isHarvestActionEffect = false; // 수확 작업 Effect가 있는지 여부를 판단하기 위한 플래그
    public bool spawnCropProducedAtPlayerPosition;
    public HarvestActionEffect harvestActionEffect; //  수확 작용 Effect

    [ItemCodeDescription]
    public int[] harvestToolItemCode; // 지정한 도구로 수집할 수 있는 도구에 대한 항목 코드 배열 또는 도구가 필요하지 않은 경우 배열 0
    public int[] requiredHarvestActions; // number of harvest actions required for corressponding tool in harvest tool item code array
    [ItemCodeDescription]
    public int[] cropProducedItemCode; // 수확한 작물에 대해 생산된 품목 코드 배열
    public int[] cropProducedMinQuantity; // 수확한 작물에 대해 생산된 최소량의 배열
    public int[] cropProducedMaxQuantity; // 최대 수량이 > 최소 수량이면 최소와 최대 사이의 임의의 작물 수가 생성됩니다
    public int daysToRegrow; // days to regrow next crop or -1 if a single crop

    public bool CanUseToolToHarvestCrop(int toolItemCode)
    {
        if (RequiredHarvestActionsForTool(toolItemCode) == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private int RequiredHarvestActionsForTool(int toolItemCode)
    {
        for (int i = 0; i < harvestToolItemCode.Length; i++)
        {

            if (harvestToolItemCode[i] == toolItemCode)
            {
                return requiredHarvestActions[i];
            }   
        }
        return -1;
    }
}
