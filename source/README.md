# Hide Pieces 1.1.0

A small code and content mod for Vintage Story 1.22.3. No external mod dependencies.

## Cutting

Place a raw hide on the ground with sneak + right-click. Equip a knife, aim at
that placed hide, then crouch and hold left mouse button until cutting completes
(about two seconds). The hide is consumed and its pieces drop into the world.
The game supplies its normal knife cutting animation, progress and tool wear.
The interaction hint shows crouch + left-click; inventory cutting recipes are removed.

Small/medium/large/huge hides yield 1/2/3/5 raw pieces, matching vanilla normal-leather
yields. Fox, raccoon and complete bear hides are included. Existing freshness is
carried over, so cutting does not refresh an old hide. One hide is cut per action.
There is no recipe for turning pieces back into whole hides.

## Leatherworking

Every piece follows the normal barrel process, using 2 litres per liquid stage:

1. Raw piece + limewater, sealed 20 in-game hours (or diluted borax, 18 hours).
2. Soaked piece + knife in the crafting grid -> scraped piece.
3. Scraped piece + weak tannin, sealed 72 hours -> prepared piece.
4. Prepared piece + strong tannin, sealed 108 hours -> 1 normal plain leather.

Amounts scale with the number of pieces. A 50 L barrel processes up to 25 pieces.
Pieces from different hide sizes become the same item and can share a batch.
Raw pieces keep vanilla raw-hide spoilage (120 fresh hours + 24 transition hours
before environmental modifiers); the later stages do not spoil, as in vanilla.
This mod covers raw hides and normal leather, not salting or sturdy leather.

## Ground piles

Sneak + right-click on a solid surface to place/add a piece. Each pile holds 16,
like vanilla leather; normal transfer is 1 and bulk transfer is 4. Each processing
stage has its own pile and existing vanilla hide texture. Inventory stacks hold 64.
Use the game's interaction hints for pickup and bulk transfer with your keybinds.

## Installation

Close Vintage Story and place `hidepieces_1.1.0.zip` in the data folder's `Mods`
folder. Keep the ZIP closed. Only keep one version of Hide Pieces installed.
For multiplayer, the mod is needed on both server and client.
The mod adds its own items and recipes, plus a cutting behavior on ground storage.
Existing piece item codes, tanning and ground piles are unchanged from 1.0.0.

