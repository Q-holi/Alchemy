using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using static UnityEditor.Progress;


[RequireComponent(typeof(GenerateGUID))]
//-- 클래스가 GenerateGUID 컴포넌트를 필요로 한다는 것을 나타냅니다.
public class SceneItemsManager : Singleton<SceneItemsManager>, ISaveable
{
    #region 변수 
    private Transform parentItem;
    [SerializeField] private GameObject itemPrefab = null;

    private string _iSaveableUniqueID; //--<GenerateGUID>().GUID값을 받는다.
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }
    #endregion
    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
        //--Scene 로드 후 작업 -> 아이템 위치 정보 가져오기 
        //--parentItem은 하위에 각각의 아이템들이 배치 되어 있다.
    }
    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }
    private void DestroySceneItems()
    {
        Item[] itemsInScene = GameObject.FindObjectsOfType<Item>();
        //-- Scene에 있는 아이템의 정보를 전부 가져온다. 

        //-- 가져온 아이템 배열을 돌며 Destory를 한다.
        for (int i = itemsInScene.Length - 1; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }

    public void InstantiateSceneItem(int itemCode, Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemCode);
    }

    private void InstantiateSceneItems(List<SceneItem> sceneItemList)
    {
        GameObject itemGameObject;

        foreach (SceneItem sceneItem in sceneItemList)
        {
            itemGameObject = Instantiate(itemPrefab, new Vector3(sceneItem.position.x, sceneItem.position.y, sceneItem.position.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = sceneItem.itemCode;
            item.name = sceneItem.itemName;
        }
    }
    private void OnEnable()
    {
        ISaveableRegister();
        //--▲▲▲ SaveLoadManager에 있는 List<ISaveable> iSaveableObjectList에 SceneItemsManager을 추가 한다. 
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }
    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

  
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableLoad(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        { 
            GameObjectSave = gameObjectSave;

            // Restore data for current scene
            ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
    public void ISaveableRestoreScene(string sceneName)
    {
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))//--sceneSave ->  Dictionary<string, List<SceneItem>> SceneItem 리스트를 가져온다.
        {
            if (sceneSave.listSceneItem != null)
            {
                // scene list items found - destroy existing items in scene
                DestroySceneItems();

                // now instantiate the list of scene items
                InstantiateSceneItems(sceneSave.listSceneItem);
            }
        }
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public GameObjectSave ISaveableSave()
    {
        // Store current scene data
        ISaveableStoreScene(SceneManager.GetActiveScene().name);

        return GameObjectSave;
    }

    /// <summary>
    /// 현재 Scene의 있는 아이템의 리스트를 저장한다.(기존에 있던 Scene을 삭제하고 새롭게 업데이트 하여 SceneSave저장
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableStoreScene(string sceneName)
    {
        //기존에 있던 같은 이름의 scene을 지웁니다.-> Scene을 새롭게 업데이트 하는 방식
        GameObjectSave.sceneData.Remove(sceneName);

        //-- 이후 현제 Scene의 있는 아이템 전부를 가져와 SceneItem형식으로 List에 저장한다. 
        List<SceneItem> sceneItemList = new List<SceneItem>();
        Item[] itemsInScene = FindObjectsOfType<Item>();

        foreach (Item item in itemsInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemCode = item.ItemCode;
            sceneItem.position = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
            sceneItem.itemName = item.name;

            sceneItemList.Add(sceneItem);
        }
        SceneSave sceneSave = new SceneSave();
        sceneSave.listSceneItem = sceneItemList;


        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }
}