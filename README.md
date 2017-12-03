# unity-console
uConsole is a runtime command console for Unity3D

## Instructions

- mark methods with [ConsoleCmd] attribute
```c#
    [ConsoleCmd]
    public static void AddCoins(int amount, string name)
    {
        Debug.Log("added " + amount + " for " + name);
    }

    [ConsoleCmd]
    public static void Kill()
    {
        Debug.Log("killed");
    }
```

- during runtime, press [`] to toggle the command console interface

<img src="https://media.giphy.com/media/3ohs7LXMnbuafYTRao/giphy.gif">

## Roadmap
- [x] excecute static methods
- [ ] excecute non-static methods
- [x] primitive type parameter
- [ ] non-primitive type parameter
- [ ] UI indicator for method target type
- [ ] UI indicator for parameter type
