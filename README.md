# OrbtXLearn

### Injected Spy

The [spy][spy] is in charge of monitoring game state, such as the current score and whether the game
is still being played or if it has been lost. It uses [Patchwork][patchwork] to patch an OrbtXL
instance on the fly.


### Building

1. Grab a copy of [Patchwork][patchwork] TODO: Patchwork needs some pull requests to get this
   project working
2. Open `spy/spy.sln` and build the `AppInfo` and `OrbtXLearnSpy` projects
3. Place `AppInfo.dll` in the same folder as `PatchworkLauncher.exe`, and note where
   `OrbtXLearnSpy.pw.dll` is


### Launching the game with patches

1. Since this is alpha-level software, it might make sense to copy your OrbtXL installation instead
   of using your actual version of the game
2. Start `PatchworkLauncher.exe`
3. Click `Change Game Folder`
4. Navigate to your OrbtXL instalation
5. Click `Active Mods`
6. If `OrbtXLearn Spy` isn't already listed, click `Add` and navigate to `OrbtXLearnSpy.pw.dll`
7. To test that Patchwork is able to apply the patches, hit `Test Run`
8. If all is well (which it probably isn't), go ahead and try `Launch with Mods`



[spy]: spy/
[patchwork]: https://github.com/GregRos/Patchwork