using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public GameSave gameSave;
    public List<ISaveable> iSaveableObjectList;
    public bool autoSave;
    protected override void Awake()
    {
        base.Awake();
        iSaveableObjectList = new List<ISaveable>();
    }
    private void Update()
    {

    }

    public void LoadDataFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/WildHopeCreek.dat"))
        {
            gameSave = new GameSave();

            FileStream file = File.Open(Application.persistentDataPath + "/WildHopeCreek.dat", FileMode.Open);

            gameSave = (GameSave)bf.Deserialize(file);
            //--BinaryFormatter를 사용하여 역직렬화하고 gameSave에 저장되어 있는 값 불러오기

            // loop through all ISaveable objects and apply save data
            for (int i = iSaveableObjectList.Count - 1; i > -1; i--)
            {
                if (gameSave.gameObjectData.ContainsKey(iSaveableObjectList[i].ISaveableUniqueID))
                {
                    iSaveableObjectList[i].ISaveableLoad(gameSave);
                }
                else
                {
                    Component component = (Component)iSaveableObjectList[i];
                    Destroy(component.gameObject);
                }
            }

            file.Close();
        }
        //UIManager.Instance.DisablePauseMenu();
    }
    /// <summary>
    /// ISaveable objects에서 각 데이터를 저장하여 .dat파일로 지정한 경로에 저장한다. 
    /// </summary>
    public void SaveDataToFile()
    {
        gameSave = new GameSave();
        //-- 게임을 저장할 형태를 생성한다. 

        // ISaveable objects에서 각 저장오브젝트의 저장 방식을 호출하여 저장 방식에 맞게 GameSave안에 값을 저장한다. 
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
            gameSave.gameObjectData.Add(iSaveableObject.ISaveableUniqueID, iSaveableObject.ISaveableSave());
    
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/WildHopeCreek.dat", FileMode.Create);
        //--저장할 파일을 엽니다. 파일 경로는 Application.persistentDataPath에서 가져오며, 파일이 없으면 생성됩니다.

        bf.Serialize(file, gameSave);
        //--BinaryFormatter를 사용하여 직렬화하고 gameSave에 저장

        file.Close();

        //UIManager.Instance.DisablePauseMenu();
    }


    public void StoreCurrentSceneData()
    {
        // loop through all ISaveable objects and trigger store scene data for each
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }
    public void RestoreCurrentSceneData()
    {
        //▼▼▼ iSaveableObjectList ->  SaveLoadManager.Instance.iSaveableObjectList.Add(this);
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}
