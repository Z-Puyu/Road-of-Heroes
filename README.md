# Road of Heroes

## What Is This?

*Road of Heroes* is a **2D roguelike turn-based role-play game** inspired by Darkest Dungeon and influenced by For the King. Read the [game design document](GDD.md) here for more detail.

## Coding Standards

In order to maintain coding style, standardise code structure and facilitate better team collaboration, we specify the following coding conventions and standards which are to be **strictly followed** in the project's development. Another practical reason to enforce this is for aesthetic purposes.

- The game is to be developed in **C#**, **no GDScript** is allowed in scripting.

- Every operator should have **exactly one space before it** and **exactly one space after it**.

- There should be **exactly one space** between a function's name and its parameters, and before an opening curly brace.

- Keywords such as `for`, `while`, `if`, `switch`, `try`, `catch`, `throw` etc. should be surrounded by one space at each side.

- All folders should be named in **snake_case**.

- All game scenes and .cs scripts should be named in **PascalCase**, whereas the other types of files (e.g. images, text files, .csv files, .json files) should be named in **snake_case**.

- All classes, methods and properties should be named in **PascalCase**.

- All variables and class fields should be named in **camelCase**.

- All constants, i.e., `const` variables and `static readonly` fields, should be named in **SNAKE_CASE_USING_ALL_CAPITAL_LETTERS**.
 
- Names of variables and functions should be **in English**, **descriptive**. **No custom abbreviations** are allowed.

  - Examples of allowed abbreviations:

    - `height` to `h` (physical quantity following ISO standards)
   
    - `density` to `rho` (OK if you really mean the density as in physics)
   
    - `permutation` to `perm` (mathematical notations)
   
    - `group` to `grp` (universal abbreviations used by English speakers)
   
    - `damagePerSecond` to `dps` (universal abbreviations used by gamers)
   
    - `europe` to `eu` (country/region names)
   
    - `adjective` to `adj` (standard abbreviations used by English textbooks)
   
  - Examples of disallowed abbreviations:

    - `gold` to `au` (chemical symbol, but we are programmers not chemists)
   
    - `linkedList` to `ll` (ambiguous because C++ competitive programmers may say that `ll` stands for `long long`)

- Every script file should contain **one and only one** class, with the file name being **exactly the same** as the class name.

- Every class should be placed into a **namespace**.

- Each line should contain **only one** single statement. 

- `while (true)` is **banned**.

- **Do not create custom Godot signals**. As an (much better) alternative we use **C# events**.

- Use **trailing open braces** (a.k.a the Egyptian braces) instead of **leading open braces**.

- All class fields are **private** by default. Use **properties** if they need to be accessed from other classes.

- **`this` keyword** is **not to be omitted**.

- `switch` statements should always have a `default` branch.

- `ToString` should be called implicitly unless an explicit call is absolutely necessary.

- All interfaces should be prefixed with "**I**" in their names.

- All custom classes extending `EventArgs` must have their names ending with "**Event**".

- Write `x is not Class` rather than `!(x is Class)` when checking the type of variables.

- No `else` branch after `return` statements.

The followings are strongly encouraged but not strictly enforced:

- Try not to write statements which are longer than **100 characters** (not a hard rule).

- Use constructor methods explicitly, i.e., `CustomClass c = new CustomClass();` instead of `CustomClass c = new()`.

- Use collection expression to initialise collections, i.e., `List<GameObject> list = [initObj];`

- Use `TryGetValue` wherever applicable.

- Check the type and perform type-casting at the same time, i.e., do things like
```c#
if (gameObj is CustomObject obj) {
    obj.DoSomething();
}
```
instead of 
```c#
if (gameObj is CustomObject) {
    CustomObject obj = (CustomObject)gameObj;
    obj.DoSomething();
}
```

- For `do`-`while` loops, `if`-`else` blocks and `try`-`catch`-`finally` blocks, append the `while`, `else`, `catch`, `finally` keywords after the closing curly brace, not in a new line.

## Workflow

- The `main` branch is **protected**. Only code which has been comprehensively tested and debugged will be pushed there via a *pull request*.

- The `production-and-test` branch is used to merge all newly implemented feature branches *after each branch has been tested individually*. Its purpose is to test and debug a latest version of the game. All other branches should be merged to this branch once a new feature has been implemented.

- All other branches serve to classify and modularise the development of different features. Use **kebab-case** to name all branches.

- [**Discussions**](https://github.com/Z-Puyu/Road-of-Heroes/discussions) is used to make important announcement and facilitate team communication. Any new ideas or suggestions regarding the game's development should be discussed here to reach a collective decision, before they are officially documented in the to-do list.

- [**Issues**](https://github.com/Z-Puyu/Road-of-Heroes/issues) is used to document important **tasks** to be implemented or modified, so as to set clear work goals and segregation of duties.
  
### Decision Mechanism

- We adopt **simple majority** for decision making, but the project leader has **veto** rights :O

- Exception: for any matter relating to **game art**, the artist has **veto** rights.

- Exception: for any matter relating to **game music**, the musician has **veto** rights.
