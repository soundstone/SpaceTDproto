using System;
using UnityEngine;
using System.Collections;

public class DemoSceneManager : MonoBehaviour
{
    // Make sure this is hooked up to your player's WeaponSystem script for the demo GUI to work!
    public WeaponSystem playerWeaponSystemRef;
    public PlayerMovement playerMovementRef;

    public bool advancedControlsVisible, presetControlsVisible, bulletTypeControlsVisible, shooterTypeControlsVisible;

    private Color[] bulletColours = { new Color(1f, 0.9f, 0.36f, 1f), new Color(0f, 1f, 0.04f, 1f), new Color(0f, 0.95f, 1f, 1f), new Color(1f, 1f, 1f, 1f) };
    private int bulletColourCurrentIndex;

	// Use this for initialization
	void Start ()
	{
	    bulletColourCurrentIndex = 0;

        // Set up some settings based on the demo scene loaded.
        if (Application.loadedLevelName == "BulletDemoScene")
        {
            if (playerWeaponSystemRef != null)
            {
                playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.CrazySpreadPingPong;
            }
        }
        else if (Application.loadedLevelName == "TopDownGunDemoScene")
        {
            if (playerWeaponSystemRef != null)
            {
                playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.Simple;

                playerWeaponSystemRef.bulletCount = 1;
                playerWeaponSystemRef.weaponFireRate = 0.15f;
                playerWeaponSystemRef.bulletSpacing = 1f;
                playerWeaponSystemRef.bulletSpread = 0.05f;
                playerWeaponSystemRef.bulletSpeed = 12f;
                playerWeaponSystemRef.bulletRandomness = 0.15f;
            }
        }
	}

    /// <summary>
    /// All on screen GUI controls are rendered here. Note that each Unity default GUI element adds an extra draw call!
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 4 - 65, Screen.height - 60, 900, 25), "Left click to shoot, WASD or arrow keys to move. 'F' to toggle flashlight. Checkboxes at top of the screen show/hide settings.");
        GUI.Label(new Rect(Screen.width/4 - 65, Screen.height - 35, 900, 25), "Check the WeaponSystem.cs script for even more customisation settings.");

        if (playerWeaponSystemRef != null)
        {
            advancedControlsVisible = GUI.Toggle(new Rect(Screen.width -220, 5, 200, 20),
                advancedControlsVisible, "Show advanced customisation");

            presetControlsVisible = GUI.Toggle(new Rect(10, 5, 250, 20),
                presetControlsVisible, "Show weapon preset examples");

            bulletTypeControlsVisible = GUI.Toggle(new Rect(Screen.width/2 - 125, 5, 200, 20),
                bulletTypeControlsVisible, "Show bullet selection controls");

            shooterTypeControlsVisible = GUI.Toggle(new Rect(Screen.width - Screen.width / 2.55f, 5, 250, 20),
                shooterTypeControlsVisible, "Show shooter type selection controls");

            if (advancedControlsVisible)
            {
                playerWeaponSystemRef.bulletCount =
                    Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(Screen.width - 220, 50, 150, 20),
                        playerWeaponSystemRef.bulletCount, 1, 20));
                GUI.Label(new Rect(Screen.width -220, 25, 200, 20),
                    "Bullet count: " + playerWeaponSystemRef.bulletCount);

                playerWeaponSystemRef.bulletRandomness = GUI.HorizontalSlider(
                    new Rect(Screen.width -220, 90, 150, 20), playerWeaponSystemRef.bulletRandomness, 0f, 5f);
                GUI.Label(new Rect(Screen.width -220, 65, 200, 20),
                    "Bullet randomness: " + playerWeaponSystemRef.bulletRandomness);

                playerWeaponSystemRef.bulletSpacing = GUI.HorizontalSlider(new Rect(Screen.width - 220, 140, 150, 20),
                    playerWeaponSystemRef.bulletSpacing, 0f, 5f);
                GUI.Label(new Rect(Screen.width -220, 115, 200, 20),
                    "Bullet spacing: " + playerWeaponSystemRef.bulletSpacing);

                playerWeaponSystemRef.bulletSpeed = GUI.HorizontalSlider(new Rect(Screen.width -220, 190, 150, 20),
                    playerWeaponSystemRef.bulletSpeed, 5f, 35f);
                GUI.Label(new Rect(Screen.width -220, 165, 200, 20),
                    "Bullet speed: " + playerWeaponSystemRef.bulletSpeed);

                playerWeaponSystemRef.bulletSpread = GUI.HorizontalSlider(new Rect(Screen.width -220, 240, 150, 20),
                    playerWeaponSystemRef.bulletSpread, 0f, 5f);
                GUI.Label(new Rect(Screen.width -220, 215, 200, 20),
                    "Bullet spread: " + playerWeaponSystemRef.bulletSpread);

                playerWeaponSystemRef.bulletSpreadPingPongMax =
                    GUI.HorizontalSlider(new Rect(Screen.width -220, 290, 150, 20),
                        playerWeaponSystemRef.bulletSpreadPingPongMax, 0f, 5f);
                GUI.Label(new Rect(Screen.width -220, 265, 200, 20),
                    "Spread pingpong max: " + playerWeaponSystemRef.bulletSpreadPingPongMax);

                playerWeaponSystemRef.weaponFireRate = GUI.HorizontalSlider(new Rect(Screen.width -220, 340, 150, 20),
                    playerWeaponSystemRef.weaponFireRate, 0f, 3f);
                GUI.Label(new Rect(Screen.width -220, 315, 200, 20),
                    "Weapon fire rate: " + playerWeaponSystemRef.weaponFireRate);

                playerWeaponSystemRef.autoFire = GUI.Toggle(new Rect(Screen.width - 220, 365, 150, 20),
                    playerWeaponSystemRef.autoFire, "Autofire");

                playerWeaponSystemRef.pingPongSpread = GUI.Toggle(new Rect(Screen.width - 220, 390, 150, 20),
                    playerWeaponSystemRef.pingPongSpread, "PingPong Spread");

                // X and Y offset controls

                playerWeaponSystemRef.weaponXOffset = GUI.HorizontalSlider(new Rect(Screen.width - 220, 440, 150, 20),
                    playerWeaponSystemRef.weaponXOffset, -3f, 3f);

                GUI.Label(new Rect(Screen.width - 220, 415, 200, 20),
                    "Weapon X Offset: " + playerWeaponSystemRef.weaponXOffset);

                playerWeaponSystemRef.weaponYOffset = GUI.HorizontalSlider(new Rect(Screen.width - 220, 490, 150, 20),
                    playerWeaponSystemRef.weaponYOffset, -3f, 3f);

                GUI.Label(new Rect(Screen.width - 220, 465, 200, 20),
                    "Weapon Y Offset: " + playerWeaponSystemRef.weaponYOffset);

                GUI.Label(new Rect(Screen.width - 220, 515, 200, 20), "Set bullet colour:");

                if (GUI.Button(new Rect(Screen.width - 200, 540, 150, 20), "Change bullet colour"))
                {
                    bulletColourCurrentIndex++;

                    if (bulletColourCurrentIndex > bulletColours.Length - 1)
                    {
                        bulletColourCurrentIndex = 0;
                    }

                    if (bulletColours[bulletColourCurrentIndex] != null)
                    {
                        playerWeaponSystemRef.bulletColour = bulletColours[bulletColourCurrentIndex];
                    }
                }

                GUI.Label(new Rect(Screen.width - 220, 570, 200, 20), "Chance % to ricochet on hit");

                playerWeaponSystemRef.ricochetChancePercent = GUI.HorizontalSlider(new Rect(Screen.width - 220, 600, 150, 20),
                    playerWeaponSystemRef.ricochetChancePercent, 0f, 100f);

            }

            if (presetControlsVisible)
            {

                if (GUI.Button(new Rect(10, 50, 160, 30), "Simple"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.Simple;
                }

                if (GUI.Button(new Rect(10, 90, 160, 30), "Crazy Spread PingPong"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.CrazySpreadPingPong;
                }

                if (GUI.Button(new Rect(10, 130, 160, 30), "Gatling Gun"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.GatlingGun;
                }

                if (GUI.Button(new Rect(10, 170, 160, 30), "Shotgun"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.Shotgun;
                }

                if (GUI.Button(new Rect(10, 210, 160, 30), "Wild Fire"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.WildFire;
                }

                if (GUI.Button(new Rect(10, 250, 160, 30), "Three Shot Spread"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.ThreeShotSpread;
                }

                if (GUI.Button(new Rect(10, 290, 160, 30), "Dual Spread"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.DualSpread;
                }

                if (GUI.Button(new Rect(10, 330, 160, 30), "Imba Cannon"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.ImbaCannon;
                }

                if (GUI.Button(new Rect(10, 370, 160, 30), "Shower"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.Shower;
                }

                if (GUI.Button(new Rect(10, 410, 160, 30), "Dual Alternating"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.DualAlternating;
                }

                if (GUI.Button(new Rect(10, 450, 160, 30), "Dual Machinegun"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.DualMachineGun;
                }

                if (GUI.Button(new Rect(10, 490, 160, 30), "Tarantula"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.Tarantula;
                }

                if (GUI.Button(new Rect(10, 530, 160, 30), "Circle Spray"))
                {
                    playerWeaponSystemRef.BulletPreset = WeaponSystem.BulletPresetType.CircleSpray;
                }
            }

            if (bulletTypeControlsVisible)
            {
                if (GUI.Button(new Rect(Screen.width/2 - 190, 30, 160, 30), "Spherical"))
                {
                    playerWeaponSystemRef.bulletOptionType = WeaponSystem.BulletOption.Spherical;
                }

                if (GUI.Button(new Rect(Screen.width/2 - 10, 30, 160, 30), "Horizontal Tracer"))
                {
                    playerWeaponSystemRef.bulletOptionType = WeaponSystem.BulletOption.TracerHorizontal;
                }
            }

            if (shooterTypeControlsVisible)
            {
                if (playerWeaponSystemRef.shooterDirectionType == WeaponSystem.ShooterType.Horizonal ||
                    playerWeaponSystemRef.shooterDirectionType == WeaponSystem.ShooterType.Vertical)
                {
                    if (GUI.Button(new Rect(Screen.width - Screen.width / 3 - 20, 30, 160, 30), "Free Aim"))
                    {
                        playerWeaponSystemRef.shooterDirectionType = WeaponSystem.ShooterType.FreeAim;
                        playerMovementRef.playerMovementType = PlayerMovement.PlayerMovementType.FreeAim;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(Screen.width - Screen.width / 3 - 20, 30, 160, 30), "Horizontal shmup"))
                    {
                        playerWeaponSystemRef.shooterDirectionType = WeaponSystem.ShooterType.Horizonal;
                        playerMovementRef.playerMovementType = PlayerMovement.PlayerMovementType.Normal;
                    }
                    if (GUI.Button(new Rect(Screen.width - Screen.width / 3 - 20, 70, 160, 30), "Vertical shmup"))
                    {
                        playerWeaponSystemRef.shooterDirectionType = WeaponSystem.ShooterType.Vertical;
                        playerMovementRef.playerMovementType = PlayerMovement.PlayerMovementType.Normal;
                    }
                }
                
            }

            if (Application.loadedLevelName == "BulletDemoScene")
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 90, 200, 30), "Load Top Down Demo Scene"))
                {
                    Application.LoadLevel("TopDownGunDemoScene");
                }
            }
            else if (Application.loadedLevelName == "TopDownGunDemoScene")
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 90, 200, 30), "Load Shmup Demo Scene"))
                {
                    Application.LoadLevel("BulletDemoScene");
                }
            }
            else
            {
                Debug.LogError("Could not find scene with specific name to load.");
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
