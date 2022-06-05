# unity3d-advanced-grid-movement

An more advanced approach for grid based movement in Unity3d games.

In the past I wrote the https://github.com/LutzGrosshennig/unity3d-AnimatedGridMovement-Camera script which I used for my 'Xenomorph 2409' game project.

Some fellow gridders at www.dungeoncrawlers.org pointed out that the movement of that script is way to linear and yes they are right about it.
So I started to write this more advanced script that will give you more control over the movement.

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
 * Oblique viewing frustum. A common problem of new 3d dungeon crawlers is, that you can't see enought of the tile you are currently standing on, this is caused by the default symetrical view frustrum on most 3d engines, switching to an asymetrical view frustrum adresses the issue. This is done by shifting the camera lens down in the physical camera emulation.

# Materials used

 * The materials and textures in the FreePBR folder are taken from www.freepbr.com
 * The simple wall texture is taken from https://github.com/LutzGrosshennig/amiga-xeno-dungeon-crawler (be gentle to it, its 30y old Amiga pixel art).

# Usage

There is a prebuild test level to walk around. Since this level is prebuild there is no 2d map to back it up but it gives you full control of your 3d world. Of course you can also generate the 3d level based on a 2d level map and then use the 2d map to apply your game logic, or you derive a 2d map based on what you build in 3d the choice is yours, both paths have there pro's and con's.

I'll update the documentation step by step but if you have a specific question, just open an issue and I'm happy to help.

Should be pretty self explainatory. If you have questions create an issue and I will answer and update this readme.
In depth documentation is work in progress. 

# Whats next?

The code could definiatly use some refactorings. There are some dependcies and responsiblities I'm not happy with. However "done!" is always better than "I will only release it when its perfect!". Of course I will use the scripts in "Xenomorph 2409" as well.

# Challenge

You are not happy with the default animation curves and you found better ones? Great! Please share them with us!

Have fun with it!
Lutz
