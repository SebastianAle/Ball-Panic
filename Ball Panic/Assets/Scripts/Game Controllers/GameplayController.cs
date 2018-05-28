using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour 
{
	public static GameplayController instance;

	[SerializeField]
	private GameObject[] topAndBottomBricks, leftBricks, rightBricks;

	public GameObject panelBG, levelFinishedPanel, playerDiedPanel, pausePanel;

	private GameObject topBrick, bottomBrick, leftBrick, rightBrick;

	private Vector3 coordinates;

	[SerializeField]
	private GameObject[] players;

	public float levelTime;

	public Text liveText, scoreText, levelTimerText, showScoreAtTheEndOfTheLevelText, countDownAndBeginLevelText, watchVideoText;

	private float countDownBeforeLevelBegins = 3.0f;

	public static int smallBallsCount = 0;

	public int playerLives, playerScore, coins;

	public bool isGamePaused, hasLevelBegan, levelInProgress, countDownLevel;

	[SerializeField]
	private GameObject[] endOfLevelRewards;

	[SerializeField]
	private Button pauseBtn;

	void Awake()
	{
		CreateInstance ();
		InitializeBricksAndPlayer ();
	}

	// Use this for initialization
	void Start () 
	{
		InitializeGameplayController ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateGameplayController ();
	}

	void CreateInstance()
	{
		if (instance == null) 
		{
			instance = this;
		}
	}

	void InitializeBricksAndPlayer()
	{
		coordinates = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, Screen.height, 0));
		
		int index = Random.Range (0, topAndBottomBricks.Length);

		topBrick = Instantiate (topAndBottomBricks [index]);
		bottomBrick = Instantiate (topAndBottomBricks [index]);
		leftBrick = 
			Instantiate (leftBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, -90))) as GameObject;
		rightBrick = 
			Instantiate (rightBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, 90))) as GameObject;

		topBrick.tag = "TopBrick";

		Vector3 temp = topBrick.transform.localScale;
		temp.y = -1;
		topBrick.transform.localScale = temp;

		topBrick.transform.position = new Vector3 (coordinates.x - 25, coordinates.y, 0);
		bottomBrick.transform.position = new Vector3(coordinates.x - 25, coordinates.y - 10, 0);
		leftBrick.transform.position = new Vector3(-coordinates.x, coordinates.y + 10f, 0);
		rightBrick.transform.position = new Vector3(coordinates.x, coordinates.y - 20f, 0);

		Instantiate(players[GameController.instance.selectedPlayer]);
	}

	void InitializeGameplayController()
	{
		if (GameController.instance.isGameStartedFromLevelMenu) 
		{
			playerScore = 0;
			playerLives = 2;
			GameController.instance.currentScore = playerScore;
			GameController.instance.currentLives = playerLives;
			GameController.instance.isGameStartedFromLevelMenu = false;
		} else 
		{
			playerScore = GameController.instance.currentScore;
			playerLives = GameController.instance.currentLives;
		}

		levelTimerText.text = levelTime.ToString ("F0");
		scoreText.text = "Score x" + playerScore;
		liveText.text = "x" + playerLives;

		Time.timeScale = 0;
		countDownAndBeginLevelText.text = countDownBeforeLevelBegins.ToString ("F0");
	}

	void UpdateGameplayController()
	{
		scoreText.text = "Score x" + playerScore;
		if (hasLevelBegan) 
		{
			CountDownAndBeginLevel ();
		}

		if (countDownLevel) 
		{
			LevelCountDownTimer ();
		}
	}

	public void setHasLevelBegan(bool hasLevelBegan)
	{
		this.hasLevelBegan = hasLevelBegan;
	}

	void CountDownAndBeginLevel()
	{
		countDownBeforeLevelBegins -= (0.19f * 0.15f);
		countDownAndBeginLevelText.text = countDownBeforeLevelBegins.ToString ("F0");
		if (countDownBeforeLevelBegins <= 0) 
		{
			Time.timeScale = 1;
			hasLevelBegan = false;
			levelInProgress = true;
			countDownLevel = true;
			countDownAndBeginLevelText.gameObject.SetActive (false);
		}
	}

	void LevelCountDownTimer()
	{
		if (Time.timeScale == 1) 
		{
			levelTime -= Time.deltaTime;
			levelTimerText.text = levelTime.ToString ("F0");

			if (levelTime <= 0) 
			{
				playerLives--;
				GameController.instance.currentLives = playerLives;
				GameController.instance.currentScore = playerScore;

				if (playerLives < 0) 
				{
					StartCoroutine (PromptTheUserToWatchAVideo());

				} else 
				{
					StartCoroutine (PlayerDiedRestartLevel());
				}
			}
		}
	}

	IEnumerator PlayerDiedRestartLevel()
	{
		levelInProgress = false;
		coins = 0;
		smallBallsCount = 0;

		Time.timeScale = 0;

		if (LoadingScreen.instance != null) 
		{
			LoadingScreen.instance.FadeOut ();
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		SceneManager.LoadScene(SceneManager.GetActiveScene ().name);

		if (LoadingScreen.instance != null) 
		{
			LoadingScreen.instance.PlayFadeInAnimation ();
		}
	}

	public void PlayerDied()
	{
		countDownLevel = false;
		pauseBtn.interactable = false;
		levelInProgress = false;

		smallBallsCount = 0;

		playerLives--;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		if (playerLives < 0) 
		{
			StartCoroutine (PromptTheUserToWatchAVideo());

		} else 
		{
			StartCoroutine (PlayerDiedRestartLevel());
		}
	}

	IEnumerator PromptTheUserToWatchAVideo()
	{
		levelInProgress = false;
		countDownLevel = false;
		pauseBtn.interactable = false;

		Time.timeScale = 0;

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.7f));

		playerDiedPanel.SetActive (true);
	}

	IEnumerator LevelCompleted()
	{
		countDownLevel = false;
		pauseBtn.interactable = false;

		int unlockedLevel = GameController.instance.currentLevel;
		unlockedLevel++;

		if(!(unlockedLevel >= GameController.instance.levels.Length))
		{
			GameController.instance.levels [unlockedLevel] = true;
		}

		Instantiate (endOfLevelRewards [GameController.instance.currentLevel], 
			new Vector3 (0, Camera.main.orthographicSize, 0), Quaternion.identity);

		if (GameController.instance.doubleCoins) 
		{
			coins *= 2;
		}

		GameController.instance.coins = coins;
		GameController.instance.Save ();

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (4f));
		levelInProgress = false;

		PlayerScript.instance.StopMoving ();
		Time.timeScale = 0;

		levelFinishedPanel.SetActive (true);
		showScoreAtTheEndOfTheLevelText.text = "" + playerScore;
	}


	public void CountSmallBalls()
	{
		smallBallsCount--;

		if (smallBallsCount == 0) 
		{
			StartCoroutine (LevelCompleted());
		}
	}

	public void GoToMapButton()
	{
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highScore < GameController.instance.currentScore) 
		{
			GameController.instance.highScore = GameController.instance.currentScore;
			GameController.instance.Save();
		}

		if(Time.timeScale == 0)
		{
			Time.timeScale = 1;
		}

		SceneManager.LoadScene("LevelMenu");

		if(LoadingScreen.instance != null)
		{
			LoadingScreen.instance.PlayLoadingScreen();
		}
	}

	public void RestartCurrentLevel()
	{
		smallBallsCount = 0;
		coins = 0;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		SceneManager.LoadScene (SceneManager.GetActiveScene().name);

		if(LoadingScreen.instance != null)
		{
			LoadingScreen.instance.PlayLoadingScreen();
		}
	}

	public void NextLevel()
	{
		GameController.instance.currentScore = playerScore;
		GameController.instance.currentLives = playerLives;

		if (GameController.instance.highScore < GameController.instance.currentScore) 
		{
			GameController.instance.highScore = GameController.instance.currentScore;
			GameController.instance.Save();
		}

		int nextLevel = GameController.instance.currentLevel;
		nextLevel++;

		if(!(nextLevel >= GameController.instance.levels.Length))
		{
			GameController.instance.currentLevel = nextLevel;
		}

		SceneManager.LoadScene ("Level" + nextLevel);

		if(LoadingScreen.instance != null)
		{
			LoadingScreen.instance.PlayLoadingScreen();
		}
	}


	public void PauseGame()
	{
		if (!hasLevelBegan) 
		{
			if (levelInProgress) 
			{
				if (!isGamePaused) 
				{
					countDownLevel = false;
					levelInProgress = false;
					isGamePaused = true;

					panelBG.SetActive (true);
					pausePanel.SetActive (true);

					Time.timeScale = 0;
				}
			}
		}
	}

	public void ResumeGame()
	{
		countDownLevel = true;
		levelInProgress = true;
		isGamePaused = false;

		panelBG.SetActive (false);
		pausePanel.SetActive (false);

		Time.timeScale = 1;
	}

	IEnumerator GivePlayerLiveRewardsAfterWatchingVideo()
	{
		watchVideoText.text = "Thank You...";

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (2f));

		coins = 0;
		playerLives = 2;
		smallBallsCount = 0;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		Time.timeScale = 0;

		if(LoadingScreen.instance != null)
		{
			LoadingScreen.instance.FadeOut ();
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		SceneManager.LoadScene (SceneManager.GetActiveScene().name);

		if(LoadingScreen.instance != null)
		{
			LoadingScreen.instance.PlayFadeInAnimation();
		}
	}

	public void DontWatchVideoInsteadQuit()
	{
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highScore < GameController.instance.currentScore) 
		{
			GameController.instance.highScore = GameController.instance.currentScore;
			GameController.instance.Save();
		}

		Time.timeScale = 1;

		SceneManager.LoadScene("LevelMenu");
		if(LoadingScreen.instance != null)
		{
			LoadingScreen.instance.PlayLoadingScreen();
		}
	}






}
