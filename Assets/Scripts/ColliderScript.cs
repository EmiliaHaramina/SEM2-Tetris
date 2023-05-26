using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColliderScript : MonoBehaviour
{
    [SerializeField] bool full;
    [SerializeField] public MeshRenderer outlineRenderer;
    [SerializeField] int rowIndex;
    [SerializeField] int i;
    [SerializeField] int j;

    private GameObject _parentCube;
    // Start is called before the first frame update
    void Start()
    {
        if (rowIndex < 0 || i < 0 || j < 0) {
            Debug.Log(this + " not properly set!");
        }
    }

    public MeshRenderer GetOutlineRenderer() {
        return outlineRenderer;
    }

    public int GetRowIndex() {
        return rowIndex;
    }

    public int GetI() {
        return i;
    }

    public int GetJ() {
        return j;
    }

    public void SetFull(GameObject go)
    {
        full = true;
        _parentCube = go;
    }

    public void SetEmpty()
    {
        full = false;
        if (_parentCube != null) { 
            Destroy(_parentCube);
        }
    }

    public bool GetFull()
    {
        return full;
    }

    public GameObject GetParentCube() {
        return _parentCube;
    }

    public void RemoveParentCube() {
        _parentCube = null;
    }
}