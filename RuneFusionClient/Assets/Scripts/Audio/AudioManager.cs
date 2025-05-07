using System;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
        public static AudioManager Instance { get; private set; }
        [field: SerializeField] public AudioPropsSO AudioPropsSO { get; private set; }
        

        private void Awake()
        {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
        }
        
        
        
        
}