/*
 * Author: Christian Mullins
 * Summary: Stores and gives functions to adjust player options in game.
 *      Features include:
 *          -Adjusting music volume
 *          -Adjusting sound effects volume
 *          -Windowed to fullscreen
 *          -Change audio tracks
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOptions : MonoBehaviour
{
    //public variables
    public float musicVolume;
    public float soundFXVolume;
    public bool isFullScreen;
    public AudioClip[] audioTracks;

    ///private variables
    private AudioListener _listener;
    private AudioSource _musicSource;
    private AudioClip _currentTrack;
    

    void Start() {
        //initialize values
        isFullScreen = Screen.fullScreen;
        //grab AudioListener and AudioSource
        if (true/*SceneManager.*/) {
            _musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
            _listener = Camera.main.GetComponent<AudioListener>();
        }
    }

    /*    BUTTON FUNCTIONS    */
    //simply toggles if the game will go to fullscreen or not.
    public void toggleFullscreen() {
        //check the current value of the bool
        if (isFullScreen) {
            //go to windowed screen
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else {
            //go to fullscreen
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        //resave current value
        isFullScreen = Screen.fullScreen;
    }

    //use a slider to adjust music volume
    public void adjustMusicVolume(float newVolume) {

    }

    //use a slider to adjust soundFX
    public void adjustSoundFXVolume(float newVolume) {

    }

    //moves to the next audio track in the array, or resets to start
    public void changeAudioTrack() {
        int curTrack = _getCurrentAudioTrackIndex();

        //check if we're listening to the last track
        if (curTrack < audioTracks.Length - 1) {
            _currentTrack = audioTracks[++curTrack];
        }
        else {
            //reset to first
            _currentTrack = audioTracks[0];
        }
    }

    //go back to the main menu scene
    public void backToMainMenu() {
        //store option values to persistent data

        //move back to original scene
    }

    /**
     *  PRIVATE HELPER FUNCTIONS
     */ 
    //return the index val of the current audio object in the array
    private int _getCurrentAudioTrackIndex() {
        //set to -1 to flag an error if it's not found
        int index = 0;

        for (int i = 0; i < audioTracks.Length; ++i) {
            //check
            if (_currentTrack == audioTracks[i]) {
                index = i;
            }
        }
        return index;
    }

    //store/rewrite options in a scriptable objects
    private void _writeNewOptions() {

    }

    //create if does not exist
    private /*AudioSettings*/ void _createNewOptionsFile() {

    }
}
