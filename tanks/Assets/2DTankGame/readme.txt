[DON'T OPEN THIS FILE IN NOTEPAD]

TANKS GAME
----------------
This file will explain the
project in detail, outlining the scripts, project layout and how
to use it.

The Game
----------------
This Game, has many game modes 
In co-op two players control a tank and every kill
rewards the player with a point, with the first 
to reach the max score, to be the winner. It has Ai 
Singleplayer mode where a player goes against an npc
enemy or campaign where there is a simple story
and a target in order to win and then there is the
multiplayer mode which is like an online version
of the co-op mode.

Controls
----------------

CO-OP (Scenes -> Game.unity GameArena.unity And Space.unity)
----------------
Pause Game			[ESCAPE]

- Player 1
Move Forward		[W]
Move Backwards		[S]
Turn Left			[A]
Turn Right			[D]
Shoot				[SPACE]
Rocket				[R]

- Player 2
Move Forward		[UP ARROW]
Move Backwards		[DOWN ARROW]
Turn Left			[LEFT ARROW]
Turn Right			[RIGHT ARROW]
Shoot				[O]
Rocket				[P]

SinglePlayer (Scenes vsPC.unity and )
----------------
Pause Game			[ESCAPE]

- Player 1
Move Forward		[W]
Move Backwards		[S]
Turn Left			[A]
Turn Right			[D]
Shoot				[SPACE]
Rocket				[P]

Multiplayer
----------------
Pause Game			[ESCAPE]

- Player 1
Move Forward		[W]
Move Backwards		[S]
Turn Left			[A]
Turn Right			[D]
Shoot				[SPACE]

Controls can be changed in the GameManager object, except multiplayer controls which can be changes in TankMultiplayer.cs.

------------------------------------------------------------------------------------------------------------------------------

Project Features
----------------
- Fully documented code
- Full two player one keyboard functionality
- A complete short campaign
- Multiplayer Mode with photon
- Ai and pathfinding (ASTAR)
- Many variables, so the game can be tweaked to your liking
- Map Creator
- (Almost) black and white tanks so that you can change their colour to 
whatever you want

------------------------------------------------------------------------------------------------------------------------------

Things To Note
----------------
In the Game.unity scene, go to the GameManager game object and 
look at the Game.cs script.
Under the heading Setup, are many different options of which can
be used to tweak your game.

- You can change each player's tank color. (player1Color)
- You can choose if you want the tanks to do one hit kills. (oneHitKill)
- You can choose if a tank can damage itself. (canDamageOwnTank)
- You can choose the respawn delay. (respawnDelay)
- You can choose the score at which the game will end at, once a 
player has reached it. (maxScore)
- You can change how many times a projectile can bounce off a wall
before getting destroyed. (maxProjectileBounces)

- Also, you can change the start values for the tanks when the game
plays. Like their health, damage, speed, ect. If you wish to change
the tank's health or other stats, change it there in the Game.cs
script, and not in the Tank.cs script, as that gets overridden when
the game starts.

- DO NOT include the MapCreator.unity scene in the build settings, as
it cannot be used outside of the editor.

Map Creator
----------------
The map creator is a tool for you to create maps for the game. It
allows you to place down walls and spawn points, as well as removing
them. You then name the map, and save and play it.
This map creator cannot be used in the built project though, only
the editor as it adds files to the Resources folder, which cannot be
done in a built version.
If you don't have the CoOpTankGame folder directly in the Assets folder,
go to the MapCreator.cs

Folder Layout
----------------
> CoOpTankGame
	> PhysicsMaterials
		- Bounce.physicsMaterial2D
	> Prefabs
		- MapButton.prefab
		- Projectile.prefab
		- TankDeathEffect.prefab
		- TankHitEffect.prefab
		- Wall.prefab
	> Resources
		> Maps
			- (this is where all the maps go)
	> Scenes
		- Game.unity
		- MapCreator.unity
		- Menu.unity
	> Scripts
		> MapCreator
			- MapCreator.cs
			- MapCreatorTile.cs
			- MapCreatorUI.cs
		> Menu
			- Menu.cs
			- MenuUI.cs
		- Controls.cs
		- Game.cs
		- Projectile.cs
		- Tank.cs
		- UI.cs
	> Sprites
		- Tank.png
		- Ground.png
		- Projectile.png
		- SpawnPoint.png
		- Wall.png
	- readme.txt

Scripts
----------------
- Game.cs
This scripts controls the game, loads the map, monitors the scores
and controls the win state.

- Tank.cs
This script controls the tanks. It moves, turns and shoots it. This

- Projectile.cs
This script controls the projectiles that the tanks shoot. It checks
for collisions and deals damage to the enemy. It also handles different
types of ammunition sych as rockets where bounce physics are added
and an agle needs to be calculated in order for the sprite to be shown
correctly

- UI.cs
This script controls all the in-game UI. The health bars, score text, 
pause menu, home screen, kill screen and win screen.

- Controls.cs
This script controls all the keyboard input for the two players. 
All the keys can be changed to suit your liking.

- MapCreator.cs
This script controls the placing of tiles in the map creator, as well
as saving the map.

- MapCreatorTile.cs
This script holds the data of a tile in the map creator.

- MapCreatorUI.cs
This script controls all the map creator UI, from the button presses
to the name input.

- Menu.cs
This script controls the menu. It changes the pages when needed, loads
in the maps and begins the game.

- MenuUI.cs
This script controls all the menu UI. It spawns in the map select buttons
and controls what happens when the buttons get pressed.

Scenes
----------------
- Menu.unity
This scene is where the game will launch and allows the player to go and 
choose a map to play, or quit the game.

- MapCreator.unity
This scene is where you can create maps for the game. More info on the map
creator can be found at the top of the document.

- Game.unity
This scene is where the standard game is played (jungle level). On loading 
the scene, it searches the PlayerPrefs for the "MapToLoad" string, which 
indicated which map the game will load up to play. The players can press 
escape to pause the game and go to menu or quit.

- GameArena.unity
This scene is where the arena game is played. On loading the scene, it searches 
the PlayerPrefs for the "MapToLoad" string, which indicated which map the game 
will load up to play. The players can press escape to pause the game and go to
menu or quit.

Space.unity
This scene is where the space level is played. On loading the scene, it searches 
the PlayerPrefs for the "MapToLoad" string, which indicated which map the game 
will load up to play. The players can press escape to pause the game and go to
menu or quit.