# DungeonDelver
what worked
The textbook implementation was lengthy, but very robust, easy to understand, and had no issues.
GitHub Project boards and the burndown chart helped with organizing who does what and collaborating.


what was hard
The dungeon maps were saved to a text file, but something with github made them unreadable by Unity. You had to send them separately to your teammates over email.
The textbook prototype was set for 1920 x 1080p. The problem this poses is that the text and few other art assets do not scale well with screen resolutions other than 1080p. 
Not understanding what files to stage in a git commit.
Tile swaps for skeletos, spikers and keys were initially a problem but eventually got resolved.




what extensions you implemented
2 new dungeons
3 new enemies: a bat which moves fast but does not hit hard, a knight who moves slow but hits hard, and an upgraded form of the knight who is impervious to attacks from his front
Spikers move.
The end of the dungeon has a treasure item which loads the next scene.
A start and game over screen.
Enemies do not move unless the player is in the same room.
Original music composed by John Miller for all three dungeons.
6 sound effects for the following actions: taking damage, dealing damage, blocking damage (KnightBoss only), unlocking doors, picking up grappler and treasure, picking up health and keys


Why this project is Complex and Large-Scale
This project required the longest amount of time invested into it than any other project this semester. Peter and Joseph commented that it was the longest-running project in their college career. The project had many different components: over 20 scripts. Some scripts required understanding of inheritance and polymorphism. As one example, the Knight script which drives the Knight enemy’s behavior inherits from an interface IFacingMover and a super-class Enemy. This project also had complexity from remote collaboration and working with Git repositories.
