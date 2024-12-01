using System.Collections;
using System.Security.Cryptography; //For SHA256 encoding
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    public delegate void ResFromSerCallBack(string res);

    //Player data
    [HideInInspector]
    public string username;

    private string userId;

    private string devId;

    private string httpServer = "https://api-bit-cs.ibgtech.co";

    private string gameID = "6540b25153266351dba288f6";

    public static API Instance;

    private void Awake()
    {
        devId = SystemInfo.deviceUniqueIdentifier;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);
        StartCoroutine(Login(devId, (r) => {
            JSONObject o = JSONObject.Create(r).GetField("data").GetField("userData");
            userId = o.GetField("userId").ToString().Trim('\"'); 
            Debug.Log("UserId " + userId);
            username = o.GetField("username").ToString().Trim('\"'); 
            Debug.Log("Username " + username);
        }));
    }

    static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public IEnumerator Login(string devId, ResFromSerCallBack callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("t", "d");
        form.AddField("deviceId", devId);

        UnityWebRequest www = UnityWebRequest.Post(httpServer + "/user/login", form);
        www.SetRequestHeader("x-auth-signature", ComputeSha256Hash(devId + "AoiLvSkiNZkeSDxXuf3N61lz"));
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Login info " + www.downloadHandler.text);
            string res = www.downloadHandler.text;
            if (callback != null)
                callback(res);
        }
    }

    public IEnumerator UpdateScore(int map, long level)
    {
        WWWForm form = new WWWForm();
        JSONObject json = new JSONObject();
        Debug.Log(gameID + " " + username + " " + userId);
        json.AddField("game", gameID);
        json.AddField("username", username);
        json.AddField("userId", userId);
        json.AddField("map", map);
        json.AddField("score", level);
        Debug.Log(json);
        UnityWebRequest www = UnityWebRequest.Put(httpServer + "/score/update", json.ToString());
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!  --> Update Score");
        }
    }

    public IEnumerator GetTopScore(ResFromSerCallBack callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(httpServer + "/score/top10/" + gameID);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {   
            Debug.Log("Top score " + www.downloadHandler.text);
            string res = www.downloadHandler.text;
           	Debug.Log("Result of GetTopScore: " + res);
            if (callback != null)
                callback(res);
        }
    }
}
