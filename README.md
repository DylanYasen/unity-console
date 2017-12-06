[![Codacy Badge](https://api.codacy.com/project/badge/Grade/11b2441d38ec474fb1f5d5c2ece6ed3d)](https://www.codacy.com/app/DylanYasen/unity-console?utm_source=github.com&utm_medium=referral&utm_content=DylanYasen/unity-console&utm_campaign=badger)
# unity-console [![Build Status](https://travis-ci.org/DylanYasen/unity-console.svg?branch=0.1)](https://travis-ci.org/DylanYasen/unity-console)
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
- press [Tab] to auto complete
- [Return] to excecute command

<img src="https://media.giphy.com/media/3ohs7LXMnbuafYTRao/giphy.gif">

## Roadmap
- [x] execute static methods
- [ ] execute non-static methods
- [x] primitive type parameters
- [ ] non-primitive type parameters
- [ ] UI indicator for method target type
- [ ] UI indicator for parameter type
- [ ] Optimize reflection (caching mechanism or thread or ?)
