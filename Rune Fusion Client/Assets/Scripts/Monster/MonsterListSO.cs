using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MonsterListSO", menuName = "MonsterListSO", order = 0)]
public class MonsterListSO : ScriptableObject
{
        public List<MonsterSourceData> RawMonsterList;
        
        public Dictionary<int, MonsterSourceData> MonsterDictionary = new Dictionary<int, MonsterSourceData>();

        public void Initialize()
        {
                MonsterDictionary.Clear();
                foreach (MonsterSourceData monster in RawMonsterList)
                {
                        if (!MonsterDictionary.ContainsKey((int)monster.Id) )
                        {
                                MonsterDictionary.Add((int)monster.Id, monster);
                        }
                }
        }
}



