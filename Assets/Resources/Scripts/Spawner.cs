using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [field: SerializeField]
    public GameObject monster { get; set; }

    private int monsterCount;
    private float timer = 0;
    private Vector3 pos;
    
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        Instantiate(monster,pos, Quaternion.identity);
        monsterCount = 1;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 15 && monsterCount <3)
        {
            Instantiate(monster,pos, Quaternion.identity);
            monsterCount++;
            timer = 0;
        }
    }
}
