using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;



[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : Singleton<GridPropertiesManager>, ISaveable
{
    #region 변수
    private Transform cropParentTransform;
    private Tilemap groundDecoration1;
    private Tilemap groundDecoration2;
    public Grid grid;
    [SerializeField] private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
    [SerializeField] private SO_CropDetailsList so_CropDetailsList = null;
    [SerializeField] private SO_GridProperties[] so_gridPropertiesArray = null;
    [SerializeField] private Tile[] dugGround = null;
    [SerializeField] private Tile[] waterGround = null;
    #endregion

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }


    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent += AdvancDay;
    }
    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent -= AdvancDay;
    }

    private void Start()
    {
        InitialiseGridProperties();
    }

    private void ClearDisplayGroundDecorations()
    {
        // Remove ground decorations
        groundDecoration1.ClearAllTiles();
        groundDecoration2.ClearAllTiles();
    }
    private void ClearDisplayAllPlantedCrops()
    {
        // Destroy all crops in scene
        Crop[] cropArray;
        cropArray = FindObjectsOfType<Crop>();
        foreach (Crop crop in cropArray)
        {
            Destroy(crop.gameObject);
        }
    }
    private void ClearDisplayGridPropertyDetails()
    {
        ClearDisplayGroundDecorations();

        ClearDisplayAllPlantedCrops();
    }
    private void DisplayGridPropertyDetails()
    {
        // Loop through all grid items
        foreach (KeyValuePair<string, GridPropertyDetails> item in gridPropertyDictionary)
        {
            GridPropertyDetails gridPropertyDetails = item.Value;

            DisplayDugGround(gridPropertyDetails);
            DisplayWaterGround(gridPropertyDetails);
            DisplayPlantedCrop(gridPropertyDetails);
        }
    }

    /// <summary>
    /// ConnectDugGround(gridPropertyDetails) 호출
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayDugGround(GridPropertyDetails gridPropertyDetails)
    {
        // Dug
        if (gridPropertyDetails.daysSinceDug > -1)
        {
            ConnectDugGround(gridPropertyDetails);
        }
    }

    public void DisplayWaterGround(GridPropertyDetails gridPropertyDetails)
    {
        // Dug
        if (gridPropertyDetails.daysSinceWatered > -1)
        {
            ConnectWaterGround(gridPropertyDetails);
        }
    }

    private void DisplayPlantedCrop(GridPropertyDetails gridPropertyDetails)
    {
        if(gridPropertyDetails.seedItemCode > -1)
        {
            // 작물 스크립트 오브젝에 있는 Detail내용을 가져온다.
            CropDetails cropDetails = so_CropDetailsList.GetCropDetails(gridPropertyDetails.seedItemCode);

            // ▼▼▼ 해당 작물에 대한 Prefab을 가져올 변수 
            GameObject cropPrefab;
            
            int growthStages = cropDetails.growthDays.Length;//-- 성장에 필요한 일수의 배열길이를 가져오기 (각 배열 안에 다음 단계로 성장에 필요한 일수가 들어있다.)

            int currentGrowthStage = 0;
            int daysCounter = cropDetails.totalGrowthDays;//--성장에 필요한 총 일수를 가져온다.
            for (int i = growthStages - 1; i >= 0; i--)
            {
                if (gridPropertyDetails.growthDays >= daysCounter)
                {
                    currentGrowthStage = i;
                    break;
                }
                daysCounter = daysCounter - cropDetails.growthDays[i];
            }
            //-- so_CropDetailsList에서 성장단계별 Prefab가져오기
            cropPrefab = cropDetails.growthPrefab[currentGrowthStage];
            //-- 성장단계별 행당하는 Sprite이미지 가져오기
            Sprite growthSprite = cropDetails.growthSprite[currentGrowthStage];

            Vector3 worldPosition = groundDecoration2.CellToWorld(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0));

            worldPosition = new Vector3(worldPosition.x + Settings.gridCellSize / 2, worldPosition.y, worldPosition.z);

            GameObject cropInstance = Instantiate(cropPrefab, worldPosition, Quaternion.identity);

            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = growthSprite;
            cropInstance.transform.SetParent(cropParentTransform);
            cropInstance.GetComponent<Crop>().cropGridPosition = new Vector2Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY);

        }
    }
    private void InitialiseGridProperties()
    {
        // Loop through all gridproperties in the array
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArray)
        {
            // Create dictionary of grid property details
            Dictionary<string, GridPropertyDetails> gridPropertyDictionary = new Dictionary<string, GridPropertyDetails>();

            // Populate grid property dictionary - Iterate through all the grid properties in the so gridproperties list
            foreach (GridProperty gridProperty in so_GridProperties.gridPropertyList)
            {
                GridPropertyDetails gridPropertyDetails;

                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDictionary);

                if (gridPropertyDetails == null)
                {
                    gridPropertyDetails = new GridPropertyDetails();
                }
                switch (gridProperty.gridBoolProperty)
                {
                    case GridBoolProperty.diggable:
                        gridPropertyDetails.isDiggable = gridProperty.gridBoolValue;
                        break;

                    case GridBoolProperty.canDropItem:
                        gridPropertyDetails.canDropItem = gridProperty.gridBoolValue;
                        break;

                    case GridBoolProperty.canPlaceFurniture:
                        gridPropertyDetails.canPlaceFurniture = gridProperty.gridBoolValue;
                        break;

                    case GridBoolProperty.isPath:
                        gridPropertyDetails.isPath = gridProperty.gridBoolValue;
                        break;

                    case GridBoolProperty.isNPCObstacle:
                        gridPropertyDetails.isNPCObstacle = gridProperty.gridBoolValue;
                        break;

                    default:
                        break;
                }
                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetails, gridPropertyDictionary);
            }
            // Create scene save for this gameobject
            SceneSave sceneSave = new SceneSave();

            // Add grid property dictionary to scene save data
            sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

            // If starting scene set the gridPropertyDictionary member variable to the current iteration
            if (so_GridProperties.sceneName.ToString() == SceneControllerManager.Instance.startingSceneName.ToString())
            {
                this.gridPropertyDictionary = gridPropertyDictionary;
            }
            // Add scene save to game object scene data
            GameObjectSave.sceneData.Add(so_GridProperties.sceneName.ToString(), sceneSave);
        }
    }

    /// <summary>
    /// Returns the gridPropertyDetails at the gridlocation for the supplied dictionary, or null if no properties exist at that location.
    /// </summary>
    private GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        // Construct key from coordinate
        string key = "x" + gridX + "y" + gridY;

        GridPropertyDetails gridPropertyDetails;

        // Check if grid property details exist forcoordinate and retrieve
        if (!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails))
        {
            // if not found
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }
    public void ISaveableLoad(GameSave gameSave)
    {
        throw new System.NotImplementedException();
    }

    public GameObjectSave ISaveableSave()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Get the grid property details for the tile at (gridX,gridY). If no grid property details exist null is returned and can assume that all grid property details values are null or false
    /// </summary>
    public GridPropertyDetails GetGridPropertyDetails(int gridx, int gridY)
    {
        return GetGridPropertyDetails(gridx, gridY, gridPropertyDictionary);
    }
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // Get sceneSave for scene - it exists since we created it in initialise
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            // get grid property details dictionary - it exists since we created it in initialise
            if (sceneSave.gridPropertyDetailsDictionary != null)
            {
                gridPropertyDictionary = sceneSave.gridPropertyDetailsDictionary;
            }
            // If grid properties exist
            if (gridPropertyDictionary.Count > 0)
            {
                // grid property details found for the current scene destroy existing ground decoration
                ClearDisplayGridPropertyDetails();

                // Instantiate grid property details for current scene
                DisplayGridPropertyDetails();
            }
        }
    }
    public void ISaveableStoreScene(string sceneName)
    {
        // Remove sceneSave for scene
        GameObjectSave.sceneData.Remove(sceneName);

        // Create sceneSave for scene
        SceneSave sceneSave = new SceneSave();

        // create & add dict grid property details dictionary
        sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

        // Add scene save to game object scene data
        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    private void AfterSceneLoaded()
    {
        if (GameObject.FindGameObjectWithTag(Tags.CropParentTransform) != null)
        {
            cropParentTransform = GameObject.FindGameObjectWithTag(Tags.CropParentTransform).transform;
        }
        else
        {
            cropParentTransform = null;
        }

        grid = GameObject.FindAnyObjectByType<Grid>();
        // Get tilemaps
        groundDecoration1 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration1).GetComponent<Tilemap>();
        groundDecoration2 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration2).GetComponent<Tilemap>();
    }

    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyDetails gridPropertyDetails)
    {
        SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertyDictionary);
    }

    /// <summary>
    ///gridPropertyDictionary에 전달받은 gridPropertyDetails의 정보를 저장한다. 
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="gridPropertyDictionary"></param>
    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        // gridPropertyDictionary에 접근 하기 위한 좌표 Key값
        string key = "x" + gridX + "y" + gridY;

        gridPropertyDetails.gridX = gridX;
        gridPropertyDetails.gridY = gridY;

        // gridPropertyDictionary에 변경된 gridPropertyDetail 을 적용시킨다. 
        gridPropertyDictionary[key] = gridPropertyDetails;
    }

    /// <summary>
    /// 클릭한 지형포함 4방향 (상,하,좌,우) 총5지형을 검사 후 상황에 맞는 Tile로변경한다. 
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectDugGround(GridPropertyDetails gridPropertyDetails)
    {
        // 클릭한 gird tile 변경 

        Tile dugTile0 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), dugTile0);

        // 주변 4방향으로 영향을 받는 타일들을 검사하고 위와 같은 방법으로 타일을 변경(상,하,좌,우)

        GridPropertyDetails adjacentGridPropertyDetails;

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile1 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), dugTile1);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), dugTile2);
        }
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile3 = SetDugTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), dugTile3);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            Tile dugTile4 = SetDugTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), dugTile4);
        }
    }

    private void ConnectWaterGround(GridPropertyDetails gridPropertyDetails)
    {
        // Select tile based on surrounding watered tiles

        Tile wateredTile0 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), wateredTile0);

        // Set 4 tiles if watered surrounding current tile - up, down, left, right now that this central tile has been watered

        GridPropertyDetails adjacentGridPropertyDetails;

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile1 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), wateredTile1);
        }
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile2 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), wateredTile2);
        }
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile3 = SetWateredTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), wateredTile3);
        }
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            Tile wateredTile4 = SetWateredTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), wateredTile4);
        }
    }

    /// <summary>
    /// 클릭한 grid tile 밑 4방향(상,하,좌,우) tile이 변경되어 있는지 확인하고 상황에 맞는 타일은 선택하여 반환
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private Tile SetDugTile(int xGrid, int yGrid)
    {
        //Get whether surrounding tiles (up,down,left, and right) are dug or not

        bool upDug = IsGridSquareDug(xGrid, yGrid + 1);
        bool downDug = IsGridSquareDug(xGrid, yGrid - 1);
        bool leftDug = IsGridSquareDug(xGrid - 1, yGrid);
        bool rightDug = IsGridSquareDug(xGrid + 1, yGrid);

        #region 주변 타일의 상태와 맞게 tile 이미지를 반환

        if (!upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[0];
        }
        else if (!upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[1];
        }
        else if (!upDug && downDug && rightDug && leftDug)
        {
            return dugGround[2];
        }
        else if (!upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[3];
        }
        else if (!upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[4];
        }
        else if (upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[5];
        }
        else if (upDug && downDug && rightDug && leftDug)
        {
            return dugGround[6];
        }
        else if (upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[7];
        }
        else if (upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[8];
        }
        else if (upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[9];
        }
        else if (upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[10];
        }
        else if (upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[11];
        }
        else if (upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[12];
        }
        else if (!upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[13];
        }
        else if (!upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[14];
        }
        else if (!upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[15];
        }

        return null;

        #endregion Set appropriate tile based on whether surrounding tiles are dug or not
    }

    private Tile SetWateredTile(int xGrid, int yGrid)
    {
        bool upWater = IsGridSquareWater(xGrid, yGrid + 1);
        bool downWater = IsGridSquareWater(xGrid, yGrid - 1);
        bool leftWater = IsGridSquareWater(xGrid - 1, yGrid);
        bool rightWater = IsGridSquareWater(xGrid + 1, yGrid);

        #region 주변 타일의 상태와 맞게 tile 이미지를 반환

        if (!upWater && !downWater && !rightWater && !leftWater)
        {
            return waterGround[0];
        }
        else if (!upWater && downWater && rightWater && !leftWater)
        {
            return waterGround[1];
        }
        else if (!upWater && downWater && rightWater && leftWater)
        {
            return waterGround[2];
        }
        else if (!upWater && downWater && !rightWater && leftWater)
        {
            return waterGround[3];
        }
        else if (!upWater && downWater && !rightWater && !leftWater)
        {
            return waterGround[4];
        }
        else if (upWater && downWater && rightWater && !leftWater)
        {
            return waterGround[5];
        }
        else if (upWater && downWater && rightWater && leftWater)
        {
            return waterGround[6];
        }
        else if (upWater && downWater && !rightWater && leftWater)
        {
            return waterGround[7];
        }
        else if (upWater && downWater && !rightWater && !leftWater)
        {
            return waterGround[8];
        }
        else if (upWater && !downWater && rightWater && !leftWater)
        {
            return waterGround[9];
        }
        else if (upWater && !downWater && rightWater && leftWater)
        {
            return waterGround[10];
        }
        else if (upWater && !downWater && !rightWater && leftWater)
        {
            return waterGround[11];
        }
        else if (upWater && !downWater && !rightWater && !leftWater)
        {
            return waterGround[12];
        }
        else if (!upWater && !downWater && rightWater && !leftWater)
        {
            return waterGround[13];
        }
        else if (!upWater && !downWater && rightWater && leftWater)
        {
            return waterGround[14];
        }
        else if (!upWater && !downWater && !rightWater && leftWater)
        {
            return waterGround[15];
        }

        return null;

        #endregion
    }

    /// <summary>
    /// 전달해준 grid 속성중 땅을 호미로 파낸 부분인지 확인
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private bool IsGridSquareDug(int xGrid, int yGrid)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.daysSinceDug > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsGridSquareWater(int xGrid, int yGrid)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.daysSinceWatered > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AdvancDay(int gameYear, Season gmaeSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // Clear Display All Grid Property Details
        ClearDisplayGridPropertyDetails();
        // Loop through all scenes - by looping through all gridproperties in the array
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArray)
        {
            // Get gridpropertydetails dictionary for scene
            if (GameObjectSave.sceneData.TryGetValue(so_GridProperties.sceneName.ToString(), out SceneSave sceneSave))
            {
                if (sceneSave.gridPropertyDetailsDictionary != null)
                {
                    for (int i = sceneSave.gridPropertyDetailsDictionary.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertyDetails> item = sceneSave.gridPropertyDetailsDictionary.ElementAt(i);

                        GridPropertyDetails gridPropertyDetails = item.Value;

                        #region Update all grid properties to reflect the advance in the day
                        // If ground is watered, then clear water
                        if (gridPropertyDetails.daysSinceWatered > -1)
                        {
                            gridPropertyDetails.daysSinceWatered = -1;
                        }
                        // Set gridpropertydetails
                        SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails, sceneSave.gridPropertyDetailsDictionary);
                        #endregion Update all grid properties to reflect the advance in the day
                    }
                }
            }
        }
        DisplayGridPropertyDetails();
    }
}
