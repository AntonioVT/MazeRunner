using UnityEngine;
using System.Collections;

public class connectMaze : MonoBehaviour
{
	private bool alreadyDead = false;
	private float elevateTime = 0.0f;
	
	void Start()
	{
		GameObject Manager = GameObject.Find("MazeManager");
		
		for (int i = 0; i < Manager.GetComponent<mazeSpawner>().connectMazePos.Count; i++)
		{
			if(Manager.GetComponent<mazeSpawner>().connectMazePos[i].x == transform.position.x &&
				Manager.GetComponent<mazeSpawner>().connectMazePos[i].y == transform.position.z)
				{
					Destroy(gameObject);
				}
		}
		
		
		GameObject SafeZone = GameObject.Find("SafeZone(Clone)");
		
		GameObject mazeWall = Manager.GetComponent<mazeSpawner>().mazeWall;
		float mazeWallWidth = mazeWall.transform.localScale.x;
		
		Vector3 szPosition  = SafeZone.transform.position;
		Vector3 szScale     = SafeZone.transform.localScale;
		
		int mazeSizeX = Manager.GetComponent<mazeSpawner>().mazeSizeX;
		int mazeSizeY = Manager.GetComponent<mazeSpawner>().mazeSizeY;
		
		int sizeX = (mazeSizeX >= mazeSizeY) ? mazeSizeX : mazeSizeY;
		int sizeY = (mazeSizeX < mazeSizeY) ? mazeSizeX : mazeSizeY;
		
		Vector3 myPosition = transform.position;
		
		
		
		Vector3 furthestWall  = new Vector3(sizeX * 12.0f * (mazeWallWidth / 2.0f), 0, 0);
		Vector3 closestWall   = new Vector3((szPosition.x + (szScale.x / 2.50f)) / 2.25f, 0, 0);
		
		UnityEngine.Debug.Log( closestWall + " " + furthestWall);
		
		//elevateTime = 
		
		StartCoroutine(elevateWall());
		
		//StartCoroutine(elevateTimer());
	}
	
	void OnCollisionEnter(Collision collision) {
		if (!alreadyDead && collision.gameObject.name == "Cube(Clone)" ) {
			connectMaze script = collision.gameObject.GetComponent<connectMaze>();
			if (script != null) {
				 script.alreadyDead = true;
				 Destroy(collision.gameObject);
			}
		}
		
		if ( collision.gameObject.name == "SafeZone(Clone)" )
		{
			Destroy(gameObject);
		}
	}
	
	/*
	IEnumerator elevateTimer(){
		
		if ( < )
			StartCoroutine(elevateWall());
		
		else
			yield return new WaitForSeconds(0.5f);
	}*/
	
	IEnumerator elevateWall(){
		float yDelta  = 0.30f;
		float maxPosY = transform.localScale.y / 2.0f;
		
		Vector3 posOriginal = transform.position;
		
		for(float newPosY = posOriginal.y;  newPosY <= maxPosY ; newPosY += yDelta){
			transform.position = new Vector3(posOriginal.x, newPosY, posOriginal.z);
			yield return new WaitForSeconds(0.005f);
		}
	}
	
	
}
