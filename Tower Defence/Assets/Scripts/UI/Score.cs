using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Min(0f)]
    public double score = 0;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        double exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(score))));
        double mantissa = (score / System.Math.Pow(10, exponent));

        if(score >= 1000000)
        {
            scoreText.text = mantissa.ToString("F3") + "e" + exponent.ToString();
        }
        else
        {
            scoreText.text = score.ToString();
        }
    }
}
