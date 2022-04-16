using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class ChapterController : MonoBehaviour
    {
        private List<Transform> pageContentTransforms = new List<Transform>();

        private void Awake()
        {
            foreach (Transform child in transform)
                pageContentTransforms.Add(child);
        }

        private void Update()
        {
            foreach (Transform content in pageContentTransforms)
                if (content.parent == null)
                    content.SetParent(transform);
        }
    }
}