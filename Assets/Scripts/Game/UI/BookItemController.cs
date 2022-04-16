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
    public class BookItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
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

        public InventoryItem ItemData { get; set; }

        private Transform originalParent;
        private Vector2 originalPosition;

        private bool dragging;
        private Coroutine moveBackCoroutine;

        public void Construct(InventoryItem newItemData)
        {
            ItemData = newItemData;
            image.sprite = newItemData.Icon;
        }

        private Vector2 GetMousePos()
        {
            return Input.mousePosition;
        }

        private float time = 0;

        private void Update()
        {
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
                moveBackCoroutine = StartCoroutine(MoveBackEnum());
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                dragging = true;

                originalPosition = transform.position;
                originalParent = transform.parent;
                transform.SetParent(BookItemsManager.Instance.DragDropHolder);
                time = 0;

                pickUpSfx.Play();
                animator.Play("Item Shake");

                if (moveBackCoroutine != null)
                    StopCoroutine(moveBackCoroutine);
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
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            BookItemsManager.Instance.DisplayTooltip(ItemData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            BookItemsManager.Instance.ClearTooltip();
        }
    }
}