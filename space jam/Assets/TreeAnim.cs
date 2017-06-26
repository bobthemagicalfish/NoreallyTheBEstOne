using UnityEngine;
using System.Collections;

public class TreeAnim : MonoBehaviour {
	[SerializeField]
	public Animator Myanim;
	// Use this for initialization
	void Awake(){
		Myanim = GetComponent<Animator> ();
	
	}

	void OnTriggerEnter(Collider other)
	{
		Myanim.SetBool ("TopOff", true);

	}
}
