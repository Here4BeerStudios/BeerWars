using UnityEditor.Experimental.UIElements.GraphView;
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
    public InputField NetPort;
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

    // Online mode menu
    public void OnNetInit()
    {
        var netHandler = NetHandler.Self;
        netHandler.RegisterHandler(MsgType.Connect, msg =>
        {
            Debug.Log("Connected to " + netHandler.IpAddress + " at port " + netHandler.Port);
            netHandler.Join();
        });
        netHandler.Config(NetIpAddress.text,int.Parse(NetPort.text));
    }

    public void OnNetStart()
    {
        var netHandler = NetHandler.Self;
        if (netHandler.IsHost)
        {
            SceneManager.LoadScene("Scene");
            netHandler.StartGame();
        }
    }
}