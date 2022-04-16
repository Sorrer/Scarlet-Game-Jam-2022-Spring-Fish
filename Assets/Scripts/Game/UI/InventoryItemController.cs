using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private Vector2 originalPosition;

        private bool dragging;
        private Coroutine moveBackCoroutine;
        private float time = 0;
        private bool hover;

        public void Construct(InventoryItem newItemData)
        {
            ItemData = newItemData;
            image.sprite = newItemData.Icon;
        }

        private Vector2 GetMousePos()
        {
            return Input.mousePosition;
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
                    transform.position = Vector2.Lerp(transform.position, GetMousePos(), moveToMouseCurve.Evaluate(time / moveToMouseDuration));
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

                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    pointerId = -1,
                };
                pointerData.position = Input.mousePosition;

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
                            originalPosition = slotController.transform.position;
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
                    originalPosition = transform.position;
                    originalParent = transform.parent;
                    transform.SetParent(InventoryItemsManager.Instance.DragDropHolder);
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
                transform.position = Vector2.Lerp(transform.position, originalPosition, moveBackCurve.Evaluate(time / moveBackDuration));
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            transform.position = originalPosition;
            transform.SetParent(originalParent, true);
            moveBackCoroutine = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hover = true;
            InventoryItemsManager.Instance.DisplayTooltip(ItemData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;
            InventoryItemsManager.Instance.ClearTooltip();
        }
    }
}