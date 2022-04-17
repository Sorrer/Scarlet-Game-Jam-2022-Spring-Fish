using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Systems.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Dependencies")]
        [SerializeField]
        private Image image;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private AudioSource pickUpSfx;
        [SerializeField]
        private AudioSource putDownSfx;

        [Header("Settings")]
        [SerializeField]
        private AnimationCurve moveToMouseCurve;
        [SerializeField]
        private AnimationCurve moveBackCurve;
        [SerializeField]
        private float moveToMouseDuration;
        [SerializeField]
        private float moveBackDuration;

        [SerializeField]
        private InventoryItem itemData;
        public InventoryItem ItemData { get => itemData; set => itemData = value; }

        private Transform originalParent;
        private Vector3 originalLocalPos;

        private bool dragging;
        private Coroutine moveBackCoroutine;
        private float time = 0;
        private bool hover;
        private bool showingTooltip;

        public void Construct(InventoryItem newItemData)
        {
            ItemData = newItemData;
            image.sprite = newItemData.Icon;
        }

        public PlayerCursorData cursorData;
        
        private Vector3 GetMousePos()
        {
            return UIManager.Instance.ScreenToWorldPoint(cursorData.position);
        }

        private void Awake()
        {
            if (itemData != null)
                Construct(ItemData);
        }

        private void Update()
        {
            if (dragging || hover)
            {
                InventoryItemsManager.Instance.MoveTooltip(GetMousePos());
            }

            if (dragging)
            {
                if (time < moveToMouseDuration)
                {
                    transform.position = Vector3.Lerp(transform.position, GetMousePos(), moveToMouseCurve.Evaluate(time / moveToMouseDuration));
                    time += Time.deltaTime;
                } else 
                {
                    transform.position = GetMousePos();
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                dragging = false;

                
                if (showingTooltip)
                {
                    showingTooltip = false;
                    InventoryItemsManager.Instance.ClearTooltip();
                }

                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    pointerId = -1,
                };
                pointerData.position = cursorData.position;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        var slotController = result.gameObject.GetComponent<InventoryItemSlotController>();
                        if (slotController != null && slotController.TryAcceptItem(this)) 
                        {
                            if (originalParent != null)
                            {
                                var parentSlotController = originalParent.GetComponent<InventoryItemSlotController>();
                                // We've moved ourselves to a different slot, so we need to remove
                                // this item from it's original slot.
                                if (parentSlotController != null)
                                    parentSlotController.ItemController = null;
                            }
                            originalParent = slotController.transform;
                            originalLocalPos = transform.parent.InverseTransformPoint(slotController.transform.position);
                            break;
                        }
                    }
                }

                moveBackCoroutine = StartCoroutine(MoveBackEnum());
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                dragging = true;

                if (moveBackCoroutine != null)
                {
                    // We are interuppting the move back to pick it up.
                    // Therefore we do not need to set set original position or
                    // original parent, because that was all saved before.
                    StopCoroutine(moveBackCoroutine);
                } else
                {
                    originalParent = transform.parent;
                    transform.SetParent(InventoryItemsManager.Instance.DragDropHolder);
                    originalLocalPos = transform.localPosition;
                }
                time = 0;

                pickUpSfx.Play();
                animator.Play("Item Shake");
            }
        }

        public IEnumerator MoveBackEnum()
        {
            putDownSfx.Play();
            animator.Play("Item Shake");
            time = 0;
            while (time < moveBackDuration)
            {
                yield return new WaitForEndOfFrame();
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPos, moveBackCurve.Evaluate(time / moveBackDuration));
                time += Time.deltaTime;
            }
            transform.localPosition = originalLocalPos;
            transform.SetParent(originalParent, true);
            moveBackCoroutine = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hover = true;
            showingTooltip = true;
            InventoryItemsManager.Instance.DisplayTooltip(ItemData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;
            if (!dragging)
            {
                showingTooltip = false;
                InventoryItemsManager.Instance.ClearTooltip();
            }
        }
    }
}