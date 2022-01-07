using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player1Controller : MonoBehaviour
{
    string intialNameEntered;
    string nameInFirebase;
    // Start is called before the first frame update
    void Start()
    {
        intialNameEntered = GameManager.intialNameP1;
        nameInFirebase = FirebaseController.sGameName1;

        if (intialNameEntered == nameInFirebase)
        {
            Debug.Log("NamesMatched");
            if (FirebaseController.playerTurn == "p1")
            {
                Debug.Log("P1 TURN");
                GameObject.Find("WFP").GetComponent<TextMeshProUGUI>().text = "Your Turn";
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (intialNameEntered == nameInFirebase)
        {

            if (FirebaseController.playerTurn == "p1")
            {
                GameObject.Find("WFP").GetComponent<TextMeshProUGUI>().text = "Your Turn";
            }
            else if (FirebaseController.playerTurn == "p2")
            {
                GameObject.Find("WFP").GetComponent<TextMeshProUGUI>().text = "Wait For Other Player To Play";
            }
        }

        if (intialNameEntered == nameInFirebase && Input.GetMouseButtonDown(0) && FirebaseController.playerTurn == "p1")
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                FirebaseController.optionCounter = 0;
                GameObject tempGameObject = GameObject.Find(hit.transform.gameObject.name);

                if (tempGameObject.tag == "Scissor" || tempGameObject.tag == "Rock" || tempGameObject.tag=="Paper")
                {
                    Debug.Log(tempGameObject.tag + "TAG");
                    GameManager.Player1Turn = false; 
                    GameManager.Player2Turn = true;
                    FirebaseController.player1CurrentOption = tempGameObject.tag;
                    StartCoroutine(FirebaseController.SetOptionP1());
                }
                else
                {
                    Debug.Log("Not VALID");
                }
            }
        }

    }

 
   
}
