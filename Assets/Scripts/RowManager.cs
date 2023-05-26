using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<ColliderScript> ColliderScriptList;

    public static Action onRowCleared;

    private List<List<bool>> markerList;

    public bool CheckMark(int rowIndex, int i, int j) {
        return markerList[rowIndex][i * 4 + j];
    }

    private void OnEnable()
    {
        CubePlaceSetter.onShapeAdded += UpdateStatus;
        GameManager.onGameStoppedClearBox += ClearBox;
    }

    private void OnDisable()
    {
        CubePlaceSetter.onShapeAdded -= UpdateStatus;
        GameManager.onGameStoppedClearBox -= ClearBox;
    }

    private void Start()
    {
        markerList = new List<List<bool>>();
        for (int i = 0; i < 10; i++) {
            List<bool> list = new List<bool>();
            for (int j = 0; j < 16; j++) {
                list.Add(false);
            }
            markerList.Add(list);
        }
    }

    public void UpdateStatus()
    {
        int ind1 = 0;
        int ind2 = 0;
        foreach (ColliderScript marker in ColliderScriptList)
        {
            if (marker.GetFull())
            {
                markerList[ind1][ind2] = true;
            }
            else
            {
                markerList[ind1][ind2] = false;
            }
            ind2++;
            if (ind2 == 16)
            {
                ind1++;
                ind2 = 0;
            }
        }
        ClearRows();
    }

    public void ClearRows()
    {
        for (int ind1 = 9; ind1 >= 0; ind1--) {
            bool rowFull = true;
            
            for (int ind2 = 0; ind2 < 16; ind2++) {
                if (!markerList[ind1][ind2]) {
                    rowFull = false;
                    break;
                }
            }

            if (rowFull) {
                for (int i = 0; i < 16; i++) {
                    ColliderScriptList[ind1 * 16 + i].SetEmpty();
                    markerList[ind1][i] = false;
                }

                // tu se dodaje bod na screen za unisten cijeli red
                onRowCleared?.Invoke();

                // call function for putting all other rows 1 row down
                LowerRows(ind1);
            }
        }
    }

    private void LowerRows(int clearedRow) {
        for (int i = clearedRow + 1; i < 10; i++) {
            for (int j = 0; j < 16; j++) {
                if (markerList[i][j]) {
                    markerList[i - 1][j] = true;
                    ColliderScript fallingColliderCube = ColliderScriptList[i * 16 + j];
                    ColliderScript lowerColliderCube = ColliderScriptList[(i - 1) * 16 + j];
                    GameObject cube = fallingColliderCube.GetParentCube();
                    cube.transform.position = lowerColliderCube.gameObject.transform.position;
                    cube.transform.rotation = lowerColliderCube.gameObject.transform.rotation;

                    fallingColliderCube.RemoveParentCube();
                    lowerColliderCube.SetFull(cube);

                    markerList[i][j] = false;
                    fallingColliderCube.SetEmpty();
                }
            }
        }
    }

    public void ClearBox()
    {
        int ind1 = 0;
        int ind2 = 0;
        foreach (ColliderScript marker in ColliderScriptList)
        {
            markerList[ind1][ind2] = false;
            ColliderScriptList[ind1 * 16 + ind2].SetEmpty();
            ind2++;
            if (ind2 == 16)
            {
                ind1++;
                ind2 = 0;
            }
           
        }
    }

    public static void PlaceCubeInNetwork(ColliderScript colliderCube, CubeScript tetrisCube, GameObject tetrisShape) {
        GameObject clone = Instantiate(tetrisCube.gameObject, colliderCube.gameObject.transform.position, colliderCube.gameObject.transform.rotation);
        clone.transform.localScale = tetrisShape.transform.localScale;
        colliderCube.SetFull(clone);
    }
}