using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor.ShaderGraph.Internal;

public class Spawner : MonoBehaviourPun
{
    [Header("References")]
    public GameObject defaultPrefab, jumperPrefab, movedPrefab, cyclePrefab, stunPrefab;
    [Space]
    public GameObject collectAbilityPrefab;
    private GameObject selectedPrefab;

    [Header("Values")]
    [SerializeField] private int platformCount;
    [SerializeField] private float XValueChange;
    [SerializeField] private float minimumY, maximumY;
    //[SerializeField] private float platformGap;

    private Vector3 spawnLocate;
    private int randomNumber;

    void Start()
    {
        for (int i = 0; i < platformCount; i++)
        {
            selectedPrefab = defaultPrefab;
            spawnLocate.x = Random.Range(-XValueChange, XValueChange);
            spawnLocate.y += Random.Range(minimumY, maximumY);

            GameObject platform = PhotonNetwork.Instantiate(selectedPrefab.name, spawnLocate, Quaternion.identity);
        }
        if(PhotonNetwork.IsMasterClient)
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
                randomNumber = Random.Range(0, 30);

                GameObject selectedPrefab = SelectPrefab(randomNumber);

                spawnLocate.x = Random.Range(-XValueChange, XValueChange);
                spawnLocate.y += Random.Range(minimumY, maximumY);

                GameObject platform = PhotonNetwork.Instantiate(selectedPrefab.name, spawnLocate, Quaternion.identity);
                if (randomNumber == 20 || randomNumber == 21)
                {
                    SpawnAbilityOnPlatform(platform);

                }
                yield return new WaitForSeconds(0.5f); 
            }
        }
    }
    private GameObject SelectPrefab(int randomNumber)
    {
        switch (randomNumber)
        {
            case 1:
            case 2:
                return jumperPrefab;
            case 3:
                return movedPrefab;
            case 4:
                return cyclePrefab;
            default:
                return defaultPrefab;
        }
    }

    //private void SpawnOppositePlatform(Vector3 originalPosition)
    //{
    //    Vector3 oppositeSpawnLocate = originalPosition;
    //    oppositeSpawnLocate.x = -originalPosition.x;
    //    if (originalPosition.x > 0)
    //    {
    //        oppositeSpawnLocate.x -= platformGap;
    //    }
    //    else if (originalPosition.x < 0)
    //    {
    //        oppositeSpawnLocate.x += platformGap;
    //    }
    //    PhotonNetwork.Instantiate(defaultPrefab.name, oppositeSpawnLocate, Quaternion.identity);
    //}

    private void SpawnAbilityOnPlatform(GameObject platform)
    {
        if(platform != null)
        {
            Vector3 coinSpawnPosition = platform.transform.position;
            coinSpawnPosition.y += .7f;

            PhotonNetwork.Instantiate(collectAbilityPrefab.name, coinSpawnPosition, Quaternion.identity);
        }
    }
}

