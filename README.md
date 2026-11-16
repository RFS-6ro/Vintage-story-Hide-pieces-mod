# Hide Pieces

Cut placed raw hides into smaller pieces and process them into leather through vanilla leatherworking.

**Vintage Story:** tested on 1.22.3  
**Mod version:** 1.1.0  
**Side:** required on both client and server  
**Dependencies:** the base game and Survival; no additional mods required

## Download and installation

Download **hidepieces_1.1.0.zip** from [Releases](https://github.com/RFS-6ro/Vintage-story-Hide-pieces-mod/releases/latest).

1. Close Vintage Story.
2. Put the ZIP in your Vintage Story data folder's `Mods` directory. Keep it zipped.
3. Keep only one version of Hide Pieces installed.
4. For multiplayer, install the mod on both the server and every client.

The installable mod is the `hidepieces_1.1.0.zip` release attachment. The source on `main` can be built using the instructions below. The original `v1.1.0` tag predates source publication, so its automatic source archives contain documentation only.

## Cutting hides

1. Place a raw hide on the ground with crouch + right-click.
2. Equip a knife and aim at the placed hide.
3. Hold crouch + left-click until cutting finishes. The hide pieces drop onto the ground.

| Raw hide size | Pieces | Final leather yield |
| --- | ---: | ---: |
| Small | 1 | 1 |
| Medium | 2 | 2 |
| Large | 3 | 3 |
| Huge | 5 | 5 |

Vanilla fox, raccoon and complete bear hides are supported. Cutting carries over freshness, and raw pieces spoil like raw hides. Raw-hide cutting is performed in the world; it has no inventory recipe.

## Making leather

Each piece follows these steps:

1. Seal a raw piece in **2 L limewater for 20 in-game hours**, or **2 L diluted borax for 18 hours**.
2. Combine the soaked piece with a knife in the crafting grid to scrape it.
3. Seal the scraped piece in **2 L weak tannin for 72 hours**.
4. Seal the prepared piece in **2 L strong tannin for 108 hours** to make **one normal plain leather**.

Liquid quantities scale with the number of pieces. A 50 L barrel can process up to 25 pieces per batch.

## Floor piles

Crouch + right-click on a solid surface to place or add pieces. Each processing stage has its own pile, holding up to 16 pieces. Inventory stacks hold 64.

## Compatibility and scope

- Tested with Vintage Story 1.22.3. Other game versions are unverified.
- Cutting was confirmed in-game. Dedicated server startup was checked alongside Chop 1.1.2 and VS Roofing 2.1.1; multiplayer interaction has not yet been verified.
- Existing pieces from Hide Pieces 1.0.0 retain their item identifiers.
- This release covers raw hides and normal leather. Salting and sturdy leather are outside its scope.
- The mod references the game's existing shapes, textures and sounds. Game assemblies and textures are not bundled.

## Building from source

The source on `main` implements Hide Pieces 1.1.0. Download or clone this branch to build it.

Requirements:

- Windows and PowerShell.
- An installed copy of Vintage Story **1.22.3**.
- A .NET SDK and the .NET 10 runtime available through `dotnet`. The build was verified with SDK 9.0.305 and runtime 10.0.9.

From the repository folder, run:

```powershell
.\build.ps1 -GamePath 'C:\Path\To\Vintagestory'
```

Supply the actual game installation folder. The script reads the game's API, Survival and Newtonsoft.Json assemblies locally; they are not distributed in this repository.

The script compiles `code/HidePieces.cs` to `source/HidePieces.dll`, then creates `dist/hidepieces_1.1.0.zip` and its SHA-256 file. The ZIP includes the mod assets, instructions and MIT license. It does not install the mod or change the game. Move an existing archive aside before rebuilding the same version.

Repository layout:

- `code/HidePieces.cs`: cutting interaction and mod registration.
- `source/modinfo.json`: version and required game dependencies.
- `source/assets/hidepieces/`: item definition, translations, ground-storage patch and recipes.
- `source/README.md`: instructions included in the mod ZIP.
- `build.ps1`: local compile and packaging script.

Compiled binaries, release archives, local backups and verification data are ignored by Git. Obtain the ready-to-install ZIP from Releases.

## Feedback

Use this repository's Issues tab to report problems. Include the game and mod versions, steps to reproduce, and relevant log errors. Remove personal information from logs before sharing.

## Author and repository

Created for Daniil, with AI-assisted development. The C# source, recipes, item definitions and translations are available here under the MIT license.

## License

MIT. See [LICENSE](LICENSE). Vintage Story and its built-in assets remain the property of Anego Studios.
