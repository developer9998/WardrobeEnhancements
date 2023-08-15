using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class Current : WardrobePage
    {
        public override string DisplayName => "CURRENT SET";
        public override bool OverrideItems => true;

        public override PageType Type => PageType.Outfit;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.None;
    }
}
