using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using System.Text;

public class DataSender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.RootReference;
        string _dataStr = "Key:" + FirebaseController.sUniqueKey + "\n PlayerWon:" + FirebaseController.playerWon + "\n Duration:" + FirebaseController.gameTimer + "s \n Date: " + FirebaseController.now;

        byte[] data = Encoding.ASCII.GetBytes(_dataStr);

        StartCoroutine(UploadTextFile(storageRef, data));
    }

    private IEnumerator UploadTextFile(StorageReference storageRef, byte[] data)
    {// Create a reference to the file you want to upload
        StorageReference textFileRef = storageRef.Child("gamestats"+ FirebaseController.sUniqueKey +".txt");

        // Upload the file to the path "images/rivers.jpg"
        yield return textFileRef.PutBytesAsync(data)
             .ContinueWith(task => {
                 if (task.IsFaulted || task.IsCanceled)
                 {
                     Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                 else
                 {
                    // Metadata contains file metadata such as size, content-type, and md5hash.
                    StorageMetadata metadata = task.Result;
                     string md5Hash = metadata.Md5Hash;
                     Debug.Log("Finished uploading...");
                     Debug.Log("md5 hash = " + md5Hash);
                 }
             });

        yield return null;
    }
}
