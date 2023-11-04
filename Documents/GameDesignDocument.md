# Be Neutral
# Overview and vision statement
An immersive game that throws you into the heart of NeutralVille, a world with perfect balance and neutrality. But the equilibrium once defined has been destroyed by a scientific accident. As a player you will deal with enemies and obstacles, the concept of balance isn't just a theme, but a gameplay mechanic. As you traverse through NeutralVille's levels you will have to solve intricate puzzles and face formidable challenges, you'll need to make choices that will also influence your teammate game.

# Gameplay (Alessandro S.)


# Characters (Alessandro M.)

## Andy
- **Description**: Andy is a young boy characterized by his positive electrical charge, represented by the color red.
- **Abilities**:
  - *Projectile Attacks*: Andy can shoot positively charged projectiles, colored in red, to eliminate enemies with a negative electrical charge.
  - *Positive Force Field*: Andy has the ability to create a positive force field that can repel objects with a negative electrical charge in the environment. However, using this ability consumes his energy.
  - *Weapon Preference*: Andy excels in using heavy and powerful weapons, such as hammers and rocket launchers.

## Caty
- **Description**: Caty is a young girl characterized by her negative electrical charge, represented by the color blue.
- **Abilities**:
  - *Projectile Attacks*: Caty can shoot negatively charged projectiles, colored in blue, to defeat enemies with a positive electrical charge.
  - *Negative Force Field*: Caty possesses the ability to create a negative force field that can push away objects with a positive electrical charge in the environment. Using this ability drains her energy.
  - *Weapon Preference*: Caty is proficient in using small and fast weapons, including swords and rapid-fire firearms.

**Relationship and Gameplay Mechanics**:
- Andy and Caty are the game's two main characters, each having a distinct electrical charge (positive and negative) represented by their respective colors (red and blue).
- The attraction between their opposite electrical charges allows them to stay anchored to the ground, preventing them from floating away.
- Both characters have a life bar, denoted by the amount of electrical charge they possess. This charge can be stolen by enemies with the opposite electrical charge.
- To maintain their connection, Andy and Caty need to stay within one screen's distance of each other. If they move too far apart, the force anchoring them to the ground weakens, leading to a game over.
- The two characters are typically positioned on opposite sides of the screen, but they can swap positions using a special in-game item.
- Their primary means of offense are their charge-based projectiles, making it crucial for them to cooperate in battling enemies and solving puzzles in the game world.


# Story (Salah)


# World (Rui)


## Level 1


## Level 2


## Level 3


# Media list


# Technical specification (Matteo)
## Genre
  Puzzle game, 2D
## Platform
  PC game.

## Multiplayer
  The game is based on cooperation and collaboration with your teammate. The necessary number of player it's 2, that represents the two main characters of the game.
  
## Game Mechanics
 1. Character Proximity Alarm: 
      Notify players if characters become too distant, leading to a game-over scenario if not         addressed.
    Important to track the position of the two players to calculate the distance.
 3. Character Switching:
      Allow players to switch between characters/sides for puzzle-solving and progression.
    Important to temporary save both players' positions to switch them.
 4. Shared and Unique Abilities:
      Characters has both shared abilities and unique abilites. The unique ones are linked to         the characters' powers.
    Important to implement some variables to identify which character is playing and so which       abilitie you need to use
 6. Synchronized Jumping:
      Enable characters to jump higher when they jump simultaneously or when they are distant.
    Important to create a function that allows you to calculate the variation of the jump.

## Game Elements
 1. Positive and Negative Interactions:
      Objects and enemies divided into positive (red) and negative (blue) categories, each with       unique interactions.
 2. Life and Charge System:
      Player life is represented by a charge system, and enemies aim to steal this charge.
 3. Cooperative and Exclusive Sections:
      Design levels where players generally progress together but include exclusive sections          for each character, encouraging cooperation.
    Important to create separate levels remembering that the other player has to do something
    meanwhile his teammate is playing
 5. Different Weapons:
      Implement different weapon mechanics for each player (e.g., guns for one, swords for the        other).



## Game physics
  The two players will walk on the two sides of the same level, one on the opposite side of the other.

## Data flow

## Game Shell
### Starting screen
  Where you can start a new game, load a game you saved before
### New game screen
  Where you can choose the character you will use during the game
### Load game screen
  Where you can choose the saved game from a list
### Main play screen
  Gameplay screen, with a camera that follows your movements.



# Team
1. Chen Rui (Art)
2. Mahadi Salah (Developer)
3. Melchiorre Matteo (Developer) 
4. Mosconi Alessandro (Developer)
5. Sassi Alessandro (Developer)

# Deadlines
