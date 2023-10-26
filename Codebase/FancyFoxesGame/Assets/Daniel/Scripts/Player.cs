#define CHARACTERCONTROLLER
//#define LERPING
//#define TERRAINHUGGING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if CHARACTERCONTROLLER
[RequireComponent(typeof(CharacterController), typeof(Animator))]
#else
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
#endif
public class Player : MonoBehaviour {

	// Values for correction offset of position extract from cursor for animation accuracy
	[Tooltip("Take all factors into account in relation to the camera! (position, rotation, scale)")]
	public Vector3 mousePointCorrection = new Vector3(0, 0, 0.5f);

	// Player stats (currently "power" isn't in use
	public float healthValue = 10f;
	public float healthMax = 10f;
	public float staminaMax = 5f;
	public float stamina = 5f;
	public float staminaExhuastRate = 2f;
	public float staminaRestoreRate = 2f;
	public float staminaRestoreDelay = 4f;
	[HideInInspector] public float speedMultiplier = 1f;
	public float power = 1f;

	[HideInInspector]
	public bool hasItem = false;

	// Whether or not the player follows certian movement logic or not (polar)
	[SerializeField]
	[InspectorName("Control Style (Cartesian/Polar)")]
	[Tooltip("enable for Cartesian, disable for Polar")]
	public bool carterian;

	// Player movement speed values in world space
	[Min(1)]
	public float movementSpeed = 5;
	[Tooltip("Rotation speed is only relevant when camera rotation follow is enabled")]
	[Min(1)]
	public float rotationSpeed = 5;
	private float currentSpeed = 0;
	[Min(1)]
	public float accSpeed = 2;
	[Min(1)]
	public float animAcc = 2;
	[Min(1)]
	public float mass = 1;
	[Min(0)]
	public float stopForce = 1;

	public float gravityValue = 1f;
	private float gravityAcc = 9.8f;

	// Movement axis input values
	private float horizontal, vertical;
	private Vector3 moveLerp = Vector3.zero;

	// Variables for use with cursor follow logic
	private Vector3 mouseTurn;
	private Vector3 mousePoint;
	public Vector3 MousePoint { get => mousePoint; }

	[HideInInspector] public Animator anim;
	private CharacterController controller;

	private bool damageActive = false;

	public void SavePlayerData() {
		GlobalSettings.Instance.playerHasItem = hasItem;
		GlobalSettings.Instance.playerHealth = healthValue;
		GlobalSettings.Instance.playerAbility = GetComponent<UseElementalAbility>().ability;
		GlobalSettings.Instance.shotsRemaining = GetComponent<UseElementalAbility>().shotsRemaining;
		GlobalSettings.Instance.creatureAbility = GetComponent<UseCreatureAbility>().creatureAbility;
		GlobalSettings.Instance.loadPlayer = true;
	}

	public void LoadPlayerData() {
		healthValue = GlobalSettings.Instance.playerHealth;
		hasItem = GlobalSettings.Instance.playerHasItem;
	}

	void Awake() {
		Init();
		if (GlobalSettings.Instance.loadPlayer)
			LoadPlayerData();
		else
			SavePlayerData();
	}

	void Start() {
		anim.SetFloat("AttackSpeed", 5);
	}

	public void Init() {
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		carterian = GlobalSettings.Instance.cartesian;
		anim.SetBool("Death", false);
		healthValue = healthMax;
		hasItem = false;
	}

	public void PerformMove() {
		moveLerp *= speedMultiplier;
		if (carterian && !FollowPlayer.Instance.FollowRotation) {
			// Cartesian Control style

#if CHARACTERCONTROLLER
			// CharacterController
			controller.Move(moveLerp);
#else
			// Rigidbody style
			transform.position += new Vector3(moveLerp.x, 0, moveLerp.z);
#endif
		}
		else {
			// Polar Control style

#if CHARACTERCONTROLLER
			// CharacterController
			controller.Move(controller.transform.forward * moveLerp.z);
			controller.Move(controller.transform.right * moveLerp.x);
			controller.Move(controller.transform.up * moveLerp.y);
#else
			// Rigidbody style
			transform.Translate(new Vector3(moveLerp.x, 0, moveLerp.z));
#endif
		}
	}

#if LERPING
	void FixedUpdate() {
		// Main condition, validating that the scene is unpause and the player is alive and not hit stunned
		if (!GameManager.Instance.isPaused && 0 < healthValue) {

			// Adjusment of player position in world space in two control styles (Cartesian/Polar)
			// Cartesian is only relevant when FollowRotation by camera is disabled
			OnNavigate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
			Vector3 axis = new Vector3(horizontal, 0, vertical);
			float accModded = Mathf.Max(0, accSpeed + ((1 - axis.normalized.magnitude) * stopForce / mass));

			currentSpeed = Mathf.Clamp(currentSpeed + accModded * (2 * axis.normalized.magnitude - 1) * Time.fixedDeltaTime, 0, movementSpeed);
			moveLerp = Vector3.Lerp(moveLerp, axis.normalized * (currentSpeed * Time.fixedDeltaTime + 0.5f * accModded * Mathf.Pow(Time.fixedDeltaTime, 2)), Time.fixedDeltaTime + Time.fixedDeltaTime * ((1 - axis.normalized.magnitude) * stopForce / mass));
			moveLerp.y = controller.isGrounded && moveLerp.y < 0f ? 0f : (moveLerp.y - gravityValue);
			moveLerp.y *= Time.fixedDeltaTime * gravityAcc * mass;

			PerformMove();

			float orientation = Vector3.Angle(moveLerp.normalized, transform.forward) > 90f ? -1 : 1;
			anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), axis.normalized.magnitude, Time.fixedDeltaTime * animAcc) * orientation);
			anim.SetFloat("SpeedMagnitude", Mathf.Lerp(anim.GetFloat("Speed"), axis.normalized.magnitude, Time.fixedDeltaTime * animAcc));
		}
	}
#endif

	// Update is called once per frame
	void Update() {
		// Main condition, validating that the scene is unpause and the player is alive and not hit stunned
		if (!GameManager.Instance.isPaused && 0 < healthValue) {

#if !LERPING
			// Adjusment of player position in world space in two control styles (Cartesian/Polar)
			// Cartesian is only relevant when FollowRotation by camera is disabled
			OnNavigate(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
			Vector3 axis = new Vector3(horizontal, 0, vertical);
			float accModded = Mathf.Max(0, accSpeed + ((1 - axis.normalized.magnitude) * stopForce / mass));

			moveLerp += axis.normalized;
			moveLerp *= Time.deltaTime * (currentSpeed = movementSpeed * 2 * axis.normalized.magnitude);
			moveLerp.y = controller.isGrounded && moveLerp.y < 0f ? 0f : (moveLerp.y - gravityValue);
			moveLerp.y *= Time.deltaTime * gravityAcc * mass;

			PerformMove();

			float orientation = Vector3.Angle(moveLerp.normalized, transform.forward) > 90f ? -1 : 1;
			anim.SetFloat("Speed", axis.normalized.magnitude * orientation);
			anim.SetFloat("SpeedMagnitude", axis.normalized.magnitude);
#endif

			// Cursor follow logic, making the gameobject rotate to look at the cursor point in world space
			// if camera rotation follow is enable, the cursor is locked (by another code) and thse degree
			// of rotation is determined by the delta of the mouse X-axis.
			OnMouseMove(Input.mousePosition);
			Vector3 nearestPoint = new Vector3(mousePoint.x, transform.position.y, mousePoint.z - transform.localScale.y / 2);

#if TERRAINHUGGING
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.y + 1f, LayerMask.GetMask("Floor"))) {
				Plane p = new Plane(hit.normal, transform.position);
				if (!FollowPlayer.Instance.FollowRotation)
					nearestPoint = p.ClosestPointOnPlane(mousePoint);
				else
					nearestPoint = p.ClosestPointOnPlane(transform.position + transform.forward.normalized + (Vector3.up * transform.localScale.y / 2));
				nearestPoint -= new Vector3(0, 0, transform.localScale.y / 2);
			}
#endif

			transform.LookAt(nearestPoint);

			if (FollowPlayer.Instance.FollowRotation) {
				transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, mouseTurn.x, transform.localRotation.eulerAngles.z);
			}

			if (Input.GetButtonDown("Fire1") && !anim.GetBool("Cooldown"))
				OnMouseFire();
		}
	}

	/// <summary>
	/// Target method for apply damage logic to the player from external sources.
	/// Only functions if player has valid health value to work with.
	/// Currently the method triggers Damage and Death animation accordingly.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="source"></param>
	public void TakeDamage(float value, GameObject source) {
		if (0 < healthValue) {
			anim.SetTrigger("Damage");
			healthValue -= value;
			if (0 >= healthValue) {
				healthValue = 0;
				anim.SetBool("Death", true);
				GlobalSettings.Instance.playerHealth = healthMax;
				GlobalSettings.Instance.playerHasItem = false;
				GlobalSettings.Instance.playerAbility = GetComponent<UseElementalAbility>().baseAbility;
				GlobalSettings.Instance.creatureAbility = null;
				StartCoroutine(GameManager.Instance.OnGameOver(3f));
			}
		}
	}

	/// <summary>
	/// Target method for restoring health to the player from external sources.
	/// Only functions if player has valid health value to work with, not full.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="source"></param>
	public void RestoreHealth(float value) {
		// if we have health to restore
		if (0 < healthValue && healthValue < healthMax) {
			// add the value to the player health
			healthValue += value;
			// but only go to the max
			if (healthValue > healthMax)
				healthValue = healthMax;
		}
	}

	/// <summary>
	/// Additional required logic upon input event for projectile fire
	/// </summary>
	void OnMouseFire() {
		if (0 == Random.Range(0, 2))
			anim.SetTrigger("Fire1");
		else
			anim.SetTrigger("Fire2");
	}

	/// <summary>
	/// Logic for extracting mouse/cursor information into usable form
	/// </summary>
	/// <param name="value"></param>
	void OnMouseMove(Vector3 value) {
		// axis value for later use when camera is set for rotation follow
		mouseTurn.x += Input.GetAxis("Mouse X") * rotationSpeed;

		if (Camera.main.orthographic) {
			// Orthographic
			mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePointCorrection;

			// adjustments to allow a more accurate rendering of projectile direction towards cursor
			mousePoint -= mousePointCorrection;
		}
		else {
			// Perspective
			// a process for creating a ray originating from cursor SCREEN point into the world space
			// and utilizing the ray impact point with the floor
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;
			if (Physics.Raycast(mouseRay, out rayHit, Mathf.Infinity, LayerMask.GetMask("Floor")))
				mousePoint = rayHit.point - mousePointCorrection;
		}
	}

	/// <summary>
	/// Alternative logic implementation for following the Cursor in game, is still
	/// </summary>
	/// <param name="value"></param>
	//void OnMouseMoveWIP() {
	//	Vector3 center = GameManager.Instance.transform.position - GameManager.Instance.transform.localScale / 2 * CursorPosition.unitCubeMagnitude;
	//	transform.LookAt(center);
	//	transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	//}

	/// <summary>
	/// Extraction of x, y values from horizontal & vertical input axis (vector2 value)
	/// </summary>
	/// <param name="value"></param>
	void OnNavigate(Vector2 value) {
		horizontal = value.x;
		vertical = value.y;
	}

	public void ToggleDamageActive() => damageActive = !damageActive;
}
