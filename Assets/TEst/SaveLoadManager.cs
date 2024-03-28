using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public GameSave gameSave;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveDataToFile();
            Debug.Log("Save");
        }
    }

}
