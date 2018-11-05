using Patchwork.AutoPatching;
using System.IO;

namespace OrbtXLearnSpy {
    [PatchInfoAttribute]
    class PatchInfo : IPatchInfo {
        private string Combine(string first, params string[] rest) {
            string accum = first;
            foreach (var part in rest)
                accum = Path.Combine(accum, part);
            return accum;
        }

        public FileInfo GetTargetFile(AppInfo app) =>
            new FileInfo(Combine(app.BaseDirectory.FullName, "orbtxl_Data", "Managed", "Assembly-CSharp.dll"));

        public string CanPatch(AppInfo app) {
            FileInfo target = GetTargetFile(app);
            if (target.Exists)
                return null;

            return "Could not find orbtxl_Data/Managed/Assembly-CSharp.dll, are you sure this is the right directory?";
        }

        public string PatchVersion => "0.1";

        public string Requirements => "";

        public string PatchName => "OrbtXLearn Spy";
    }
}
