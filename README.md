# The Ultimate Alchemist - Unity Game

## Overview
**The Ultimate Alchemist** is a Unity-based game where players combine elements to discover new ones.  
The game features an **alchemy system** where ingredients can be mixed, unlocking new elements in an ever-growing **encyclopedia of alchemy**.

---

## Gameplay Features
- **Element Combination**: Mix ingredients to create new discoveries.
- **Encyclopedia System**: Keeps track of all discovered elements.
- **Progress Tracking**: Saves unlocked ingredients for future use.
- **Customizable Recipes**: Users can easily add new alchemy recipes.

---

## Customizing the Game - Adding New Recipes
This game reads **`Resources/recipes.txt`**, which contains all the **alchemy recipes**.  
Players can **add, remove, or modify** the recipes by editing this file.

### **How to Add New Recipes**
1. Go to `Assets/Resources/recipes.txt`.
2. Follow the existing **recipe format** to define new combinations.
3. Ensure that the **element sprites** exist in `Resources/Sprites/`and that the names matches.
4. Run the game, and your new **alchemy combinations** will be available!

> **This system allows unlimited recipe expansion**, making the game fully customizable.

---

## Core Scripts
### **`Encyclopedia.cs`**
- Manages **all known ingredients**.
- Loads recipes from **`Resources/recipes.txt`**.
- Provides methods to **retrieve** new ingredients from combinations.

### **`Ingredient.cs`**
- Represents **individual ingredients**.
- Stores **name, sprite, recipe, and basic status**.
- Overrides `Equals()` and `GetHashCode()` for **comparisons**.

### **`Progress.cs`**
- Keeps track of the **player's unlocked ingredients**.
- Allows adding and checking for **ingredient availability**.

---

## How to Run the Project
### **Clone the Repository**
```sh
git clone https://github.com/yourusername/TheUltimateAlchemist.git
cd TheUltimateAlchemist
```

### **Open in Unity**
- Open **Unity Hub**.
- Click **"Add Project"** and select the folder.
- Open the project in **Unity Editor**.

### **Play the Game**
- Click **Play** in Unity to start testing the alchemy system.
- You can play the release version in `Bin/The_Ultimate_Alchemist.exe`
