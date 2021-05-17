
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool startDeathWall;
    Vector3 startPos;
   [SerializeField] ParticleSystem[] _particleSystems;
    bool isFirstCall = true;
    public void WallCloseUp()
    {
        if (isFirstCall)
        {
            if (_particleSystems != null)
            {
                for (int i = 0; i < _particleSystems.Length; i++)
                    _particleSystems[i].Play();
            }
            LeanTween.scale(this.gameObject, new Vector3(206, 206, 1), 3f).setEaseInBack();
            isFirstCall = false;
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
    
    public bool SetStartDeathWall { set { startDeathWall = value; } }
    private void Start()
    {
        LevelManager.ResetLevelParams += ResetDeathWall;
        startPos = transform.position;
        transform.localScale = Vector3.zero;


       
    }


    private void ResetDeathWall()
    {
        isFirstCall = true;
        startDeathWall = false;
        if (_particleSystems != null)
        {
            for (int i = 0; i < _particleSystems.Length; i++)
                _particleSystems[i].Stop();
        }
        transform.position = startPos;
        transform.localScale = Vector3.zero;
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
