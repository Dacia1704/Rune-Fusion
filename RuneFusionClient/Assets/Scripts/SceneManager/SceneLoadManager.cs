using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
        public static SceneLoadManager Instance;
        [field: SerializeField ]public string MainMenuSceneName { get; private set; }
        [field: SerializeField ]public string PickSceneName { get; private set; }
        [field: SerializeField ]public string GameSceneName { get; private set; }

        [HideInInspector] public float ProgressValue;
        
        public string NextSceneName { get; private set; }
        
        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;

                ProgressValue = 0f;
                DontDestroyOnLoad(this.gameObject);
        }

        public void LoadSceneImmediately(string sceneName)
        {
                Debug.Log("Loading scene " + sceneName);
                SceneManager.LoadScene(sceneName);
        }

        private int tabIndexToOpen = -1;
        public void LoadMainSceneWithSpecificTab(int index)
        {
                Debug.Log(JsonConvert.SerializeObject(SocketManager.Instance.PlayerData));
                tabIndexToOpen = index;
                SceneManager.sceneLoaded += OnSceneLoaded; 
                LoadSceneImmediately(MainMenuSceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
                StartCoroutine(LoadSceneCoroutine());
                SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private IEnumerator LoadSceneCoroutine()
        {
                yield return new WaitForSeconds(0.01f);
                UIMainMenuManager.Instance.ChangeToNewScreen(UIMainMenuManager.Instance.UITabManager);
                UIMainMenuManager.Instance.UITabManager.SwitchToTab(tabIndexToOpen);
                if (tabIndexToOpen == 0)
                {
                        SocketManager.Instance.RequestMonsterOwnData();
                }
        }

        public void LoadSceneAsync(string sceneName)
        {
                NextSceneName = sceneName;
                StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        }
        private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
                if (loadOperation != null)
                {
                        loadOperation.allowSceneActivation = false;
                        while (loadOperation is { isDone: false })
                        {
                                ProgressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                                yield return null;
                        }
                }
        }
}