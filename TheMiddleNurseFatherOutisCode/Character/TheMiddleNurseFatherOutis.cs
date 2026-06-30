using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Cards;
using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Relics;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Character;


 
public class TheMiddleNurseFatherOutis : PlaceholderCharacterModel
{
    public const string CharacterId = "TheMiddleNurseFatherOutis";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 85;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<MiddleStrike>(),
        ModelDb.Card<MiddleStrike>(),
        ModelDb.Card<MiddleStrike>(),
        ModelDb.Card<MiddleStrike>(),
        ModelDb.Card<Middle_Defend>(),
        ModelDb.Card<Middle_Defend>(),
        ModelDb.Card<Middle_Defend>(),
        ModelDb.Card<Middle_Defend>(),
        ModelDb.Card<Unpacking>()

    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<SealedSword>()
    ];
    
    //Note1
    public override NCreatureVisuals? CreateCustomVisuals()
    {
        return NodeFactory<NCreatureVisuals>.CreateFromScene("res://TheMiddleNurseFatherOutisCode/Character/middle_outis.tscn");
        
    }

    public override CardPoolModel CardPool => ModelDb.CardPool<TheMiddleNurseFatherOutisCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheMiddleNurseFatherOutisRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheMiddleNurseFatherOutisPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    
    //NOTE 1 2 AND 3
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}