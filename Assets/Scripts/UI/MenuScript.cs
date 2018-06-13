using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public enum MenuState
    {
        MainMenu,
        OptionsMenu,
        StoryMenu,
        OnlineMenu
    };

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject storyMenu;
    public GameObject onlineMenu;

    public InputField text;
	public InputField NetIpAddress;
	public Button hostGameButton;
	public Button joinGameButton;
	public Button startGameButton;
    public SpriteChooser chooser;

    public MenuState current;

    void Awake()
    {
        current = MenuState.MainMenu;
    }

    void Update()
    {
        switch (current)
        {
            case MenuState.MainMenu:
                mainMenu.SetActive(true);
                optionsMenu.SetActive(false);
                storyMenu.SetActive(false);
                onlineMenu.SetActive(false);
                break;
            case MenuState.OptionsMenu:
                mainMenu.SetActive(false);
                optionsMenu.SetActive(true);
                storyMenu.SetActive(false);
                onlineMenu.SetActive(false);
                break;
            case MenuState.StoryMenu:
                mainMenu.SetActive(false);
                optionsMenu.SetActive(false);
                storyMenu.SetActive(true);
                onlineMenu.SetActive(false);
                break;
            case MenuState.OnlineMenu:
                mainMenu.SetActive(false);
                optionsMenu.SetActive(false);
                storyMenu.SetActive(false);
                onlineMenu.SetActive(true);
                break;
        }
    }

    public void OnMainMenu()
    {
        current = MenuState.MainMenu;
    }

    public void OnOptionsMenu()
    {
        current = MenuState.OptionsMenu;

        LoadPlayerToOptions();
    }

    public void OnStoryMenu()
    {
        current = MenuState.StoryMenu;
    }

    public void OnOnlineMenu()
    {
        current = MenuState.OnlineMenu;
    }

    public void SaveToPlayer()
    {
        if (text.text.Length > 0)
            LocalPlayerInfo.self.Name = text.text;
        LocalPlayerInfo.self.Emblem = chooser.selected;

        LocalPlayerInfo.self.Save();
    }

    public void LoadPlayerToOptions()
    {
        text.text = LocalPlayerInfo.self.Name;
        chooser.SelectSprite(LocalPlayerInfo.self.Emblem);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    // Story mode menu
    public void OnStoryLevel(int levelId)
    {
        SceneManager.LoadScene("Scene");
    }

	// Online mode create server
	public void OnNetInit()
	{
		var netHandler = NetHandler.Self;
		netHandler.RegisterHandler(MsgType.Connect, msg =>
			{
				Debug.Log("Joined local server");
				netHandler.Join();
			});
		netHandler.RegisterHandler(BwMsgTypes.InitScene, msg =>
			{
				Debug.Log("Loading Scene");
				SceneManager.LoadScene("Scene");
			});
		netHandler.Config("127.0.0.1", 7777, true);

		startGameButton.interactable = true;
		hostGameButton.interactable = false;
		joinGameButton.interactable = false;
		NetIpAddress.enabled = false;
	}

    // Online mode join server
    public void OnNetJoin()
    {
        var netHandler = NetHandler.Self;
        netHandler.RegisterHandler(MsgType.Connect, msg =>
        {
            Debug.Log("Connected to " + netHandler.IpAddress + " at port 7777");
            netHandler.Join();
        });
        netHandler.RegisterHandler(BwMsgTypes.InitScene, msg =>
        {
            Debug.Log("Loading Scene");
            SceneManager.LoadScene("Scene");
        });
        netHandler.Config(NetIpAddress.text, 7777, false);

		startGameButton.interactable = false;
		hostGameButton.interactable = false;
		joinGameButton.interactable = false;
		NetIpAddress.enabled = false;
    }

    public void OnNetStart()
    {
        var netHandler = NetHandler.Self;
        if (netHandler.IsHost)
        {
            netHandler.LoadScene();
        }
    }
}