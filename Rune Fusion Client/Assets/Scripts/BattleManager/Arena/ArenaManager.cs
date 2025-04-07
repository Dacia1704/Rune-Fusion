using System;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [field: SerializeField]public MonsterPosition MonsterTeam1 { get; private set; }
    [field: SerializeField]public MonsterPosition MonsterTeam2 { get; private set; }
    private SpriteRenderer arenaSpriteRenderer;

    private void Awake()
    {
        arenaSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public float GetArenaHeight()
    {
        return arenaSpriteRenderer.bounds.size.y;
    }

    public float GetArenaWidth()
    {
        return arenaSpriteRenderer.bounds.size.x;
    }
}
