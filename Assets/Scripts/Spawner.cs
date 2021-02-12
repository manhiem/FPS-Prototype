using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject Zombie;
    public float countDown = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countDown += Time.deltaTime;
        //Debug.Log(countDown.ToString());

        if(countDown >= 10 && countDown <= 11)
        {
            StartCoroutine(spawnSlow());
            countDown = 12;
        }

        if (countDown >= 20 && countDown <=21)
        {
            StartCoroutine(spawnFast());
            countDown = 0;
        }
    }

    IEnumerator spawnSlow()
    {
        foreach(Transform tr in SpawnPoints)
        {
            Instantiate(Zombie, tr.position, Quaternion.identity);
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(1);
    }

    IEnumerator spawnFast()
    {
        foreach (Transform tr in SpawnPoints)
        {
            Instantiate(Zombie, tr.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
    }
}
