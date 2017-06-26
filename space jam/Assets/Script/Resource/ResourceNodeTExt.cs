using UnityEngine;
using System.Collections;

public class ResourceNodeTExt : MonoBehaviour {
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
		if (myParent.GetComponent<ResNode> ().surveyed == true) {
			GetComponent<TextMesh> ().text = myParent.name + "\n";
		}
			//		if (myParent.GetComponent<Renderer>().enabled==true)
		//		{
		//		GetComponent<Renderer>().enabled=true;	
		//		}
	}
		//.Remove(myParent.name.LastIndexOf("Node")+4)

	void LateUpdate()
	{

		mytransform.rotation = datCamera.transform.rotation;	
	}
}

