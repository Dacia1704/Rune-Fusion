using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MonsterColliderManager : MonoBehaviour
{
    private Collider2D monsterCollider;

    private void Awake()
    {
        monsterCollider = GetComponent<Collider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        
    }
}