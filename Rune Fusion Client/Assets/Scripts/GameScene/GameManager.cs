using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public static GameManager Instance {get; private set;}
        [field: SerializeField] public GameManagerSO GameManagerSO { get; private set; }

        [field: SerializeField]public RuneManager RuneManager {get; private set;}
        [field: SerializeField]public InputManager InputManager {get; private set;}
        [field: SerializeField]public TurnManager TurnManager {get; private set;}

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }
                RuneManager = FindFirstObjectByType<RuneManager>();
                InputManager = FindFirstObjectByType<InputManager>();
                TurnManager = FindFirstObjectByType<TurnManager>();
        }

        private void Start()
        {
                StartCoroutine(GenMapCoroutine());
        }

        private IEnumerator GenMapCoroutine()
        {
                yield return new WaitUntil(() => SocketManager.Instance.MapStart != null);
                GameManagerSO.SetHeightRuneMap(SocketManager.Instance.MapStart.Count);
                GameManagerSO.SetWidthRuneMap(SocketManager.Instance.MapStart[0].Count);
                RuneManager.GenerateRunesMap(SocketManager.Instance.MapStart);
                SetUpTilePosition();
        }

        public void SetUpTilePosition()
        {
                Transform tilesTransform = RuneManager.transform;
                tilesTransform.position = new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + RuneManager.GetHeightRunesMap()/2) ;
                RuneManager.UpdateRunesPostionMap();
        }
        
        public void CheckIfMainThread()
        {
                if (Thread.CurrentThread.ManagedThreadId == 1)
                {
                        Debug.Log("We are on the main thread!");
                }
                else
                {
                        Debug.Log("We are on a different thread!");
                }
        }
}