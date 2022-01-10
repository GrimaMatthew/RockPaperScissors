using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    string intialNameEntered;
    string nameInFirebase;
    // Start is called before the first frame update
    void Start()
    {
        intialNameEntered = GameManager.intialNameP2;
        nameInFirebase = FirebaseController.sGameName2;

    }

    // Update is called once per frame
    void Update()
    {
        if (intialNameEntered == nameInFirebase)
        {
            if (FirebaseController.playerTurn == "p2")
            {
                GameObject.Find("WFP").GetComponent<TextMeshProUGUI>().text = "Your Turn";
            }
            else if (FirebaseController.playerTurn == "p1")
            {
                GameObject.Find("WFP").GetComponent<TextMeshProUGUI>().text = "Wait For Other Player To Play";
            }
        }


        if (intialNameEntered == nameInFirebase && Input.GetMouseButtonDown(0) &&  FirebaseController.playerTurn == "p2")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
               
            if (Physics.Raycast(ray, out hit))
                {
                GameObject tempGameObject = GameObject.Find(hit.transform.gameObject.name);

                if (tempGameObject.tag == "Scissor" || tempGameObject.tag == "Rock" || tempGameObject.tag == "Paper")
                {
                    Debug.Log(tempGameObject.tag + "TAG");
                    GameManager.Player1Turn = true;
                    GameManager.Player2Turn = false;
                    FirebaseController.player2CurrentOption = tempGameObject.tag;
                    StartCoroutine(FirebaseController.SetOptionP2());

                }
                else
                {
                    Debug.Log("Not VALID");
                }
            }
        }
        

    }
}
