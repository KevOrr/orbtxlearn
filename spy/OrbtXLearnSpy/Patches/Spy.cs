using Patchwork;
using System;
using System.Net.Sockets;
using System.Text;

namespace OrbtXLearnSpy.Patches {

    [NewType]
    class Spy {
        private const string HOST = "localhost";
        private const int PORT = 2600;

        private static TcpClient tcp = null;
        private static NetworkStream tcpStream = null;

        private static int lastScore = 0;
        private static int lastDir = 0; // 0: initial, -1: CCW, 1: CW
        private static bool lastPause;
        private static bool lastGameOn;

        public static void OnUpdate(Gameplay gp) {
            int dir = 0;
            if (Gameplay.playerSpinLeft && !Gameplay.playerSpinRight)
                dir = -1;
            else if (!Gameplay.playerSpinLeft && Gameplay.playerSpinRight)
                dir = 1;

            if (dir != lastDir) {
                if (dir == -1)
                    Send("d:CCW");
                else if (dir == 1)
                    Send("d:CW");

                lastDir = dir;
            }

            if (Gameplay.playerScore != lastScore) {
                Send("s:" + Gameplay.playerScore.ToString());
                lastScore = Gameplay.playerScore;
            }

            if (gp.pause != lastPause) {
                Send(gp.pause ? "e:pause" : "e:unpause");
                lastPause = gp.pause;
            }

            if (Gameplay.gameOn != lastGameOn) {
                Send(Gameplay.gameOn ? "e:gameon" : "e:gameoff");
                lastGameOn = Gameplay.gameOn;
            }
        }

        public static void SendEvent(String msg) => Send("e:" + msg);

        private static void Send(string msg) {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            tcpStream.BeginWrite(bytes, 0, bytes.Length,
                new System.AsyncCallback(SendMessageCallback), tcpStream);
        }

        private static void SendMessageCallback(IAsyncResult iar) { }

        public static void Connect() {
            if (tcpStream != null) {
                tcpStream.Close(0);
                tcpStream = null;
            }

            if (tcp != null)
                tcp.Close();

            tcp.Connect(HOST, PORT);
            tcpStream = tcp.GetStream();
        }
    }
}
