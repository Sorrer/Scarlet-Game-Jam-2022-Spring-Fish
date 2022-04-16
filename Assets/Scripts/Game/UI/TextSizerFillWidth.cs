using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    [ExecuteAlways]
    public class TextSizerFillWidth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private TextSizer textSizer;
        [SerializeField]
        private RectTransform targetWidthTrans;
        [SerializeField]
        private Vector2 leftRightPadding;
        [SerializeField]
        private float minWidth;

        public void Update()
        {
            if (textSizer != null && targetWidthTrans != null)
            {
                var newMaxSize = new Vector2(Mathf.Max(minWidth, targetWidthTrans.sizeDelta.x - leftRightPadding.x - leftRightPadding.y), textSizer.maxSize.y);
                if (textSizer.maxSize != newMaxSize)
                {
                    textSizer.maxSize = newMaxSize;
                    textSizer.Refresh();
                }
            }
        }

        public void Refresh()
        {
            Update();
        }
    }
}