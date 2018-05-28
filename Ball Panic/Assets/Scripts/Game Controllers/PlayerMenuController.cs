using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerMenuController : MonoBehaviour 
{
	// score and coin text
	public Text scoreText, coinText;

	// keeping track of which characters are unlocked
	public bool[] players;

	// keeping track of which weapons are unlocked
	public bool[] weapons;

	// reference to price tags in the scene
	public Image[] priceTags;

	// reference to weapon icons in the scene
	public Image[] weaponIcons;

	// reference to weapon sprites to change weapon icons in the scene
	public Sprite[] weaponArrows;

	// keeping track of current selected weapon
	public int selectedWeapon;

	// keeping track of current selected character
	public int selectedPlayer;

	// referece to buy a character player panel in the scene
	public GameObject buyPlayerPanel;

	// referece to yes button from buy player panel
	public Button yesBtn;

	// referece to buy player text
	public Text buyPlayerText;

	public GameObject coinShop;


	// Use this for initialization
	void Start () 
	{
		InitializePlayerMenuController ();
	}

	void InitializePlayerMenuController()
	{
		scoreText.text = "" + GameController.instance.highScore;
		coinText.text = "" + GameController.instance.coins;

		players = GameController.instance.players;
		weapons = GameController.instance.weapons;

		selectedPlayer = GameController.instance.selectedPlayer;
		selectedWeapon = GameController.instance.selectedWeapon;

		for (int i = 0; i < weaponIcons.Length; i++) 
		{
			weaponIcons [i].gameObject.SetActive (false);
		}

		for (int i = 1; i < players.Length; i++) 
		{
			if (players [i] == true) 
			{
				priceTags [i - 1].gameObject.SetActive (false);
			}
		}

		weaponIcons [selectedPlayer].gameObject.SetActive (true);
		weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
	}

	public void Player1Button()
	{
		if (selectedPlayer != 0) 
		{
			selectedPlayer = 0;
			selectedWeapon = 0;

			weaponIcons [selectedPlayer].gameObject.SetActive (true);
			weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

			for (int i = 0; i < weaponIcons.Length; i++) 
			{
				if (i == selectedPlayer) 
				{
					continue;
				}

				weaponIcons [i].gameObject.SetActive (false);
			}

			GameController.instance.selectedPlayer = selectedPlayer;
			GameController.instance.selectedWeapon = selectedWeapon;
			GameController.instance.Save ();
		}

		else 
		{
			selectedWeapon++;

			if (selectedWeapon == weapons.Length) 
			{
				selectedWeapon = 0;
			}

			bool foundWeapon = true;

			while (foundWeapon) 
			{
				if (weapons [selectedWeapon] == true) 
				{
					weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
					GameController.instance.selectedWeapon = selectedWeapon;
					GameController.instance.Save ();
					foundWeapon = false;
				} 

				else 
				{
					selectedWeapon++;

					if (selectedWeapon == weapons.Length) 
					{
						selectedWeapon = 0;
					}
				}
			}
		}
	}

	public void PirateButton()
	{
		if (players [1] == true) 
		{
			if (selectedPlayer != 1) 
			{
				selectedPlayer = 1;
				selectedWeapon = 0;

				weaponIcons [selectedPlayer].gameObject.SetActive (true);
				weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

				for (int i = 0; i < weaponIcons.Length; i++) 
				{
					if (i == selectedPlayer) 
					{
						continue;
					}

					weaponIcons [i].gameObject.SetActive (false);
				}

				GameController.instance.selectedPlayer = selectedPlayer;
				GameController.instance.selectedWeapon = selectedWeapon;
				GameController.instance.Save ();
			} else 
			{
				selectedWeapon++;

				if (selectedWeapon == weapons.Length) 
				{
					selectedWeapon = 0;
				}

				bool foundWeapon = true;

				while (foundWeapon) 
				{
					if (weapons [selectedWeapon] == true) 
					{
						weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
						GameController.instance.selectedWeapon = selectedWeapon;
						GameController.instance.Save ();
						foundWeapon = false;
					} else 
					{
						selectedWeapon++;

						if (selectedWeapon == weapons.Length) 
						{
							selectedWeapon = 0;
						}
					}
				}
			}
		} else 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "Do You Want to Purchase This Awesome Character?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyPlayer (1));

			} else 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "You Do Not Have Enough Coins. Do You Want to Purchase Coins?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void ZombieButton()
	{
		if (players [2] == true) 
		{
			if (selectedPlayer != 2) 
			{
				selectedPlayer = 2;
				selectedWeapon = 0;

				weaponIcons [selectedPlayer].gameObject.SetActive (true);
				weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

				for (int i = 0; i < weaponIcons.Length; i++) 
				{
					if (i == selectedPlayer) 
					{
						continue;
					}

					weaponIcons [i].gameObject.SetActive (false);
				}

				GameController.instance.selectedPlayer = selectedPlayer;
				GameController.instance.selectedWeapon = selectedWeapon;
				GameController.instance.Save ();
			} else 
			{
				selectedWeapon++;

				if (selectedWeapon == weapons.Length) 
				{
					selectedWeapon = 0;
				}

				bool foundWeapon = true;

				while (foundWeapon) 
				{
					if (weapons [selectedWeapon] == true) 
					{
						weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
						GameController.instance.selectedWeapon = selectedWeapon;
						GameController.instance.Save ();
						foundWeapon = false;
					} else 
					{
						selectedWeapon++;

						if (selectedWeapon == weapons.Length) 
						{
							selectedWeapon = 0;
						}
					}
				}
			}
		} else 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "Do You Want to Purchase This Awesome Character?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyPlayer (2));

			} else 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "You Do Not Have Enough Coins. Do You Want to Purchase Coins?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void HomosapienButton()
	{
		if (players [3] == true) 
		{
			if (selectedPlayer != 3) 
			{
				selectedPlayer = 3;
				selectedWeapon = 0;

				weaponIcons [selectedPlayer].gameObject.SetActive (true);
				weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

				for (int i = 0; i < weaponIcons.Length; i++) 
				{
					if (i == selectedPlayer) 
					{
						continue;
					}

					weaponIcons [i].gameObject.SetActive (false);
				}

				GameController.instance.selectedPlayer = selectedPlayer;
				GameController.instance.selectedWeapon = selectedWeapon;
				GameController.instance.Save ();
			} else 
			{
				selectedWeapon++;

				if (selectedWeapon == weapons.Length) 
				{
					selectedWeapon = 0;
				}

				bool foundWeapon = true;

				while (foundWeapon) 
				{
					if (weapons [selectedWeapon] == true) 
					{
						weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
						GameController.instance.selectedWeapon = selectedWeapon;
						GameController.instance.Save ();
						foundWeapon = false;
					} else 
					{
						selectedWeapon++;

						if (selectedWeapon == weapons.Length) 
						{
							selectedWeapon = 0;
						}
					}
				}
			}
		} else 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "Do You Want to Purchase This Awesome Character?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyPlayer (3));

			} else 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "You Do Not Have Enough Coins. Do You Want to Purchase Coins?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void JokerButton()
	{
		if (players [4] == true) 
		{
			if (selectedPlayer != 4) 
			{
				selectedPlayer = 4;
				selectedWeapon = 0;

				weaponIcons [selectedPlayer].gameObject.SetActive (true);
				weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

				for (int i = 0; i < weaponIcons.Length; i++) 
				{
					if (i == selectedPlayer) 
					{
						continue;
					}

					weaponIcons [i].gameObject.SetActive (false);
				}

				GameController.instance.selectedPlayer = selectedPlayer;
				GameController.instance.selectedWeapon = selectedWeapon;
				GameController.instance.Save ();
			} else 
			{
				selectedWeapon++;

				if (selectedWeapon == weapons.Length) 
				{
					selectedWeapon = 0;
				}

				bool foundWeapon = true;

				while (foundWeapon) 
				{
					if (weapons [selectedWeapon] == true) 
					{
						weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
						GameController.instance.selectedWeapon = selectedWeapon;
						GameController.instance.Save ();
						foundWeapon = false;
					} else 
					{
						selectedWeapon++;

						if (selectedWeapon == weapons.Length) 
						{
							selectedWeapon = 0;
						}
					}
				}
			}
		} else 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "Do You Want to Purchase This Awesome Character?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyPlayer (4));

			} else 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "You Do Not Have Enough Coins. Do You Want to Purchase Coins?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void SpartanButton()
	{
		if (players [5] == true) 
		{
			if (selectedPlayer != 5) 
			{
				selectedPlayer = 5;
				selectedWeapon = 0;

				weaponIcons [selectedPlayer].gameObject.SetActive (true);
				weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

				for (int i = 0; i < weaponIcons.Length; i++) 
				{
					if (i == selectedPlayer) 
					{
						continue;
					}

					weaponIcons [i].gameObject.SetActive (false);
				}

				GameController.instance.selectedPlayer = selectedPlayer;
				GameController.instance.selectedWeapon = selectedWeapon;
				GameController.instance.Save ();
			} else 
			{
				selectedWeapon++;

				if (selectedWeapon == weapons.Length) 
				{
					selectedWeapon = 0;
				}

				bool foundWeapon = true;

				while (foundWeapon) 
				{
					if (weapons [selectedWeapon] == true) 
					{
						weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
						GameController.instance.selectedWeapon = selectedWeapon;
						GameController.instance.Save ();
						foundWeapon = false;
					} else 
					{
						selectedWeapon++;

						if (selectedWeapon == weapons.Length) 
						{
							selectedWeapon = 0;
						}
					}
				}
			}
		} else 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "Do You Want to Purchase This Awesome Character?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyPlayer (5));

			} else 
			{
				buyPlayerPanel.SetActive (true);
				buyPlayerText.text = "You Do Not Have Enough Coins. Do You Want to Purchase Coins?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}



	public void BuyPlayer(int index)
	{
		GameController.instance.players [index] = true;
		GameController.instance.coins -= 7000;
		GameController.instance.Save ();
		InitializePlayerMenuController ();

		buyPlayerPanel.SetActive (false);
	}

	public void OpenCoinShop()
	{
		if (buyPlayerPanel.activeInHierarchy) 
		{
			buyPlayerPanel.SetActive (false);
		}

		coinShop.SetActive (true);
	}

	public void CloseCoinShop()
	{
		coinShop.SetActive (false);
	}

	public void DontBuyPlayerOrCoins()
	{
		buyPlayerPanel.SetActive (false);
	}
	
	public void GoToLevelMenu()
	{
		SceneManager.LoadScene ("LevelMenu");
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
