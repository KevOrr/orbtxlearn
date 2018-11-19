# OrbtXLearn Spy

The spy is in charge of monitoring game state, such as the current score and whether the game
is still being played or if it has been lost. It uses [Patchwork][patchwork] to patch an OrbtXL
instance on the fly.


### Building

*NOTE*: Right now, building the included version of Patchwork creates an endless stream of
terrifying errors, so in the meantime, grab a separate copy of `Patchwork` and build it according to
the instructions there.

1. Clone this repo with `git clone --recursive`
2. Open `spy.sln`
3. If necessary, fix paths to referenced assemblies in the `OrbtXLearnSpy` project.
	`Assembly-Csharp.dll` can be found in `<orbtxl>/orbtxl_Data/Managed`. `UnityEngine.CoreModule`
	needs to be from a 2018.2 version of Unity (I think? Not actually sure why I think this now. If
	it works with other versions, let me know)
4. Build the `AppInfo` and `Libraries/Patchwork/PatchworkLauncher` projects
5. Copy `AppInfo.dll` in the same folder as `PatchworkLauncher.exe` (TODO: make this a post-build task)
6. Note the path of `OrbtXLearnSpy.pw.dll`


### Launching the game with patches

1. Since this is alpha-level software, it might make sense to copy your OrbtXL installation instead
   of using your actual version of the game
2. Start `PatchworkLauncher.exe`
3. Click `Change Game Folder`
4. Navigate to your OrbtXL installation
5. Click `Active Mods`
6. If `OrbtXLearn Spy` isn't already listed, click `Add` and navigate to `OrbtXLearnSpy.pw.dll`
7. To test that Patchwork is able to apply the patches, hit `Test Run`
8. If all is well (which it probably isn't), go ahead and try `Launch with Mods`

If there are errors when you attempt to apply the patch, open an issue here first. It's likely to
be an error in this repo, even though the error message might not seem to indicate that.


[spy]: spy/
[patchwork]: https://github.com/GregRos/Patchwork
