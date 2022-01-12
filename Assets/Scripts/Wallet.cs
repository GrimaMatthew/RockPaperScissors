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
    public static float byteCount = 145;
    public static float percent;

    // Start is called before the first frame update
    void Start()
    {
        playerCoins = 88900;
  

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
                StorageReference sRefBox = storeRef.Child("DLC").Child("box.png");

                for (int i = 0; i <= byteCount; i++)
                {
                    
                    downloadDlcSideBar(sRefBox);
                    PS1_loader(percent);
                    Debug.Log("INLOOP");
                    if (slider1.value < slider1Target)
                    {
                        if (slider1.value != 1)
                        {
                            slider1.value += fillSpeed * Time.deltaTime;
                        }
                       

                    }

                }

                Debug.Log(slider1Target + "SliderTarget");
                Debug.Log(slider1.value + "SliderTarget");

              
                fslider1 = false;
            }
        }

        if (fslider2)
        {
            if (slider2.value < slider2Target)
            {
                slider2.value += fillSpeed * Time.deltaTime;
                fslider2 = true;
            }
        }
        if (slider3)
        {
            if (slider3.value < slider3Target)
            {
                slider3.value += fillSpeed * Time.deltaTime;
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


    public static void downloadDlcSideBar(StorageReference refe )
    {
        const long maxAllowedSize = 1 * 2024 * 2024;

        refe.GetBytesAsync(maxAllowedSize,
            new StorageProgress<DownloadState>(state =>
            {
                Debug.Log(string.Format("Progress: {0} of {1} bytes transferred.",
                     state.BytesTransferred
                    ,state.TotalByteCount
                    ));

                byteTransferred = 100;
                byteCount = state.TotalByteCount;

                Debug.Log(byteTransferred + "byteTransfered");
                Debug.Log(byteCount + "byte Count");
                percent = ((byteTransferred/byteCount) * 100);
                Debug.Log(percent + "PERCETN");
            }));

     

    }
}
