using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform tModel;
    public Transform player;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        Follow();
        Rotate();
    }

    void Follow()
    {
        agent.SetDestination(player.transform.position);
    }

    void Rotate()
    {
        tModel.LookAt(player);
        Quaternion qt = tModel.rotation;
        Vector3 euler = qt.eulerAngles;
        euler.y += 90;
        euler.x = 0;
        tModel.rotation = Quaternion.Euler(euler);
    }
}
