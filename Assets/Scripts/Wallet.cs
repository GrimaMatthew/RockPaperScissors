using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Storage;
using Firebase.Extensions;


public class Wallet : MonoBehaviour
{
    public GameObject walletWindow;
    public static bool windowSet = false;

    public int playerCoins;
    public TextMeshProUGUI coinTxt;

    public Slider slider1;
    public Slider slider2;
    public Slider slider3;

    public float fillSpeed = 0.5f;

    private float slider1Target = 0f;
    private float slider2Target = 0f;
    private float slider3Target = 0f;

    public bool fslider1 = false;
    public bool fslider2 = false;
    public bool fslider3 = false;


    public static float byteTransferred;
    public static float byteCount ;
    public static float percent;

    public bool loadbar1 = false;
    public bool loadbar2 = false;
    public bool loadbar3 = false;



    // Start is called before the first frame update
    void Start()
    {
        playerCoins = 2000;
  

    }

    // Update is called once per frame
    void Update()
    {
        coinTxt.text = playerCoins.ToString();
   
        if (fslider1)
        {
            if (playerCoins >= 200)
            {
                playerCoins = playerCoins - 200;

                FirebaseStorage storage = FirebaseStorage.DefaultInstance;
                StorageReference storeRef = storage.GetReferenceFromUrl("gs://rockpaper-89f8c.appspot.com");
                StorageReference sRefBox = storeRef.Child("DLC").Child("box_black.png");

                downloadDlcSideBar(sRefBox , "slider1");
                fslider1 = false;
                
            }
        }

        if (loadbar1)
        {
            Debug.Log("INLOADER");
            slider1.value += fillSpeed * Time.deltaTime;
            
            if (slider1.value == 1)
            {
                loadbar1 = false;
                slider1.value = 0;
            }
        }

        if (fslider2)
        {
            if (playerCoins >= 200)
            {
                playerCoins = playerCoins - 200;

                FirebaseStorage storage = FirebaseStorage.DefaultInstance;
                StorageReference storeRef = storage.GetReferenceFromUrl("gs://rockpaper-89f8c.appspot.com");
                StorageReference sRefBox = storeRef.Child("DLC").Child("box_black.png");

                downloadDlcSideBar(sRefBox , "slider2");
                fslider2 = false;

            }
           
        }

        if (loadbar2)
        {
            slider2.value += fillSpeed * Time.deltaTime;

            if (slider2.value == 1)
            {
                loadbar2 = false;
                slider2.value = 0;
            }
        }

        if (fslider3)
        {
            if (playerCoins >= 600)
            {
                playerCoins = playerCoins - 600;

                FirebaseStorage storage = FirebaseStorage.DefaultInstance;
                StorageReference storeRef = storage.GetReferenceFromUrl("gs://rockpaper-89f8c.appspot.com");
                StorageReference sRefBox = storeRef.Child("DLC").Child("box_black.png");

                downloadDlcSideBar(sRefBox, "slider3");
                fslider3 = false;
            }
        }

        if (loadbar3)
        {
            slider3.value += fillSpeed * Time.deltaTime;

            if (slider3.value == 1)
            {
                loadbar3 = false;
                slider3.value = 0;
            }
        }

    }

    public void setWindowWallet()
    {
        if (windowSet)
        {
            walletWindow.SetActive(true);

        }
        else if (!windowSet)
        {
            walletWindow.SetActive(false);

        }
    }

    public void setTrueWindow()
    {
        windowSet = true;
        setWindowWallet();
    }

    public void setFalseWindow()
    {
        windowSet = false;
        setWindowWallet();
    }

    public void firstSliderTrue()
    {
        fslider1 = true;
    }
    public void sedondSliderTrue()
    {
        fslider2 = true;
    }
    public void thirdSliderTrue()
    {
        fslider3 = true;
    }

    public void PS1_loader(float increaseProg)
    {
        Debug.Log(slider1.value + "SliderVALUE");
        Debug.Log(slider1Target + "SLIDER1TARGET");
        Debug.Log(increaseProg + "IncreaseProg");
        slider1Target = slider1.value + increaseProg;
    }
    public void PS2_loader(float increaseProg)
    {
        slider2Target = slider2.value + increaseProg;
    }
    public void PS3_loader(float increaseProg)
    {
        slider3Target = slider3.value + increaseProg;
    }


    public  void downloadDlcSideBar(StorageReference refe , string sliderChoice)
    {
        const long maxAllowedSize = 1 * 2024 * 2024;

        refe.GetBytesAsync(maxAllowedSize,
            new StorageProgress<DownloadState>(state =>
            {
                Debug.Log(string.Format("Progress: {0} of {1} bytes transferred.",
                     state.BytesTransferred,
                     state.TotalByteCount
                    ));

                byteTransferred = state.BytesTransferred;
                byteCount = state.TotalByteCount;

            
                percent = ((byteTransferred/byteCount) * 100);


                if (sliderChoice == "slider1")
                {
                    PS1_loader(percent);

                    if (slider1.value < slider1Target)
                    {

                        loadbar1 = true;

                    }
                    else
                    {
                        Debug.Log("DONE");
                    }
                }

                else if (sliderChoice == "slider2")
                {
                    PS2_loader(percent);
                    if (slider2.value < slider2Target)
                    {
                        loadbar2 = true;
                    }
                    else
                    {
                        Debug.Log("DONE");
                    }
                }
                else if (sliderChoice == "slider3")
                {
                    PS3_loader(percent);
                    if (slider3.value < slider3Target)
                    {
                        loadbar3 = true;
                    }
                    else
                    {
                        Debug.Log("DONE");
                    }
                }

              

            }));

     

    }
}
