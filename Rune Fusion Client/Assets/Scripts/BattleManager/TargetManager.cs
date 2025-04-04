using UnityEngine;

public class TargetManager: MonoBehaviour
{
    [SerializeField]public GameObject targetPrefab;

    private GameObject targetGo;
    
    public MonsterBase TargetedMonster { get; private set; }

    public void TargetMonster(MonsterBase monster)
    {
        TargetedMonster = monster;
        if (targetGo == null)
        {
            targetGo = Instantiate(targetPrefab);
        }
        targetGo.SetActive(true);
        targetGo.transform.position = TargetedMonster.transform.position;
    }

    public void DisableTarget()
    {
        if (targetGo == null) return;
        TargetedMonster = null;
        targetGo.SetActive(false);
    }

}