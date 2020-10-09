using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private float lifeTimer = 0;
    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= 0.8f)
        {
            Destroy(gameObject);
        }
    }
}
