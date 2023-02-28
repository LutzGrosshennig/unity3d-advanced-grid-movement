# Advanced grid based movement for dungeon crawler type games.

An more advanced approach for Dungeon Master, Eye of the Beholder, Legend of Grimrock style grid based movement in Unity3d games.

In the past I wrote the https://github.com/LutzGrosshennig/unity3d-AnimatedGridMovement-Camera script which I used for my 'Xenomorph 2409' game project. back then.

Some fellow gridders at www.dungeoncrawlers.org pointed out that the movement of that script is way to linear and they are right about it.
So I started to write this more advanced script that will give you a lot more control over the player movement.

# Screenshots

![Screenshot](https://github.com/LutzGrosshennig/unity3d-advanced-grid-movement/blob/main/Screenshots/Screenshot_1.jpg)
![Screenshot](https://github.com/LutzGrosshennig/unity3d-advanced-grid-movement/blob/main/Screenshots/Screenshot_2.jpg)

# Whats new?
 * Fully editable animation curve for the movement and head bob
 * Command queueing. Plan your steps in advance!
 * Run/dash mode (experimental). Hold and press 'W' to advance even faster (with separate headbob/movement curves).
 * Height system. You can now climp stairs and slopes. This enables a new dimension (literally) for level design!
 * A basic event based footstep system and an generated example "footstep" sample you need to replace really soon ;-)
 * Full Unity3d project including a prebuild sample level and some crappy 3d collision detection code (just to showcase)
 * Oblique viewing frustum. A common problem of new 3d dungeon crawlers is, that you can't see enought of the tile you are currently standing on, this is caused by the default symetrical view frustrum on most 3d engines, switching to an asymetrical view frustrum adresses the issue. This is done by shifting the camera lens down in the physical camera emulation of Unity3d.
 * Improved torch light. I added some Perlin noise based animation to the torch to make it look more realistic and added some sound to it.
 * Switched over the linear colorspace.
 * Made the level objects static to improve performance quiet a lot. (This is just a example none the less).

# Materials used

 * The materials and textures in the FreePBR folder are taken from www.freepbr.com
 * The simple wall texture is taken from https://github.com/LutzGrosshennig/amiga-xeno-dungeon-crawler (be gentle to it, its 30y old Amiga pixel 'art').

# SFX used

* The "Footstep" was done in SFXR by myself. You should replace this with something better.
* bonfire-hq-6991.mp3 Sound Effect from <a href="https://pixabay.com/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=6991">Pixabay</a>

# Usage

There is a prebuild test level to walk around. Since this level is prebuild there is no 2d map to back it up but it gives you full control of your 3d world. Of course you can also generate the 3d level based on a 2d level map and then use the 2d map to apply your game logic, or you derive a 2d map based on what you build in 3d the choice is yours, both paths have there pro's and con's.

I'll update the documentation step by step but if you have a specific question, just open an issue and I'm happy to help.

Should be pretty self explainatory if you are not completly new to Unity3d. If you have questions create an issue and I will answer and update this readme. Otherwise you may want to check out the simpler version of the script (linked above).

Documentation is work in progress.

## The scripts

You will find four scripts that do everything you need to get going.

### AdvancedGridMovement.cs

This is the main script that handels the actual movement and the animation of the movement. It also contains some very basic collision detection that I want to move out of the responsibilites of the script at some point in time. The collision detection code is very crude and I advice against the usage inside of production code as it uses tags to find gameobjects which can get rather slow pretty quickly in large scenes.

![AdvancedOptions](https://github.com/LutzGrosshennig/unity3d-advanced-grid-movement/blob/main/Screenshots/AdvancedGridMovement.png)

### InputHandler.cs

Since the sample still uses the old Input-System I wanted a way to separated the game logic from the pesky GetKeyXYZ calls required by the old system. Think of this as a simplified version of new Input-System where you are able to bind certain keys to an action. Its not really needed for the grid movement but makes it easy to separate game logic from the Input system. If you want to use the new Input-System in your project you dont need this.

![InputHandlerOptions](https://github.com/LutzGrosshennig/unity3d-advanced-grid-movement/blob/main/Screenshots/InputHandler.png)

### MovementQueue.cs

This script handels queuing of commands and has a dependcy towards the AdvancedGridMovement.cs script to detect if the last movement command is completed before triggering a queued command. Therefore they need to exist on the same Gameobject.

![MovementQueueOptions](https://github.com/LutzGrosshennig/unity3d-advanced-grid-movement/blob/main/Screenshots/MovementQueue.png)

### FootstepSystem.cs

This is a simple system that can be used to add a footstep system to the player. Just assign two AudioSources for the left and right foot and assign some Audioclips that are played when the method Step() is called. The scipt handles left and right steps automatically so you just need to call Step() or Turn() to initiate the playback of the assigned AudioClips.

![FootstepSystemOptions](https://github.com/LutzGrosshennig/unity3d-advanced-grid-movement/blob/main/Screenshots/FootstepSystem.png)

# Whats next?

The code could definiatly use some refactorings. There are some dependcies and responsiblities I'm not happy with. However "done!" is always better than "I will only release it when its perfect!". Of course I will use the scripts in "Xenomorph 2409" as well.

# Challenge

You are not happy with the default animation curves and you found better ones? Great! Please share them with us!

Have fun with it!

Lutz
