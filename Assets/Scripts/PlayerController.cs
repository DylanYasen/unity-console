using System.Collections;
using System.Collections.Generic;
using UConsole;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [ConsoleCmd]
    public static void AddCoins(int amount)
    {
        Debug.Log("added " + amount);
    }

    [ConsoleCmd]
    public static void Kill()
    {

    }
}
