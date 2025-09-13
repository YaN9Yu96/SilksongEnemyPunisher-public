# 丝之歌敌人AI惩罚 (Silksong EnemyPunisher)

**BepInEx Plugin for Hollow Knight: Silksong**

[![License: GPLv3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0.en.html)

---

## 🔹 许可证 (License)

本插件使用 **GNU GPLv3 许可证**开源。  
你可以自由使用、修改和分发代码，但**必须遵守 GPLv3**的条款。  
This plugin is open-sourced under the **GNU GPLv3 License**.  
You are free to use, modify, and distribute the code, but **must comply with the terms of GPLv3**.

[GPLv3 许可证全文 / Full text of GPLv3](https://www.gnu.org/licenses/gpl-3.0.en.html)


---

## 🔹 MOD 说明 (MOD Description)

### 中文说明

- 放慢敌人的准备动作（可能是准备攻击、跳开等）。
- 当敌人进行连续的快速位移（可能是冲刺、闪避、跳开）时，减慢其位移的速度。
- ⚠️ 尚未进行大量测试特别是 Boss 行为，因此暂时无法确保稳定性和意外情况。

**为什么需要这个 MOD**  
部分敌人的准备动作只有 100ms 左右，这已经低于一般人类的反应时间（200–500ms），且连续的快速位移也容易让玩家感到被戏耍，使战斗显得不够公平。  
本 MOD 旨在缓解这些情况，尽量不破坏开发组的意图，只是维持合理公平的平衡性（当然这取决于你调整的参数）。

### English Description

- Slows down enemy preparation actions (such as attack wind-ups, jumps, etc.).
- When enemies perform consecutive rapid displacements (such as dashes, dodges, or leaps), their movement speed will be reduced.
- ⚠️ Extensive testing, especially of boss behavior, has not yet been conducted, so stability and unforeseen issues cannot be guaranteed for now.

**Why this MOD is needed**  

Some enemies have preparation actions that last only about 100ms, which is already below the typical human reaction time (200–500ms). This not only affects combat but also makes enemies’ rapid consecutive movements feel like they are toying with the player, making fights feel somewhat unfair.
This MOD aims to alleviate these situations as much as possible, while trying not to undermine the developers’ original intentions—simply maintaining a reasonable and fair balance (of course, this depends on the parameters you adjust).

---

## 🔹 使用与编译指南 (Usage & Compilation)

请参考 [BepInEx 官方仓库](https://github.com/BepInEx/BepInEx) 了解如何设置开发环境、编译插件以及管理依赖。  
Please refer to the [BepInEx official repository](https://github.com/BepInEx/BepInEx) for setting up the development environment, compiling plugins, and managing dependencies.

---

## 🔹 DLL 依赖名单 (Required DLLs)

- `0Harmony.dll` 
- `BepInEx.dll`   
- `BepInEx.Harmony.dll`
- `Assembly-CSharp.dll`  
- `UnityEngine.dll`    
- `UnityEngine.CoreModule.dll`    
- `UnityEngine.Physics2DModule.dll`    
- `PlayMaker.dll`  
- `TeamCherry.TK2D.dll`  

> ⚠️ 这些 DLL **不包含在仓库中**，请从你自己的游戏安装目录获取。<br>
> ⚠️ These DLLs are not included in this repository; please obtain them from your own game installation directory.
