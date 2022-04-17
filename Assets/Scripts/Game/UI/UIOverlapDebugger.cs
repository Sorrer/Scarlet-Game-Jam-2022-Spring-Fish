using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.UI
{
    public class UIOverlapDebugger : MonoBehaviour
    {
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
    }
}
