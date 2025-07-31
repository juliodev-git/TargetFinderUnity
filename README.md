# Z-Target in Unity 6
Two Unity scripts that, in conjuction, pool valid targets that are processed to allow target-camera focusing similar to The Legend of Zelda: Ocarina of Time's Z-Targeting System.

What you will find:

Target.cs - the script that is attached to any game object with a collider that self-inserts itself into the static list of transforms in TargetFinder.cs called "pool." The only dependency is 
