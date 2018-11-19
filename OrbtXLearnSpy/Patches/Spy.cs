using Patchwork;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private StreamReader tcpStreamReader;

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

            tcpStreamReader = new StreamReader(tcpStream, Encoding.UTF8);
            new Thread(() => {
                while (tcpStreamReader.Peek() >= 0)
                    ReadMessageCallback(tcpStreamReader.ReadLine());
            }).Start();
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
                gp.restartRequest = true;
        }
    }
}
