using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalc : MonoBehaviour
{
    private int score;

    private Text scoreBoard;
    [field: SerializeField]
    public GameObject winWindow { get; set; }

    private ISoundSystem ss;
    // Start is called before the first frame update
    void Start()
    {
        scoreBoard = GetComponent<Text>();
        ss = new SoundSystemDefault(gameObject,Sounds.VictoryScore, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        score = int.Parse(scoreBoard.text);
        if (score >= 15)
        {
            ss.MakeSound();
            winWindow.SetActive(true);
            score = 0;
        }
    }
}
