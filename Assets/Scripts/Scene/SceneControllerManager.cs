using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControllerManager : Singleton<SceneControllerManager>
{
    #region 변수
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    public SceneName startingSceneName;
    #endregion
    /// <summary>
    /// faderCanvasGroup의 씬 레이케스트를 차단하여 사용자의 입력을 차단 후 해당 faderCanvasGroup의 알파값을 조절하여
    /// Fade 효과를 진행 후 완료되면  씬 레이케스트를 허용하여 입력을 받을수 있게 한다. 
    /// </summary>
    /// <param name="finalAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float finalAlpha)
    {
        // Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again.
        isFading = true;

        faderCanvasGroup.blocksRaycasts = true;
        //-- blocksRaycasts 속성을 true로 설정하여 씬의 레이캐스트를 차단하여 사용자 입력을 더 이상 받지 않도록 합니다.

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
        //-- 현재 알파 값에서 목표 알파 값까지 변경되는데 걸리는 시간에 따른 속도를 저장

        //-- 현재 알파 값이 목표 알파 값에 도달할 때까지 반복되는 루프가 실행.
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            //-- faderCanvasGroup 알파값 조정 
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            //-- 1프레임 다음에 진행
            yield return null;
        }
        isFading = false;
        //-- faderCanvasGroup의 알파값이 정해진 알파값 = 1.0f까지 도달하면 isFading을 false로 저장 
        faderCanvasGroup.blocksRaycasts = false;
        //-- blocksRaycasts 속성을 false 설정하여 씬의 레이캐스트를 허용 사용자의 입력을 받을수 있게 설정한다. 
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        // Call before scene unload fade out event
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();

        // Start fading to black and wait for it to finish before continuing.
        yield return StartCoroutine(Fade(1f));

        // Store scene data
        SaveLoadManager.Instance.StoreCurrentSceneData();

        // Set player position
        Player.Instance.gameObject.transform.position = spawnPosition;

        // Call before scene unload event.
        EventHandler.CallBeforeSceneUnloadEvent();

        // Unload the current active scene.
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Start loading the given scene and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += OnSceneLoaded;
        // 씬 로드 완료 시 호출될 콜백 함수

        if (EventHandler.CallSetSpawnPointEvent() != Vector3.zero)
            Player.Instance.gameObject.transform.position = EventHandler.CallSetSpawnPointEvent();

        // Call after scene load event
        EventHandler.CallAfterSceneLoadEvent();

        // Restore new scene data
        SaveLoadManager.Instance.RestoreCurrentSceneData();

        // Start fading back in and wait for it to finish before exiting the function.
        yield return StartCoroutine(Fade(0f));

        // Call after scene load fade in event
        EventHandler.CallAfterSceneLoadFadeInEvent();
    }
    /// <summary>
    /// 파라미터로 받은 sceneName을 가진 씬을 비동기로 로드하고 해당 씬을 활성화 시킨다.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //--sceneName을 비동기로드한다.  yield return 이기 때문에 비동기 로드가 완료될때까지 아래 있는 코드는 대기한다.
        //--LoadSceneMode.Additive: 기존 씬에 새로운 씬을 추가로 로드하는 옵션 이렇게 함으로써 현재 씬은 유지되고 새로운 씬이 추가.

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        //-- SceneManager.sceneCount 는 현재 로드가 되어 있는 씬의 수를 가져옵니다. 그리고 -1을 하여 가장 최근에 로드가 된 씬 인덱스를 가져옵니다.
        //-- SceneManager.GetSceneAt는 반환덴 인덱스에 접근하여 해당하는 씬의 정보를 가져옵니다. 그렇게 위에서 사용한 
        //-- SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); 때문에 비동기 로드가 진행되서 완료된 sceneName씬을
        //-- newlyLoadedScene에 정보를 넘겨줍니다.

        SceneManager.SetActiveScene(newlyLoadedScene);
        //-- 로드가 된 newlyLoadedScene을 활성화 시킨다.
    }
    /// <summary>
    /// IEnumerator Start ◀ 메서드는 MonoBehaviour의 Start 메서드를 대체하고, 게임 오브젝트가 활성화될 때 호출됩니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));
        //--▲▲▲ 코루틴 Start가 실행히 되면 LoadSceneAndSetActive 함수가 완료될때까지 대기한다.
        //-- 즉 아래 있는 코드들은 yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));
        //-- 함수가 작업을 마무리 하고 실행이 되는 절차

        EventHandler.CallAfterSceneLoadEvent();
        //--SwitchConfineBoundingShape.SwitchBoundingShape() //--맵으로 부터 카메라 위치 제한
        //--UI_InventorySlot.SceneLoaded() //-- 씬에 있는 아이템 위치로드 Item-> 필드에 있는 아이템 오브젝트 상위오브젝트

        SaveLoadManager.Instance.RestoreCurrentSceneData();
        //--현재 Load된 Scene의 데이터를 복원한다.(Start되는 부분이라 복원한 Scene 데이터가 없다->기본상태)

        StartCoroutine(Fade(0f));
        //-- 화면 페이트 진행
    }
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (!isFading)
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
    }

    private void OnSceneLoaded(AsyncOperation operation)
    {
        // 로드된 씬에 있는 특정 게임 오브젝트의 스크립트를 실행하는 예시
        GameObject loadedSceneRoot = SceneManager.GetSceneByName("Forest").GetRootGameObjects()[0];
        MapGenerator yourScript = loadedSceneRoot.GetComponent<MapGenerator>();
        if (yourScript != null)
        {
            // 특정 스크립트의 함수 호출 등 실행
            yourScript.GenerateMap();
        }
    }
}
