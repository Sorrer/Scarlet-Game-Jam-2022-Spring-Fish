using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class ChapterController : MonoBehaviour
    {
        public UnityEvent OnPreload;
        public UnityEvent OnLoad;
        public UnityEvent OnUnload;

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

        public void Preload()
        {
            OnPreload.Invoke();
        }

        public void Load()
        {
            OnLoad.Invoke();
        }

        public void Unload()
        {
            OnUnload.Invoke();
        }
    }
}