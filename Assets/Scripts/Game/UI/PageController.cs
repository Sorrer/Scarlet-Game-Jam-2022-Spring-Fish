using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class PageController : MonoBehaviour
    {
        public enum PageType
        {
            Left,
            Right
        }

        public PageType Type;
        public UnityEvent OnConstruct;

        private GameObject content;
        [Header("Dependencies")]
        [SerializeField]
        private TextMeshProUGUI pageNumberText;

        [SerializeField]
        private RectTransform pageContentAndDecorHolder;
        [SerializeField]
        private RectTransform contentHolder;

        [SerializeField]
        private Image bottomDecorImage;
        [SerializeField]
        private Sprite bottomDecorSprite;
        [SerializeField]
        private Sprite bottomDecorFlippableSprite;

        public bool CanFlip { get; set; }

        public void Construct(int pageIdx, bool canFlip, GameObject newContent, bool reversed = false)
        {
            if (content != null)
                Destroy(content);
            content = newContent;

            if (reversed)
            {
                pageContentAndDecorHolder.localScale = new Vector2(-1, 1);
                pageContentAndDecorHolder.localPosition = new Vector2((Type == PageType.Left ? -1 : 1) * 600, 0);
            }
            else
            {
                pageContentAndDecorHolder.localScale = Vector2.one;
                pageContentAndDecorHolder.localPosition = Vector2.zero;
            }

            content.transform.SetParent(contentHolder, false);
            content.transform.localPosition = Vector2.zero;
            content.transform.localScale = Vector2.one;
            CanFlip = canFlip;

            bottomDecorImage.sprite = CanFlip ? bottomDecorFlippableSprite : bottomDecorSprite;
            pageNumberText.text = (pageIdx + 1).ToString();

            OnConstruct.Invoke();
        }
    }
}
