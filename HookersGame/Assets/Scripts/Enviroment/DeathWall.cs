
using UnityEngine;

public class DeathWall : MonoBehaviour 
{
    [SerializeField] float speed;
    DeathWall deathwall;
    bool startDeathWall;
    Vector3 startPos;
    public DeathWall Instance { get; private set; }
    private void Start()
    {
        Instance = this;
        startPos = transform.position;
    }



    public void ResetDeathWall() {
        startDeathWall = false;
        transform.position = startPos;
    }

    public void StartRunAfterPlayer() { 
    
    
    }
    // Update is called once per frame
    void Update()
    {
      
           if( startDeathWall)
            StartRunAfterPlayer();
    }
}
