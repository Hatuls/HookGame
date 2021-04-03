﻿
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

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
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