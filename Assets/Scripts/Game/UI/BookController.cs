using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class BookController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private AnimationClip flipLeftClip;
        [SerializeField]
        private AnimationClip flipRightClip;

        [Header("Dependencies")]
        [SerializeField]
        private PageController leftPageController;
        [SerializeField]
        private PageController rightPageController;
        [SerializeField]
        private Transform leftPageTrans;
        [SerializeField]
        private Transform rightPageTrans;
        [SerializeField]
        private PageController backLeftPageController;
        [SerializeField]
        private PageController backRightPageController;
        [SerializeField]
        private AudioSource flipPageAudioSource;
        [SerializeField]
        private GameObject emptyPageContent;

        [Header("Settings")]
        [SerializeField]
        private List<GameObject> pageContentList;
        [SerializeField]
        private int currPageIdx;

        public List<GameObject> PageContentList { get => pageContentList; set => pageContentList = value; }

        public int CurrPageIdx { get => currPageIdx; set => currPageIdx = value; }
        private int LeftPageIdx => CurrPageIdx;
        private int RightPageIdx => CurrPageIdx + 1;
        private bool LeftPageCanFlip => LeftPageIdx > 0;
        private bool RightPageCanFlip => RightPageIdx < PageContentList.Count - 1;

        public GameObject CurrPageContent => PageContentList[CurrPageIdx];

        private Coroutine flipCoroutine;

        public void Awake()
        {
            if (pageContentList.Count > 0)
                Construct(pageContentList.ToArray());
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    pointerId = -1,
                };

                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                Debug.Log(results.Count);
                foreach (var result in results)
                    Debug.Log(result.gameObject.name);
            }
        }

        public void Construct(IEnumerable<GameObject> newPageContentList)
        {
            PageContentList.Clear();
            pageContentList.AddRange(newPageContentList);
            // Make sure we always have an even count of pages
            if (pageContentList.Count % 2 == 1)
                pageContentList.Add(emptyPageContent);
            if (pageContentList.Count == 0)
                throw new Exception("BookController must have at least 2 pages!");

            ResetBook();
        }

        public void ResetBook()
        {
            currPageIdx = 0;
            leftPageController.SetContent(LeftPageIdx, LeftPageCanFlip, PageContentList[LeftPageIdx]);
            rightPageController.SetContent(RightPageIdx, RightPageCanFlip, PageContentList[RightPageIdx]);
        }

        public void FlipLeft()
        {
            if (flipCoroutine != null)
            {
                // There is an ongoing flip animation.
                // We must finish the animation.
                FinishFlip();
                return;
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
            flipPageAudioSource.Play();
            animator.Play("Flip Left");
            var newLeftPageContentPrefab = pageContentList[currPageIdx];
            var newRightPageContentPrefab = pageContentList[RightPageIdx];

            backRightPageController.SetContent(RightPageIdx, RightPageCanFlip, newRightPageContentPrefab);

            yield return new WaitForEndOfFrame();

            float time = 0;
            bool switched = false;
            while (time <= flipLeftClip.length)
            {
                // If we cross, then switch content
                if (!switched && rightPageTrans.localScale.x <= 0)
                {
                    switched = true;
                    // Remember, during the flip, the right page becomes the left page temporarily
                    rightPageController.SetContent(LeftPageIdx, LeftPageCanFlip, newLeftPageContentPrefab, true);
                }

                yield return new WaitForEndOfFrame();
                
                time += Time.deltaTime;
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
                return;
            }


            if (currPageIdx - 2 < 0)
            {
                // Don't flip if we can't
                return;
            }
            currPageIdx -= 2;
            flipCoroutine = StartCoroutine(FlipRightCoroutine());
        }

        private IEnumerator FlipRightCoroutine()
        {
            flipPageAudioSource.Play();
            animator.Play("Flip Right");
            var newLeftPageContentPrefab = pageContentList[currPageIdx];
            var newRightPageContentPrefab = pageContentList[RightPageIdx];

            backLeftPageController.SetContent(LeftPageIdx, LeftPageCanFlip, newLeftPageContentPrefab);

            float time = 0;
            bool switched = false;
            while (time <= flipRightClip.length)
            {
                // If we cross, then switch content
                if (!switched && leftPageTrans.localScale.x <= 0)
                {
                    switched = true;
                    // Remember, during the flip, the left page becomes the right page temporarily
                    leftPageController.SetContent(RightPageIdx, RightPageCanFlip, newRightPageContentPrefab, true);
                }

                yield return new WaitForEndOfFrame();

                time += Time.deltaTime;
            }

            FinishFlip();
        }

        private void FinishFlip()
        {
            if (flipCoroutine == null)
                return;

            StopCoroutine(flipCoroutine);
            flipCoroutine = null;
            animator.Play("Reset");

            // At end of animation, we reset animation and assign the correct pages to the left and right
            leftPageController.SetContent(LeftPageIdx, LeftPageCanFlip, pageContentList[LeftPageIdx]);
            rightPageController.SetContent(RightPageIdx, RightPageCanFlip, pageContentList[RightPageIdx]);
            
            backLeftPageController.ClearContent();
            backRightPageController.ClearContent();
        }

        public void LoadChapter(Transform chapter)
        {
            PageContentList.Clear();
            foreach (Transform child in chapter)
                PageContentList.Add(child.gameObject);

            ResetBook();
        }
    }
}