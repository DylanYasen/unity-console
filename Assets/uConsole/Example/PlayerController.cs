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
    public static void Kill1()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill2()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill3()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill4()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill5()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill6()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill7()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill8()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill9()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill10()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public static void Kill11()
    {
        Debug.Log("killed");
    }

    [ConsoleCmd]
    public void Hello()
    {
        Debug.Log("hello");
    }
}
