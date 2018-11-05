using System.IO;
using Patchwork.AutoPatching;

[AppInfoFactoryAttribute]
class OrbtXLAppInfo : AppInfoFactory {
    public OrbtXLAppInfo() { }

    override public AppInfo CreateInfo(DirectoryInfo dir) {
        var ai = new AppInfo {
            AppName = "OrbtXL",
            BaseDirectory = dir,
            Executable = new FileInfo(Path.Combine(dir.FullName, "orbtxl.exe"))
        };

        ai.IconLocation = ai.Executable;

        using (var sr = new StreamReader(Path.Combine(Path.Combine(dir.FullName, "orbtxl_Data"), "app.info"))) {
            ai.AppVersion = sr.ReadLine().Trim();
        }

        return ai;
    }
}
