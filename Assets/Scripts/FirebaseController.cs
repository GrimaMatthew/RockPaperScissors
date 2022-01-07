using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using System;



[SerializeField]
public class cls_GameLobby
{
    public string gameNameplr1, gameNameplr2,  DateTimeCreated, player1Option, player2Option , playerTurn ,p1Points ,p2Points , numRounds, playerWon;

    public cls_GameLobby(string gName1, string gName2,  string dTCreated, string p1Op, string p2Op , string pTurn , string p1Score, string p2Score , string nRounds , string pWon)
    {
        this.gameNameplr1 = gName1;
        this.gameNameplr2 = gName2;
        this.DateTimeCreated = dTCreated;
        this.player1Option = p1Op;
        this.player2Option = p2Op;
        this.playerTurn = pTurn;
        this.p1Points = p1Score;
        this.p2Points = p2Score;
        this.numRounds = nRounds;
        this.playerWon = pWon;
    }

}



public class FirebaseController : MonoBehaviour
{

    //Reference to database
    private static DatabaseReference dbRef;

    //Unique Key generated by firebase
    public static string sUniqueKey = "";

    //Names chosen for games by players
    public static string sGameName1 = "";

    public static string sGameName2 = "";

    public static DateTime now = DateTime.Now;

    public static string player1Option = ""; //replace pos with the item chosen by the user

    public static string player2Option= "";

    public static string player1CurrentOption = "";

    public static string player2CurrentOption = "";

    public static string playerWinner = "";

    public static string playerTurn = "";

    private int namecounter = 0;

    public static  int optionCounter = 0;

    public static int player1Points = 0;

    public static int player2Points = 0;

    public static int roundCounter = 0;

    public static string playerWon = "";



    public static IEnumerator CreateGameInstance(string sGName1)
    {


        //Intialing the first game name
        sGameName1 = sGName1;

        //Creates a unique key by firebase and sets objects as one of the children
        sUniqueKey = dbRef.Child("Objects").Push().Key;

        //Intialising the lobby
        cls_GameLobby lobby = new cls_GameLobby(sGName1, "", now.ToString(), player1Option, player2Option , playerTurn ,player1Points.ToString(), player2Points.ToString(), roundCounter.ToString(), playerWon);

        //puts the key as a child of the object  
        dbRef.Child("Objects").Child(sUniqueKey).ValueChanged += HandleValueChanged;


        yield return dbRef.Child("Objects").Child(sUniqueKey).SetRawJsonValueAsync(JsonUtility.ToJson(lobby));
        GameManager.LoadScene("LobbyKey");

        Debug.Log("Unique Key Generated by Firebase" + sUniqueKey);

    }


    public static void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        else
        {

            foreach (var child in args.Snapshot.Children)
            {
                if (child.Key == "gameNameplr2")
                {
                    sGameName2 = child.Value.ToString();
                }

                if (child.Key == "gameNameplr1")
                {
                    sGameName1 = child.Value.ToString();
                }
                if (child.Key == "p1Points")
                {
                    player1Points = int.Parse(child.Value.ToString());
                }

                if (child.Key == "p2Points")
                {
                    player2Points = int.Parse(child.Value.ToString());
                }

                if (child.Key == "player1Option")
                {
                    player1CurrentOption = child.Value.ToString();
                }

                if (child.Key == "player2Option")
                {
                    player2CurrentOption = child.Value.ToString();
                }

                if (child.Key == "playerTurn")
                {
                    playerTurn = child.Value.ToString();
                }

                if (child.Key == "numRounds")
                {
                    roundCounter = int.Parse(child.Value.ToString());
                }
                if (child.Key == "playerWon")
                {
                    playerWon = child.Value.ToString();
                }
             

            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        playerTurn = "p1";
    }

    // Update is called once per frame
    void Update()
    {


        //Leve it runing until GameNameP2 != ""

        if (sGameName2 != "" && sGameName1 != "")
        {
            if (namecounter == 0)
            {
                GameManager.setupNames(sGameName1,sGameName2);
                namecounter++;

            }
        }

        if (GameManager.Player2Turn == true)
        {
            GameManager.Player1Turn = false;
            dbRef.Child("Objects").Child(sUniqueKey).Child("playerTurn").SetValueAsync("p2");
            GameManager.Player2Turn = false ;
        }

        if (GameManager.Player1Turn == true)
        {
            Debug.Log("P1 TURN TRUE");
            GameManager.Player2Turn = false;
            dbRef.Child("Objects").Child(sUniqueKey).Child("playerTurn").SetValueAsync("p1");
            GameManager.Player1Turn = false;
        }

    

  


        if (player1CurrentOption != "" && player2CurrentOption != "")
        {
            roundCounter+=1;
            dbRef.Child("Objects").Child(sUniqueKey).Child("numRounds").SetValueAsync(roundCounter);
            Debug.Log("ROUND COUNTER" + roundCounter);

            if (player1CurrentOption == player2CurrentOption)
            {
                Debug.Log("TIEE");
                Debug.Log("UNIQUE:" + sUniqueKey);
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);
            }

            else if (player1CurrentOption == "Rock" && player2CurrentOption == "Paper")
            {
                player2Points++;
                Debug.Log("PLAYER 2 POINTS: " + player2Points );
                dbRef.Child("Objects").Child(sUniqueKey).Child("p2Points").SetValueAsync(player2Points.ToString());
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);
            }

            else if (player1CurrentOption == "Rock" && player2CurrentOption == "Scissor")
            {
                player1Points++;
                dbRef.Child("Objects").Child(sUniqueKey).Child("p1Points").SetValueAsync(player1Points.ToString());
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);
            }
            else if (player1CurrentOption == "Scissor" && player2CurrentOption == "Paper")
            {
               player1Points++;
                dbRef.Child("Objects").Child(sUniqueKey).Child("p1Points").SetValueAsync(player1Points.ToString());
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);
            }
            else if (player1CurrentOption == "Scissor" && player2CurrentOption == "Rock")
            {
                player2Points++;
                Debug.Log("PLAYER 2 POINTS: " + player2Points);
                dbRef.Child("Objects").Child(sUniqueKey).Child("p2Points").SetValueAsync(player2Points.ToString());
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);

            }
            else if (player1CurrentOption == "Paper" && player2CurrentOption == "Rock")
            {
                player1Points++;
                dbRef.Child("Objects").Child(sUniqueKey).Child("p1Points").SetValueAsync(player1Points.ToString());
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);

            }
            else if (player1CurrentOption == "Paper" && player2CurrentOption == "Scissor")
            {
                player2Points++;
                Debug.Log("PLAYER 2 POINTS: " + player2Points);
                dbRef.Child("Objects").Child(sUniqueKey).Child("p2Points").SetValueAsync(player2Points.ToString());
                player1CurrentOption = "";
                player2CurrentOption = "";
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);

            }

        }

        if (roundCounter == 5)
        {
            if (player2Points > player1Points)
            {
                playerWon = "p2";
                dbRef.Child("Objects").Child(sUniqueKey).Child("playerWon").SetValueAsync(playerWon);
            }
            else if (player2Points < player1Points)
            {
                playerWon = "p1";
                dbRef.Child("Objects").Child(sUniqueKey).Child("playerWon").SetValueAsync(playerWon);
            }
            else if (player2Points == player1Points)
            {
                playerWon = "Tie";
                dbRef.Child("Objects").Child(sUniqueKey).Child("playerWon").SetValueAsync(playerWon);
                
            }
        }
    
        

    }


    public static IEnumerator ValidateKey(string uniqKey)
    {
        

        yield return dbRef.Child("Objects").Child(uniqKey).GetValueAsync().ContinueWith(task =>
        {

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Value != null)
                {
                    Debug.Log("Key Match");
                    sUniqueKey = uniqKey;

                    foreach (var child in snapshot.Children)
                    {
                        if (child.Key == "gameNameplr1")
                        {
                            sGameName1 = child.Value.ToString();
                        }
                    }

                    AddPlayersToLobby(sGameName1, sGameName2, uniqKey);
                }
                dbRef.Child("Objects").Child(uniqKey).ValueChanged += HandleValueChanged;

            }
        });
    }


    public static IEnumerator SetOptionP1()
    {
        if (player1CurrentOption != null && player1CurrentOption != "")
        {
            if (optionCounter == 0)
            {
                dbRef.Child("Objects").Child(sUniqueKey).Child("player1Option").SetValueAsync(player1CurrentOption);
                optionCounter++;
                yield return null;
            }
        }

    }

    public static IEnumerator SetOptionP2()
    {
        if (player2CurrentOption != null && player2CurrentOption != "")
        {

            dbRef.Child("Objects").Child(sUniqueKey).Child("player2Option").SetValueAsync(player2CurrentOption);
            yield return null;
        }
    }


        public static void AddPlayersToLobby(string gameName1, string gameName2, string key)
    {

        cls_GameLobby GameLobby = new cls_GameLobby(gameName1, gameName2, now.ToString(), player1Option, player2Option ,playerTurn,player1Points.ToString(),player2Points.ToString(),roundCounter.ToString(),playerWon);
        dbRef.Child("Objects").Child(key).SetRawJsonValueAsync(JsonUtility.ToJson(GameLobby));
    }
}
