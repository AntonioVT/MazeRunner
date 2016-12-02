using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform tModel;
    public Transform player;
    public float yOffset;
    private Animator anim;

	void Start () {
        SetReferences();
        StartCoroutine(DelayAnimator());
	}

	void Update () {
        if (anim.enabled)
        {
            Follow();
            Rotate();
        }
    }

    void SetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
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
        euler.y += yOffset;
        euler.x = 0;
        tModel.rotation = Quaternion.Euler(euler);
    }

    IEnumerator DelayAnimator()
    {
        anim.enabled = false;
        yield return new WaitForSeconds(Random.Range(0, 2.0f));
        anim.enabled = true;
    }
}
