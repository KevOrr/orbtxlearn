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
        private const int READ_BUFFER_LENGTH = 1024;

        private readonly Gameplay gp;

        private StreamWriter log = new StreamWriter(@"orbtxlearn.spy.log", true, Encoding.UTF8) {
            AutoFlush = true
        };

        private TcpClient tcp;
        private NetworkStream tcpStream;

        private int lastScore = 0;
        private int lastDir = 0; // 0: initial, -1: CCW, 1: CW
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

            new Thread(() => {
                byte[] buffer = new byte[READ_BUFFER_LENGTH];
                int offset = 0;
                while (true) {
                    if (tcpStream.DataAvailable) {
                        tcpStream.Read(buffer, offset, 1);
                        if (buffer[offset] == (char)'\n') {
                            ReadMessageCallback(Encoding.ASCII.GetString(buffer, 0, offset));
                            offset = 0;
                        } else {
                            offset++;
                        }
                    } else {
                        Thread.Sleep(500);
                    }
                }
            }) {
                IsBackground = true
            }.Start();
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
            log.WriteLine("Sending: " + msg);
            byte[] bytes = Encoding.UTF8.GetBytes(msg + "\n");
            tcpStream.BeginWrite(bytes, 0, bytes.Length,
                new AsyncCallback((IAsyncResult iar) => { }), tcpStream);
        }

        private void ReadMessageCallback(string line) {
            if (line.ToLower() == "command:restart") {
                log.WriteLine("Requesting restart");
                gp.restartRequest = true;
            }
        }
    }
}
