
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject doorHinge;

    void OnTriggerEnter(Collider other)
    {Debug.Log("TRIGGER ENTERED BY: " + other.name);
        if (other.gameObject.CompareTag("Player"))
        doorHinge.GetComponent<Animator>().SetTrigger("DoorTrigger");
    }
}
