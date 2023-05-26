using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuUpdater : MonoBehaviour
{
    public void DisableCanvas()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
}
