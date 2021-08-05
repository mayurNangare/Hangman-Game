using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour {
    [SerializeField] private Text timeText;
    [SerializeField] private Text wordToFindText;
    [SerializeField] private GameObject[] hangMan;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject replayButton;
    private float _timeInSeconds = 0f;
    private string[] words = File.ReadAllLines (@"Assets/Scripts/Words.txt");
    private string chosenWord;
    private string hiddenWord;
    private int fails;
    private bool gameEnd = false;

    private void Start () {
        chosenWord = words[Random.Range (0, words.Length)];

        for (int i = 0; i < chosenWord.Length; i++) {
            char letter = chosenWord[i];
            if (char.IsWhiteSpace (letter)) {
                hiddenWord += " ";
            } else {
                hiddenWord += "_";
            }
        }
        wordToFindText.text = hiddenWord;
    }

    private void Update () {
        if (!gameEnd) {
            ProcessText ();
        }
    }

    private void OnGUI () {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode.ToString ().Length == 1) {
            string pressedLetter = e.keyCode.ToString ();
            if (chosenWord.Contains (pressedLetter)) {
                int i = chosenWord.IndexOf (pressedLetter);

                while (i != -1) {
                    hiddenWord = hiddenWord.Substring (0, i) + pressedLetter + hiddenWord.Substring (i + 1);
                    Debug.Log (hiddenWord);
                    chosenWord = chosenWord.Substring (0, i) + "_" + chosenWord.Substring (i + 1);
                    Debug.Log (chosenWord);
                    i = chosenWord.IndexOf (pressedLetter);
                }

                wordToFindText.text = hiddenWord;
            } else {

                if (fails == hangMan.Length) return;
                hangMan[fails].SetActive (true);
                fails++;

            }

            if (fails == hangMan.Length) {
                loseText.SetActive (true);
                replayButton.SetActive (true);
                gameEnd = true;
            }

            if (!hiddenWord.Contains ("_")) {
                winText.SetActive (true);
                replayButton.SetActive (true);
                gameEnd = true;
            }
        }
    }

    private void ProcessText () {
        _timeInSeconds += Time.deltaTime;
        timeText.text = _timeInSeconds.ToString ();
    }
} // class