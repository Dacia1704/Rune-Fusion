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
        
        public Dictionary<string,MonsterBase> MonsterTeam1Dictionary = new Dictionary<string, MonsterBase>();
        public Dictionary<string,MonsterBase> MonsterTeam2Dictionary = new Dictionary<string, MonsterBase>();
        

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }

                MonsterListSO.Initialize();
        }

        private void Start()
        {
                TurnManager = FindFirstObjectByType<TurnManager>();
                ArenaManager = FindFirstObjectByType<ArenaManager>();
                // MonsterListData monsterListData = new MonsterListData();
                // monsterListData.player1 = new List<MonsterData>
                // {
                //         new MonsterData
                //         {
                //                 id = 0,
                //                 id_in_battle = "11",
                //                 speed = 100,
                //         },
                //         new MonsterData
                //         {
                //                 id = 1,
                //                 id_in_battle = "12",
                //                 speed = 102,
                //         },
                //         new MonsterData
                //         {
                //                 id = 2,
                //                 id_in_battle = "13",
                //                 speed = 96,
                //         }
                // };
                // monsterListData.player2 = new List<MonsterData>
                // {
                //         new MonsterData
                //         {
                //                 id= 3,
                //                 id_in_battle= "21",
                //                 speed= 112,
                //         },
                //         new MonsterData
                //         {
                //                 id= 4,
                //                 id_in_battle= "22",
                //                 speed= 108,
                //         },
                //         new MonsterData
                //         {
                //                 id= 5,
                //                 id_in_battle= "23",
                //                 speed= 116,
                //         }
                // };
                // SetUpMonster(monsterListData);
                // StartCoroutine(StartAttack());
        }
        public void SetUpMonster(MonsterListData monsterListData)
        {
                GameObject monster1Team1 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player1[0].id].Prefab,ArenaManager.MonsterTeam1.StartPosList[0].transform.position, Quaternion.identity);
                MonsterTeam1Dictionary.Add(monsterListData.player1[0].id_in_battle, monster1Team1.GetComponent<MonsterBase>());
                MonsterTeam1Dictionary[monsterListData.player1[0].id_in_battle].MonsterIndexinBattle = 0;
                
                GameObject monster2Team1 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player1[1].id].Prefab,ArenaManager.MonsterTeam1.StartPosList[1].transform.position, Quaternion.identity);
                MonsterTeam1Dictionary.Add(monsterListData.player1[1].id_in_battle, monster2Team1.GetComponent<MonsterBase>());
                MonsterTeam1Dictionary[monsterListData.player1[1].id_in_battle].MonsterIndexinBattle = 1;
                
                GameObject monster3Team1 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player1[2].id].Prefab,ArenaManager.MonsterTeam1.StartPosList[2].transform.position, Quaternion.identity);
                MonsterTeam1Dictionary.Add(monsterListData.player1[2].id_in_battle, monster3Team1.GetComponent<MonsterBase>());
                MonsterTeam1Dictionary[monsterListData.player1[2].id_in_battle].MonsterIndexinBattle = 2;
                
                GameObject monster1Team2 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player2[0].id].Prefab,ArenaManager.MonsterTeam2.StartPosList[0].transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2Dictionary.Add(monsterListData.player2[0].id_in_battle, monster1Team2.GetComponent<MonsterBase>());
                MonsterTeam2Dictionary[monsterListData.player2[0].id_in_battle].MonsterIndexinBattle = 0;
                
                GameObject monster2Team2 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player2[1].id].Prefab,ArenaManager.MonsterTeam2.StartPosList[1].transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2Dictionary.Add(monsterListData.player2[1].id_in_battle, monster2Team2.GetComponent<MonsterBase>());
                MonsterTeam2Dictionary[monsterListData.player2[1].id_in_battle].MonsterIndexinBattle = 1;
                
                GameObject monster3Team2 = Instantiate(MonsterListSO.MonsterDictionary[monsterListData.player2[2].id].Prefab,ArenaManager.MonsterTeam2.StartPosList[2].transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2Dictionary.Add(monsterListData.player2[2].id_in_battle, monster3Team2.GetComponent<MonsterBase>());
                MonsterTeam2Dictionary[monsterListData.player2[2].id_in_battle].MonsterIndexinBattle = 2;
        }

        private IEnumerator StartAttack()
        {
                yield return new WaitForSeconds(1f);
                // MonsterTeam1Dictionary["11"].Attack(MonsterTeam2Dictionary["23"]);
                // MonsterTeam1Dictionary["12"].Attack(MonsterTeam2Dictionary["22"]);
                // MonsterTeam1Dictionary["13"].Attack(MonsterTeam2Dictionary["21"]);
                
                // MonsterTeam2Dictionary["21"].Attack(MonsterTeam1Dictionary["12"]);
                // MonsterTeam2Dictionary["22"].Attack(MonsterTeam1Dictionary["13"]);
                MonsterTeam2Dictionary["23"].Attack(MonsterTeam1Dictionary["11"]);
                
                yield return null;
        }


        public MonsterBase GetMonsterById(string id)
        {
                if (MonsterTeam1Dictionary.ContainsKey(id)) return MonsterTeam1Dictionary[id];
                if (MonsterTeam2Dictionary.ContainsKey(id)) return MonsterTeam2Dictionary[id];
                return null;
        }
        
        
}