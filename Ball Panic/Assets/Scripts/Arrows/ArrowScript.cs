using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour 
{
	[SerializeField]
	private float arrowSpeed = 7f;

	private bool canShootStickyArrow;

	[SerializeField]
	private AudioClip clip;

	// Use this for initialization
	void Start () 
	{
		canShootStickyArrow = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameObject.tag == "First Sticky Arrow") 
		{
			if (canShootStickyArrow) 
			{
				ShootArrow ();
			}
		} 
		else if (this.gameObject.tag == "Second Sticky Arrow") 
		{
			if (canShootStickyArrow) 
			{
				ShootArrow ();
			}
		} 
		else 
		{
			ShootArrow ();
		}

	}

	void ShootArrow()
	{
		Vector3 temp = transform.position;
		temp.y += arrowSpeed * Time.unscaledDeltaTime;
		transform.position = temp;
	}

	IEnumerator ResetStickyArrow()
	{
		yield return new WaitForSeconds (2.5f);

		if (this.gameObject.tag == "First Sticky Arrow") 
		{
			PlayerScript.instance.PlayerShootOnce (true);
			this.gameObject.SetActive (false);
		}
		else if (this.gameObject.tag == "Second Sticky Arrow") 
		{
			PlayerScript.instance.PlayerShootTwice (true);
			this.gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D target)
	{
		if(target.tag == "LargestBall" || target.tag == "LargeBall" || target.tag == "MediumBall" ||
			target.tag == "SmallBall" || target.tag == "SmallestBall")
		{
			if (gameObject.tag == "First Arrow" || gameObject.tag == "First Sticky Arrow") 
			{
				PlayerScript.instance.PlayerShootOnce (true);
			}

			else if (gameObject.tag == "Second Arrow" || gameObject.tag == "Second Sticky Arrow")
			{
				PlayerScript.instance.PlayerShootTwice (true);
			}
			gameObject.SetActive (false);
		}
		
		if (target.tag == "TopBrick" || target.tag == "UnbreakableBrickTop" || target.tag == "UnbreakableBrickBottom" ||
			target.tag == "UnbreakableBrickLeft" || target.tag == "UnbreakableBrickRight" 
			|| target.tag == "UnbreakableBrickBottomVertical") 
		{
			if (this.gameObject.tag == "First Arrow") 
			{
				PlayerScript.instance.PlayerShootOnce (true);
				this.gameObject.SetActive (false);
			} 

			else if (this.gameObject.tag == "Second Arrow") 
			{
				PlayerScript.instance.PlayerShootTwice (true);
				this.gameObject.SetActive (false);
			}

			if (this.gameObject.tag == "First Sticky Arrow") 
			{
				canShootStickyArrow = false;

				Vector3 targetPos = target.transform.position;
				Vector3 temp = transform.position;

				if (target.tag == "TopBrick") 
				{
					targetPos.y -= 0.989f;
				} 

				else if (target.tag == "UnbreakableBrickTop" || target.tag == "UnbreakableBrickBottom" ||
				           target.tag == "UnbreakableBrickLeft" || target.tag == "UnbreakableBrickRight") 
				{
					targetPos.y -= 0.75f;
				} 

				else if (target.tag == "UnbreakableBrickBottomVertical") 
				{
					targetPos.y -= 0.97f;
				}

				temp.y = targetPos.y;
				transform.position = temp;

				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine ("ResetStickyArrow");
			} 

			else if (this.gameObject.tag == "Second Sticky Arrow") 
			{
				canShootStickyArrow = false;

				Vector3 targetPos = target.transform.position;
				Vector3 temp = transform.position;

				if (target.tag == "TopBrick") 
				{
					targetPos.y -= 0.989f;
				} 

				else if (target.tag == "UnbreakableBrickTop" || target.tag == "UnbreakableBrickBottom" ||
					target.tag == "UnbreakableBrickLeft" || target.tag == "UnbreakableBrickRight") 
				{
					targetPos.y -= 0.75f;
				} 

				else if (target.tag == "UnbreakableBrickBottomVertical") 
				{
					targetPos.y -= 0.97f;
				}

				temp.y = targetPos.y;
				transform.position = temp;

				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine ("ResetStickyArrow");
			} 
		}

		if (target.tag == "BrokenBrickTop" || target.tag == "BrokenBrickBottom" ||
		    target.tag == "BrokenBrickLeft" || target.tag == "BrokenBrickRight") 
		{
			BrickScript brick = target.gameObject.GetComponentInParent<BrickScript> ();
			brick.StartCoroutine (brick.BreakTheBrick());
			
			if (gameObject.tag == "First Arrow" || gameObject.tag == "First Sticky Arrow") 
			{
				PlayerScript.instance.PlayerShootOnce (true);
			} 

			else if (gameObject.tag == "Second Arrow" || gameObject.tag == "Second Sticky Arrow") 
			{
				PlayerScript.instance.PlayerShootTwice (true);
			}

			gameObject.SetActive (false);
		}
	}
}
