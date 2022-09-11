# Installation:
- Download [Bepinex](https://builds.bepinex.dev/projects/bepinex_be/577/BepInEx_UnityIL2CPP_x64_ec79ad0_6.0.0-be.577.zip).
- Open your game folder (In steam, right click crab game, Manage > Browse local files).
- extract Bepinex so that all of it's files are in the game folder.
- run the game once. This will take some time.
- close the game.
- download `snowballJump.dll` from [Releases](https://github.com/o7Moon/CrabGame.SnowballJump/releases).
- move `snowballJump.dll` to `(Game Folder from step 2)/BepInEx/plugins/snowballJump.dll`.

# FAQ
### why can't i swap to the snowball?
The mod prevents you from swapping to the snowball so you can continue holding the tag stick or other items. Instead of using the snowball directly, you can right click to throw the snowball (even though you arent holding it!). if you would prefer to use a different mouse button or key, change the right click keybind in the game's settings.
### where are all the lobbies?
crab game detects mods and only lets you play in lobbies hosted by other players with mods. if you would like to play unmodded lobbies, move the `BepInEx` folder and `winhttp.dll` file out of your game folder (I have a folder called `Game Folder/mod_storage` that i move them to) to temporarily disable mods.
