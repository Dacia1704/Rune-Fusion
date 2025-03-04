using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "GameManagerSO", order = 0)]
public class GameManagerSO : ScriptableObject
{
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
}