using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeScript : MonoBehaviour
{
    private GameObject _collidedObject;
    public void SetCubePositions()
    {
        //zove se prilikom otpustanja Objekta na svu djecu objekta, odnosno 1 skriptu za svaku kockicu(njih 4)
        //pivot mora bit sredina da ovo radi?
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.transform.position = _collidedObject.transform.position;
        this.gameObject.transform.rotation = _collidedObject.transform.rotation;
        _collidedObject.GetComponent<ColliderScript>().SetFull(this.gameObject);
    }
    public void OnTriggerEnter(Collider other)

    {
        //Mozda treba checkat layer da se dobije kad kocka ima onTriggerEnter, a ne slucajno ruka
        _collidedObject = other.gameObject;
    }

}