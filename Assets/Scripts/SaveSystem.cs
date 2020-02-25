using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class SaveSystem
{
    static SaveData saveData = new SaveData();
    static string saveFilename = "saveData.lol";

    public static void Save()
    {
        SavePlayerData();
        SaveScene();
        WriteSaveData();
    }

    public static void Load()
    {
        ReadSaveData();

        LoadScene();

       }

    static void SavePlayerData()
    {
        Transform playerTransform = ((PlayerController)Object.FindObjectOfType( typeof( PlayerController ) )).transform;
        saveData.playerPosition = playerTransform.position;
        saveData.playerRotation = playerTransform.rotation;
    }

    static void SaveScene()
    {
        saveData.currentScene = SceneManager.GetActiveScene().buildIndex;

        GameController gc = (GameController)Object.FindObjectOfType( typeof( GameController ) );

        saveData.collectableIDs = new int[ gc.collectables.Length ];
        for( int i = 0; i < gc.collectables.Length; ++i )
            if( gc.collectables[ i ] != null )
                saveData.collectableIDs[ i ] = i;
    }

    static void WriteSaveData()
    {
        string json = JsonUtility.ToJson( saveData, true );
        string filePath = Application.persistentDataPath + "/" + saveFilename;

        if( File.Exists( filePath ) )
        {
            File.Delete( filePath );

            if( File.Exists( filePath ) )
            {
                Debug.LogError( "Cannot delete save file at " + filePath );
                return;
            }
        }

        File.WriteAllText( filePath, json );
        Debug.Log( "Saved:\n" + json + "\nto " + filePath );
    }

    static void ReadSaveData()
    {
        string filePath = Application.persistentDataPath + "/" + saveFilename;

        if( !File.Exists( filePath ) )
        {
            Debug.LogError( "No save file found at " + filePath );
            return;
        }

        string json = File.ReadAllText( filePath );
        saveData = JsonUtility.FromJson<SaveData>( json );

        Debug.Log( "Loaded:\n" + json + "\nfrom" + filePath );
    }

    static void LoadScene()
    {
        SceneManager.LoadScene( saveData.currentScene, LoadSceneMode.Single );
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded( Scene arg0, LoadSceneMode arg1 )
    {
        ((PlayerController)Object.FindObjectOfType( typeof( PlayerController ) )).LoadSaveData( saveData );
        ((GameController)Object.FindObjectOfType( typeof( GameController ) )).LoadSaveData( saveData );

        Debug.Log( "Load complete" );
    }
}
