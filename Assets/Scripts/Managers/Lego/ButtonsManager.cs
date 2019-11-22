using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField]
    private int startingAmount = 5;

    private static int cash;

    private static Color toFlash = Color.white;

    private Text text;

    private void Awake()
    {
        cash = startingAmount;
        text = GetComponent<Text>();    
    }

    public static void flash()
    {
        toFlash = Color.red;
    }

    public static void add(int value)
    {
        cash += value;
    }

    public static int getAmount()
    {
        return cash;
    }

    public static bool spend(int amount)
    {
        if (cash < amount) return false;
        cash -= amount;
        return true;
    }

    void Update()
    {
        text.text = "" + cash;
        text.color = toFlash;
        toFlash = Color.white;
    }
}
