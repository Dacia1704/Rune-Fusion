using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public static GameManager Instance {get; private set;}
        [field: SerializeField] public GameManagerSO GameManagerSO { get; private set; }

        [field: SerializeField]public MatchBoard MatchBoard {get; private set;}
        [field: SerializeField]public InputManager InputManager {get; private set;}
        [field: SerializeField] public Match Match {get; private set;}
        private AudioSource audioSource;
        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;

                MatchBoard = FindFirstObjectByType<MatchBoard>();
                InputManager = FindFirstObjectByType<InputManager>();
                Match = FindFirstObjectByType<Match>();
                audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
                SocketManager.Instance.GameStartRequest();
                StartCoroutine(GenMapCoroutine());
                GameUIManager.Instance.UITimeCounter.OnTimeCounterEnd += InputManager.SetDisablePlayerInput;
                GameUIManager.Instance.UITimeCounter.OnTimeCounterEnd += InputManager.SetDisableMonsterInput;
                PlayStartGameSound();

        }

        private IEnumerator GenMapCoroutine()
        {
                yield return new WaitUntil(() => SocketManager.Instance.MapStart != null);
                GameManagerSO.SetHeightRuneMap(SocketManager.Instance.MapStart.Count);
                GameManagerSO.SetWidthRuneMap(SocketManager.Instance.MapStart[0].Count);
                MatchBoard.GenerateRunesMap(SocketManager.Instance.MapStart);
                SetUpTilePosition();
        }

        public void SetUpTilePosition()
        {
                Transform tilesTransform = MatchBoard.transform;
                tilesTransform.position = new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + MatchBoard.GetHeightRunesMap()/2) ;
                MatchBoard.UpdateRunesPostionMap();
                Match.ArenaManager.transform.position = new Vector2(Match.ArenaManager.transform.position.x,
                        tilesTransform.position.y + MatchBoard.GetHeightRunesMap()/2 +Match.ArenaManager.GetArenaHeight()/2);
                Match.TurnManager.ActionLine.SetActionLinePostion(new Vector2(Match.TurnManager.ActionLine.transform.position.x,
                        Match.ArenaManager.transform.position.y-Match.ArenaManager.GetArenaHeight()/2 + Match.TurnManager.ActionLine.GetActionLineHeight()/2 ));
                GameUIManager.Instance.UIRunePointManager.transform.position = Match.TurnManager.ActionLine.transform.position + new Vector3(0, 0.5f, 0);
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

        public void PlayStartGameSound()
        {
                audioSource.clip = AudioManager.Instance.AudioPropsSO.StartGameSound;
                audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
                audioSource.Play();
        }
        
}