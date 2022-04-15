using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Book
{
    public class BookController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField]
        private Animation animation;
        [SerializeField]
        private AnimationClip flipLeftClip;
        [SerializeField]
        private AnimationClip flipRightClip;

        [Header("Dependencies")]
        [SerializeField]
        private Transform leftPageTrans;
        [SerializeField]
        private Transform rightPageTrans;

        private PageController leftPageContent
        {
            get => leftPageTrans.childCount > 0 ? leftPageTrans.GetChild(0).GetComponent<PageController>() : null;
            set
            {
                if (leftPageContent != null)
                    Destroy(leftPageContent);
                value.transform.parent = leftPageTrans;

                // Reset local position and scale
                value.transform.position = Vector2.zero;
                value.transform.localScale = Vector2.one;
            }
        }
        private PageController rightPageContent
        {
            get => rightPageTrans.childCount > 0 ? rightPageTrans.GetChild(0).GetComponent<PageController>() : null;
            set
            {
                value.transform.parent = rightPageTrans;

                // Reset local position and scale
                value.transform.position = Vector2.zero;
                value.transform.localScale = Vector2.one;
            }
        }
        private PageController backLeftPageContent
        {
            get => backLeftPageTrans.childCount > 0 ? backLeftPageTrans.GetChild(0).GetComponent<PageController>() : null;
            set
            {
                value.transform.parent = backLeftPageTrans;

                // Reset local position and scale
                value.transform.position = Vector2.zero;
                value.transform.localScale = Vector2.one;
            }
        }
        private PageController backRIghtPageContent
        {
            get => backRIghtPageTrans.childCount > 0 ? backRIghtPageTrans.GetChild(0).GetComponent<PageController>() : null;
            set
            {
                if (backRIghtPageContent != null)
                    Destroy(backRIghtPageContent);
                value.transform.parent = backRIghtPageTrans;

                // Reset local position and scale
                value.transform.position = Vector2.zero;
                value.transform.localScale = Vector2.one;
            }
        }

        [SerializeField]
        private Transform backLeftPageTrans;
        [SerializeField]
        private Transform backRIghtPageTrans;
        [SerializeField]
        private GameObject emptyPageContentPrefab;

        [Header("Settings")]
        [SerializeField]
        private List<GameObject> pageContentList;
        [SerializeField]
        private int currPageIdx;

        public List<GameObject> PageContentList { get => pageContentList; set => pageContentList = value; }
        public int CurrPageIdx { get => currPageIdx; set => currPageIdx = value; }
        public GameObject CurrPageContent => PageContentList[CurrPageIdx];

        private Coroutine flipCoroutine;
        private PageController leftPageContentInst;
        private PageController rightPageContentInst;

        public void Construct(IEnumerable<GameObject> newPageContentList)
        {
            PageContentList.Clear();
            pageContentList.AddRange(newPageContentList);
            // Make sure we always have an even count of pages
            if (pageContentList.Count % 2 == 1)
                pageContentList.Add(emptyPageContentPrefab);
            if (pageContentList.Count == 0)
                throw new Exception("BookController must have at least 2 pages!");


            currPageIdx = 0;
            leftPageContent = Instantiate(PageContentList[0]).GetComponent<PageController>();
            leftPageContent.Construct(0, currPageIdx > 0);
            rightPageContent = Instantiate(PageContentList[1]).GetComponent<PageController>();
            rightPageContent.Construct(1, currPageIdx < PageContentList.Count - 1);
        }

        public void FlipLeft()
        {
            if (flipCoroutine != null)
            {
                // There is an ongoing flip animation.
                // We must finish the animation.
                FinishFlip();
            }


            if (currPageIdx + 2 >= PageContentList.Count)
            {
                // Don't flip if we can't
                return;
            }
            currPageIdx += 2;
            flipCoroutine = StartCoroutine(FlipLeftCoroutine());
        }

        private IEnumerator FlipLeftCoroutine()
        {
            animation.clip = flipLeftClip;
            animation.Play();
            var newLeftPageContentPrefab = pageContentList[currPageIdx];
            var newRightPageContentPrefab = pageContentList[currPageIdx + 1];

            rightPageContentInst = Instantiate(newLeftPageContentPrefab, backRIghtPageTrans).GetComponent<PageController>();
            rightPageContentInst.Construct(currPageIdx + 1, currPageIdx < PageContentList.Count - 1);

            while (animation.isPlaying)
            {
                // If we cross, then switch content
                if (rightPageTrans.localScale.x <= 0)
                {
                    leftPageContentInst = Instantiate(newLeftPageContentPrefab, leftPageTrans).GetComponent<PageController>();
                    leftPageContentInst.Construct(currPageIdx, currPageIdx > 0);

                    if (rightPageContent != null)
                    {
                        Destroy(rightPageContent.gameObject);
                    }

                    // Remember, during the flip, the right page becomes the left page temporarily
                    rightPageContent = leftPageContentInst;
                }
                yield return new WaitForEndOfFrame();
            }

            FinishFlip();
        }

        public void FlipRight()
        {
            if (flipCoroutine != null)
            {
                // There is an ongoing flip animation.
                // We must finish the animation.
                FinishFlip();
            }


            if (currPageIdx - 2 <= 0)
            {
                // Don't flip if we can't
                return;
            }
            currPageIdx -= 2;
            flipCoroutine = StartCoroutine(FlipRightCoroutine());
        }

        private IEnumerator FlipRightCoroutine()
        {
            animation.clip = flipRightClip;
            animation.Play();
            var newLeftPageContentPrefab = pageContentList[currPageIdx];
            var newRightPageContentPrefab = pageContentList[currPageIdx + 1];

            leftPageContentInst = Instantiate(newLeftPageContentPrefab, backLeftPageTrans).GetComponent<PageController>();
            leftPageContentInst.Construct(currPageIdx + 1, currPageIdx > 0);

            while (animation.isPlaying)
            {
                // If we cross, then switch content
                if (leftPageTrans.localScale.x <= 0)
                {
                    rightPageContentInst = Instantiate(newRightPageContentPrefab, rightPageTrans).GetComponent<PageController>();
                    rightPageContentInst.Construct(currPageIdx, currPageIdx < PageContentList.Count - 1);

                    if (leftPageContent != null)
                    {
                        Destroy(leftPageContent.gameObject);
                    }

                    // Remember, during the flip, the left page becomes the right page temporarily
                    leftPageContent = rightPageContentInst;
                }
                yield return new WaitForEndOfFrame();
            }

            FinishFlip();
        }

        private void FinishFlip()
        {
            if (flipCoroutine == null)
                return;

            StopCoroutine(flipCoroutine);
            flipCoroutine = null;

            // At end of animation, we reset animation and swap left and right page content
            rightPageContent = rightPageContentInst;
            leftPageContent = leftPageContentInst;
        }
    }
}