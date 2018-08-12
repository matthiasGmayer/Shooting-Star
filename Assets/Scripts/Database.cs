using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Database : MonoBehaviour
{

    [SerializeField]
    public string databaseInput;
    public static string database;
    public static int? sessionId;
    // Use this for initialization
    void Start()
    {
        database = databaseInput;

        //Database.Login("Max", "s");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static Stream GetStream(string url)
    {
        return WebRequest.Create(url).GetResponse().GetResponseStream();
    }
    private static StreamReader GetStreamReader(string url)
    {
        return new StreamReader(GetStream(url));
    }

    private static string Command(string url, params string[] commands)
    {
        url = url.Replace("!", database);
        if (!url.EndsWith(".php")) url += ".php";
        if (commands.Length > 0) url += "?" + commands[0];
        for (int i = 1; i < commands.Length; i++)
        {
            url += "&" + commands[i];
        }
        Debug.Log(url);
        return url;
    }

    private static string GetLine(string url)
    {
        return GetStreamReader(url).ReadLine();
    }

    private static IEnumerable<string> GetLines(string url)
    {
        StreamReader r = GetStreamReader(url);
        string s;
        while ((s = r.ReadLine()) != null) yield return s;
    }

    public static void Login(string username, string password)
    {
        string sessionId = GetLine(Command("!AccountSystem/loginlater", "user=" + username, "password=" + password, "lira=1"));
        int id;
        if (int.TryParse(sessionId, out id))
        {
            Database.sessionId = id;
        }
        else
        {
            Database.sessionId = null;
        }

    }
}
