2D Shooter Bullet and Weapon System for Unity3D README
======================================================

Asset contents
==============
1. Bullet pattern and weapon preset/selection script
2. Bullet/Object Pooling manager script
3. Bullet script + base bullet prefabs
4. Player movement/control script
5. Background, bullet, and player sprites (space ship sprite and a zombie character sprite with walk animation)
6. 2 x demo scenes with fully customisable controls/examples
7. This readme file

Web demo: http://hobbyistcoder.com/demos/2DShooterBulletAndWeaponSystem/2DShooterBulletAndWeaponSystem.html

Introduction
============

Website: http://hobbyistcoder.com

This asset provides you with a highly customisable bullet and weapon system, along with a bullet pooling manager which is simple to drop into your Unity game or project and start using right away. The system allows for many different bullet pattern configurations and customisations, as well as ricochet (bounce) and bullet impact effects. In addition, you also get a customisable player movement script which allows for two modes of player movement controls (top down shooter and shoot-em-up/shmup).

The system allows you to easily create preset bullet patterns or weapon types for your 2D games, whether they be bullet hell shmups, or top-down zombie defense games, the bullet and weapon system is fully customisable. the WeaponSystem script contains sliders for every property on the weapon allowing you to easily control properties like bullet count, fire rate, bullet randomness, spread, offsets, and even alternate properties like spread size on an alternating timer system. This amounts to a huge number of possible bullet patterns and weapon types.

The included demo scene gives you 13 preset weapon types to try, 2 different bullet types, 4 different bullet colour options as well as the sliders needed to create your own customised bullet/weapon types too.

Usage
=====

Setup requirements:
-------------------

1. Ensure you create two new layers in your project under Edit -> Project Settings -> Tags and Layers. Name one layer "BulletLayer" and the other layer "PlayerLayer".
2. Bullets are set by default to be created on "BulletLayer". You should also set your player/character to the "PlayerLayer".
3. Go to Edit -> Project Settings -> Physics 2D, and disable gravity (set to 0). You'll also need to disable collisions between the BulletLayer (itself) and the BulletLayer and PlayerLayer. Here is what your settings should resemble: http://hobbyistcoder.com/UnityAssets/Media/layer-collision-matrix-setup.jpg (this is so that bullets do not collide with eachother, and do not collide with the player either).
4. Ensure any objects you wish bullets to bounce off of, or have hit effects on, have a 2D collider that is not set to a trigger.

These steps are important to ensure bullet collisions and hit effects work correctly, as well as to ensure that the collision detection code does not fail.

General usage:
--------------

To use the bullet/weapon system asset, all you need to do is import the asset into your project, and then drag and drop the WeaponSystem.cs script onto your player GameObject. Ensure you also have a GameObject added to your scene that holds the ObjectPoolManager.cs script. Drag and drop a bullet prefab from the prefabs folder to your ObjectPoolManager bullet prefab properties, set the number of bullets to initially pool at the start of the scene. The player gameobject that holds your WeaponSystem.cs script should also have a 'child' gameobject with just a standard Transform component added. Drag and drop this child gameobject to the slot on your WeaponSystem.cs script component called "GunPoint" - this is a reference point that is used for the bullets to be fired from, so you can position this child gameobject where your player's weapon sits normally. That's it! This script includes the 13 preset weapon types so you can get started right away with included weapons such as the gatling gun, shotgun, dual arcing machine gun, etc...

To create your own weapon types, play with the sliders on the WeaponSystem.cs script when your game is running to see how the pattern looks, note down the current properties, then make your own weapon entries in the BulletPresetType Enum in the WeaponSystem.cs script. Make sure you handle the new presets you create in the "BulletPresetChangedHandler" method which fires whenever the BulletPreset public property is changed.

This makes it easy for you to change your player's weapons during the game. All you need to do is change the BulletPreset property on your player WeaponSystem component from anywhere in your game! As soon as you do this, the BulletPresetChanged event will fire, and when this happens the BulletPresetchangedHanlder method is run right away, setting the correct settings on your weapon, so that when it fires, the new type of bullet pattern is emitted.

An example of referencing your WeaponScript bullet preset property from anywhere in C# code would be:

var weaponSystemRef = MyPlayerGameObject.GetComponent<WeaponSystem>();
weaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.Tarantula; // Selects the "Tarantula" weapon type and the next time the player shoots, he/she will use this weapon and bullet pattern.

Setting game type - horizontal, vertical shmup or free aim top down shooter:
----------------------------------------------------------------------------

There are more properties to change the system to run in different player control / styles (horizontal shmup, vertical shmup, free aim top down shooter), or to change bullet types and colours. There is also a PlayerMovement.cs script included. When you wish to change the game style - e.g. horizontal shmup or free aim top down shooter, you could change the shooterDirectionType enum property on the WeaponScript component as well as the playerMovementType enum property on the PlayerMovement component accordingly. You can mix modes too. For example, you could have horizontal shmup controls on your player, but with a free aim WeaponSystem component. WASD and the arrow keys would move your player up down / left and right, and you would use the mouse to aim your weapons. Or you could have a FreeAim movement selection on your PlayerMovement, with free aim shooting on your WeaponSystem component. Here is an example of setting the player to use a FreeAim weapon and player movement configuration:

var weaponSystemRef = MyPlayerGameObject.GetComponent<WeaponSystem>();
var playerMovementRef = MyPlayerGameObject.GetComponent<PlayerMovement>();

weaponSystemRef.shooterDirectionType = WeaponSystem.ShooterType.FreeAim;
playerMovementRef.playerMovementType = PlayerMovement.PlayerMovementType.FreeAim

Bullet hit effects and ricochet support
---------------------------------------

You are also able to enable bullet hit effects (a spark particle system hooked up to the built in object pool manager). To use bullet impact effects, simply check the box on the WeaponSystem script to enable bullet hit effects.

To allow bullets to ricochet, you need to check the box on the WeaponSystem script to enable ricochets. There is a slider that will allow you to determine the percent chance that a bullet has to ricochet on impact with a 2D collider. Each subsequent bounce/ricochet will then have this same percent chance to ricochet again. If a bullet hits a collider without a ricochet occuring, then the bullet will be destroyed (returned back to the bullet object pool by being disabled).

If you wish for bullets to collide with objects, ensure your object has a 2D collider attached, and that it is NOT set to be a "trigger". If you look at the Demo Scene included with this asset you will also see an example of collision layers being used to ensure bullets are kept on a separate bullet layer. This layer is then marked in the collision layer matrix to not allow collisions with itself - therefore bullets do not affect each other in terms of collisions. If you wish to exclude other layers from being affected by bullet collisions, ensure you mark them as such in the layer collision matrix (Physics2D settings in Unity). It is also a good idea to have your main character/player on a layer that is excluded from collisions with the bullet layer - this way if your player uses a collider too, then it will not be hit by it's own bullets.

You can look at the DemoSceneManager.cs script for more examples of setting different weapon types, bullet patterns, and movement types.

Bullet and object pool usage:
-----------------------------

The included ObjectPoolManager.cs script is a single instance manager script which has two entries defined already for the two bullet types included with this asset, as well as the spark hit effect. You can use this manager to pool other items in your game too if you wish - enemies, items, etc... All you need to do is make an entry for the gameobject prefab, set an integer to define how many of each you would like pooled at the start of the game, and create a method used for pulling out an item from the relevant pool. For examples on how to do this, simply refer to the existing three methods in the script. 