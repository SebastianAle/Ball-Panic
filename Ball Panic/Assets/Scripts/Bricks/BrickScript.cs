using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour 
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private AnimationClip clip;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public IEnumerator BreakTheBrick()
	{
		animator.Play ("BrickBroke");
		yield return new WaitForSeconds (clip.length);
		gameObject.SetActive (false);
	}
}
