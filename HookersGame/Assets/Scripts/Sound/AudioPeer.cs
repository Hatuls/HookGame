
using UnityEngine;
[RequireComponent (typeof (AudioSource)) ]
public partial class SoundManager 
{
    [SerializeField] float bufferDecreaserValue, BufferAdderValuer;
    int amountOfGO = 8;
     float[] _samples = new float[512];
     float[] _freqBand = new float[8];
     float[] _bandBuffer = new float[8];
     float[] _bufferDecrease = new float[8];


    [SerializeField] float audioProfile = 5f;
    
    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float Amplitude,AmplitudeBuffer;
    float AmplitudeHighest;


    [Header("Timer For Audio Check")]
    [SerializeField] float _timer;

   
    void Update()
    {
        //audio freq
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
        BandBuffer();

        // audio beat
        BeatDetection();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(CheckBeat());
        }
    }
    private void GetAmplitude()
    {
        float currentAmplitude= 0, currentAmpltudeBuffer =0;
        for (int i = 0; i < _audioBand.Length; i++)
        {
            currentAmplitude += _audioBand[i];
            currentAmpltudeBuffer += _audioBandBuffer[i];
        }
        if (currentAmplitude > AmplitudeHighest)
            AmplitudeHighest = currentAmplitude;

        Amplitude = currentAmplitude/AmplitudeHighest;
        AmplitudeBuffer = currentAmpltudeBuffer/AmplitudeHighest;
    }
    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i]> _freqBandHighest[i])
                _freqBandHighest[i] = _freqBand[i];
            
            _audioBand[i]=(_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i]=(_bandBuffer[i] / _freqBandHighest[i]);
        }
    }
    private void AdjustAudioProfile(float _audioProfile) {
        for (int i = 0; i < _freqBandHighest.Length; i++)
            _freqBandHighest[i] = _audioProfile;
    }
    private void BandBuffer() {

        for (int i = 0; i < amountOfGO; i++)
        {
            if (_freqBand[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = _freqBand[i];
                _bufferDecrease[i] = bufferDecreaserValue;
            }
            if (_freqBand[i] < _bandBuffer[i])
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= BufferAdderValuer;
            }
        }
    
    }
    private void GetSpectrumAudioSource() {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }
    private void MakeFrequencyBands()
    {
        /*
         * hertz = first need to know the amount of hertz of the song
         * hertz / 512 (bands) = amount of hertz per sample
         * 
         * 7 bands channels:
         * 20-60 hertz
         * 60-250 hertz
         * 250-500 hertz
         * 500-2000 hertz
         * 2000-4000 hertz
         * 4000-6000 hertz
         * 6000-20000 hertz
         * 
         * @@@@@@@@@@EXAMPLE@@@@@@@@@@@@
         * hertz = 22050
         * hertz per sample = 22050 / 512 = 43 hertz per sample
         *         * Frequancy bands:
         * counter |frequancy sample| amount of hertz per sample * amount of boxes | Range
         * 0       | 2              | 43 hertz * 2 = 86 hertz                      | 0 - 86
         * 1       | 4              | 43 hertz * 4 =  172 hertz                    | 87 - 258
         * 2       | 8              | 43 hertz * 8 =  344 hertz                    | 259 - 600
         * 3       | 16             | 43 hertz * 16 =  688 hertz                   | 601 - 1288
         * 4       | 32             | 43 hertz * 32 =  1376 hertz                  | 1289 - 2665
         * 5       | 64             | 43 hertz * 64 =  2752 hertz                  | 2666 - 5418
         * 6       | 128            | 43 hertz * 128 =  5504 hertz                 | 5419 - 10922
         * 7       | 256            | 43 hertz * 256 =  11008 hertz                | 10923 - 21930
         *           =
         *           510
         * @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
         * 
         * 
         * 
         * Frequancy bands:
         * boxes
         * counter | frequancy sample | amount of hertz per sample * amount of boxes
         * 0       | 2                | 
         * 1       | 4                | 
         * 2       | 8                | 
         * 3       | 16               | 
         * 4       | 32               | 
         * 5       | 64               | 
         * 6       | 128              | 
         * 7       | 256              | 
         * 
  

         */


        int count = 0;
        for (int i = 0; i < amountOfGO; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            float average = 0;
            if (i == 7)
                sampleCount += 2;



            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count+1);
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }


    }
}

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                   BEAT MANAGER              @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
public partial class SoundManager {

    [SerializeField] bool toGoByBeat;
    public static bool IsByBeat => Instance.toGoByBeat;
    public static bool _beatFull, _beatD8;  // become true when a beat aqccured
    public static int _beatCountFull, _beatCountD8; // an option to count how many beats happend


    private float _beatInterval; // the time between full beat based on the bpm
    private float _beatTimer; // the timer that calculate based on the beat interval when there is supposed to be the next beat;


    private float _beatIntervalD8, _beatTimerD8; // example of the same system but divided by 8

    public static float _currentTime;

    public delegate void OnInputToBeat(bool onPress);
    public static event OnInputToBeat OnBeatPressed;
    public delegate void OnFullBeatEvent();
    public static event OnFullBeatEvent FullBeatEvent;
    public static event OnFullBeatEvent D8BeatEvent;

    bool isValid;
    [Header("Click Beat Options")]
    [Tooltip("Example of use: 1 second will mean  second before the beat happens")]
    [SerializeField] float timerToSucessBeforePress;
    [Tooltip("Example of use: 1 second will mean 1 second after the beat happends")]
    [SerializeField] float timerToSucessAfterPress;
    public float BeatSpeed => _beatInterval + timerToSucessAfterPress; 

    float timerForTotalBeat;

    public static bool IsOnBeat
   =>  Instance.timerForTotalBeat>= GetTimeBetweenBeat() - Instance.timerToSucessBeforePress;
          
        
    
    
    
    void BeatDetection()
    {

        _beatFull = false;

        _beatInterval = 60 / currentSong.GetBPM;

        _beatTimer += Time.deltaTime;
        timerForTotalBeat += Time.deltaTime;

        if (_beatTimer >= _beatInterval) // check the time of the up coming beat
        {
            // beat aqccured
            _beatTimer -= _beatInterval;
            _beatFull = true;
            _beatCountFull++;
            //Debug.Log("Full ");

            FullBeatEvent?.Invoke();
        }


        // timer for overall beat time
        if (timerForTotalBeat > timerToSucessAfterPress + GetTimeBetweenBeat())
            timerForTotalBeat = 0;
        

        // divided beat count
        // this example D8 mean bpm / 8
        _beatD8 = false;
        _beatIntervalD8 = _beatInterval / 8;
        _beatTimerD8 += Time.deltaTime;
        if (_beatTimerD8 >= _beatIntervalD8)
        {
            _beatTimerD8 -= _beatIntervalD8;
            _beatD8 = true;
            _beatCountD8++;
            D8BeatEvent?.Invoke();
            // Debug.Log("D8");
        }
        BeatActionCalculator();
    }
    NoteIcon[] notes;
    private void BeatActionCalculator() {
        _currentTime += Time.deltaTime;

        if (_currentTime <= _beatInterval + (timerToSucessAfterPress))
        {
            if (_currentTime >= _beatInterval - (timerToSucessBeforePress ))
                isValid = true;
        }
        else
        {
            _currentTime -= _beatInterval + (timerToSucessAfterPress);
            if (notes != null && notes.Length > 0)
            {
                for (int i = 0; i < notes.Length; i++)
                {
                    notes[i].ResetColor();
                }
            }
            isValid = false;
        }
        
    }


    public bool CheckBeat() {
        OnBeatPressed?.Invoke(isValid);
        return isValid;
    }
}