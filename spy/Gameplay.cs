public class Gameplay {
    public void Initialize() {
        OrbtxLearnSpy.Initialize();
    }

    public void EndRound() {
        // ...
        gameOn = false;
        OrbtxLearnSpy.UpdateInGame("true");
        // ...
    }

    public void ResetRound() {
        // ...
        gameOn = true;
        OrbtxLearnSpy.UpdateInGame("false");
        // ...
    }

    public void Update() {
        // ...
        if (/*...*/) {
            pause = true;
            OrbtxLearnSpy.UpdatePaused("true");
        } else if (/*...*/) {
            pause = false;
            OrbtxLearnSpy.UpdatePaused("false");
        }

        // ...

        if (/*...*/) {
            score++;
            OrbtxLearnSpy.UpdateScore(Int.ToString(score));
        }
    }
}
