using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameOnRightClick : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }
    }
}
