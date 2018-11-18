using Patchwork;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace OrbtXLearnSpy.Patches {

    [NewType]
    public class Spy {
        private const string HOST = "127.0.0.1";
        private const int PORT = 2600;

        private readonly Gameplay gp;

        private StreamWriter log = new StreamWriter(@"orbtxlearn.spy.log", true, Encoding.UTF8) {
            AutoFlush = true
        };

        private TcpClient tcp;
        private NetworkStream tcpStream;
        private CommandReader commandReader;

        private int lastScore = 0;
        private int lastDir = 0; // 0: initial, -1: CCW, 1: CW

        // private bool lastPause = false;
        private bool lastGameOn = true;

        public Spy(Gameplay gp) {
            this.gp = gp;
            log.WriteLine("Created Spy");
            log.WriteLine("Connecting...");
            tcp = new TcpClient(HOST, PORT) {
                NoDelay = true
            };
            tcpStream = tcp.GetStream();
            log.WriteLine("Connected");

            commandReader = new CommandReader(tcpStream, new CommandReader.CallbackDelegate(ReadMessageCallback));
        }

        public void OnStart() {
            log.WriteLine("Game start");
            SendEvent("start");
            SendEvent("gameon");
        }

        public void OnUpdate() {
            int dir = 0;
            if (Gameplay.playerSpinLeft && !Gameplay.playerSpinRight)
                dir = -1;
            else if (!Gameplay.playerSpinLeft && Gameplay.playerSpinRight)
                dir = 1;

            if (dir != lastDir) {
                if (dir == -1)
                    Send("dir:CCW");
                else if (dir == 1)
                    Send("dir:CW");

                lastDir = dir;
            }

            if (Gameplay.playerScore != lastScore) {
                Send("score:" + Gameplay.playerScore.ToString());
                lastScore = Gameplay.playerScore;
            }

            // This causes errors in Gameplay
            // TODO find out why
            // if (gp.pause != lastPause) {
            //     Send(gp.pause ? "event:pause" : "event:unpause");
            //     lastPause = gp.pause;
            // }

            if (Gameplay.gameOn != lastGameOn) {
                SendEvent(Gameplay.gameOn ? "gameon" : "gameoff");
                lastGameOn = Gameplay.gameOn;
            }
        }

        public void OnRoundEnd() {
            if (lastGameOn == true) {
                SendEvent("gameoff");
                lastGameOn = false;
            }
        }

        public void OnRoundReset() {
            if (lastGameOn == false) {
                SendEvent("gameon");
                lastGameOn = true;
            }
        }

        public void SendEvent(String msg) => Send("event:" + msg);

        private void Send(string msg) {
            // log.WriteLine("Sending: " + msg);
            byte[] bytes = Encoding.UTF8.GetBytes(msg + "\n");
            tcpStream.BeginWrite(bytes, 0, bytes.Length,
                new AsyncCallback((IAsyncResult iar) => { }), tcpStream);
        }

        private void ReadMessageCallback(string line) {
            if (line.ToLower() == "command:restart")
                gp.RoundReset();
        }
    }

    [NewType]
    public class CommandReader {

        [NewType]
        public delegate void CallbackDelegate(string line);

        private const int READ_BUFFER_SIZE = 1024;

        private Stream stream;
        private byte[] buffer;
        private CallbackDelegate callback;

        public string Line { get; private set; }

        public CommandReader(Stream stream, CallbackDelegate callback) {
            this.stream = stream;
            this.buffer = new byte[READ_BUFFER_SIZE];
            this.callback = callback;
            this.Line = "";
        }

        public void Start() {
            stream.BeginRead(buffer, 0, READ_BUFFER_SIZE, new AsyncCallback(ReadReady), this);
        }

        private static void ReadReady(IAsyncResult iar) {
            var commandReader = (CommandReader)iar.AsyncState;
            int bytesRead = commandReader.stream.EndRead(iar);

            int eolIndex = Array.IndexOf(commandReader.buffer, (char)'\n', 0, bytesRead);
            if (eolIndex >= 0) {
                string line = commandReader.Line + Encoding.ASCII.GetString(commandReader.buffer, 0, eolIndex);
                commandReader.Line = "";
                commandReader.stream.BeginRead(commandReader.buffer, 0, READ_BUFFER_SIZE,
                    new AsyncCallback(ReadReady), commandReader);
                commandReader.callback(line);
            }
        }
    }
}
