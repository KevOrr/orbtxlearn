using System;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;

public class OrbtxLearnSpy {
    public const String DEFAULT_URL = "http://localhost:2600/api";

    private static String url = DEFAULT_URL;

    private static Dictionary<String, String> dict = new Dictionary<String, String> {
        {"inGame", "false"},
        {"paused", "false"},
        {"unpausing", "false"},
        {"score", "0"}
    };

    public static void Initialize() {
        try {
            url = Environment.GetEnvironmentVariable("ORBTXLEARN_URL");
        } catch (ArgumentNullException) {
            url = DEFAULT_URL;
        }

        Console.WriteLine("OrbtxLearn: setting address to {0}", url);

        dict["inGame"] = "false";
        dict["paused"] = "false";
        dict["score"] = "0";
    }

    public static void UpdateStatus(String key, String value) {
        if (key != null) {
            dict[key] = value ?? "";
            SendStatus();
        }
    }

    public static void UpdateStatus(String key, bool value) => UpdateStatus(key, value ? "true" : "false");
    public static void UpdateStatus(String key, int value) => UpdateStatus(key, value.ToString());

    public static void SendStatus() {
        using (var client = new WebClient()) {
            var values = new NameValueCollection();
            foreach (var kv in dict)
                values.Add(kv.Key.ToString(), kv.Value.ToString());

            client.UploadValuesAsync(new Uri(url), values);
        }
    }
}
