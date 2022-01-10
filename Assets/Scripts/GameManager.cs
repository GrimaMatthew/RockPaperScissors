using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static string intialNameP1;
    public static string intialNameP2;

    public static string keyvar;

    [SerializeField] private TMPro.TMP_InputField inpGameName;
    [SerializeField] private TMPro.TMP_InputField inpUniqueCodeToShare;
    [SerializeField] private TMPro.TMP_InputField inpUniqueCodeToJoin;
    [SerializeField] private TextMeshProUGUI player1Score;
    [SerializeField] private TextMeshProUGUI player2Score;


    public static bool activateNames;

    public static GameObject Gamename1;
    public static GameObject Gamename2;

    public static bool Player2Turn;
    public static bool Player1Turn;



    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Score != null && player2Score != null)
        {

            player1Score.text = FirebaseController.player1Points.ToString();
            player2Score.text = FirebaseController.player2Points.ToString();
        }

        if (FirebaseController.playerWon != "")
        {
            LoadScene("End");
        }
    }


    public static void LoadScene(string sSceneName)
    {
        SceneManager.LoadScene(sSceneName);
    }

    public void createNewGameInstance()
    {
        if (inpGameName.text != null)
        {
            intialNameP1 = inpGameName.text;
            StartCoroutine(FirebaseController.CreateGameInstance(inpGameName.text));
        }
    }

    public void JoinGame()
    {
        if (inpGameName.text != null)
        {
            intialNameP2 = inpGameName.text;
            FirebaseController.sGameName2 = inpGameName.text;
            LoadScene("JoinLobby");
        }
    }

    public void JoinGameLobby()
    {
        if (inpUniqueCodeToJoin.text != null)
        {
            keyvar = inpUniqueCodeToJoin.text;
            StartCoroutine(FirebaseController.ValidateKey(inpUniqueCodeToJoin.text));
            joinLobby();
        }
    }


    public void joinLobby()
    {
        LoadScene("LiveGame");
        activateNames = true; 
    }


    public static void setupNames(string name1 , string name2)
    {
        Gamename1 = GameObject.Find("Player1");
        Gamename2 = GameObject.Find("Player2");

        Gamename1.GetComponent<TextMeshProUGUI>().text = name1;
        Gamename2.GetComponent<TextMeshProUGUI>().text = name2;

    }

    public void Awake()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Start":
                break;

            case "LobbyKey":
                inpUniqueCodeToShare.text = FirebaseController.sUniqueKey;
                break;

            case "LiveGame":
               
                break;

            case "End":
                GameObject WinnerText = GameObject.Find("WinnerTxt");
                if (FirebaseController.playerWon == "p1")
                {
                    WinnerText.GetComponent<TextMeshProUGUI>().text = "Winner Player 1";
                }
                else if (FirebaseController.playerWon == "p2")
                {
                    WinnerText.GetComponent<TextMeshProUGUI>().text = "Winner Player 2";
                }
                break;

            default:
                break;
        }
    }



}
