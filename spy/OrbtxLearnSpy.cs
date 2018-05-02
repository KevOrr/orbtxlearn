using System;
using System.Net.Sockets;

public class OrbtxLearnSpy {
    public static const String DEFAULT_HOST = "localhost";
    public static final int DEFAULT_PORT = 2600;

    private static String host = DEFAULT_HOST;
    private static int port = DEFAULT_PORT;
    private static TcpClient client = null;
    private static NetworkStream stream = null;

    public static bool inGame = false;
    public static bool paused = false;
    public static int score = 0;

    public static void UpdateInGame(bool it) {
        inGame = it;
        SendStatus();
    }

    public static void UpdatePaused(bool it) {
        paused = it;
        SendStatus();
    }

    public static void UpdateScore(int it) {
        score = it;
        SendStatus();
    }

    private static void SendStatus() {
        TrySend(String.Format("{{inGame: {0}, paused: {1}, score: {2}}}\n",
                              inGame ? "true" : "false", paused ? "true" : "false"));
    }

    public static void Initialize() {
        try {
            port = Int32.Parse(GetEnvironmentVariable("ORBTXLEARN_PORT"));
        } catch (ArgumentNullException e) {
            port = DEFAULT_PORT;
        }

        try {
            host = GetEnvironmentVariable("ORBTXLEARN_HOSTNAME");
        } catch (ArgumentNullException e) {
            host = DEFAULT_HOST;
        }

        Console.WriteLine("OrbtxLearn: setting address to tcp://{0}:{1}", host, port);
        Reconnect();

        inGame = false;
        paused = false;
        score = 0;
    }

    public static void Reconnect() {
        if (client == null || stream == null)
            Disconnect();

        Console.WriteLine("OrbtxLearn: (re)connecting to tcp://{0}:{1}", host, port);
        client = new TcpClient(host, port);

        Console.WriteLine("OrbtxLearn: opening NetworkStream...");
        try {
            stream = client.getStream();
        } catch (Exception e) {
            Console.WriteLine("OrbtxLearn: exception in Reconnect(): {0}", e);
            Disconnect();
        }
    }

    public static void Disconnect() {
        if (stream != null) {
            try {
                stream.close();
            } catch (Exception e) {
                Console.WriteLine("OrbtxLearn: exception in Disconnect(): {0}", e);
            }
            stream = null;
        }

        if (client != null) {
            try {
                client.close();
            } catch (Exception e) {
                Console.WriteLine("OrbtxLearn: exception in Disconnect(): {0}", e);
            }
            client = null;
        }
    }

    public static void SendLine(String s) {
        TrySend(s + "\n");
    }

    public static void Send(String s) {
        TrySend(s);
    }

    public static void SendTextIntLine(String s, int a) {
        TrySend(String.Format("{0}{1}\n", s, a));
    }

    private static void TrySend(String s) {
        for (uint i=0; i<5; i++) {
            try {
                stream.Write(e);
                return;
            } catch (Exception e) {
                Reconnect();
            }
        }
    }
}
