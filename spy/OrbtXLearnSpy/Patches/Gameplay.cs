using Patchwork;

namespace OrbtXLearnSpy.Patches {

    [ModifiesType("Gameplay")]
    class Gameplay : global::Gameplay {

        [DuplicatesBody("Start")]
        private void Orig_Start() { }

        [DuplicatesBody("Update")]
        private void Orig_Update() { }

        [DuplicatesBody("RoundEnd")]
        public void Orig_RoundEnd() { }

        [DuplicatesBody("RoundReset")]
        public void Orig_RoundReset() { }


        [ModifiesMember("Start")]
        private void Mod_Start() {
            Spy.Connect();
            Spy.SendEvent("start");
            Orig_Start();
        }

        [ModifiesMember("Update")]
        private void Mod_Update() {
            Orig_Update();
            Spy.OnUpdate(this);
        }

        [ModifiesMember("RoundEnd")]
        public void Mod_RoundEnd() {
            Spy.SendEvent("round_end");
            Orig_RoundEnd();
        }

        [ModifiesMember("RoundReset")]
        public void Mod_RoundReset() {
            Spy.SendEvent("round_reset");
            Orig_RoundReset();
        }
    }
}
