using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed { get; set; } = 3f;
    public Vector2 bulletDirection { get; set; }
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask mask;

    private float timer = 0;
    private float timerCollision = 0;

    private IDamageDealer dd;
    private ISoundSystem ss;
    
    [field: SerializeField]
    public GameObject light { get; set; }
    
    private bool isNotPlaying = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dd = new DamageSystemBullet(gameObject, 1,1);
        ss = new SoundSystemDefault(gameObject,Sounds.GunCollision, 0.5f);
        rb.AddForce(bulletDirection*bulletSpeed,ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!isNotPlaying)
        {
            timerCollision += Time.deltaTime;
        }
        if (timer >= 8 || timerCollision >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        dd.DamageDealing(other);
        Instantiate(light,other.GetContact(0).point,Quaternion.Euler(0,0,0));
        if (isNotPlaying)
        {
            isNotPlaying = false;
            ss.MakeSound();
            //if(other.gameObject.layer == mask)
        }
    }
}
