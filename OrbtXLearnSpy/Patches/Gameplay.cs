using Patchwork;

namespace OrbtXLearnSpy.Patches {

    [ModifiesType("Gameplay")]
    public class Gameplay : global::Gameplay {

        // For some reason, pause is private, but we need to access it from Spy
        [ModifiesAccessibility("pause")]
        new public bool pause;

        [NewMember]
        private Spy spy;

        [NewMember]
        [DuplicatesBody("Start")]
        public void Orig_Start() { }

        [NewMember]
        [DuplicatesBody("Update")]
        public void Orig_Update() { }

        [NewMember]
        [DuplicatesBody("RoundEnd")]
        public void Orig_RoundEnd() { }

        [NewMember]
        [DuplicatesBody("RoundReset")]
        public void Orig_RoundReset() { }

        [ModifiesMember("Start")]
        private void Mod_Start() {
            spy = new Spy(this);
            Orig_Start();
            spy.OnStart();
        }

        [ModifiesMember("Update")]
        private void Mod_Update() {
            Orig_Update();
            spy.OnUpdate();
        }

        [ModifiesMember("RoundEnd")]
        public void Mod_RoundEnd() {
            Orig_RoundEnd();
            spy.OnRoundEnd();
        }

        [ModifiesMember("RoundReset")]
        public void Mod_RoundReset() {
            Orig_RoundReset();
            spy.OnRoundReset();
        }
    }
}
