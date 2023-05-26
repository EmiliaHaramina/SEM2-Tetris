using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPointUpdater : MonoBehaviour
{
    private void OnEnable()
    {
        AttachPointFinder.onNewAttachPoint += UpdateAttachPointPosition;
    }

    private void OnDisable()
    {
        AttachPointFinder.onNewAttachPoint -= UpdateAttachPointPosition;
    }

    private void UpdateAttachPointPosition(Vector3 obj)
    {
        transform.position = obj;
    }
}
