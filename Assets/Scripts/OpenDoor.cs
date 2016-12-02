using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{

    public AnimationClip[] clips;// clipOpen, clipClose;
    [SerializeField]
    private Animation anim;
    private bool bOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ToggleDoor();
    }

    void ToggleDoor()
    {
        if (!anim.isPlaying)
        {
            anim.clip = clips[bOpen ? 1 : 0];
            anim.Play();
            bOpen = !bOpen;
        }
    }
}
