using UnityEngine;

public partial class VolumeBox : MonoBehaviour
{
    [Header("Sound Effects:")]
    [Header("By Band:")]
    [Tooltip("Emission affected from audio")]
    [SerializeField] bool toAffectEmission;

    [Tooltip("Scale on Y affected from audio")]
    [SerializeField] bool toScaleYObject;

    [Tooltip("Buffer will make the audio affect more smoother")]
    [SerializeField] bool useBuffer;
    [Tooltip("What band to be affected by (0-8)")]
    [Range(0, 8)]
    [SerializeField] int band;

    [SerializeField] float startScale,emissiomMaxValue, emissionBuffer, emissionStarter,scaleBuffer, maxScale;

    [HideInInspector]
    [SerializeField] MeshRenderer mr;
    Material mat;
    public int GetSetBand
    {
        get => band;
        set
        {
            if (value <= 0)
                band = 0;
            else if (value >= 7)
                band = 7;
            else
                band = value;

        }

    }
    private void Awake()
    {
        SoundManager.D8BeatEvent += CheckBeat;
    }
    private void Start()
    {
        Init();

    }
    void Init() {
      
        if (mr== null)
            Debug.Log("Didnt Find the meshRenderer");
        else
        {
        mat = mr.materials[0];
        if(mat == null)
            Debug.LogError("Error Volume box didnt find his material");
        }

        _currentSize = _shrinkSize;
   
    }
    // Update is called once per frame
    void Update()
    {
        if (SoundManager.IsByBeat)
        {
            Shrink();
            if (toAffectEmission)
            Emission(_currentSize);
        }
        else
        {
            if (toScaleYObject)
        {
            transform.localScale = new Vector3(
                        transform.localScale.x,
                         ScaleY(),
                        transform.localScale.z);
        }

        if (mat != null && toAffectEmission)
            Emission(GetBandValue(useBuffer));
        else if (mat == null)
            Init();
        }
        



    }
    float ScaleY()
    {

        float y = (GetBandValue(useBuffer) * scaleBuffer) + startScale;
        if (y > maxScale && y != 0)
            y = maxScale;
        return y;
    }
    void Emission(float value)
    {
        float bandValue =0;
        if (!SoundManager.IsByBeat)
        {
         bandValue = emissiomMaxValue * (emissionBuffer*value) + emissionStarter;
        }
        else
        {
            bandValue = transform.localScale.y / maxScale;
        }

        mat.SetColor("Color_53031B36",
            new Color(bandValue, bandValue, bandValue));
    }

    float GetBandValue(bool _useBuffer)
    => _useBuffer ? SoundManager._audioBandBuffer[band] : SoundManager._audioBand[band];






}
public partial class VolumeBox {


    private float _currentSize;
    [Header("By Beat:")] 
    [Header("Behaviour Settings")]
    public float _growSize;
    public float _shrinkSize;
    [Range(0.8f, 0.99f)]
    public float _shrinkFactor;

    [Header("Beat Settings")]
    [Range(1, 8)]
    public int _beatSteps;
    [Range(0, 7)]
    [Tooltip("A string using the MultiLine attribute")]
    public int _onFullBeat;
    [Range(0, 7)]
    public int[] _onBeatD8;

    private int _beatCountFull;

  

    // Update is called once per frame
   

    private void CheckBeat()
    {
        if (_beatSteps != 0)
        {

            _beatCountFull = SoundManager._beatCountFull % _beatSteps;


            for (int i = 0; i < _onBeatD8.Length; i++)
            {
                if (_beatCountFull == _onFullBeat && SoundManager._beatCountD8 % 8 == _onBeatD8[i])
                    Grow();
            }
        }
    }

    void Grow()
    {
        _currentSize = _growSize;
        //if (_onBeatD8[0] == 6)
        //    Debug.Log(true);
        //else if (_onBeatD8[0] == 2)
        //    Debug.Log(false);

    }
    void Shrink()
    {
        if (_currentSize > _shrinkSize)
            _currentSize *= _shrinkFactor;
        else
            _currentSize = _shrinkSize;

        transform.localScale = new Vector3(transform.localScale.x, _currentSize, transform.localScale.z); ;
    }
    void UnSubscribeEvents()
    {
        SoundManager.D8BeatEvent -= CheckBeat;

    }
    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}