using UnityEngine;
using System.Collections;

public class EnemyHpDisplay : MonoBehaviour {
	public GameObject myParent;
	public Transform datCamera;
	public Transform mytransform;
	// Use this for initialization
	void Start () {
		mytransform=transform;
		datCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		GetComponent<TextMesh>().text=myParent.name+ "\n"+ myParent.GetComponent<badGuyAi>().Hp.ToString();
//		if (myParent.GetComponent<Renderer>().enabled==true)
//		{
//		GetComponent<Renderer>().enabled=true;	
//		}
	}
	
	
	void LateUpdate()
	{
		
		mytransform.rotation = datCamera.transform.rotation;	
	}
}
