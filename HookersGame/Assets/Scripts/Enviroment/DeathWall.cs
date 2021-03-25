
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool startDeathWall;
    Vector3 startPos;


    public void WallCloseUp()
    => transform.Translate(0, 0, speed * Time.deltaTime);

    
    public bool SetStartDeathWall { set { startDeathWall = value; } }
    private void Start()
    {
        LevelManager.ResetLevelParams += ResetDeathWall;
        startPos = transform.position;
    }


    private void ResetDeathWall()
    {
        startDeathWall = false;
        transform.position = startPos;
        speed = LevelManager.Instance.GetLevelDeathWallSpeed();
    }


    // Update is called once per frame
    void Update()
    {
        //change it to corutine
        if (startDeathWall && !CheatMenu.Instance.GetCanLoseCondition)
            WallCloseUp();
    }


    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ResetDeathWall;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && startDeathWall)
            LevelManager.Instance.ResetLevelValues();


    }
}
