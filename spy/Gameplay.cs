public class Gameplay {
    public void Initialize() {
        OrbtxLearnSpy.Initialize();
        OrbtxLearnSpy.Reconnect();
    }

    public void EndRound() {
        // ...
        gameOn = false;
        OrbtxLearnSpy.SendLine("playing=0");
        // ...
    }

    public void ResetRound() {
        // ...
        gameOn = true;
        OrbtxLearnSpy.SendLine("playing=1");
        // ...
    }

    public void Update() {
        // ...
        if (/*...*/) {
            pause = true;
            OrbtxLearnSpy.SendLine("paused=1");
        } else if (/*...*/) {
            pause = false;
            OrbtxLearnSpy.SendLine("paused=0");
        }
    }
}
