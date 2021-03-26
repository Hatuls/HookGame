
using UnityEngine;

public class SoundTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
    private void OnTriggerExit2D(Collider2D collision) 
    {

        Debug.Log("Exit");

    }
}
