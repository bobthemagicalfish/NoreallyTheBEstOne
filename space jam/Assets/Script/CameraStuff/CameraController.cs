using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float Hor  = 0f;
    public float Ver = 0f;
    public float Wheel = 0f;
    public float what = 0f;
    public float Speed=0.9f;
    public float myangle;

	public float MaxX;
	public float MaxZ;
	public float MinZ=-40.0f;
	public float[] maxxz = new float[2];

	public Vector3 test;
    public Vector3 Moving;

    public GameObject heroSpawner;
	public GameObject player;
    private HeroSpawner heroSpawn;


    public Quaternion myroiate;
		void Start()
		{

	   this.GetComponent<FogOfWar> ().Unfog(new Vector3(0,0,0),1000,0);

        this.GetComponent<FogOfWar>().Unfog(new Rect(new Vector2(0.0f,5.0f),new Vector2(100f,100f)));
        heroSpawner= GameObject.Find("HeroController");
        heroSpawn = heroSpawner.GetComponent<HeroSpawner>();    
		Speed=0.9f;
	
		maxxz = heroSpawn.HowFarCanIGo (1);
//		MaxX = maxxz [0];
	//	MaxZ = maxxz [1];
			MaxX = 200.0f;
			MaxZ = 600.0f;
        myroiate = transform.rotation;
		}
	// Update is called once per frame
	void FixedUpdate () {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {


            transform.rotation =  myroiate;

        }

        myangle = transform.rotation.eulerAngles.x;
//		if(Input.GetKey(KeyCode.Q))
//		{
//			player.transform.RotateAround(player.transform.position,Vector3.up,Time.deltaTime*40);
//
//			//transform.position = player.transform.position +offsets;
//		//}
//		}
//		if(Input.GetKey(KeyCode.E))
//		{
//            player.transform.RotateAround(player.transform.position,Vector3.down,Time.deltaTime*40);
//			//transform.position = player.transform.position +offsets;
//		//}
//		}
		
//		
//		if(Input.GetKey(KeyCode.LeftAlt))
//		{
//            transform.Rotate(Vector3.left,1f);
//			//transform.position = player.transform.position +offsets;
//		//}
//		}
//		if(Input.GetKey(KeyCode.RightAlt))
//		{
//            transform.Rotate(Vector3.right,1f);
//			//transform.position = player.transform.position +offsets;
//		//}
//		}
//        if (Input.GetMouseButton(1)==true){
//
//        
//    
//            if ((Input.GetAxis("Mouse X") != 0))
//            {
//                player.transform.RotateAround(player.transform.position,Vector3.up*Input.GetAxis("Mouse X")*20.0f,Time.deltaTime*80);
//
//
//
//            }//x
//            if (Input.GetAxis("Mouse Y") < 0&&transform.rotation.eulerAngles.x <80)
//            {
//                        transform.Rotate(Vector3.right*.5f);
//
//               
//            }
//            if (Input.GetAxis("Mouse Y") > 0&&transform.rotation.eulerAngles.x>30)
//            {
//
//                    transform.Rotate(-Vector3.right*.5f);
//
// 
//
//            }
//
//     //   Ver = Input.GetAxis("Mouse Y")*Speed; //z
//     //   Wheel = Input.GetAxis("Mouse ScrollWheel")*Speed;//y
//        }

        Hor = Input.GetAxis("Horizontal")*Speed;//x
        Ver = Input.GetAxis("Vertical")*Speed; //z
        Wheel = Input.GetAxis("Mouse ScrollWheel")*Speed;//y
        test=player.transform.forward;
        Moving = new Vector3(Hor,Wheel*-1,Ver);

        if (Moving != new Vector3(0, 0, 0))
        {
            Moving = new Vector3();
//            test = test + Moving;
//
//            if (player.transform.position.y+test.y <= -10 || player.transform.position.y+test.y >= 23 || GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().IsAWindowOpen())
//            {
//
//                Wheel = 0;
//            }
//            if (player.transform.position.x<= -heroSpawn.maxDistancex || player.transform.position.x>= heroSpawn.maxDistancex)
//            {
//
//                Hor = 0;
//            }
//            if (player.transform.position.z <= -50 || player.transform.position.z >= heroSpawn.maxDistancez)
//            {
//
//                Ver = 0;
//            }
//            Moving = new Vector3(Hor, Wheel * -1, Ver);
         
            if (Ver< 0)
            {
                Moving =   -(player.transform.forward) ;
            }
            if (Ver > 0)
            {
                Moving =  (player.transform.forward) ;
            }


            if (Hor < 0)
            {

                Moving = Moving + -(player.transform.right);
            }
            if (Hor > 0)
            {
                Moving = Moving + (player.transform.right);
            }

            if (Wheel< 0)
            {

                Moving = Moving + (player.transform.up*2);
            }
            if (Wheel > 0)
            {
                Moving = Moving + -(player.transform.up*2);
            }


			test =  player.transform.position + Moving*Speed;

		//	|| GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().IsAWindowOpen()
            if (test.y <= 0|| test.y >= 30 )
            {

                Moving = new Vector3();
            }

         

			if (test.x<= -MaxX || test.x>= MaxX)
            {

                Moving = new Vector3();
            }
			if (test.z <= -75 || test.z >= MaxZ)
            {

                Moving = new Vector3();
            }
			player.transform.position = player.transform.position + Moving *Speed;
                      
        }
	}
}
