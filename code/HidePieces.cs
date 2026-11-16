using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

[assembly: System.Reflection.AssemblyVersion("1.1.0.0")]

namespace HidePieces;

public sealed class HidePiecesModSystem : ModSystem
{
    public override void Start(ICoreAPI api)
    {
        api.RegisterBlockBehaviorClass("HidePiecesCutting", typeof(HideCuttingBehavior));
    }
}

// Uses the game's normal held-left-click block-breaking pipeline: knife animation,
// progress, cancellation, claim checks and tool wear are handled by the game.
// Only the completion of crouch + knife on a raw hide changes its usual drops.
public sealed class HideCuttingBehavior : BlockBehavior
{
    private ItemStack[] knives;

    public HideCuttingBehavior(Block block) : base(block) { }

    private static int Yield(ItemStack stack)
    {
        if (stack?.Collectible.Code.Domain != "game") return 0;
        return stack.Collectible.Code.Path switch
        {
            "hide-raw-small" or "hide-raw-fox-red" or "hide-raw-fox-arctic"
                or "hide-raw-raccoon-common" => 1,
            "hide-raw-medium" or "hide-raw-bear-sun-complete" => 2,
            "hide-raw-large" or "hide-raw-bear-black-complete"
                or "hide-raw-bear-panda-complete" => 3,
            "hide-raw-huge" or "hide-raw-bear-brown-complete"
                or "hide-raw-bear-polar-complete" => 5,
            _ => 0
        };
    }

    private static ItemSlot RawHideSlot(IWorldAccessor world, BlockPos pos)
    {
        var storage = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityGroundStorage;
        if (storage?.StorageProps?.Layout != EnumGroundStorageLayout.SingleCenter) return null;
        var slot = storage.Inventory[0];
        return Yield(slot.Itemstack) > 0 ? slot : null;
    }

    private static bool IsCutting(IPlayer player) =>
        player?.Entity?.Controls.ShiftKey == true
        && player.InventoryManager.ActiveTool == EnumTool.Knife;

    public override float GetMiningSpeedModifier(IWorldAccessor world, BlockPos pos, IPlayer player)
    {
        // About two seconds with an ordinary knife, without changing other ground storage.
        return IsCutting(player) && RawHideSlot(world, pos) != null
            ? block.GetResistance(world.BlockAccessor, pos) / 2f
            : 1f;
    }

    public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer player,
        float dropQuantityMultiplier, ref EnumHandling handling)
    {
        var slot = RawHideSlot(world, pos);
        if (slot == null || !IsCutting(player)) return;

        handling = EnumHandling.PreventSubsequent;
        if (world.Side != EnumAppSide.Server) return;
        if (!world.Claims.TryAccess(player, pos, EnumBlockAccessFlags.Use)) return;

        var piece = world.GetItem(new AssetLocation("hidepieces", "hidepiece-raw"));
        if (piece == null) return;

        var storage = (BlockEntityGroundStorage)world.BlockAccessor.GetBlockEntity(pos);
        var pieces = new ItemStack(piece, Yield(slot.Itemstack));
        var perish = piece.GetTransitionableProperties(world, pieces, null)
            ?.FirstOrDefault(prop => prop.Type == EnumTransitionType.Perish);
        if (perish != null) CollectibleObject.CarryOverFreshness(storage.Api, slot, pieces, perish);

        slot.TakeOut(1);
        slot.MarkDirty();
        if (storage.Inventory.Empty) world.BlockAccessor.SetBlock(0, pos);
        else storage.MarkDirty(true);

        // Always drop the pieces into the world, even when the inventory has room.
        world.SpawnItemEntity(pieces, pos, new Vec3d(0, 0.05, 0));
        world.PlaySoundAt(new AssetLocation("game:sounds/tool/groundcrafting/hidecut*"), pos, 0);
    }

    public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world,
        BlockSelection selection, IPlayer player, ref EnumHandling handling)
    {
        if (RawHideSlot(world, selection.Position) == null) return Array.Empty<WorldInteraction>();
        knives ??= world.Collectibles.Where(obj => obj.Tool == EnumTool.Knife)
            .Select(obj => new ItemStack(obj)).ToArray();
        return new[]
        {
            new WorldInteraction
            {
                ActionLangCode = "hidepieces:blockhelp-cut-hide",
                MouseButton = EnumMouseButton.Left,
                HotKeyCode = "shift",
                Itemstacks = knives
            }
        };
    }
}
