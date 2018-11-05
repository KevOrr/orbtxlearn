using Patchwork;

namespace OrbtXLearnSpy.Patches {

    [ModifiesType("Gameplay")]
    class PatchedGameplay : Gameplay {

        [DuplicatesBody("Start")]
        private void orig_Start() { }

        [DuplicatesBody("Update")]
        private void orig_Update() { }

        [DuplicatesBody("RoundEnd")]
        public void orig_RoundEnd() { }

        [DuplicatesBody("RoundReset")]
        public void orig_RoundReset() { }


        [ModifiesMember("Start")]
        private void mod_Start() {
            Spy.Connect();
            Spy.SendEvent("start");
            orig_Start();
        }

        [ModifiesMember("Update")]
        private void mod_Update() {
            orig_Update();
            Spy.OnUpdate(this);
        }

        [ModifiesMember("RoundEnd")]
        public void mod_RoundEnd() {
            Spy.SendEvent("round_end");
            orig_RoundEnd();
        }

        [ModifiesMember("RoundReset")]
        public void mod_RoundReset() {
            Spy.SendEvent("round_reset");
            orig_RoundReset();
        }
    }
}
