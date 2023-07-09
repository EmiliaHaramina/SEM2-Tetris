using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;

    bool isPressed;
    public bool enabledAtStart;
    bool enabled = true;

    private List<int> presserLayers;
    [SerializeField] List<string> presserLayerNames;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        enabled = enabledAtStart;

        presserLayers = new List<int>();

        foreach (string layerName in presserLayerNames)
        {
            presserLayers.Add(LayerMask.NameToLayer(layerName));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (enabled && !isPressed && presserLayers.Contains(other.gameObject.layer))
        {
            button.transform.localPosition = new Vector3(0, 0.005f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (enabled && other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.02f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void Disable()
    {
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void InvokeOnRelease()
    {
        onRelease.Invoke();
    }

}
