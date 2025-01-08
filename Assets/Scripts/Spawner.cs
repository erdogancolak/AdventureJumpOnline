using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviourPun
{
    [Header("References")]
    public GameObject defaultPrefab, jumperPrefab, movedPrefab;
    private GameObject selectedPrefab;

    [Header("Values")]
    [SerializeField] private int platformCount;
    [SerializeField] private float XValueChange;
    [SerializeField] private float minimumY, maximumY;

    private Vector3 spawnLocate;
    private int randomNumber;

    void Start()
    {
        for (int i = 0; i < platformCount; i++)
        {
            selectedPrefab = defaultPrefab;
            spawnLocate.x = Random.Range(-XValueChange, XValueChange);
            spawnLocate.y += Random.Range(minimumY, maximumY);

            GameObject platform = Instantiate(selectedPrefab, spawnLocate, Quaternion.identity);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(PlatformSpawner());
        }
    }
      
    IEnumerator PlatformSpawner()
    {
        while (true) 
        {
            for (int i = 0; i < platformCount; i++)
            {
                randomNumber = Random.Range(0, 18);
                switch (randomNumber)
                {
                    case 1:
                    case 2:
                        selectedPrefab = jumperPrefab;
                        break;
                    case 3:
                        selectedPrefab = movedPrefab;
                        break;
                    default:
                        selectedPrefab = defaultPrefab;
                        break;
                }

                spawnLocate.x = Random.Range(-XValueChange, XValueChange);
                spawnLocate.y += Random.Range(minimumY, maximumY);

                GameObject platform = Instantiate(selectedPrefab, spawnLocate, Quaternion.identity);
                yield return new WaitForSeconds(0.5f); 
            }
        }
    }
}

