using UnityEngine;

public class RuneObjectPoolManager : MonoBehaviour
{
        [field: SerializeField] public ObjectPooling BasicRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling PoisonRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling VerticalRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling HorizontalRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling ExplosiveRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling SpecialRuneObjectPooling { get; private set; }
        [field: SerializeField] public ObjectPooling RuneDestroyEffectObjectPooling {get; private set;}
        
        //release
        public void ReleaseRune(GameObject runeObj)
        {
                Rune rune = runeObj.GetComponent<Rune>();
                switch (rune.Form)
                {
                        case RuneForm.Base: BasicRuneObjectPooling.ReleaseObject(runeObj); break;
                        case RuneForm.Poison: PoisonRuneObjectPooling.ReleaseObject(runeObj); break;
                        case RuneForm.Special: SpecialRuneObjectPooling.ReleaseObject(runeObj); break;
                        case RuneForm.Vertical: VerticalRuneObjectPooling.ReleaseObject(runeObj); break;
                        case RuneForm.Horizontal: HorizontalRuneObjectPooling.ReleaseObject(runeObj); break;
                        case RuneForm.Explosive: ExplosiveRuneObjectPooling.ReleaseObject(runeObj); break;
                }
        }

        public void ReleaseDestroyEffect(GameObject runeObj)
        {
                RuneDestroyEffectObjectPooling.ReleaseObject(runeObj);
        } 
        
        
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

        public GameObject GetExplosiveRuneObjectFromIndex(int index)
        {
                return ExplosiveRuneObjectPooling.GetObject(GetExplosiveRuneKeyFromIndex(index));
        }

        public GameObject GetSpecialRuneObjectFromIndex()
        {
                return SpecialRuneObjectPooling.GetObject(GetSpecialRuneKey());
        }

        public GameObject GetRuneDestroyEffectObject(int runeType, int runeForm)
        {
                return RuneDestroyEffectObjectPooling.GetObject(GetRuneDestroyEffectKey(runeType, runeForm));
        }

        public GameObject GetProtectDestroyEffectObject()
        {
                return RuneDestroyEffectObjectPooling.GetObject(GetProtectDestroyEffectKey());
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

        private string GetExplosiveRuneKeyFromIndex(int index)
        {
                return "Explosive" + ((RuneType)index).ToString();
        }

        private string GetSpecialRuneKey()
        {
                return "Special";
        }

        private string GetRuneDestroyEffectKey(int runeType, int runeForm)
        {
                switch (runeForm)
                {
                        case (int)RuneForm.Horizontal: return "HorizontalRuneDestroyEffect"; 
                        case (int)RuneForm.Vertical: return "VerticalRuneDestroyEffect"; 
                        case (int)RuneForm.Explosive: return "ExplosiveRuneDestroyEffect"; 
                        case (int)RuneForm.Special: return "SpecialRuneDestroyEffect"; 
                        case (int)RuneForm.Base: return ((RuneType)runeType).ToString() + "RuneDestroyEffect"; 
                }

                return "";


        }

        private string GetProtectDestroyEffectKey()
        {
                return "ProtectDestroyEffect";
        }
        
        
        
}