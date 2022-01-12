using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Storage;

public class DLCManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] fileNames = { "box.png" , "box_black.png" , "box_lines.png"};
        int randBack = Random.Range(0, 3);
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storeRef = storage.GetReferenceFromUrl("gs://rockpaper-89f8c.appspot.com");
        Debug.Log(randBack + "RANDOMNUM");
        StorageReference imagePla = storeRef.Child("DLC").Child(fileNames[randBack]);
        GameObject playerIcon = new GameObject();
        playerIcon.transform.parent = GameObject.Find("DLCManager").transform;
        playerIcon.AddComponent<SpriteRenderer>();
        DownloadDLC(imagePla, playerIcon);

    }

    public static void DownloadDLC(StorageReference reference, GameObject playericon)
    {
        const long maxSize = 1 * 1024 * 1024;
        reference.GetBytesAsync(maxSize).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogException(task.Exception);
            }
            else
            {
                byte[] fileContent = task.Result;
                Texture2D tex = new Texture2D(1024, 1024);
                tex.LoadImage(fileContent);
                Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                playericon.GetComponent<SpriteRenderer>().sprite = mySprite;
                playericon.transform.localScale = new Vector3(10f, 10f, 0f);
            }
        });
    }
}
