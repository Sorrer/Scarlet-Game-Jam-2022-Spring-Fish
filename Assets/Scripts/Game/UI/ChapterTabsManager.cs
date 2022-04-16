using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class ChapterTabsManager : MonoBehaviour
    {
        [SerializeField]
        private List<ChapterTabButton> tabs;

        public ChapterTabButton SelectedTab { get; set; }

        private void Awake()
        {
            foreach (var tab in tabs)
            {
                tab.OnClick.AddListener(delegate() 
                { 
                    OnTabClicked(tab); 
                });
            }

            if (tabs.Count > 0)
            {
                SelectedTab = tabs[0];
                SelectedTab.Selected = true;
                SelectedTab.OnClick.Invoke();
            }
        }

        private void OnTabClicked(ChapterTabButton tab)
        {
            if (SelectedTab != null)
                SelectedTab.Selected = false;
            SelectedTab = tab;
            SelectedTab.Selected = true;
        }

        public void LoadChapter()
        {
            // TODO
        }
    }
}