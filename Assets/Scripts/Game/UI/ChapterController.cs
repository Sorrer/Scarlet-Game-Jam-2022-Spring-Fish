using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class ChapterController : MonoBehaviour
    {
        private List<GameObject> pageContentList = new List<GameObject>();

        public List<GameObject> PageContentList { get => pageContentList; }

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                pageContentList.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            foreach (GameObject content in pageContentList)
                if (content.transform.parent == null)
                    content.transform.SetParent(transform);
        }
    }
}