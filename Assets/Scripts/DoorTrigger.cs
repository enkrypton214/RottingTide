
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject doorHinge;

    void OnTriggerEnter(Collider other)
    {   
        SoundManager.Instance.backgroundMusicChannel.PlayOneShot(SoundManager.Instance.gameMusic);
        if (other.gameObject.CompareTag("Player"))
        doorHinge.GetComponent<Animator>().SetTrigger("DoorTrigger");
        Destroy(gameObject);
    }
}
