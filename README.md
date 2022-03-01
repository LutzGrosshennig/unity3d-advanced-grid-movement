# unity3d-advanced-grid-movement

An more advanced approach for grid based movement in Unity3d games.

In the past I wrote the https://github.com/LutzGrosshennig/unity3d-AnimatedGridMovement-Camera script which I used for my 'Xenomorph 2409' game project.

Some fellow gridders at www.dungeoncrawlers.org pointed out that the movement of that script is way to linear and yes they are right about it.
So I started to write this more advanced script that will give you more control over the movement.

# Whats new?
 * Fully editable animation curve for the movement
 * Fully editable animation curve for the head bob
 * Command queueing. Plan your steps in advance!
 * Run/dash mode. Hold and press 'W' to advance even faster (with separate headbob/movement curves)
 * Height system. You can now climp stairs and slopes. This enables a new dimension (literally) for level design!
 * A basic event based footstep system and an generated example "footstep" sample you need to replace really soon ;-)
 * Full Unity3d project including sample level and some crappy collision detection code (just as simple showcase)

# Materials used

 * The materials and textures in the FreePBR folder are taken from www.freepbr.com
 * The simple wall texture is taken from https://github.com/LutzGrosshennig/amiga-xeno-dungeon-crawler (be gentle to it, its 30y old Amiga pixel art).

# Usage

Should be pretty self explainatory. If you have questions create an issue and I will answer and update this readme.

In depth documentation is work in progress. 

# Whats next?

The code could use an refactoring. There are some dependcies and responsiblities I am not happy with. However "done!" is always better than "It will only release it when its perfect!"
Of course I will use the script in "Xenomorph 2409" as well.

# Challenge

You are not happy with the default animation curves and you found better ones? Great! Please share them with us!

Have fun with it!
Lutz
