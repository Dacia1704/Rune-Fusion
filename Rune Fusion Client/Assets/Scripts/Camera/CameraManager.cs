using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
        public static CameraManager Instance;
        private void Awake()
        {
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