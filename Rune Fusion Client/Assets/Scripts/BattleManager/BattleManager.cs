using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
        public static BattleManager Instance { get; private set; }
        public GameObject[] MonsterTeam1;
        public GameObject[] MonsterTeam2;
        
        private ArenaManager arenaManager;

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;
                }
        }

        private void Start()
        {
                arenaManager = FindFirstObjectByType<ArenaManager>();
                SetUpPosition();

                // StartCoroutine(StartAttack());
        }

        private void SetUpPosition()
        {
                GameObject monster1Team1 = Instantiate(MonsterTeam1[0],arenaManager.MonsterTeam1.Pos1.transform.position, Quaternion.identity);
                MonsterTeam1[0] = monster1Team1;
                GameObject monster2Team1 = Instantiate(MonsterTeam1[1],arenaManager.MonsterTeam1.Pos2.transform.position, Quaternion.identity);
                MonsterTeam1[1] = monster2Team1;
                GameObject monster3Team1 = Instantiate(MonsterTeam1[2],arenaManager.MonsterTeam1.Pos3.transform.position, Quaternion.identity);
                MonsterTeam1[2] = monster3Team1;
                GameObject monster1Team2 = Instantiate(MonsterTeam2[0],arenaManager.MonsterTeam2.Pos1.transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2[0] = monster1Team2;
                GameObject monster2Team2 = Instantiate(MonsterTeam2[1],arenaManager.MonsterTeam2.Pos2.transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2[1] = monster2Team2;
                GameObject monster3Team2 = Instantiate(MonsterTeam2[2],arenaManager.MonsterTeam2.Pos3.transform.position, Quaternion.Euler(0, 180, 0));
                MonsterTeam2[2] = monster3Team2;
        }

        private IEnumerator StartAttack()
        {
                while (true)
                {
                        MonsterTeam1[0].GetComponent<Archer>().Attack(MonsterTeam2[1].transform);
                        MonsterTeam1[1].GetComponent<Archer>().Attack(MonsterTeam2[2].transform);
                        MonsterTeam1[2].GetComponent<Archer>().Attack(MonsterTeam2[0].transform);
                        MonsterTeam2[0].GetComponent<Archer>().Attack(MonsterTeam1[2].transform);
                        MonsterTeam2[1].GetComponent<Archer>().Attack(MonsterTeam1[1].transform);
                        MonsterTeam2[2].GetComponent<Archer>().Attack(MonsterTeam1[0].transform);
                        yield return new WaitForSeconds(2f);
                }
        }
        
        
}