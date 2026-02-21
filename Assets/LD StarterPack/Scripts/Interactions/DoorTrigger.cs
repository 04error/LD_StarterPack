using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private DoorController[] doors;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (DoorController door in doors)
            {
                door.Open();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (DoorController door in doors)
            {
                door.Close();
            }
        }
    }
}

