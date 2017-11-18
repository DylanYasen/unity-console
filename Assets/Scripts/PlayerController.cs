using System.Collections;
using System.Collections.Generic;
using UConsole;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Exec]
    public void AddCoins(int amount)
    {
        Debug.Log("added " + amount);
    }

    [Exec]
    public static void Kill()
    {

    }
}
