using UnityEngine;
using System.Collections;

public class TeleporterEnter : MonoBehaviour
{

    public Transform destination;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.GetComponent<Player>() != null)
            collider.transform.position = destination.position;
    }
}
