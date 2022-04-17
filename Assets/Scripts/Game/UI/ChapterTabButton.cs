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
    public class ChapterTabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private AudioSource hoverAudio;
        [SerializeField]
        private AudioSource clickAudio;

        public UnityEvent OnClick;
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;

        public bool MouseHover { get; set; }
        
        private bool selected;
        public bool Selected { get => selected; 
            set 
            {
                selected = value;
                animator.SetBool("stickout", value);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            animator.Play("Tab Bump");
            clickAudio.Play();
            OnClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverAudio.Play();
            animator.SetBool("stickout", true);
          
            MouseHover = true;
            OnHoverEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Selected)
                animator.SetBool("stickout", false);

            MouseHover = false;
            OnHoverExit.Invoke();
        }
    }
}