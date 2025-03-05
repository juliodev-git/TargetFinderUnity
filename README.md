# TargetFinderUnity
Unity script that attaches to a game object and adds the object to a static target pool if it is within view of main camera. The target pool

DISCLAIMER (to the person who asked about it in the Youtube Comment Section):

This was left somewhat unfinished - some objects were marked as "visible" even behind objects. There's a raycast in there that was meant to fix this - if the target couldn't be raycasted it was probably behind a wall and thus, not visible.

Other than that, the code is also a bit messy since it was a work in progress (apologies). **I can't guarantee the code works as is.**

WHAT YOU WILL FIND:

There are two scripts, the main target.cs script and a target viewer script. The only one you need really is the target.cs. **Target.cs** attaches to a game object and checks every frame if the transform of the object is within the fulstrum of the camera (essentially checking if the game object is in view). If it is, it adds itself to the target pool. If the object leaves the view of the camera, it removes itself and only itself.

The static properties in target.cs are a list of transforms which are the targets, a custom class called DistanceClass, and a function that returns the target nearest the center of the camera view. The DistanceClass is an IComparer, but all that means is that we can use the class in conjunction with List.Sort() to sort our targets from left to right. The function uses the DistanceClass to sort the objects, then a second foreach loop checks which of the targets is closer to the camera - even if a target is in the center of the screen, the focused target will be the target closest to the camera itself.

Good luck sorting this out, I'm not sure if I'm going to use this technique in the future but if I do go back and refine it, I'll make sure to update the repo.
