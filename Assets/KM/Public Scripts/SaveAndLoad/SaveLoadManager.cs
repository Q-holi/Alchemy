using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public GameSave gameSave;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        LoadDataFromFile();
    }

    public void SaveDataToFile()
    {
        gameSave = new GameSave();
        InventoryManager.Instance.ISaveableStoreScene("TestSaveScene");
        gameSave.gameObjectData.Add(InventoryManager.Instance.ISaveableUniqueID, InventoryManager.Instance.GameObjectSave);

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/TestItemInventory.dat", FileMode.Create);

        bf.Serialize(file, gameSave);

        file.Close();
    }
    public void LoadDataFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/TestItemInventory.dat"))
        {
            gameSave = new GameSave();

            FileStream file = File.Open(Application.persistentDataPath + "/TestItemInventory.dat", FileMode.Open);

            gameSave = (GameSave)bf.Deserialize(file);
            //--BinaryFormatter를 사용하여 역직렬화하고 gameSave에 저장되어 있는 값 불러오기

            InventoryManager.Instance.LoadItemSaveData(gameSave);

            file.Close();
        }
        //UIManager.Instance.DisablePauseMenu();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveDataToFile();
            Debug.Log("Save");
        }
    }

}
