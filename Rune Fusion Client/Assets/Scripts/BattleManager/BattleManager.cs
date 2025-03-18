using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
        public static BattleManager Instance { get; private set; }
        [field: SerializeField]public TurnManager TurnManager {get; private set;}
        private ArenaManager arenaManager;
        
        [Header("Test")]
        public GameObject[] MonsterTeam1;
        public GameObject[] MonsterTeam2;
        
        public Dictionary<string,MonsterBase> MonsterTeam1Dictionary = new Dictionary<string, MonsterBase>();
        public Dictionary<string,MonsterBase> MonsterTeam2Dictionary = new Dictionary<string, MonsterBase>();
        
        

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }
        }

        private void Start()
        {
                TurnManager = FindFirstObjectByType<TurnManager>();
                arenaManager = FindFirstObjectByType<ArenaManager>();
                SetUpMonster();

                // StartCoroutine(StartAttack());
        }

        private void SetUpMonster()
        {
                GameObject monster1Team1 = Instantiate(MonsterTeam1[0],arenaManager.MonsterTeam1.Pos1.transform.position, Quaternion.identity);
                MonsterTeam1Dictionary.Add("11", monster1Team1.GetComponent<MonsterBase>());
                GameObject monster2Team1 = Instantiate(MonsterTeam1[1],arenaManager.MonsterTeam1.Pos2.transform.position, Quaternion.identity);
                MonsterTeam1Dictionary.Add("12", monster2Team1.GetComponent<MonsterBase>());
                GameObject monster3Team1 = Instantiate(MonsterTeam1[2],arenaManager.MonsterTeam1.Pos3.transform.position, Quaternion.identity);
                MonsterTeam1Dictionary.Add("13", monster3Team1.GetComponent<MonsterBase>());
                GameObject monster1Team2 = Instantiate(MonsterTeam2[0],arenaManager.MonsterTeam2.Pos1.transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2Dictionary.Add("21", monster1Team2.GetComponent<MonsterBase>());
                GameObject monster2Team2 = Instantiate(MonsterTeam2[1],arenaManager.MonsterTeam2.Pos2.transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2Dictionary.Add("22", monster2Team2.GetComponent<MonsterBase>());
                GameObject monster3Team2 = Instantiate(MonsterTeam2[2],arenaManager.MonsterTeam2.Pos3.transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2Dictionary.Add("23", monster3Team2.GetComponent<MonsterBase>());
        }



        public MonsterBase GetMonsterById(string id)
        {
                if (MonsterTeam1Dictionary.ContainsKey(id)) return MonsterTeam1Dictionary[id];
                if (MonsterTeam2Dictionary.ContainsKey(id)) return MonsterTeam2Dictionary[id];
                return null;
        }
        
        
}