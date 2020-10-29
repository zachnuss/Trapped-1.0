/*
 * Author: Christian Mullins
 * Summary: This enables the storage and retrieval
 */ 
using UnityEngine; //for AudioClips

[System.Serializable]
public class OptionsData {

    //saved values, denoted w/ "s_"
    public float s_musicVolume;
    public float s_soundFXVolume;
    public bool s_isFullScreen;
    public int s_curTrackIndex;

    //empty constructor
    public OptionsData() {
        //set as default values w/ volume at max value
        s_musicVolume = 1.0f;
        s_soundFXVolume = 1.0f;
        s_isFullScreen = Screen.fullScreen;
        s_curTrackIndex = 0;
    }

    //initialized constructor
    public OptionsData(float mV, float sFXV, bool iFS, int cTI) {
        //store vals using arguments
        s_musicVolume = mV;
        s_soundFXVolume = sFXV;
        s_isFullScreen = iFS;
        s_curTrackIndex = cTI;
    }
}
