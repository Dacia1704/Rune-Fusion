using UnityEngine;

public class RuneObjectPoolManager : MonoBehaviour
{
        [field: SerializeField] public ObjectPooling BasicRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling ProtectedRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling PoisonRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling VerticalRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling HorizontalRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling ExplosiveRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling SpecialRuneObjectPooling { get; private set; }
        
        //object
        public GameObject GetBasicRuneObjectFromIndex(int index)
        {
                return BasicRuneObjectPooling.GetObject(GetBasicRuneKeyFromIndex(index));
        }
        public GameObject GetHorizontalRuneObjectFromIndex(int index)
        {
                return HorizontalRuneObjectPooling.GetObject(GetHorizontalRuneKeyFromIndex(index));
        }
        public GameObject GetVerticalRuneObjectFromIndex(int index)
        {
                return VerticalRuneObjectPooling.GetObject(GetVerticalRuneKeyFromIndex(index));
        }
        
        
        
        // key
        private string GetBasicRuneKeyFromIndex(int index)
        {
                return "Basic" + ((RuneType)index).ToString();
        }
        private string GetHorizontalRuneKeyFromIndex(int index)
        {
                return "Horizontal" + ((RuneType)index).ToString();
        }
        private string GetVerticalRuneKeyFromIndex(int index)
        {
                return "Vertical" + ((RuneType)index).ToString();
        }
        
        
        
}