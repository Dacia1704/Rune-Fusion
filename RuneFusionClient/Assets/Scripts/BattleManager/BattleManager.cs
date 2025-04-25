using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
        public static BattleManager Instance { get; private set; }
        [field: SerializeField] public MonsterListSO MonsterListSO {get; private set;}
        public TurnManager TurnManager {get; private set;}
        public ArenaManager ArenaManager { get; private set; }
        public TargetManager TargetManager { get; private set; }
        
        public Dictionary<string,MonsterBase> MonsterTeam1Dictionary = new Dictionary<string, MonsterBase>();
        public Dictionary<string,MonsterBase> MonsterTeam2Dictionary = new Dictionary<string, MonsterBase>();
        
        public Dictionary<string,int> ShieldAllyMonsters { get; private set; } = new Dictionary<string,int>();
        public Dictionary<string,int> ShieldOpponentMonsters { get; private set; } = new Dictionary<string,int>();
        

        [HideInInspector]public bool CanChangeTurn;
        private AudioSource audioSource;

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }
                audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
                TurnManager = FindFirstObjectByType<TurnManager>();
                ArenaManager = FindFirstObjectByType<ArenaManager>();
                TargetManager = FindFirstObjectByType<TargetManager>();
                GameManager.Instance.InputManager.OnMonsterTarget += TargetManager.TargetMonster;
                GameManager.Instance.InputManager.OnMonsterAllyDoubleClick += SkillInputManager;
                GameManager.Instance.InputManager.OnShieldTarget += ShieldMonster;
                
                BGMManager.Instance.PlayBGMCombat();
                
        }

        public void SetUpMonster(MonsterListData monsterListData)
        {
                GameObject monster1Team1 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player1[0].id].Prefab,ArenaManager.MonsterTeam1.StartPosList[0].transform.position, Quaternion.identity);
                monster1Team1.GetComponentInChildren<SpriteRenderer>().flipX = false;
                MonsterTeam1Dictionary.Add(monsterListData.player1[0].id_in_battle, monster1Team1.GetComponent<MonsterBase>());
                ShieldAllyMonsters.Add(monsterListData.player1[0].id_in_battle, 0);
                MonsterTeam1Dictionary[monsterListData.player1[0].id_in_battle].MonsterIndexinBattle = 0;
                MonsterTeam1Dictionary[monsterListData.player1[0].id_in_battle].MonsterIdInBattle = monsterListData.player1[0].id_in_battle ;
                MonsterTeam1Dictionary[monsterListData.player1[0].id_in_battle].transform.SetParent(ArenaManager.transform);
                
                GameObject monster2Team1 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player1[1].id].Prefab,ArenaManager.MonsterTeam1.StartPosList[1].transform.position, Quaternion.identity);
                monster2Team1.GetComponentInChildren<SpriteRenderer>().flipX = false;
                MonsterTeam1Dictionary.Add(monsterListData.player1[1].id_in_battle, monster2Team1.GetComponent<MonsterBase>());
                ShieldAllyMonsters.Add(monsterListData.player1[1].id_in_battle, 0);
                MonsterTeam1Dictionary[monsterListData.player1[1].id_in_battle].MonsterIndexinBattle = 1;
                MonsterTeam1Dictionary[monsterListData.player1[1].id_in_battle].MonsterIdInBattle = monsterListData.player1[1].id_in_battle;
                MonsterTeam1Dictionary[monsterListData.player1[1].id_in_battle].transform.SetParent(ArenaManager.transform);
                
                GameObject monster3Team1 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player1[2].id].Prefab,ArenaManager.MonsterTeam1.StartPosList[2].transform.position, Quaternion.identity);
                monster3Team1.GetComponentInChildren<SpriteRenderer>().flipX = false;
                MonsterTeam1Dictionary.Add(monsterListData.player1[2].id_in_battle, monster3Team1.GetComponent<MonsterBase>());
                ShieldAllyMonsters.Add(monsterListData.player1[2].id_in_battle, 0);
                MonsterTeam1Dictionary[monsterListData.player1[2].id_in_battle].MonsterIndexinBattle = 2;
                MonsterTeam1Dictionary[monsterListData.player1[2].id_in_battle].MonsterIdInBattle = monsterListData.player1[2].id_in_battle;
                MonsterTeam1Dictionary[monsterListData.player1[2].id_in_battle].transform.SetParent(ArenaManager.transform);
                
                GameObject monster1Team2 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player2[0].id].Prefab,ArenaManager.MonsterTeam2.StartPosList[0].transform.position, Quaternion.identity);
                monster1Team2.GetComponentInChildren<SpriteRenderer>().flipX = true;
                MonsterTeam2Dictionary.Add(monsterListData.player2[0].id_in_battle, monster1Team2.GetComponent<MonsterBase>());
                MonsterTeam2Dictionary[monsterListData.player2[0].id_in_battle].MonsterIndexinBattle = 0;
                ShieldOpponentMonsters.Add(monsterListData.player2[0].id_in_battle, 0);
                MonsterTeam2Dictionary[monsterListData.player2[0].id_in_battle].MonsterIdInBattle = monsterListData.player2[0].id_in_battle;
                MonsterTeam2Dictionary[monsterListData.player2[0].id_in_battle].transform.SetParent(ArenaManager.transform);
                
                GameObject monster2Team2 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player2[1].id].Prefab,ArenaManager.MonsterTeam2.StartPosList[1].transform.position, Quaternion.identity);
                monster2Team2.GetComponentInChildren<SpriteRenderer>().flipX = true;
                MonsterTeam2Dictionary.Add(monsterListData.player2[1].id_in_battle, monster2Team2.GetComponent<MonsterBase>());
                MonsterTeam2Dictionary[monsterListData.player2[1].id_in_battle].MonsterIndexinBattle = 1;
                ShieldOpponentMonsters.Add(monsterListData.player2[1].id_in_battle, 0);
                MonsterTeam2Dictionary[monsterListData.player2[1].id_in_battle].MonsterIdInBattle = monsterListData.player2[1].id_in_battle;
                MonsterTeam2Dictionary[monsterListData.player2[1].id_in_battle].transform.SetParent(ArenaManager.transform);
                
                GameObject monster3Team2 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player2[2].id].Prefab,ArenaManager.MonsterTeam2.StartPosList[2].transform.position, Quaternion.identity);
                monster3Team2.GetComponentInChildren<SpriteRenderer>().flipX = true;
                MonsterTeam2Dictionary.Add(monsterListData.player2[2].id_in_battle, monster3Team2.GetComponent<MonsterBase>());
                MonsterTeam2Dictionary[monsterListData.player2[2].id_in_battle].MonsterIndexinBattle = 2;
                ShieldOpponentMonsters.Add(monsterListData.player2[2].id_in_battle, 0);
                MonsterTeam2Dictionary[monsterListData.player2[2].id_in_battle].MonsterIdInBattle = monsterListData.player2[2].id_in_battle;
                MonsterTeam2Dictionary[monsterListData.player2[2].id_in_battle].transform.SetParent(ArenaManager.transform);
                
                SocketManager.Instance.RequestPointInit();
                if (TurnManager.PlayerIndex == 0)
                {
                        foreach (MonsterBase monsterBase in MonsterTeam2Dictionary.Values)
                        {
                                monsterBase.SetOpponent();
                        }
                        foreach (MonsterBase monsterBase in MonsterTeam1Dictionary.Values)
                        {
                                monsterBase.SetAlly();
                        }
                }
                else
                {
                        foreach (MonsterBase monsterBase in MonsterTeam1Dictionary.Values)
                        {
                                monsterBase.SetOpponent();
                        }
                        foreach (MonsterBase monsterBase in MonsterTeam2Dictionary.Values)
                        {
                                monsterBase.SetAlly();
                        }
                }
        }

        public void SkillInputManager(MonsterBase monster)
        {
                if (TurnManager.TurnBaseQueue[0].id_in_battle == monster.MonsterIdInBattle && monster.MonsterPropsSO.MonsterData.Skills[1].PointCost <= GameManager.Instance.RuneManager.RunePointsPlayer[(int)monster.MonsterPropsSO.MonsterData.Type])
                {
                        monster.EnableSkillMode();
                        Debug.Log("Enable skill" + monster.gameObject.name);
                }
        }

        public void ShieldMonster(MonsterBase monster)
        {
                if (monster == null)
                {
                        GameUIManager.Instance.UIRunePointManager.UIShieldRunePoint.ChangeNotUseShieldPointApperance();
                }
                else
                {
                        SocketManager.Instance.PushUseShieldData(monster.MonsterIdInBattle);
                }
        }
        public void UpdateMonsterShield(UseShieldData useShieldData)
        {
                MonsterBase monster = GetMonsterByIdInBattle(useShieldData.monster_id_in_battle);
                int shieldRunePoint = SocketManager.Instance.PlayerData.id == useShieldData.player_id
                        ? GameManager.Instance.RuneManager.RunePointsPlayer[(int)RuneType.Shield]
                        : GameManager.Instance.RuneManager.RunePointsOpponent[(int)RuneType.Shield];
                monster.EnableShield(shieldRunePoint);
                GameManager.Instance.RuneManager.ReleaseShieldRunePoint(useShieldData);
        }

        public MonsterBase GetMonsterByIdInBattle(string id)
        {
                if (MonsterTeam1Dictionary.ContainsKey(id)) return MonsterTeam1Dictionary[id];
                if (MonsterTeam2Dictionary.ContainsKey(id)) return MonsterTeam2Dictionary[id];
                return null;
        }

        public bool CheckAnimationMonster()
        {
                if (MonsterTeam1Dictionary["11"].IsAllAnimationEnd &&
                    MonsterTeam1Dictionary["12"].IsAllAnimationEnd &&
                    MonsterTeam1Dictionary["13"].IsAllAnimationEnd &&
                    MonsterTeam2Dictionary["21"].IsAllAnimationEnd &&
                    MonsterTeam2Dictionary["22"].IsAllAnimationEnd &&
                    MonsterTeam2Dictionary["23"].IsAllAnimationEnd)
                {
                        SetFalseAnimation();
                        return true;
                }
                return false;
        }

        public void SetStartTurnMonsterAnimation(MonsterActionResponse monsterActionResponse,int turn)
        {
                SetTrueAnimation();
                foreach (ActionResponse monster in monsterActionResponse.action_affect_list[turn])
                {
                        GetMonsterByIdInBattle(monster.id_in_battle).IsAllAnimationEnd = false;
                }
                GetMonsterByIdInBattle(monsterActionResponse.monster_id).IsAllAnimationEnd = false;
        }

        public void SetFalseAnimation()
        {
                MonsterTeam1Dictionary["11"].IsAllAnimationEnd = false;
                MonsterTeam1Dictionary["12"].IsAllAnimationEnd = false;
                MonsterTeam1Dictionary["13"].IsAllAnimationEnd = false;
                MonsterTeam2Dictionary["21"].IsAllAnimationEnd = false;
                MonsterTeam2Dictionary["22"].IsAllAnimationEnd = false;
                MonsterTeam2Dictionary["23"].IsAllAnimationEnd = false;
        }

        public void SetTrueAnimation()
        {
                MonsterTeam1Dictionary["11"].IsAllAnimationEnd = true;
                MonsterTeam1Dictionary["12"].IsAllAnimationEnd = true;
                MonsterTeam1Dictionary["13"].IsAllAnimationEnd = true;
                MonsterTeam2Dictionary["21"].IsAllAnimationEnd = true;
                MonsterTeam2Dictionary["22"].IsAllAnimationEnd = true;
                MonsterTeam2Dictionary["23"].IsAllAnimationEnd = true;
        }

        public void UpdateMonsterEffect(UpdateEffectResponse monsterEffect)
        {
                StartCoroutine(GetMonsterByIdInBattle(monsterEffect.id_in_battle).UpdateEffect(monsterEffect.dam, monsterEffect.effect_list));
        }

        public MonsterBase AutoChooseTargetMonster(string monsterId)
        {
                MonsterBase weakestMonster = null;
                if (MonsterTeam1Dictionary.ContainsKey(monsterId))
                {
                        weakestMonster = MonsterTeam2Dictionary.First().Value;
                        foreach (MonsterBase monster in MonsterTeam2Dictionary.Values)
                        {
                                if (monster.MonsterStatsInBattle.Health < weakestMonster.MonsterStatsInBattle.Health)
                                {
                                        weakestMonster = monster;
                                }
                        }
                }
                else
                {
                        weakestMonster = MonsterTeam1Dictionary.First().Value;
                        foreach (MonsterBase monster in MonsterTeam1Dictionary.Values)
                        {
                                if (monster.MonsterStatsInBattle.Health < weakestMonster.MonsterStatsInBattle.Health)
                                {
                                        weakestMonster = monster;
                                }
                        }
                }

                return weakestMonster;
        }

        

        
        
        
        
}