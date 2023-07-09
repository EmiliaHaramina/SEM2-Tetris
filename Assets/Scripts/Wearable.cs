using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearable : MonoBehaviour
{
    public GameObject snapZone;
    public GameObject highlight;
    public GameObject wornObject;
    private bool inSnapZone;

    public GameManager gameManager;

    public SymptomTracker symptomTracker;

    void Update()
    {
        if (inSnapZone)
        {
            symptomTracker.StartTracking();

            Destroy(highlight);
            wornObject.SetActive(true);
            gameManager.AddWornItem();
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == snapZone)
            inSnapZone = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == snapZone)
            inSnapZone = false;
    }

    // void OnTriggerEnter(Collider collider)
    // {
    //     if (collider.gameObject == snapZone)
    //         inSnapZone = true;
    // }

    // void OnTriggerExit(Collider collider)
    // {
    //     if (collider.gameObject == snapZone)
    //         inSnapZone = false;
    // }
}
