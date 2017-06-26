using UnityEngine;
using System.Collections;
	using System.Collections.Generic;

public class BountyController : MonoBehaviour {
	public enum BountyType
	{
	Explore,
	Destory,
	Guard,	
	};
	public int Timer=0;
	public bool Istheirbadguys;
	public float startofTimer;
	public bool Timergoing = false;
	public bool addingHeroLock = false;
	public int bountyAmount=0;
	public int currentHero=0;
	public int maxHeroAllowed=0;
	public BountyType bountyType;
	public List<GameObject> heroatthisbounty;
	public bool BountyComplete =false;
	// Use this for initialization
	
	
	void Start () {
	heroatthisbounty = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		checkIfHeroLeft();
		checkifCompleted();
		
	}
	
	public void checkifCompleted()
	{
		if(bountyType ==BountyType.Destory)
		{
			Istheirbadguys=false;
			Collider[] hitforbaddiecheck = Physics.OverlapSphere(gameObject.transform.position,gameObject.GetComponent<SphereCollider>().radius);	
			for( int i =0; i < hitforbaddiecheck.Length;i++)
			{
			//	Debug.Log((hitforbaddiecheck.Length)+hitforbaddiecheck[i].gameObject.tag.ToString());
				if (hitforbaddiecheck[i].gameObject.tag=="Enemy")
				{
					Istheirbadguys=true;	
					
				}
				
				
			}
			
			if (Istheirbadguys==false)
			{
				Paytheheros();
				Destroy(gameObject);
			}
			
			
		}
		else if (bountyType==BountyType.Guard)
		{
		
			if (Timergoing==false)
			{
		
				for (int i = 0 ;i < heroatthisbounty.Count;i++)
				{
					if (Vector3.Distance(heroatthisbounty[i].transform.position,gameObject.transform.position) <= ((gameObject.GetComponent<SphereCollider>().radius)))
					{
						Timergoing=true;
						startofTimer=Time.time;
					}
					
					
				}
						
			}
			else
			{
				//Debug.Log((Time.time- startofTimer).ToString());
				if (Time.time- startofTimer >=Timer)
				{
					Paytheheros();
					Destroy(gameObject);
				}
			
			}
		}
		
		
		
	}
	public void Explored()
	{	
		if (BountyComplete==false)
		{
			BountyComplete=true;
			Paytheheros();
			
			Destroy(gameObject);
		}
			
			
			

		
	}
	private void Paytheheros()
	{
		for (int i = 0 ;i < heroatthisbounty.Count;i++)
			{
				if (Vector3.Distance(heroatthisbounty[i].transform.position,gameObject.transform.position) <= gameObject.GetComponent<SphereCollider>().radius)
				{
//					Debug.Log((Vector3.Distance(heroatthisbounty[i].transform.position,gameObject.transform.position).ToString()));
					heroatthisbounty[i].GetComponent<HeroAI>().AddMoney(bountyAmount);	
					GameObject.FindGameObjectWithTag("PlayerTotals").GetComponent<MainMoney>().AddGold(-bountyAmount);
			//	Debug.Log(heroatthisbounty[i].GetComponent<HeroAI>().name);
				}
				
				
			}
		
	}
	
	
	// 1 for explore 2 for destory 3 for guard
	public void SetBountyType(int setter)
	{
	
		if (setter == 1)
		{
			bountyType=BountyType.Explore;	
			
		}
		else if (setter==2)
			
		{
			
			bountyType=BountyType.Destory;	
		}
		else
		{
			
			bountyType=BountyType.Guard;
		}
		
		
	}
	
	
	
	public bool HeroAdd(GameObject herotoadd)
	{
		if (addingHeroLock==false)
		{
			
			addingHeroLock=true;
			if (heroatthisbounty.Count < maxHeroAllowed)
			{
				//Debug.Log("request for bounty"+herotoadd.name.ToString());
				heroatthisbounty.Add(herotoadd);
				if (heroatthisbounty.Contains(herotoadd))
				{
					Debug.Log("Yourgood to go "+herotoadd.name.ToString());
					addingHeroLock=false;
					return true;
				}
				else
				{
					addingHeroLock=false;
					return false;
				}
			}
			else
			{
				addingHeroLock=false;
				return false;
			}
			
		}
		else
		{	
			return false;
		}
	}
	
	public bool Checkingifiminthelist (GameObject herotocheck)
	{
		if (heroatthisbounty.Contains(herotocheck))
		{
			
			return true;	
		}
		else
		{
			return false;
		}
		
		
	}
	
	
	public void RemoveHero(GameObject hertoremove)
	{
		heroatthisbounty.Remove(hertoremove);
	
		
	}
	
	
	
	public string GetBountyType()
		
	{
	if (bountyType==BountyType.Destory)
		{
			return "Destory";
		}
	else if (bountyType==BountyType.Guard)
		{
			
			return"Guard";	
		}
	else 
		{
		return "Explore";	
		}
		
	}
	
    public bool AmIFull()
    {
        if (heroatthisbounty.Count<maxHeroAllowed) 
        {
            return false;
        }
       
        else 
        {
            return true;
        }


    }

	private void checkIfHeroLeft()
	{
		// check to make sure a hero hasnt died before mucking about
		for (int i = 0; i < heroatthisbounty.Count;i++)
		{
			if(heroatthisbounty[i].gameObject==null)
			{
			heroatthisbounty.RemoveAt(i);	
				
			}
			
			
		}
		
		/*
		for (int i = 0; i < heroatthisbounty.Count;i++)
		{
			if (Vector3.Distance(heroatthisbounty[i].transform.position,transform.position) >32)
			{
				heroatthisbounty.RemoveAt(i);	
				
			}
			
			
		}
		*/
	
		
		
	}
	

}
