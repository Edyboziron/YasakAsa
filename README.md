# 🪄 YASAK ASA (Forbidden Wand)

[![Unity 6](https://img.shields.io/badge/Unity-6000.3-black?logo=unity)](https://unity.com/)
[![Render Pipeline](https://img.shields.io/badge/Render%20Pipeline-URP-blue)](https://unity.com/srp/Universal-Render-Pipeline)
[![Event](https://img.shields.io/badge/Game%20Jam-Sandwich%20Jam%202-orange)](https://itch.io)
[![Status](https://img.shields.io/badge/Status-Playable-brightgreen)]()

> *"Welcome to my table, Traveler... Where the cards whisper destiny.*  
> *You know the forest outside, don't you? It was once peaceful. But now... something has awakened beneath the soil. Innocence has decayed.*  
> *Let us see how far you can venture into this cursed forest. Choose your cards carefully—for they are your only weapon."*

---

## 📖 About The Game

**YASAK ASA** (*Forbidden Wand*) is an atmospheric, dark fantasy turn-based card battler developed for **Sandwich Jam 2** by **Enes Bozdemir** & **Eray Çocuk**.

Step into a grim, cursed forest where you must fight grotesque beasts using the volatile magic of the Forbidden Wand. In this world, brute force is not enough—every spell you cast exacts a heavy toll on your **Stability (Sanity)**. If you lose your grip on reality, your own wand may turn against you in horrifying ways.

---

## 🌟 Key Features

- **🧠 Volatile Sanity & Stability System:**  
  Casting spells drains your sanity. Maintain high Stability to stay in control—drop below **50%**, and your screen shakes, the UI bleeds red, and reality starts fracturing.

- **⚡ Spells That Betray You (Madness Backfire):**  
  When your Stability falls below 50%, your spells risk corrupted outcomes:
  - **Fireballs** deal reduced damage and inflict backlash damage onto yourself.
  - **Support & Healing spells** have an unpredictable chance to:
    - Backfire and damage you directly.
    - Accidentally empower and heal the enemy beast!
    - Trigger a violent mental collapse, draining an extra 15 Stability.

- **🐺 5 Challenging Levels & Evolving Enemies:**  
  Face innocent creatures twisted by the ancient curse. Defeating an enemy is only the beginning—upon death, each beast undergoes a violent mutation into a deadlier, evolved second phase:
  - **Level 1:** Sincap (*Squirrel / "Sipnac"*) ➔ Corrupted Squirrel
  - **Level 2:** Yaban Domuzu (*Wild Boar*) ➔ Abyssal Boar
  - **Level 3:** Koyun (*Sheep*) ➔ Horned Nightmare Beast
  - **Level 4:** Kartal (*Eagle*) ➔ Blighted Apex Predator
  - **Level 5:** Kutup Ayısı (*The Ancient Polar Beast*) ➔ Corrupted Final Horror

- **🎶 Dynamic Adaptive Audio System:**  
  Real-time dual-track audio management smoothly crossfades between eerie atmospheric melodies and distorted, glitched soundscapes as your sanity declines.

- **⚔️ Strategic Turn-Based Card Combat:**  
  Play up to 3 cards per turn. Balance offensive strikes, armor fortifications, crowd control, and mental recovery before the beasts strike.

---

## 🃏 Card Arsenal

| Card | Type | Value | Stability Cost | Description | Madness / Glitch Risk (<50% Stability) |
| :--- | :--- | :---: | :---: | :--- | :--- |
| **FireBall** | Attack | 10 DMG | 20 | Launches a searing fireball at the enemy | Reduced damage + self-inflicted damage |
| **Holy Light** (*İlahi Işık*) | Heal | +8 HP | 10 | Heals player for 8 health | May backfire, heal enemy, or drain sanity |
| **Iron Skin** (*Demir Deri*) | Shield | +5 Armor | 5 | Fortifies player defense with armor | May backfire, shield enemy, or drain sanity |
| **Reset** | Restore | +30 Sanity | 1 | Focuses mental fortitude to restore Stability | Guaranteed stability recovery |
| **Stun** (*Sersemletme*) | Control | 1 Turn | 25 | Stuns the enemy, canceling their next attack | High stability cost |

---

## 🎮 How to Play

1. **Draw & Plan:** Each turn you are granted **3 action slots**.
2. **Cast Spells (Drag & Drop):**
   - **Offensive Spells (Fireball, Stun):** Drag and release over the **Enemy**.
   - **Defensive & Utility Spells (Holy Light, Iron Skin, Reset):** Drag and release over your **Player Zone**.
3. **Manage Your Sanity:** Keep your Stability above **50%** whenever possible. If it falls into critical levels, use **Reset** before your spells betray you!
4. **Survive the Enemy Phase:** When your turns are exhausted, enemies perform their attacks with screen-shaking ferocity.
5. **Defeat Multi-Phase Foes:** Vanquish both the base and evolved forms of each beast to progress through all 5 levels to victory.

---

## 🛠️ Tech Stack & Architecture

- **Engine:** Unity 6 (`6000.3.22f1`)
- **Rendering:** Universal Render Pipeline (URP 2D)
- **Language:** C# (.NET Standard 2.1)
- **UI:** TextMesh Pro, Unity UGUI with Canvas Auto-Rebuilder
- **Input:** Unity Input System (`com.unity.inputsystem`)
- **Key Scripts:**
  - [`TurnManager.cs`](file:///Assets/Scripts/TurnManager.cs): Turn cycles, move tracking, and enemy turn coroutines.
  - [`PlayerStats.cs`](file:///Assets/Scripts/PlayerStats.cs): Health, armor, dynamic stability shakes, and reactive portraits.
  - [`CardMovement.cs`](file:///Assets/Scripts/CardMovement.cs): Drag-and-drop mechanics, target raycasting, and madness glitch calculations.
  - [`enemy.cs`](file:///Assets/Scripts/enemy.cs): Health tracking, attack routines, screen shakes, and multi-phase enemy evolutions.
  - [`MusicManager.cs`](file:///Assets/Scripts/MusicManager.cs): Adaptive volume fading between normal and corrupted audio streams.
  - [`Dialogue.cs`](file:///Assets/Scripts/Dialogue.cs) & [`TypewriterManager.cs`](file:///Assets/Scripts/TypewriterManager.cs): Storytelling typewriter dialogue system.

---

## 📁 Project Directory Structure

```text
YasakAsa/
├── Assets/
│   ├── kart/                 # ScriptableObject card definitions & abilities
│   ├── Müzikler/             # Background music tracks (Normal & Glitched layers)
│   ├── Prefabs/              # Card prefabs, UI overlays, projectile FX, and canvases
│   ├── Resources/            # Runtime loaded fonts and assets
│   ├── Scenes/               # Main Menu, 5 Battle Levels, Dialogues, Settings, Win/Lose
│   ├── Scripts/              # Gameplay logic, UI managers, combat algorithms
│   ├── sesler/               # Sound effects (attacks, spells, impacts, stuns)
│   ├── Settings/             # Universal Render Pipeline graphics & quality settings
│   ├── Sprites/              # Character art, backgrounds, enemy phases, UI icons
│   └── TextMesh Pro/         # Custom fonts and styling assets
├── Packages/                 # Package manager manifests and dependencies
├── ProjectSettings/          # Unity project and player settings
└── README.md                 # Project documentation
```

---

## 👥 Credits

Developed for **Sandwich Jam 2** by:

- **Enes Bozdemir** ([@Edyboziron](https://github.com/Edyboziron))
- **Eray Çocuk**

*Special thanks to the Sandwich Jam community for hosting the event!*
