using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Dependencies")]
        [SerializeField]
        private Camera uiCamera;
        [SerializeField]
        private Canvas uiCanvas;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public Vector3 ScreenToWorldPoint(Vector2 screenPos)
        {
            var screenPos3D = (Vector3) screenPos;
            screenPos3D.z = uiCanvas.planeDistance;
            return uiCamera.ScreenToWorldPoint(screenPos3D);
        }

        public Vector3 GlobalToUIPosition(Vector3 position)
        {
            return position - transform.position;
        }

        public Vector3 UIToGlobalPosition(Vector3 relativePosition)
        {
            return transform.position + relativePosition;
        }
    }
}