using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
        public static CameraManager Instance {get; private set;}
        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;

        }
        public float GetWidthCamera()
        {
                float widthInUnits = GetHeightCamera() * Camera.main.aspect;
                return widthInUnits;
        }
        public float GetHeightCamera()
        {
                return 2f * Camera.main.orthographicSize;
        }


}