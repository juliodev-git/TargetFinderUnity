# Z-Target in Unity 6
Two Unity scripts that, in conjuction, pool valid targets that are processed to allow target-camera focusing similar to The Legend of Zelda: Ocarina of Time's Z-Targeting System.

What you will find:

Target.cs - the script that attaches to any game object with a collider. Valid targets self-insert themselves into the static pool transforms in TargetFinder.cs that are referred to as 'targets.' The only dependency is that TargetFinder exists - please make sure a singular instance of TargetFinder.cs exists in the scene before running or add a Null check in Target.cs that verifies whether or not TargetFinder.cs has been instantiated.

TargetFinder.cs - the script that holds and processes targets, inquiring with the list which target is closest to center. It also sorts targets from left to right relative to the center of the viewport. When sorted, users can cycle to targets to the left and right of the currently selected target.

How To Use:

Simply add the Target.cs script to any game object with a collider and ensure a singular TargetFinder.cs script is attached to a seperate gameobject (TargetFinder.cs does NOT attach to a target). TargetFinder.cs requires 3 components:
- Player FreeLook CinemachineCamera
- Target Follow CinemachineCamera (with a Seperate LookAt Target)
- Animator for the Arrow Icons

Player Script is for simple rigidbody movement relative to camera direction.
Target Folder contains UI animation clips for the arrow (you will have to create the Animator StateMachine - please consider watching the linked Tutorial for more help.

[How To Z-Target in Unity Like Ocarina of Time](https://www.youtube.com/watch?v=FfDKWj-0Uyc)

Tutorial Addendum 1: Additional changes have been made to Target.cs and TargetFinder.cs to account for targets being destroyed or disabled mid-lock-on. Typically, targets are going to be destroyed during gameplay and during lock-on. The account for this, OnDestroy will now remove the target from the target pool and the TargetFinder will also check every frame if the currentTarget still exists.
