using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlaceSetter : MonoBehaviour
{
    List<ColliderScript> activeColliders = new List<ColliderScript>();

    public static Action onShapeAdded;

    private int boolCount;
    private List<ColliderScript> lastShape = new List<ColliderScript>();

    private RowManager rowManager;

    void Start() {
        rowManager = GameObject.Find("RowManager").GetComponent<RowManager>();
    }

    void Update() {
        foreach (ColliderScript cube in lastShape) {
            cube.GetOutlineRenderer().enabled = false;
        }

        lastShape = correctShape();
        if (lastShape.Count == 4) {
            foreach (ColliderScript cube in lastShape) {
                cube.GetOutlineRenderer().enabled = true;
            }
        }
    }

    public void ResetActiveColliders()
    {
        while (activeColliders.Count > 0)
        {
            activeColliders.RemoveAt(0);
        }

        foreach (ColliderScript cube in lastShape) {
            cube.GetOutlineRenderer().enabled = false;
        }
    }

    private List<ColliderScript> correctShape() {
        List<ColliderScript> shape = new List<ColliderScript>();

        if (activeColliders.Count == 4) {
            List<ColliderScript> activeCollidersCopy = new List<ColliderScript>(activeColliders);
            bool touching = false;
            // this only works because each tetris piece has 4 cubes
            bool touchingTwo = false;
            bool solidGround = false;

            foreach (ColliderScript collider1 in activeCollidersCopy) {
                touching = false;
                int rowIndex = collider1.GetRowIndex();
                int i = collider1.GetI();
                int j = collider1.GetJ();

                foreach (ColliderScript collider2 in activeCollidersCopy) {
                    if (collider1 == collider2) {
                        continue;
                    }

                    int differenceRow = Mathf.Abs(rowIndex - collider2.GetRowIndex());
                    int differenceI = Mathf.Abs(i - collider2.GetI());
                    int differenceJ = Mathf.Abs(j - collider2.GetJ());
                    if ((differenceI == 1 || differenceJ == 1 || differenceRow == 1) &&
                        differenceI <= 1 && differenceJ <= 1 && differenceRow <= 1 &&
                        !(differenceI == 1 && differenceJ == 1) &&
                        !(differenceI == 1 && differenceRow == 1) &&
                        !(differenceJ == 1 && differenceRow == 1)) {
                            if (touching) {
                                touchingTwo = true;
                                break;
                            }

                            touching = true;
                            shape.Add(collider1);
                    }
                }
                if (!touching) {
                    break;
                }

                if (!solidGround &&
                    (rowIndex == 0 ||
                    rowManager.CheckMark(rowIndex - 1, i, j))) {
                    solidGround = true;
                }
            }

            if (!touching || !touchingTwo || !solidGround) {
                return new List<ColliderScript>();
            }
        }

        return shape;
    }

    public void setAllChildrenCubePosition() {
        foreach (var c in GetComponentsInChildren<CubeScript>())
        {
            c.SetCubePositions();
        }
    }

    public void PutChildrenInNetwork() {
        TetrisPieceScript tetrisPiece = this.gameObject.GetComponent<TetrisPieceScript>();

        if (lastShape.Count == 4 && !tetrisPiece.IsExploded()) {
            CubeScript[] tetrisCubes = GetComponentsInChildren<CubeScript>();

            for (int i = 0; i < 4; i++) {
                ColliderScript colliderCube = lastShape[i];
                CubeScript tetrisCube = tetrisCubes[i];
                RowManager.PlaceCubeInNetwork(colliderCube, tetrisCube, this.gameObject);
            }
            
            foreach (ColliderScript cube in lastShape) {
                cube.GetOutlineRenderer().enabled = false;
            }
            Destroy(this.gameObject);

            onShapeAdded?.Invoke();
        }
    }
    
    // i think the on trigger enter and exit functions don't behave well,
    // so it sometimes bugs out the placement of tetris shapes in the network
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "BoxHitCollider") {
            ColliderScript collider = other.gameObject.GetComponent<ColliderScript>();
            if (!collider.GetFull() && !activeColliders.Contains(collider)) {
                activeColliders.Add(collider);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "BoxHitCollider") {
            ColliderScript collider = other.gameObject.GetComponent<ColliderScript>();
            if (activeColliders.Contains(collider)) {
                activeColliders.Remove(collider);
            }
        }
    }
}
