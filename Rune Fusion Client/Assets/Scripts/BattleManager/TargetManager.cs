using UnityEngine;

public class TargetManager: MonoBehaviour
{
    public GameObject TargetPrefab;

    private GameObject targetGo;
    
    public MonsterBase TargetedMonster { get; private set; }

    public void TargetMonster(MonsterBase monster)
    {
        TargetedMonster = monster;
        if (targetGo == null)
        {
            targetGo = Instantiate(TargetPrefab);
        }
        if (monster == null)
        {
            targetGo.SetActive(false);
        }
        targetGo.SetActive(true);
        targetGo.transform.position = TargetedMonster.transform.position;
    }

    public void DisableTarget()
    {
        targetGo.SetActive(false);
    }

}