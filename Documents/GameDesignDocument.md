# Be Neutral
# Overview and vision statement
An immersive game that throws you into the heart of NeutralVille, a world with perfect balance and neutrality. But the equilibrium once defined has been destroyed by a scientific accident. As a player you will deal with enemies and obstacles, the concept of balance isn't just a theme, but a gameplay mechanic. As you traverse through NeutralVille's levels you will have to solve intricate puzzles and face formidable challenges, you'll need to make choices that will also influence your teammate game.

# Gameplay
### Core Gameplay Aspects
BeNeutral at its core is developed as a 2D side-scrolling platformer divided into 3 main levels, where the two main playable characters, Andy and Cathy, find themselves separated on the opposite sides of the screen. The main goal of the game is to progress through each level, gaining access to unique abilities and powerful weapons essential to defeat the many enemies and final bosses obstructing the way to the end.

The game revolves around the idea of "reconciling the opposites" with a focus on magnetism. The two characters progress through the levels on the two opposite sides of the platform and level terrain (i.e. one character is above, the other one below), with such levels developing on both sides of the screen even in different ways, which sometimes oblige the two players to collaborate towards the final goal.

As the two characters want to be reconciled, they are sensible to being separated too much. Thus, if their relative distance exceeds a certain threshold for too much time (for example, 3 seconds), the two playable characters will start losing their magnetic charge. The amount by which their charge depletes increases dynamically (i.e. the more they stay too far apart, the higher the charge delta), leading to a game over if their charge reaches zero.

Each character owns unique abilities and weapon preferences, making them fit for two different playstyles. Andy, the boy, is able to handle high-damage heavy weapons, but moves slower when he has them equipped, while Cathy, the girl, uses lighter weapons which make it more suitable for fast-paced combat. At the beginning of the game, players will start without any weapons equipped, and will have to use the unique abilities of each character to defeat enemies and progress, and weapons are acquired afterwards.

Each level (described in detail in the [World section](#world-rui)) is themed around a specific area of the world, and has a checkpoint in the middle to let the players restart from there instead of the beginning of the world in case of game over.

### Units and Traps
Each level also contains multiple enemies and environmental traps, among which we have:
- **Enemies**:
  - **Magneblocks**: small, cubic, walking robots with a cute but dangerous appearance, trying to get as close as possible to steal charge from the player. These are the most common and weakest enemy class
  - **Gaussguards**: Giant, slow-moving golems, with a rarer, higher-level variant equipped with a magnetic shield. When destroyed, they release a burst of magnetic energy that damages the player if too close
  - **Fluxbombers**: Enemies equipped with magnetic bombs that can be thrown at the player. When hitting the player, they deal a high amount of damage, while if they hit the ground they create a temporary spherical field that, if touched by the player, stuns them and deals a low amount of damage
  - **Magnemortars**: stationary turrets shooting projectiles in the direction of the player
  - **Boss Enemy**: it has to be faced in a "chamber" (i.e. an environment which gives no way to escape) and is present in the two parts of the screen (to allow both players to fight against it at the same time). The two "halves" of the boss are synchronized and represent the same enemy, so that the health is unified between the two.
- **Environmental Traps**:
  - **Magnetized Spikes**: spikes that become magnetized intermittently, so players must time their movements to avoid getting pulled into the spikes or pushed into dangerous areas.
  - **Rotating Spikes** placed in areas of levels to encourage players to make longer jumps, find alternative paths or just make it more difficult to progress
  - **EMP Wall**: when the player enters a specific section of a level, this trap is activated and starts chasing after players. The two players will need to reach the end of this section before the timer expires. The Wall speeds up gradually over time, getting closer and closer to players, so they have to complete the section fast enough to avoid game over.
  - **Magnetic Vortex**: A tile on the ground that creates a powerful magnetic pull that attracts or repels players

### Gameplay Elements
Levels contain some small environmental puzzles to encourage collaboration between players. To progress in levels, some parts are locked behind **magnetic barriers**: by default, only one of the two players (depending on the polarity) is allowed to pass through. To deactivate it and allow the other player to pass, the two players may have to be at the same time on two connected **pressure plates** or defeat an enemy (the player that could pass through the barrier is the one to fight). Pressure plates can also be used to reveal hidden treasures: when a player steps on them, the game pauses for a bit to focus on the appearance of a treasure chest and immediately resumes afterwards.

During their progression, players will find **treasure chests** containing items or power-ups, including the main weapons of each character. These can be visible from the get-go, or hidden and spawned only by defeating an enemy or stepping on a pressure plate.

**Life tokens** (used to resume from the latest checkpoint) can be found in each level, as they are strategically hidden in places that are accessible to players, or after players manage to collectively obtain a set amount of **coins** (200 coins total, summing both players' coins). Coins can be obtained by collecting the ones spread across levels or by defeating enemies, and the total number of earned coins is always shared between players.

Players can also gain magnetic charge, by getting **charge containers** spread throughout each level or in treasure chests.

Characters can also find **weapons** to deal more damage. There are different types of weapons, each specific to a character:
- **Magmallet**: exclusive to [Andy](#andy), stops the user from moving during operation, is slow but deals a very high amount of close-range damage
- **Magnetmissiles**: exclusive to [Andy](#andy), shoots one high-damage RPG in the direction it is pointed to. The user is free to point the launcher in whatever direction they choose, and can be used while moving (but with reduced speed). It stops the player for some instants during the launch of the missile
- **Fluxblade**: exclusive to [Cathy](#caty), is a longsword that deals moderate damage but has a high damage-per-second rate and enables full mobility while equipped
- **FluxPistol**: exclusive to [Cathy](#caty), is a light pistol that shoots low-damage magnetic bullets at a high rate, without hampering mobility.
Additional abilities specific to each character are described in the [Characters section](#characters).

### Game Physics and Stats
#### Player Stats
Characters also have two main statistics: **magnetic charge** and **stamina**.

The **magnetic charge** level acts as the characters' health. Players can lose their charge in multiple ways, for example by being hit by enemy attacks or due to it being stolen by oppositely-charged enemies (which, as a result, increase their health), and need to keep it above zero in order to avoid game over.
When the game starts, a set number of **life tokens** are given to players, and whenever one of the two characters dies a life token is consumed and the game is restarted (for both players) from the latest checkpoint, but if no tokens are available the players will need to restart from the beginning of the first level.

The **stamina** level is used to let players strategically take advantage of some of their abilities without using them too much repetitively. Each ability use will cost some predefined amount of stamina, and if its level is not enough the ability cannot be activated. Stamina is then regained gradually over time using a linear time-dependent law, in the form of `∂Stamina = K * ∂T` with `K` being the stamina points per second to be awarded and `∂T` the delta time (in seconds).

#### Physics
Players are subject to gravity which has different directions depending on the side of the screen the player is located (downward gravity for the player at top, upward for the one at the bottom). Movement is horizontal with some acceleration/deceleration being present when starting/stopping to walk or run, but players can also jump to reach higher platforms.

#### Combat
Combat is both close-range and at a distance using ranged weapons or ranged abilities.
When using ranged attacks, each attack has a maximum range of effect, after which it decays, the bullet disappears and the attack has no effect.
With close-range attacks, since characters are humanoids, they punch enemies to deal damage.
Abilities can be used by players to deal additional damage or defend from specific attacks, but consume stamina, so they cannot be used continuously.

### Enemy AI
Enemies are controlled by an internal game "AI", which takes into account the aim of the enemy and tries to reach such goal.
- *Magneblocks*, which aim at stealing the player's charge, attempt to move towards the player using their (limited) moveset, which in this case is restricted only to horizontal movement. Their "intelligence" may be parametrized as the frequency at which they update their movement target.
- *Gaussguards* will have a more "patrol"-style behavior: when idle they move back and forth slowly, but if they spot the player they try to get closer to them and start attacking if at close range.
- *Fluxbombers* behave similarly to *Gaussguards* but when the player is spotted they start throwing bombs towards the player at regular time intervals:
- *Magnemortars* are stationary turrets with a limited-range laser radar used to identify enemies. When they spot the player, they start tracking it and shoot projectiles in their direction. Here "intelligence" can be parametrized as the delay/damp of the bullet direction with respect to the actual player direction, as well as the "error rate" in such direction.

# Characters

## Relationship and Characters Mechanics:
- Andy and Caty are the game's two main characters, each having a distinct electrical charge (positive and negative) represented by their respective colors (red and blue).
- The attraction between their opposite electrical charges allows them to stay anchored to the ground, preventing them from floating away.
- Both characters have a life bar, denoted by the amount of electrical charge they possess. This charge can be stolen by enemies with the opposite electrical charge.
- To maintain their connection, Andy and Caty need to stay within one screen's distance of each other. If they move too far apart, the force anchoring them to the ground weakens, leading to a game over.
- The two characters are typically positioned on opposite sides of the screen, but they can swap positions using a special in-game item.
- Their primary means of offense are their charge-based projectiles, making it crucial for them to cooperate in battling enemies and solving puzzles in the game world.

### Andy
- **Description**: Andy is a young boy characterized by his positive electrical charge, represented by the color red.
- **Abilities**:
  - *Projectile Attacks*: Andy can shoot positively charged projectiles, colored in red, to eliminate enemies with a negative electrical charge.
  - *Positive Force Field*: Andy has the ability to create a positive force field that can repel objects with a negative electrical charge in the environment. However, using this ability consumes his energy.
  - *Weapon Preference*: Andy excels in using heavy and powerful weapons, such as hammers and rocket launchers.

### Caty
- **Description**: Caty is a young girl characterized by her negative electrical charge, represented by the color blue.
- **Abilities**:
  - *Projectile Attacks*: Caty can shoot negatively charged projectiles, colored in blue, to defeat enemies with a positive electrical charge.
  - *Negative Force Field*: Caty possesses the ability to create a negative force field that can push away objects with a positive electrical charge in the environment. Using this ability drains her energy.
  - *Weapon Preference*: Caty is proficient in using small and fast weapons, including swords and rapid-fire firearms.



# Story
NeutralVille wasn’t always a place full of harmony and equilibrium. After decades of struggles and conflicts, the townspeople finally managed to isolate themselves from the rest of the world. With the help of a brilliant scientist, they built a forcefield around the town to protect it from outside danger and allow the citizens of NeutralVille to live in this utopia. However, this forcefield required a lot to maintain, and the scientists had to continuously enforce it using advanced technologies to prevent it from weakening.

After years of peacefulness, the grandson of the genius scientist who helped erect the barrier was born. He was smart, and hardworking, but also mischievous at times. He aspired to be a great scientist, just like his grandfather. So, he joined the scientists who worked in the energy laboratory, and he was on the cusp of making a grand discovery that would help the force shield last forever without the need for constant reinforcement. However, this discovery would require the collective power of every person in town.

At first, the other scientists were skeptical about this, as it would drain every ounce of power from the town and make it vulnerable if something went wrong. But eventually, everyone became excited about the prospect, wholeheartedly believing in this young scientist just as the older generation believed in  his grandfather. All the scientists came together and decided to help the young scientist achieve this goal. They invested all their resources, efforts, and time into it.

When it came time to test his invention, he realized he had made an error, but it was too late to fix it as the experiment had already begun. Not knowing what to do, he told everyone to evacuate and stayed in the lab, hoping that he could somehow stop the experiment or at least limit the catastrophe that would happen. Everyone else ran out of the lab, horrified, as it looked like the lab was about to explode.

In his last attempt to save everyone, the young scientist decided to contain the explosion in another dimension using a device he had built a while ago but had never been able to test it as he never knew what the consequences would be. In this desperate moment, he activated the device, and just as the device started to work, the explosion happened. In the aftermath of this horrific accident, the scientist found himself severed from another part of himself, and his mind and soul were split into two. And thus, the journey of our protagonist begins.


# World (Rui)
In the peaceful town of NeutralVille, where citizens coexisted harmoniously, embracing their neutral charges and stability, life unfolded like scenes from a Medieval European town, but with a touch of high-tech marvels. As the citizens peacefully slumbered, they charged themselves, preparing for another day of routine activities. They went about their days, buying groceries, attending school, engaging in lively conversations, and dancing in the town square. Amid this serene setting, a brilliant yet eccentric scientist embarked on an experiment within the confines of a state-of-the-art laboratory. The purpose of this experiment remained a mystery, shrouded in the scientist's ingenious mind.

![02!](/Documents/02.jpg)

![01!](/Documents/01.jpg)

![03!](/Documents/03.jpg)

However, fate took an unexpected turn one fateful day when the lab, engulfed in a blinding explosion, tore the fabric of their peaceful existence asunder. The world was abruptly split into two distinct realms: the positive world and the negative world. Amidst the chaos, the scientist miraculously survived, but their being was divided into two entities – the optimistic and positive-minded Andy, and the cunning and resourceful Caty, embodying the negative polarity. Now separated, the duo found themselves in parallel worlds, each facing unique challenges and obstacles.

![04!](/Documents/04.jpg)

Bound by a powerful connection, positive Andy and negative Caty embarked on separate journeys, determined to reunite with one another. Their paths were laden with hurdles, requiring wit, courage, and ingenuity to overcome. With each level they traversed, they inched closer to the moment when their positivity and negativity would harmonize once more, bridging the gap between their worlds and restoring the balance that had been disrupted. Little did they know, their adventure would not only test their individual strengths but also teach them valuable lessons about the power of unity, resilience, and the unwavering spirit of hope in the face of adversity.

## Level 1
In the serene surroundings of Harmony Heights, Andy and Cathy find themselves standing on opposite sides of a series of simple platforms. The level begins with easy steps, encouraging players to coordinate their movements and magnetic charges. As they progress, they encounter the first challenge: a magnetic barrier blocking their path. To deactivate it, they must work together, standing on two connected pressure plates simultaneously. By doing so, they gain access to a treasure chest, revealing the Magmallet for Andy and the Fluxblade for Cathy.

With their new weapons in hand, they continue their journey, facing Magneblocks – small, cubic robots attempting to steal their magnetic charge. Andy and Cathy collaborate, using their unique abilities and newfound weapons to defeat these enemies. They collect coins and charge containers along the way, strengthening their magnetic charges for future challenges.

## Level 2
In Magnetic Mayhem, the landscape becomes more complex. Moving platforms and dynamic obstacles present a challenge as Andy and Cathy navigate through this level. They encounter Gaussguards, slow-moving golems with magnetic shields, requiring strategic coordination to defeat. The level also introduces rotating spikes, forcing the players to make precise jumps and find alternative paths.

As they delve deeper, they come across an EMP Wall, activated as they enter a specific section of the level. The Wall chases them, gradually gaining speed, urging them to complete the section swiftly. Andy and Cathy's magnetic synergy becomes crucial as they work together to outmaneuver the chasing wall, ensuring their survival.

## Level 3
At Equilibrium's End, the final showdown awaits. The level is split into two chambers, each housing a half of the Boss Enemy. Andy and Cathy must confront the synchronized halves simultaneously, requiring impeccable teamwork and coordination.

The Boss Enemy, a formidable foe, utilizes powerful magnetic attacks, challenging the duo's ability to balance their magnetic charges and work together effectively. To defeat the Boss, they must exploit its weaknesses, timing their attacks and movements carefully. Throughout the battle, the Boss releases Magnemortars and Fluxbombers, adding to the intensity of the fight.

As the battle reaches its climax, the players must combine their strengths, synchronizing their movements to overcome the Boss's final onslaught. Victory comes to those who maintain their balance and harmony, restoring NeutralVille to its former equilibrium.

With their mission accomplished, Andy and Cathy stand side by side, their magnetic charges perfectly balanced once more, symbolizing the restoration of harmony and unity in NeutralVille.

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

### Week 1 (November 6 deadline) 
- First meeting and brainstorming on figma platform
- Github repository (Alessandro Mosconi)
- First art prototype (Rui Chen)
- GameDesignDocument draft - Gameplay (Alessandro Sassi)
- GameDesignDocument draft - Characters (Alessandro Mosconi)
- GameDesignDocument draft - Story (Salah Mahadi)
- GameDesignDocument draft - World (Rui Chen)
- GameDesignDocument draft - Technical specification (Matteo Melchiorre)

### Team meeting (November 5) 
- Github repository created and shared
- Game design document draft completed
- First art prototype finished
