using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarInit : MonoBehaviour
{
    private int HP;
    private int currentHP;
    [field: SerializeField]
    public GameObject HPBarMiddle { get; set; }
    public HPBarHPDisplay[] HPBars { get; set; }

    private Character ch;
    // Start is called before the first frame update
    void Start()
    {
        ch = GameObject.Find("Character").GetComponent<Character>();
        HP = ch.getHealthPoints;
        currentHP = HP;
        HPBars = new HPBarHPDisplay[HP];
        for (int i = 0; i < HP; i++)
        {
            HPBars[i] = Instantiate(HPBarMiddle, transform, true).GetComponent<HPBarHPDisplay>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = ch.getHealthPoints;
        for (int i = 0; i < HP; i++)
        {
            if (i < currentHP)
            {
                HPBars[i].hasHP = true;
            }
            else
            {
                HPBars[i].hasHP = false;
            }
        }

    }
}
