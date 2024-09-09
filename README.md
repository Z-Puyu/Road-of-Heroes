# Road of Heroes

## Overview

### Story

In a fantasy world, an apocalyptic catastrophe known as *the Eternal Night* has consumed the continent. In the ever-lasting darkness which has covered the sun and blocked all the light, a profound, contagious curse has spread across the world, bringing abominations from an evil dimension.

4 adventurers, each from a different background, have come together to embark on an epic journey to seek out the sources of *the Divine FIre* in hope of fending off the terror of the Night. In their way, with determination, our heroes will meet new comrades, anihilate evil beings, save the people and help reclaim their homeland. Moreover, there is more to the mysteries behind the catastrophe and the world to be discovered in this great expedition.

### Gameplay

Project: Fire Seeker is a **2D roguelike turn-based tactical role-play game**. In the game, the player plays as a group of adventurers to set out for a journey to seek the Divine Fire in order to save the world from an apocalyptic catastrophe known as the Eternal Night. In this journey, the player will unravel the mysteries of the world, collect powerful relics to gear up the adventurers, and fend off enemies and complete quests to claim rewards. The world is divided into regions with each region controlled by a powerful demon. The player needs to defeat the demon in order to proceed to the next part of the journey, until the adventurers finally arrive at the Divine Fire.

## Characters

The player needs to recruit adventurers to power up their team to defeat the enemies in the way. In both the world map and the battle map, the player controls the adventurers' movement using mouse clicks only. 

Each adventurer has 6 base stats:

- **Speed**: characters with higher speed takes actions before those with lower speed in a battle turn.

- **Agility**: increases the chance of dodging off attacks and the distance the character can move through in a battle turn.

- **Strength**: increases the damage dealed from physical attacks.

- **Stamina**: increases the HP of the character.

- **Intelligence**: increases the Magicka of the character.

- **Perception**: increases the chance of dealing critical attacks.

Based on the above 6 stats, each adventurer's physical conditions are reflected as the following 4 values:

- **Health (HP)**: character dies when the HP reaches 0.

- **Magicka**: character consumes Magicka to cast spells.

- **Sanity**: character receives heavy penalties and de-buffs when sanity is low.

- **Fatigue**: lowers the damage dealed from attacks.

Each adventurer also has a **race** and each race offsets the above stats in its own unique way. The races include **human**, **elf**, **dwarf** and **orc**.

Adventurers use **skills** to fight in battles. What skills can be learnt is determined by the adventurer's **profession**.

## Gameplay

The game is set in a fictional fantasy world and the player starts with four default characters in their expedition team. In order to reach the Divine Fire, the player needs to pass through several **regions**. Each region is guarded by a powerful **boss** which the player must defeat in order to proceed to the next region. However, the player can freely choose the right timing to challenge the boss. Prior to the boss fight, the player can complete **levels** to upgrade their expedition team.

### Exploration

Each level emulates a **turn-based board game** with a **procedurally generated grid-based world map**. At the start of each turn, the player rolls dice to determine how many moves can be taken in the turn. Some **random encounters** might be triggered when the player moves to a certain cell in the map, which are events to grant rewards or penalties for the player. Examples of such encounters are:

- At a **survivor settlement**, the player can **recruit new adventurers**, **trade items**, **receive side quests** or **rest** (to restore team condition);

- At a **lair of darkness**, the player will enter a **battle** against fierce enemies;

- At a **place of mystery**, the player will have a chance of obtaining valuable **items** or **buffs** for the team.

- At a **campfire**, the player can perform a limited number of actions to restore team condition to prepare for future exploration.

Each level comes with a **main quest**. Completing the main quest will lead to great rewards for the player. On the other hand, finishing the level without completing the main quest will lead to penalties on the player.

### Battles

Beside encounters, battles can also be randomly spawned in the map. The difficulty of the battles is determined based on the **average level** of the adventurers in the team. In each battle, the player chooses up to four adventurers to fight the enemies. The battle system emulates a **turn-based wargame**, where the player's adventurers and the enemies take turns to make moves or use skills to attack each other, until one side wins. Adventurers killed by enemies will be permenantly lost. If the player has lost all adventurers, the current level will be forced to terminate and 4 random adventurers will become available for recruitment so that the player can continue the game.

**Relics** may be collected either as rewards from battle or through random encounters. The player can equip adventurers with these relics to upgrade their combat abilities.

### Cities

In each level there will be one and only one **city**. The player needs to reach the city in order to finish the current level. In the city, the player rolls dice to gain **action points**, which can be used to perform the following actions to prepare for the next level:

- **Training**: the player can choose one adventurer to spend money upgrading the adventurers' skills.

- **Rest**: the player can let an adventurer take a rest to restore sanity and HP and reduce fatigue.

- **Trade**: the player can sell or purchase relics and other useful **consumable items**.

- **Recruitment**: the player can recruit new adventurers or fire current adventurers from the team.

- **Treatment**: the player can spend money curing a disease or removing an undesirable trait for an adventurer.

## Game Experience

The game will have a **mysterious** base tone to symbolise a sense of the unknown in the adventurous journey. The battles and exploration should emit a sense of **darkness**, a bit **daunting** maybe, but the story will be **hopeful** in the end as the player approaches the Divine Fire.

In terms of art, we wish to use **vintage-styled paper UI** for the game map, and **hand-drawn sketches** as icons for most of the game objects in the level map. We aim to make the game playable with only mouse clicks, so as to simulate the experience of a **board game**.

## Items

There are 3 types of items which can be collected and used in the game.

**Relics** are powerful weapons, armours or trinkets which can improve the combat abilities of adventurers. They may be spawned in game levels at random, or appear as rewards after battles, or be purchase from **merchants**. 

**Consumables** are single-use items which aid in combat or level exploration. They are usually purchased from merchants, but the player might also be able to find them scattered across game levels. Some consumables include:

- **Bondage**: cancel **bleeding** effect and restore a small amount of HP.

- **Herbs**: cancel **poisoned** effect and restore a small amount of HP.

- **Mage's Brew**: restore a medium amount of Magicka.

- **Alcohol**: restore a small amount of sanity and reduce sanity loss.

- **Stimulant**: reduce s medium amount of fatigue, but increase fatigue generation.

**Gold** is the currency in the game. The player can spend gold purchasing other items, or selling other items for gold at merchants. Merchants can be found in settlements, cities. There is also a very small chance of a merchant appearing in the wild at random.
