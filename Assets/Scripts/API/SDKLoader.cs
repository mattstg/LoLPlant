using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;
using SimpleJSON;

using System.IO;

public class SDKLoader {

	// Relative to Assets /StreamingAssets/
	private static readonly string languageJSONFilePath = "language.json";
	private static readonly string questionsJSONFilePath = "questions.json";
	private static readonly string startGameJSONFilePath = "startGame.json";
    private static readonly Dictionary<string, string> textDict = new Dictionary<string, string>();
    public static JSONNode startGameData;

    private static bool languageLoaded = false;
    private static bool startgameLoaded = false;

    public static void StartLoader()
    {
        // Create the WebGL (or mock) object
#if UNITY_EDITOR
        ILOLSDK webGL = new LoLSDK.MockWebGL();
		#elif UNITY_WEBGL
			ILOLSDK webGL = new LoLSDK.WebGL();
		#endif
        
        
		// Initialize the object, passing in the WebGL
		LOLSDK.Init (webGL, "com.Pansimula.LPlant");

		// Register event handlers
		LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler (HandleStartGame);
		LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler (HandleGameStateChange);   
	    LOLSDK.Instance.QuestionsReceived += new QuestionListReceivedHandler (HandleQuestions);
	    LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler (HandleLanguageDefs);

		// Mock the platform-to-game messages when in the Unity editor.
		#if UNITY_EDITOR
			LoadMockData();
		#endif

		// Then, tell the platform the game is ready.
		LOLSDK.Instance.GameIsReady();
	}

	// Start the game here
	static void HandleStartGame (string json)
    {
        //Given start game info, load the language and text files
        startGameData = JSON.Parse(json);
        try
        {
            MainScript.progressPoint = int.Parse(startGameData["progress"].Value);
        }
        catch
        {
            Debug.Log("Faulty LoL progress key, defaulted to zero");
            MainScript.progressPoint = 1;
        }
        try
        {
            MainScript.score = int.Parse(startGameData["score"].Value);
        }
        catch
        {
            Debug.Log("Faulty LoL score key, defaulted to zero");
            MainScript.score = 0;
        }
        startgameLoaded = true;
    }

    public static bool CheckIfEverythingLoaded()
    {
        return languageLoaded && startgameLoaded;
    }

    // Handle pause / resume
    static void HandleGameStateChange (GameState gameState) {
		// Either GameState.Paused or GameState.Resumed
		Debug.Log("HandleGameStateChange");
	}

    // Store the questions and show them in order based on your game flow.
    static void HandleQuestions (MultipleChoiceQuestionList questionList) {
		Debug.Log("HandleQuestions");
        SharedState.QuestionList = questionList;
	}

    // Use language to populate UI
    static void HandleLanguageDefs (string json)
    {
		JSONNode langDefs = JSON.Parse(json);
        LangDict.Instance.SetNode(langDefs);
        languageLoaded = true;
	}
    static private void LoadMockData () {
		#if UNITY_EDITOR
			// Load Dev Language File from StreamingAssets

			string startDataFilePath = Path.Combine (Application.streamingAssetsPath, startGameJSONFilePath);
			string langCode = "en";

			Debug.Log(File.Exists (startDataFilePath));

			if (File.Exists (startDataFilePath))  {
				string startDataAsJSON = File.ReadAllText (startDataFilePath);
				JSONNode startGamePayload = JSON.Parse(startDataAsJSON);
				// Capture the language code from the start payload. Use this to switch fontss
				langCode = startGamePayload["languageCode"];
				HandleStartGame(startDataAsJSON);                
			}

			// Load Dev Language File from StreamingAssets
			string langFilePath = Path.Combine (Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists (langFilePath))  {
				string langDataAsJson = File.ReadAllText (langFilePath);
				// The dev payload in language.json includes all languages.
				// Parse this file as JSON, encode, and stringify to mock
				// the platform payload, which includes only a single language.
				JSONNode langDefs = JSON.Parse(langDataAsJson);
				// use the languageCode from startGame.json captured above
				HandleLanguageDefs(langDefs[langCode].ToString());
                languageLoaded = true;
			}

			// Load Dev Questions from StreamingAssets
			string questionsFilePath = Path.Combine (Application.streamingAssetsPath, questionsJSONFilePath);
			if (File.Exists (questionsFilePath))  {
				string questionsDataAsJson = File.ReadAllText (questionsFilePath);
				MultipleChoiceQuestionList qs =
					MultipleChoiceQuestionList.CreateFromJSON(questionsDataAsJson);
			}
		#endif
	}
}
