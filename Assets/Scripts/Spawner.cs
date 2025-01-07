using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject platformPrefab;
    Vector3 spawnLocate;


    [SerializeField] private int platformCount;
    [SerializeField] private float XValueChange;
    [SerializeField] private float minimumY, maximumY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        platformSpawner();
    }
    private void platformSpawner()
    {
            Vector2 newScale = new Vector2();

            for (int i = 0; i < platformCount; i++)
            {
                newScale.x = Random.Range(0.9f, 1.1f);
                transform.localScale = newScale;

                spawnLocate.x = Random.Range(-XValueChange, XValueChange);
                spawnLocate.y += Random.Range(minimumY, maximumY);

                Instantiate(platformPrefab, spawnLocate, Quaternion.identity);
            }
        }
}
