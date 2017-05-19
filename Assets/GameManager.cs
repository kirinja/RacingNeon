using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject Player1Prefab, Player2Prefab;
    public List<HighScore> Players;
    public string NextLevel = "Bana1";

    private string _saveDirectory;
    private string _savePath;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("CitySelect");
        
        _saveDirectory = Path.Combine(Application.dataPath, SaveDirectory);

        Cursor.visible = false;

        if (!Directory.Exists(_saveDirectory))
            Directory.CreateDirectory(_saveDirectory);

        _savePath = Path.Combine(_saveDirectory, SaveFile);
        
        Players = new List<HighScore>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPlayer(string pname, float time)
    {
        Players.Add(new HighScore(pname, time));
    }

    public HighScore[] GetSortedTimes()
    {
        // we must sort both the names and times
        Players.Sort(
            new Comparison<HighScore>(Target));
        return Players.ToArray();
    }

    private int Target(HighScore highScore, HighScore score)
    {
        if (highScore.Time > score.Time)
            return 1;
        if (highScore.Time < score.Time)
            return -1;

        return 0;
    }

    public void SaveToFiles()
    {
        SaveToFile();
        GC.Collect();
    }


    public void LoadFromFiles()
    {
        LoadFromFile();
        GC.Collect();
    }


    // the following encrypt and decrypt code is taken from http://tekeye.biz/2015/encrypt-decrypt-c-sharp-string
    //Encrypt
    public static string EncryptString(string plainText, string passPhrase)
    {
        byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        byte[] keyBytes = password.GetBytes(KeySize / 8);
        RijndaelManaged symmetricKey = new RijndaelManaged();
        symmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        byte[] cipherTextBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return Convert.ToBase64String(cipherTextBytes);
    }
    //Decrypt
    public static string DecryptString(string cipherText, string passPhrase)
    {
        byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        byte[] keyBytes = password.GetBytes(KeySize / 8);
        RijndaelManaged symmetricKey = new RijndaelManaged();
        symmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    }

    private void SaveToFile()
    {
        var highScoreData = new HighScoreData();

        highScoreData.Players = Players.ToArray();
        string[] stringified =  new string[Players.Count];
        for (int i = 0; i < highScoreData.Players.Length; i++)
        {
            stringified[i] = JsonUtility.ToJson(highScoreData.Players[i]);
        }
        //var stringifiedPlayer = JsonUtility.ToJson(highScoreData);
        //var encrypted = EncryptString(stringifiedPlayer, KeyPhrase);
        //File.WriteAllLines(_savePath, stringifiedPlayer);
        File.WriteAllLines(_savePath, stringified);
    }


    private bool LoadFromFile()
    {
        // NOTE: This is not a fail-safe way to deal with things since the file can be deleted between
        // this check and when we actually tries to use it. For simplicity we disregard proper error
        // handling and such in this tutorial.

        if (!File.Exists(_savePath)) return false; // TODO SCARY SHIT

        //var stringifiedData = File.ReadAllLines(_savePath);
        var stringifiedData = File.ReadAllLines(_savePath);
        //var decrypted = DecryptString(stringifiedData, KeyPhrase);
        // there is no safety here, so if the save file has been edited there will be errors

        Players.Clear();

        for (int i = 0; i < stringifiedData.Length; i++)
        {
            var highscore = JsonUtility.FromJson<HighScore>(stringifiedData[i]);
            AddPlayer(highscore.Name, highscore.Time);
        }

        //var playerSaveData = JsonUtility.FromJson<PlayerSaveData>(stringifiedData[0]);
        //var highScoreData = JsonUtility.FromJson<HighScoreData>(stringifiedData);
        //_playerAttributes.currentHealth = playerSaveData.HP;

        Players.Clear();

        //for (int i = 0; i < highScoreData.Players.Length; i++)
        //{
        //    var pname = highScoreData.Players[i].Name;
        //    var time = highScoreData.Players[i].Time;
        //    AddPlayer(pname, time);
        //}

        return true;
    }

    #region Constants

    private const string SaveFile = "Save.sav";
    private const string SaveDirectory = "Save";
    private const float RegularTimeScale = 1f;

    // This size of the IV (in bytes) must = (KeySize / 8).  Default KeySize is 256, so the IV must be
    // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
    private const string InitVector = "pemgail9uzpgzl88";
    // This constant is used to determine the KeySize of the encryption algorithm
    private const int KeySize = 256;

    // storing the encrypt/descrypt key inside the actual program is dangerous since you can read the memory and find the key
    // it doesnt matter in this case however since all we want to do is change the output file from english to gibberish (so the player cant easily edit it)
    private const string KeyPhrase = "Broken Dreams";

    #endregion
}
