using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoting : MonoBehaviour
{
    private Vector2 characterPosition;
    private Vector2 mousePosition;
    private GameObject bullet;
    private Camera cam;
    private Character character;
    private int facing;
    private ISoundSystem ss;
    // Start is called before the first frame update
    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        cam = Camera.main;
        character = transform.gameObject.GetComponent<Character>();
        ss = new SoundSystemDefault(gameObject,Sounds.GunShot, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        characterPosition = transform.position;
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        facing = character.facingRight ?  1 : -1;
        if (Input.GetMouseButtonUp(0))
        {
            GameObject o = Instantiate(bullet, new Vector2(characterPosition.x+facing*0.5f,characterPosition.y+0.19f),
                Quaternion.Euler(0,0,0));
            o.GetComponent<Bullet>().bulletDirection = mousePosition - characterPosition;
            ss.MakeSound();
        }
    }

}
