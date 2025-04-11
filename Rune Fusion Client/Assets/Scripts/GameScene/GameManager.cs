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
        [field: SerializeField] public BattleManager BattleManager {get; private set;}
        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }
                RuneManager = FindFirstObjectByType<RuneManager>();
                InputManager = FindFirstObjectByType<InputManager>();
                BattleManager = FindFirstObjectByType<BattleManager>();
        }

        private void Start()
        {
                SocketManager.Instance.GameStartRequest();
                StartCoroutine(GenMapCoroutine());
                GameUIManager.Instance.UITimeCounter.OnTimeCounterEnd += InputManager.SetDisablePlayerInput;
                GameUIManager.Instance.UITimeCounter.OnTimeCounterEnd += InputManager.SetDisableMonsterInput;
                
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
                BattleManager.ArenaManager.transform.position = new Vector2(BattleManager.ArenaManager.transform.position.x,
                        tilesTransform.position.y + RuneManager.GetHeightRunesMap()/2 +BattleManager.ArenaManager.GetArenaHeight()/2);
                BattleManager.TurnManager.ActionLine.SetActionLinePostion(new Vector2(BattleManager.TurnManager.ActionLine.transform.position.x,
                        BattleManager.ArenaManager.transform.position.y-BattleManager.ArenaManager.GetArenaHeight()/2 + BattleManager.TurnManager.ActionLine.GetActionLineHeight()/2 ));
                GameUIManager.Instance.UIRunePointManager.transform.position = BattleManager.TurnManager.ActionLine.transform.position + new Vector3(0, 0.5f, 0);
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