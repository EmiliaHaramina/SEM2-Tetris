using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class GenerateTetris : MonoBehaviour
{

    public GameObject[] tetrisPieces;

    //index zadnjeg stvorenog dijela i broj ponavljanja
    //svaki se moze pojaviti max 2 puta za redom
    (int index, int reps) lastSpawned;

    // Start is called before the first frame update
    void Start()
    {
        //initial
        lastSpawned.index = -1;
        lastSpawned.reps = -1;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyRedundantObjects();
    }

    void SpawnTertris()
    {
        int index = UnityEngine.Random.Range(0, tetrisPieces.Length);

        if (lastSpawned.index == index && lastSpawned.reps >= 2)
        {
            do
            {
                index = UnityEngine.Random.Range(0, tetrisPieces.Length);
            } while (index == lastSpawned.index);

            lastSpawned.index = index;
            lastSpawned.reps = 1;
        }

        else if (lastSpawned.index == index)
        {
            lastSpawned.reps++;
        }

        else
        {
            lastSpawned.index = index;
            lastSpawned.reps = 1;
        }

        GameObject tetrisPiece = Instantiate(tetrisPieces[index], this.gameObject.transform.position, this.gameObject.transform.rotation);
        StartCoroutine(despawnPiece(tetrisPiece));
    }

    private void DestroyRedundantObjects()
    {
        GameObject go = GameObject.Find("Cube");
        Destroy(go);
    }

    public void startSpawning(float startOffset, float seconds)
    {
        InvokeRepeating("SpawnTertris", startOffset, seconds);
    }

    public void stopSpawning()
    {
        CancelInvoke("SpawnTertris");
    }

    IEnumerator despawnPiece(GameObject tetrisPiece)
    {
        yield return new WaitForSeconds(10);

        if (tetrisPiece != null)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().DroppedOrDespawned();

            tetrisPiece.GetComponent<TetrisPieceScript>().Explode();
        }
    }

}