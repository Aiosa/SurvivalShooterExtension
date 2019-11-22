using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private static int score;

    Text text;

    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
    }

    public static void add(int value)
    {
        score += value;
    }


    void Update ()
    {
        text.text = "Score: " + score;
    }
}
