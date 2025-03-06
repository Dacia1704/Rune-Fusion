using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public static GameManager Instance {get; private set;}
        [field: SerializeField] public GameManagerSO GameManagerSO { get; private set; }

        public RuneManager RuneManager {get; private set;}
        public SocketManager SocketManager {get; private set;}

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }
                RuneManager = FindObjectOfType<RuneManager>();
                SocketManager = FindObjectOfType<SocketManager>();
        }
        
        public void SetUpTilePosition()
        {
                Transform tilesTransform = RuneManager.transform;
                tilesTransform.position = new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + RuneManager.GetHeightTiles()/2) ;
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