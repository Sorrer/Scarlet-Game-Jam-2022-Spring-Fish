using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Book
{
    public class PageCornerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool MouseHover { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}