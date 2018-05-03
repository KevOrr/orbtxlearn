using System;
using System.Net;
using Systems.Collections.Specialized;

public class OrbtxLearnSpy {
    public static const String DEFAULT_URL = "http://localhost:2600/api";

    private static String url = DEFAULT_URL;
    private static readonly HttpClient client = new HttpClient();

    private static Dictionary<String, String> dict = new Dectionary<String, String> {
        {"inGame", "false"},
        {"paused", "false"},
        {"score", "0"}
    };

    public static void Initialize() {
        try {
            url = GetEnvironmentVariable("ORBTXLEARN_URL");
        } catch (ArgumentNullException e) {
            url = DEFAULT_URL;
        }

        Console.WriteLine("OrbtxLearn: setting address to {0}", url);
        Reconnect();

        dict["inGame"] = "false";
        dict["paused"] = "false";
        dict["score"] = "0";
    }

    public static void UpdateInGame(String it) {
        dict["inGame"] = it;
        SendStatus();
    }

    public static void UpdateInGame(bool it) {
        dict["inGame"] = it ? "true" : "false";
        SendStatus();
    }

    public static void UpdatePaused(String it) {
        dict["paused"] = it;
        SendStatus();
    }

    public static void UpdatePaused(bool it) {
        dict["paused"] = it ? "true" : "false";
        SendStatus();
    }

    public static void UpdateScore(String it) {
        dict["score"] = it;
        SendStatus();
    }

    public static void UpdateScore(int it) {
        dict["score"] = Int.ToString(it);
        SendStatus();
    }

    // .NET 4.0+
    public static void SendStatus() {
        client.PostAsync(url, new FormUrlEncodedContent(dict));
    }

    // .NET 2.0+
    public static void SendStatus() {
        using (var client = new WebClient()) {
            var values = new NameValueCollection();
            foreach(var kv in dict)
                values.Add(kv.Key.ToString(), kv.Value.ToString());

            client.UploadValuesAsync(new Uri(url), values);
        }
    }
}
