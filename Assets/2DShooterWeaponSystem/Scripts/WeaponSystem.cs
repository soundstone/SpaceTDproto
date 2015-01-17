using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WeaponSystem : MonoBehaviour
{
    [Range(0f, 5f)] public float bulletSpread;

    [Range(0f, 5f)] public float bulletSpreadPingPongMax, bulletSpreadPingPongMin;

    [Range(1f, 3f)] public float spreadPingPongSpeed = 1f;

    [Range(0f, 5f)] public float bulletSpacing;

    [Range(0, 20)] public int bulletCount;

    [Range(0f, 5f)] public float bulletRandomness;

    [Range(5f, 35f)] public float bulletSpeed;

    [Range(0f, 3f)] public float weaponFireRate;

    [Range(-3f, 3f)] public float weaponXOffset;

    [Range(-3f, 3f)] public float weaponYOffset;

    [Range(0, 100f)] public float ricochetChancePercent;

    public Color bulletColour;

    public bool autoFire, pingPongSpread, richochetsEnabled, hitEffectEnabled;
	
    public float aimAngle;

    private float coolDown;
    private float bulletSpreadInitial;
    private float bulletSpacingInitial;
    private float bulletSpreadIncrement;
    private float bulletSpacingIncrement;

    public Transform gunPoint;

    /// <summary>
    /// The different bullet options available. This will be used to select the bullet type pulled from the Object Pool Manager.
    /// </summary>
    public enum BulletOption
    {
        Spherical,
        TracerHorizontal,
    }

    public BulletOption bulletOptionType;

    /// <summary>
    /// Allows the user to adjust the direction which the bullets/weapons fire. This makes it easy to create top down horizontal or vertical games.
    /// </summary>
    public enum ShooterType
    {
        Horizonal,
        Vertical,
        FreeAim
    }

    public ShooterType shooterDirectionType;

    /// <summary>
    /// Default weapon presets. Feel free to add new ones here! Just don't forget to handle the new bullet presets in the BulletPresetChangedHandler method that is fired with the BulletPresetChanged event.
    /// </summary>
    public enum BulletPresetType
    {
        CrazySpreadPingPong,
        GatlingGun,
        Simple,
        Shotgun,
        WildFire,
        ThreeShotSpread,
        DualSpread,
        ImbaCannon,
        Shower,
        DualAlternating,
        DualMachineGun,
        Tarantula,
        CircleSpray
    }

    private BulletPresetType _bulletPreset;

    public delegate void BulletPresetChangeHandler();

    public event BulletPresetChangeHandler BulletPresetChanged;

    /// <summary>
    /// This property dictates what weapon is selected for the WeaponSystem. Set it to any BulletPresetType Enum value and the event should take care of setting up the weapon for you.
    /// Just ensure that if you add a new Enum value to BulletPresetType, that you create an entry for it in the switch statement in the BulletPresetChangedHandler.
    /// All you need is a reference to your WeaponSystem script, once you have that, you can change you weapon selection from anywhere in your game using this.
    /// </summary>
    public BulletPresetType BulletPreset
    {
        get
        {
            return _bulletPreset; 
        }
        set
        {
            if (BulletPreset == value) return;

            // Set a few defaults back whenever the weapon is changed
            pingPongSpread = false;
            weaponXOffset = 0.74f;
            weaponYOffset = 0f;
            gunPoint.transform.localPosition = new Vector2(0.6f, 0f);

            // Set the property and fire off the event if subscribed to.
            _bulletPreset = value;
            if (BulletPresetChanged != null) BulletPresetChanged();
        }
    }
    
    // Use this for initialization
	void Start ()
	{
        // Set a default bullet colour, otherwise bullets will be invisible.
	    bulletColour = Color.white;

        // Subscribe to the BulletPresetChanged Event.
	    BulletPresetChanged += BulletPresetChangedHandler;
	}

    /// <summary>
    ///  This method should be subscribed to the BulletPresetChanged Event. This event fires whenever the BulletPreset Enum property value changes. i.e. whenever you change weapons.
    /// Set up any new BulletPresetType Enum weapon types in here. When you set the public Enum property, this method should fire, and the case statement relevant to your selection should run.
    /// </summary>
    private void BulletPresetChangedHandler()
    {
        switch (BulletPreset)
        {
            case BulletPresetType.Simple:
                bulletCount = 1;
                weaponFireRate = 0.15f;
                bulletSpacing = 1f;
                bulletSpread = 0.05f;
                bulletSpeed = 12f;
                bulletRandomness = 0.15f;
                break;

            case BulletPresetType.GatlingGun:
                bulletCount = 3;
                weaponFireRate = 0.05f;
                bulletSpacing = 0.25f;
                bulletSpread = 0.0f;
                bulletSpeed = 20f;
                bulletRandomness = 0.35f;
                break;

            case BulletPresetType.Shotgun:
                bulletCount = 8;
                weaponFireRate = 0.5f;
                bulletSpacing = 0.5f;
                bulletSpread = 0.65f;
                bulletSpeed = 15f;
                bulletRandomness = 0.65f;
                break;

            case BulletPresetType.WildFire:
                bulletCount = 4;
                weaponFireRate = 0.06f;
                bulletSpacing = 0.13f;
                bulletSpread = 0.24f;
                bulletSpeed = 15f;
                bulletRandomness = 1f;
                break;

                case BulletPresetType.Tarantula:
                bulletSpreadPingPongMin = 1.5f;
                bulletSpreadPingPongMax = 4f;
                spreadPingPongSpeed = 2.5f;
                pingPongSpread = true;
                bulletCount = 8;
                weaponFireRate = 0.063f;
                bulletSpacing = 0.53f;
                bulletSpread = 0.08f;
                bulletSpeed = 7.35f;
                bulletRandomness = 0.0f;
                break;

                case BulletPresetType.CrazySpreadPingPong:
                bulletSpreadPingPongMax = 1f;
                spreadPingPongSpeed = 2.5f;
                pingPongSpread = true;
                bulletCount = 7;
                weaponFireRate = 0.0f;
                bulletSpacing = 0.08f;
                bulletSpread = 0.08f;
                bulletSpeed = 19.35f;
                bulletRandomness = 0.08f;
                break;

                case BulletPresetType.DualSpread:
                bulletCount = 2;
                weaponFireRate = 0.15f;
                bulletSpacing = 0.1f;
                bulletSpread = 0.3f;
                bulletSpeed = 13f;
                bulletRandomness = 0.01f;
                break;

                case BulletPresetType.ThreeShotSpread:
                bulletCount = 3;
                weaponFireRate = 0.15f;
                bulletSpacing = 0.1f;
                bulletSpread = 0.3f;
                bulletSpeed = 13f;
                bulletRandomness = 0.01f;
                break;

                case BulletPresetType.ImbaCannon:
                bulletCount = 10;
                weaponFireRate = 0.02f;
                bulletSpacing = 0.05f;
                bulletSpread = 0.08f;
                bulletSpeed = 25f;
                bulletRandomness = 0.23f;
                break;

                case BulletPresetType.Shower:
                bulletCount = 9;
                weaponFireRate = 0.02f;
                bulletSpacing = 0.05f;
                bulletSpread = 0.7f;
                bulletSpeed = 21.8f;
                bulletRandomness = 0.19f;
                break;

                case BulletPresetType.DualAlternating:
                bulletSpreadPingPongMax = 1f;
                spreadPingPongSpeed = 2f;
                pingPongSpread = true;
                bulletCount = 2;
                weaponFireRate = 0.05f;
                bulletSpacing = 0.24f;
                bulletSpread = 0.08f;
                bulletSpeed = 14.5f;
                bulletRandomness = 0.0f;
                break;

                case BulletPresetType.DualMachineGun:
                bulletCount = 2;
                weaponFireRate = 0.07f;
                bulletSpacing = 0.53f;
                bulletSpread = 0.011f;
                bulletSpeed = 16f;
                bulletRandomness = 0.02f;
                break;

                case BulletPresetType.CircleSpray:
                weaponXOffset = 0f;
                weaponYOffset = 0f;
                bulletCount = 20;
                weaponFireRate = 0.19f;
                bulletSpacing = 0f;
                bulletSpread = 5f;
                bulletSpeed = 5f;
                bulletRandomness = 0f;
                break;
                
            default:
                bulletCount = 1;
                weaponFireRate = 0.15f;
                bulletSpacing = 1f;
                bulletSpread = 0.05f;
                bulletSpeed = 12f;
                bulletRandomness = 0.15f;
                break;
        }
    }

    private void ShootWithCoolDown()
    {
        if (coolDown <= 0f)
        {
            ShootGuns();
            coolDown = weaponFireRate;
        }
    }

    private void ShootGuns()
    {
        if (bulletCount > 1)
        {
            bulletSpreadInitial = -bulletSpread / 2;
            bulletSpacingInitial = bulletSpacing / 2;
            bulletSpreadIncrement = bulletSpread / (bulletCount - 1);
            bulletSpacingIncrement = bulletSpacing / (bulletCount - 1);
        }
        else
        {
            bulletSpreadInitial = 0f;
            bulletSpacingInitial = 0f;
            bulletSpreadIncrement = 0f;
            bulletSpacingIncrement = 0f;
        }

        // For each 'gun' attachment the player has we'll setup each bullet accordingly...
        for (var i = 0; i < bulletCount; i++)
        {
            GameObject bullet;

            // Pull a bullet object from our pool based on current bullet Enum selection.
            switch (bulletOptionType)
            {
                case BulletOption.Spherical:
                bullet = ObjectPoolManager.instance.GetUsableSphereBullet();
                break;

                case BulletOption.TracerHorizontal:
                bullet = ObjectPoolManager.instance.GetUsableStandardHorizontalBullet();
                break;

                default:
                bullet = ObjectPoolManager.instance.GetUsableSphereBullet();
                break;
            }

            var bulletComponent = (Bullet)bullet.GetComponent(typeof(Bullet));

            var offsetX = Mathf.Cos(aimAngle - Mathf.PI / 2) * (bulletSpacingInitial - i * bulletSpacingIncrement);
            var offsetY = Mathf.Sin(aimAngle - Mathf.PI / 2) * (bulletSpacingInitial - i * bulletSpacingIncrement);

            bulletComponent.directionAngle = aimAngle + bulletSpreadInitial + i * bulletSpreadIncrement;
            bulletComponent.speed = bulletSpeed;

            // Setup the point at which bullets need to be placed based on all the parameters
            var initialPosition = gunPoint.position + (gunPoint.transform.forward * (bulletSpacingInitial - i * bulletSpacingIncrement));
            var bulletPosition = new Vector3(initialPosition.x + offsetX + Random.Range(0f, 1f) * bulletRandomness - bulletRandomness / 2,
                initialPosition.y + offsetY + Random.Range(0f, 1f) * bulletRandomness - bulletRandomness / 2, 0f);

            bullet.transform.position = bulletPosition;

            bulletComponent.bulletXPosition = bullet.transform.position.x;
            bulletComponent.bulletYPosition = bullet.transform.position.y;

            // Initial chance to ricochet as the bullet comes out. If the bullet bounces again, this will be determined on the next bullet collision.
            bulletComponent.ricochetChancePercent = ricochetChancePercent;
            bulletComponent.canRichochet = bulletComponent.GetRicochetChance();
            bulletComponent.useHitEffects = hitEffectEnabled;

            // Activate the bullet to get it going
            bullet.SetActive(true);

            // Set the bullet colour using the renderer that we cached when the bullet was first created at the start of the scene. This is easy and accurate if we have a white/semi-transparent bullet sprite
            if (bulletComponent.bulletSpriteRenderer != null) bulletComponent.bulletSpriteRenderer.color = bulletColour;
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
	    var facingDirection = Vector2.zero;

        // We have three modes - horizontal, vertical and free. Each is a different case based on a dropdown Enum selector on the WeaponSystem script.
        // Horizontal is like a sideways scrolling horizontal shooter, vertical is the same, but with the player locked facing upward instead of right, and free allows the player to turn in any direction based on the mouse position.
        switch (shooterDirectionType)
        {
            case ShooterType.Horizonal:

            // Rotate the player to face horizontally right
            facingDirection = Vector2.right;
            break;

            case ShooterType.Vertical:

            // Rotate the player to face vertically up
            facingDirection = Vector2.up;
            break;

            case ShooterType.FreeAim:

            // Get the world position of the mouse cursor and set facing direction to that minus the player's current position.
            //var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
			//var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(NewHero.S.transform.position.x, NewHero.S.transform.position.y, 0f));
            //facingDirection = worldMousePosition - transform.position;
			var moveDirection = Vector3.zero;
			var forward = Vector3.zero;
			var right = Vector3.zero;

			//CharacterController controller;
			//controller = GetComponent(CharacterController);

			forward = transform.forward;
			//right = Vector3(forward.z, 0, -forward.x);

			var horizontalInput = Input.GetAxis("Horizontal");
			var verticalInput = Input.GetAxis("Vertical");

			var targetDirection = horizontalInput  * right + verticalInput * forward;
			moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, 200 * Mathf.Deg2Rad * Time.deltaTime, 1000);

			var movement = moveDirection * Time.deltaTime * 2;

			facingDirection = moveDirection - transform.position;
			//controller.Move(movement);



            break;

            default:

            // Default the player to face horizontally right if no selection is made
            facingDirection = Vector2.right;
            break;
        }

        CalculateAimAndFacingAngles(facingDirection);
        HandleShooting();

	    if (pingPongSpread)
	    {
	        bulletSpread = Mathf.PingPong(Time.time * spreadPingPongSpeed, bulletSpreadPingPongMax - bulletSpreadPingPongMin) + bulletSpreadPingPongMin;
	    }

        // Set the weapon offset position - the gunpoint transform needs to be a child of the player gameobject's transform!
        gunPoint.transform.localPosition = new Vector3(weaponXOffset, weaponYOffset, 0f);
	}

    /// <summary>
    /// Change weapon cooldown timer down so it gets closer to being ready to fire again based on cooldown time and handle player shooting controls input
    /// </summary>
    private void HandleShooting()
    {
        coolDown -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            ShootWithCoolDown();
        }
        else
        {
            if (autoFire)
            {
                ShootWithCoolDown();
            }
        }
    }

    /// <summary>
    /// Calculate aim angle and other settings that apply to all ShooterType orientations
    /// </summary>
    /// <param name="facingDirection"></param>
    private void CalculateAimAndFacingAngles(Vector2 facingDirection)
    {
        aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI*2 + aimAngle;
        }

        // Rotate the player to face the direction of the mouse cursor
        transform.eulerAngles = new Vector3(0.0f, 0.0f, aimAngle*Mathf.Rad2Deg);
    }
}