using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public static PlayerScript instance;
	
	[SerializeField]
	private float speed = 8f;

	[SerializeField]
	private float maxVelocity = 4f;

	private Rigidbody2D myRigidbody;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private GameObject[] arrows;

	private float height;

	private bool canWalk;

	[SerializeField]
	private AnimationClip clip;

	[SerializeField]
	private AudioClip shootSound;

	private bool shootOnce, shootTwice;

	private bool moveLeft, moveRight;

	private Button shootBtn;

	[SerializeField]
	private GameObject shield;

	private string arrow;

	public bool hasShield, isInvincible, singleArrow, doubleArrow, singleStickyArrow, 
									doubleStickyArrow, shootFirstArrow, shootSecondArrow;

	public delegate void Explode(bool touchedGoldBall);
	public static event Explode explode;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
		
		myRigidbody = GetComponent<Rigidbody2D> ();

		float cameraHeight = Camera.main.orthographicSize;
		height = -cameraHeight - 0.8f;

		shootBtn = GameObject.FindGameObjectWithTag ("ShootButton").GetComponent<Button> ();
		shootBtn.onClick.AddListener (() => ShootTheArrow ());
	}
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void FixedUpdate () 
	{
		PlayerWalkKeyboard ();
		MoveThePlayer ();
	}

	void InitializePlayer()
	{
		canWalk = true;

		switch (GameController.instance.selectedWeapon) 
		{
		case 0:
			
			arrow = "Arrow";
			shootOnce = true;
			shootTwice = false;

			singleArrow = true;
			doubleArrow = false;
			singleStickyArrow = false;
			doubleStickyArrow = false;
 
			break;

		case 1:

			arrow = "Arrow";
			shootOnce = true;
			shootTwice = true;

			singleArrow = false;
			doubleArrow = true;
			singleStickyArrow = false;
			doubleStickyArrow = false;

			break;

		case 2:

			arrow = "StickyArrow";
			shootOnce = true;
			shootTwice = false;

			singleArrow = false;
			doubleArrow = false;
			singleStickyArrow = true;
			doubleStickyArrow = false;

			break;

		case 3:

			arrow = "StickyArrow";
			shootOnce = true;
			shootTwice = true;

			singleArrow = false;
			doubleArrow = false;
			singleStickyArrow = false;
			doubleStickyArrow = true;

			break;
		}

		Vector3 bottomBrick = GameObject.FindGameObjectWithTag ("BottomBrick").transform.position;
		Vector3 temp = transform.position;

		switch (gameObject.name) 
		{
		case "Homosapien(Clone)":
			temp.y = bottomBrick.y + 1.19f;
			break;

		case "Joker(Clone)":
			temp.y = bottomBrick.y + 1.153f;
			break;

		case "Spartan(Clone)":
			temp.y = bottomBrick.y + 1.08f;
			break;

		case "Pirate(Clone)":
			temp.y = bottomBrick.y + 1.27f;
			break;

		case "Player(Clone)":
			temp.y = bottomBrick.y - 3.775f;
			break;

		case "Zombie(Clone)":
			temp.y = bottomBrick.y + 1.11f;
			break;
		}

		transform.position = temp;
	}

	public void PlayerShootOnce(bool shootOnce)
	{
		this.shootOnce = shootOnce;
		shootFirstArrow = false;
	}

	public void PlayerShootTwice(bool shootTwice)
	{
		if (doubleArrow || doubleStickyArrow) 
		{
			this.shootTwice = shootTwice;
		}
		shootSecondArrow = false;
	}

	public void ShootTheArrow()
	{
		if (GameplayController.instance.levelInProgress) 
		{
			if (shootOnce) 
			{
				if (arrow == "Arrow") 
				{
					Instantiate (arrows [0], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				} else if (arrow == "StickyArrow")
				{
					Instantiate (arrows [2], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				}

				StartCoroutine (PlayTheShootAnimation ());
				shootOnce = false;
				shootFirstArrow = true;
			} 

			else if (shootTwice) 
			{
				if (arrow == "Arrow") 
				{
					Instantiate (arrows [1], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				} else if (arrow == "StickyArrow")
				{
					Instantiate (arrows [3], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				}

				StartCoroutine (PlayTheShootAnimation());
				shootTwice = false;
				shootSecondArrow = true;
			}
		}
	}

	IEnumerator PlayTheShootAnimation()
	{
		canWalk = false;
		shootBtn.interactable = false;
		animator.Play ("Shoot");
		AudioSource.PlayClipAtPoint (shootSound, transform.position);
		yield return new WaitForSeconds (clip.length);
		animator.SetBool ("Shoot", false);
		shootBtn.interactable = true;
		canWalk = true;
	}

	public void DestroyShield()
	{
		StartCoroutine (SetPlayerInvincible ());
		hasShield = false;
		shield.SetActive (false);
	}

	IEnumerator SetPlayerInvincible()
	{
		isInvincible = true;
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (3f));
		isInvincible = false;
	}

	public void StopMoving()
	{
		moveLeft = moveRight = false;
		animator.SetBool ("Walk", false);
	}

	public void MoveThePlayerLeft()
	{
		moveLeft = true;
		moveRight = false;
	}

	public void MoveThePlayerRight()
	{
		moveRight = true;
		moveLeft = false;
	}

	void MoveThePlayer()
	{
		if (GameplayController.instance.levelInProgress) 
		{
			if (moveLeft) 
			{
				MoveLeft ();
			}

			if (moveRight) 
			{
				MoveRight ();
			}
		}
	}

	void MoveRight()
	{
		float force = 0f;
		float velocity = Mathf.Abs (myRigidbody.velocity.x);

		float h = Input.GetAxis ("Horizontal");

		if (canWalk) 
		{
			if (velocity < maxVelocity) 
				{
					force = speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = 1f;
				transform.localScale = scale;

				animator.SetBool ("Walk", true);
		}

		myRigidbody.AddForce (new Vector2 (force, 0));
	}

	void MoveLeft()
	{
		float force = 0f;
		float velocity = Mathf.Abs (myRigidbody.velocity.x);

		float h = Input.GetAxis ("Horizontal");

		if (canWalk) 
		{
			if (velocity < maxVelocity) 
			{
				force = -speed;
			}

			Vector3 scale = transform.localScale;
			scale.x = -1f;
			transform.localScale = scale;

			animator.SetBool ("Walk", true);
		}

		myRigidbody.AddForce (new Vector2 (force, 0));
	}

	void PlayerWalkKeyboard()
	{
		float force = 0f;
		float velocity = Mathf.Abs (myRigidbody.velocity.x);

		float h = Input.GetAxis ("Horizontal");

		if (canWalk) 
		{
			if (h > 0) 
			{
				if (velocity < maxVelocity) 
				{
					force = speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = 1f;
				transform.localScale = scale;

				animator.SetBool ("Walk", true);
			} 
			else if (h < 0) 
			{
				if (velocity < maxVelocity) 
				{
					force = -speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = -1f;
				transform.localScale = scale;

				animator.SetBool ("Walk", true);
			} 
			else if(h == 0)
			{
				animator.SetBool ("Walk", false);
			}
		}

		myRigidbody.AddForce (new Vector2 (force, 0));
	}

	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "SingleArrow") 
		{
			if (!singleArrow) 
			{
				arrow = "Arrow";

				if (!shootFirstArrow) 
				{
					shootOnce = true;
				}

				shootTwice = false;

				singleArrow = true;
				doubleArrow = false;
				singleStickyArrow = false;
				doubleStickyArrow = false;
			}
		}

		if (target.tag == "DoubleArrow") 
		{
			if (!doubleArrow) 
			{
				arrow = "Arrow";

				if (!shootFirstArrow) 
				{
					shootOnce = true;
				}

				if (!shootSecondArrow) 
				{
					shootTwice = true;
				}
					
				singleArrow = false;
				doubleArrow = true;
				singleStickyArrow = false;
				doubleStickyArrow = false;
			}
		}

		if (target.tag == "SingleStickyArrow") 
		{
			if (!singleStickyArrow) 
			{
				arrow = "StickyArrow";

				if (!shootFirstArrow) 
				{
					shootOnce = true;
				}

				shootTwice = false;

				singleArrow = false;
				doubleArrow = false;
				singleStickyArrow = true;
				doubleStickyArrow = false;
			}
		}

		if (target.tag == "DoubleStickyArrow") 
		{
			if (!doubleStickyArrow) 
			{
				arrow = "StickyArrow";

				if (!shootFirstArrow) 
				{
					shootOnce = true;
				}

				if (!shootSecondArrow) 
				{
					shootTwice = true;
				}

				singleArrow = false;
				doubleArrow = false;
				singleStickyArrow = false;
				doubleStickyArrow = true;
			}
		}

		if (target.tag == "Watch") 
		{
			GameplayController.instance.levelTime += Random.Range (10f, 20f);
		}

		if (target.tag == "Shield") 
		{
			hasShield = true;
			shield.SetActive (true);
		}

		if (target.tag == "Dynamite") 
		{
			if (explode != null) 
			{
				explode (false);
			}
		}
	}
}























