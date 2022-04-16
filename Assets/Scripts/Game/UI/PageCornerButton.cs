using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    [RequireComponent(typeof(Image))]
    public class PageCornerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        // PageCornerButton uses PageController
        [SerializeField]
        private PageController pageController;

        [SerializeField]
        private Image pageImage;
        [SerializeField]
        private Sprite pageSprite;
        [SerializeField]
        private Sprite pageCornerSprite;

        [SerializeField]
        private Image bottomDecorImage;
        [SerializeField]
        private Sprite bottomDecorCutSprite;
        private Sprite bottomDecorOriginalSprite;

        public UnityEvent OnClick;
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;

        public bool MouseHover { get; set; }

        public void Awake()
        {
            pageController.OnConstruct.AddListener(Construct);
        }

        public void Construct()
        {
            bottomDecorOriginalSprite = bottomDecorImage.sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Pointer clicked page corner");
            if (!pageController.CanFlip)
                return;

            pageImage.sprite = pageSprite;
            bottomDecorImage.sprite = bottomDecorOriginalSprite;

            OnClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Pointer enetered page corner");
            if (!pageController.CanFlip)
                return;

            pageImage.sprite = pageCornerSprite;
            bottomDecorImage.sprite = bottomDecorCutSprite;

            MouseHover = true;
            OnHoverEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Pointer exited page corner");
            if (!pageController.CanFlip)
                return;

            pageImage.sprite = pageSprite;
            bottomDecorImage.sprite = bottomDecorOriginalSprite;

            MouseHover = false;
            OnHoverExit.Invoke();
        }
    }
}