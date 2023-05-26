using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onRelease;

    private GameObject presser;
    private bool isPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            isPressed = true;   
            onRelease.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == presser) 
        {
            button.transform.localPosition = new Vector3(0, 0.0132f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }
}
