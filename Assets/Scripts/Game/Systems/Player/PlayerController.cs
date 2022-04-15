using System.Collections.Generic;
using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Systems.Player
{
    public class PlayerController : StandaloneInputModule
    {

        public DayNightManager dayNightManager;
        
        public PlayerCursorData data;

        public CursorInteractor interactor;
        
        public LockedLookCamera cameraController;

        public bool isFocused = false;

        public RectTransform CursorTransform;

        public float Sensitivity = 5;
        
        [SerializeField] private Vector3 lastMousePosition;

        private PointerEventData pointerData; 
        
        protected override void Start()
        {
            base.Start();

            pointerData = new PointerEventData(EventSystem.current);
            
            CursorTransform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        }

        // Update is called once per frame
        void Update()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (Application.isFocused)
            {
                var newPos = CursorTransform.position + (new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * Sensitivity);
                newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
                newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

                CursorTransform.position = newPos;
                data.position = CursorTransform.position;
            }
            
            /* FUNCTIONALITY MOVED TO STATES
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isFocused = !isFocused;
                
                if (isFocused)
                {
                    Focus();
                }
                else
                {
                    Unfocus();
                }
            }
            */

            if (Input.GetKeyDown(KeyCode.N))
            {
                dayNightManager.Progress();
            }
            
            UpdateMovement();
        }


        
        
        
        public void UpdateMovement()
        {
            if (pointerData != null)
            {
                pointerData.delta = (Vector2) data.position - pointerData.position;
                pointerData.position = data.position;

                var raycastResults = new List<RaycastResult>();
                eventSystem.RaycastAll(pointerData, raycastResults);
                pointerData.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
                eventSystem.SetSelectedGameObject(pointerData.pointerCurrentRaycast.gameObject);
                
                ProcessMove(pointerData);

                MouseButtonEventData buttonEventData = new MouseButtonEventData();
                buttonEventData.buttonData = pointerData;
                buttonEventData.buttonState = PointerEventData.FramePressState.NotChanged;
                
                if (Input.GetMouseButtonDown(0))
                {
                    buttonEventData.buttonState = PointerEventData.FramePressState.Pressed;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    buttonEventData.buttonState = PointerEventData.FramePressState.Released;
                }
                
                ProcessMousePress(buttonEventData);
            }
        }
        
        private void Focus()
        {
            CursorTransform.position = lastMousePosition;
            cameraController.enabled = true;
            
            //TEMP
            interactor.Activate(LayerMask.GetMask("Selectable"));
        }

        private void Unfocus()
        {
            lastMousePosition = CursorTransform.position;
            cameraController.enabled = false;
            
            interactor.Deactivate();
        }
    }
}
