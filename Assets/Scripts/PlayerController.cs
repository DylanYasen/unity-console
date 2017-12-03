using System.Collections;
using System.Collections.Generic;
using UConsole;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [ConsoleCmd]
    public static void AddCoins(int amount, string name)
    {
        Debug.Log("added " + amount + "coins for " + name);
    }

    [ConsoleCmd]
    public static void AddHP(int amount)
    {
        Debug.Log("added " + amount);
    }

    [ConsoleCmd]
    public static void Kill()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public void Hello()
    {
        Debug.Log("hello");
    }
}
