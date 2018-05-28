using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenuController : MonoBehaviour 
{
	public static ShopMenuController instance;

	// score and coin text
	public Text coinText, scoreText, buyArrowsText, watchVideoText;

	// reference to buttons
	public Button weaponTabBtn, specialTabBtn, earnCoinsTabBtn, yesBtn;

	public GameObject weaponItemsPanel, specialItemsPanel, earnCoinsItemsPanel, coinShopPanel, buyArrowsPanel;


	void Awake()
	{
		MakeInstance ();
	}
		
	void Start () 
	{
		InitializeShopMenuController ();
	}

	void MakeInstance()
	{
		if (instance == null) 
		{
			instance = this;
		}
	}

	public void BuyDoubleArrows()
	{
		if (!GameController.instance.weapons [1]) 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do You Want To Purchase Double Arrows?";

				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (1));
			} else 
			{
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You Do Not Have Enough Coins. Do You Want to Buy Coins?";

				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void BuyStickyArrows()
	{
		if (!GameController.instance.weapons [2]) 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do You Want To Purchase Sticky Arrows?";

				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (2));
			} else 
			{
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You Do Not Have Enough Coins. Do You Want to Buy Coins?";

				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void BuyDoubleStickyArrows()
	{
		if (!GameController.instance.weapons [3]) 
		{
			if (GameController.instance.coins >= 7000) 
			{
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do You Want To Purchase Double Sticky Arrows?";

				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (3));
			} else 
			{
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You Do Not Have Enough Coins. Do You Want to Buy Coins?";

				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop());
			}
		}
	}

	public void BuyArrow(int index)
	{
		GameController.instance.weapons[index] = true;
		GameController.instance.coins -= 7000;
		GameController.instance.Save ();

		buyArrowsPanel.SetActive (false);
		coinText.text = "" + GameController.instance.coins;
	}

	void InitializeShopMenuController()
	{
		coinText.text = "" + GameController.instance.coins;
		scoreText.text = "" + GameController.instance.highScore;
	}

	public void OpenCoinShop()
	{
		if (buyArrowsPanel.activeInHierarchy) 
		{
			buyArrowsPanel.SetActive (false);
		}
		
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop()
	{
		coinShopPanel.SetActive (false);
	}

	public void OpenWeaponItemsPanel()
	{
		weaponItemsPanel.SetActive (true);
		specialItemsPanel.SetActive (false);
		earnCoinsItemsPanel.SetActive (false);
	}

	public void OpenSpecialItemsPanel()
	{
		specialItemsPanel.SetActive (true);
		weaponItemsPanel.SetActive (false);
		earnCoinsItemsPanel.SetActive (false);
	}

	public void OpenEarnCoinItemsPanel()
	{
		earnCoinsItemsPanel.SetActive (true);
		weaponItemsPanel.SetActive (false);
		specialItemsPanel.SetActive (false);
	}

	public void PlayGame()
	{
		SceneManager.LoadScene ("PlayerMenu");
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void DontBuyArrows()
	{
		buyArrowsPanel.SetActive (false);
	}
}
