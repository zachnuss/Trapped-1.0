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
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//libraries for writing persistant data
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//libraries for error throwing
using System;
using System.Collections.Generic;

public class PlayerOptions : MonoBehaviour {
    /*
     *   TO DO:
     *       -Sound effects for player pick-up sounds
     *              --adjust volume
     *              --place to set audio tracks in scene
     *       -Sound effects for in store scene GO "StoreEmpty" get audio source there
     *              --adjust volume
     *              --place to set audio tracks in scene
     */
    ///public variables
    public float musicVolume = 1f;
    public float soundFXVolume = 1f;
    public bool isFullScreen;
    [Header("Slider Prefabs")]
    public Slider musicSlider;
    public Slider soundFXSlider;
    [Header("Button Prefabs")]
    public Button screenButton;
    public Button nextTrackButton;
    [Header("AudioClip Prefabs")]
    public AudioClip pickUpFX;
    public AudioClip storePurchaseFX;
    public AudioClip[] audioTracks;
    ///private variables
    private List<AudioSource> _enemyFXs;
    private AudioSource _musicSource;
    private AudioSource _testFX;
    private AudioSource _playerFX;
    private int _curTrackIndex; 
    private string _filePath;
    //UI text for proper string concatenation
    private Text _fullScreenText;
    private Text _curTrackText;
    private Text _backButtonText;
    //misc
    private int _numOfTracks;
    private int _buildIndex;

    void Start() {
        //initialize values
        _numOfTracks = audioTracks.Length;
        _filePath = Application.persistentDataPath + "/PlayerOptions.dat";
        isFullScreen = Screen.fullScreen;
        if (nextTrackButton) {
            _curTrackText = nextTrackButton.GetComponentInChildren<Text>();
        }

        //grab AudioListener and AudioSource if we're in game
        _buildIndex = SceneManager.GetActiveScene().buildIndex;
        //musicSource doesn't exist in store scene
        if (_buildIndex != 5) {
            if (GameObject.Find("Music") == true)
            {
                _musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
            }
        }
        if (_buildIndex >= 2 && _buildIndex <= 4) {
            _playerFX = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
            _playerFX.clip = pickUpFX;
            //get data and apply options where relavent in game
            OptionsData savedData = _getOptionsData();
            _musicSource.volume = savedData.s_musicVolume;
            float enemyFXVolume = savedData.s_soundFXVolume;
            _playerFX.volume = savedData.s_soundFXVolume;
            if (_backButtonText) {
                _backButtonText.text = "Close";
            }
            //get all audiosources from enemies and store them
            GameObject[] enemiesArr = GameObject.FindGameObjectsWithTag("Enemy");
            int numOfEnemies = enemiesArr.Length;
            _enemyFXs = new List<AudioSource>();
            for (int i = 0; i < numOfEnemies; ++i) {
                //adjust val and store
                AudioSource newSource = enemiesArr[i].GetComponent<AudioSource>();
                newSource.volume = enemyFXVolume;
                newSource.playOnAwake = false;
                _enemyFXs.Add(newSource);
            }
        }
        //check for options scene
        else if (_buildIndex == 10) {
            _testFX = Camera.main.gameObject.GetComponent<AudioSource>();
            //get stored data and apply it to scene
            OptionsData savedData = _getOptionsData();
            musicVolume = savedData.s_musicVolume;
            musicSlider.value = savedData.s_musicVolume;
            soundFXVolume = savedData.s_soundFXVolume;
            soundFXSlider.value = savedData.s_soundFXVolume;
            isFullScreen = savedData.s_isFullScreen;
            _curTrackIndex = savedData.s_curTrackIndex;
        }
        //check if this is the store screen
        else if (_buildIndex == 5) {
            //set store scene audio options
            //grab audio sources and data and apply them
            _playerFX = Camera.main.gameObject.GetComponent<AudioSource>();
            _playerFX.clip = storePurchaseFX;
            _musicSource = GetComponent<AudioSource>();
            OptionsData savedData = _getOptionsData();
            _playerFX.volume = savedData.s_soundFXVolume;
            _musicSource.volume = savedData.s_musicVolume;
            _curTrackIndex = savedData.s_curTrackIndex;
        }

        //set appropriate text for UI
        if (screenButton) {
            _fullScreenText = screenButton.GetComponentInChildren<Text>();
            _fullScreenText.text = "Screen: ";
            _fullScreenText.text += (isFullScreen) ? "Full Screen" : "Windowed";

            try {
                _curTrackText.text = "Current Track: " + audioTracks[_curTrackIndex].name;
            } catch (NullReferenceException) {
                //how will I handle this error
                _curTrackIndex = 0;
                _curTrackText.text = "Current Track: N/A";

            }
        }
    }
    //FOR PLAYER PICK UP COLLISIONS ONLY
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Currency" || other.tag == "PowerUp") {
            playPickUpFX();
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
        //edit UI Text to display if windowed or fullscreen
        _fullScreenText.text = "Screen: ";
        _fullScreenText.text += (isFullScreen) ? "Full Screen" : "Windowed";
    }

    //use a slider to adjust music volume
    public void adjustMusicVolume() {
        //volume threshhold is between 0.0f and 1.0f
        musicVolume = musicSlider.value;
        _musicSource.volume = musicSlider.value;
        if (musicSlider.value == 0.0f) {
            _musicSource.mute = true;
        }
        else {
            _musicSource.mute = false;
        }
    }

    //use a slider to adjust soundFX
    public void adjustSoundFXVolume() {
        //volume threshhold is between 0.0f and 1.0f
        soundFXVolume = soundFXSlider.value;
        //input sample sound for player with each increment?
        //test output for soundFX
        if (_testFX) { //check if not null var
            _testFX.volume = soundFXVolume;
            _playSampleFX();
        }
    }

    //moves to the next audio track in the array, or resets to start
    public void changeAudioTrack() {
        //start new text string
        _curTrackText.text = "Current Track: \n";
        try {
            //check if we're listening to the last track
            if (_curTrackIndex < audioTracks.Length - 1) {
                _curTrackText.text += audioTracks[++_curTrackIndex].name;
            }
            else {
                //reset to first
                _curTrackIndex = 0;
                _curTrackText.text += audioTracks[_curTrackIndex].name;
            }
            //play new track
            _musicSource.clip = audioTracks[_curTrackIndex];
            _musicSource.Play();
        } 
        catch (IndexOutOfRangeException indexError) {
            //possible reason: audioTrack array is not the same in every scene
            _curTrackIndex = 0;
            Debug.LogWarning("ASK CHRISTIAN: " + indexError); //give warning error
            throw;
        } 
        catch(NullReferenceException nullRefError) {
            //possible reason: audioTrack array was never set up, or script can't find it
            _curTrackText.text += "\n N/A";
            Debug.LogWarning("ASK CHRISTIAN: " + nullRefError); //give warning error
        }
    }

    //go back to the main menu scene
    public void goBack() {
        //store option values to persistent data
        OptionsData newData = new OptionsData(musicVolume,
                                              soundFXVolume,
                                              isFullScreen,
                                              _curTrackIndex);
        _storeOptionsData(newData);

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        //move back to MainMenu or go back to Pause
        if (buildIndex == 11) {
            SceneManager.LoadScene(1);
        }
        else if (buildIndex >= 1 && buildIndex <= 3) {
            //set my canvas to be SetActive(false);
            //if implemented into the pause screen

            //set new soundFX volumes on enemies
            foreach (var source in _enemyFXs) {
                source.volume = soundFXVolume;
            }
        }
    }

    //when the player makes a purchase in the store screen, play the
    //appropriate sound effect
    public void playPurchaseFX() {
        _playerFX.clip = storePurchaseFX;
        _playerFX.Play();
    }

    //when the player picks up a coin in-game, play appropriate sound effect
    public void playPickUpFX() {
        //_playerFX.clip = pickUpFX;
        _playerFX.Play();
    }

    //for testing soundFX and getting feedback from it
    private void _playSampleFX() {
        _testFX.Play();
        Invoke("_killSampleFX", 0.5f);
    }
    private void _killSampleFX() {
        _testFX.Stop();
    }

    /**
     *  PRIVATE HELPER FUNCTIONS
     */ 
    //takes arguement OptionsData and stores it into persistant data
    private void _storeOptionsData(OptionsData storing) {
        //create file, then write data to it
        FileStream fStream = File.Create(_filePath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fStream, storing);
        fStream.Close();
    }

    //load options for persistant data, if none is found, create a new instance
    private OptionsData _getOptionsData() {
        FileStream fStream;
        OptionsData data = new OptionsData();
        //if this data exists in the specified file path, retrieve it
        if (File.Exists(_filePath)) {
            fStream = File.OpenRead(_filePath);
            BinaryFormatter bf = new BinaryFormatter();
            data = (OptionsData)bf.Deserialize(fStream);
            fStream.Close();
        }
        return data;
    }
}
