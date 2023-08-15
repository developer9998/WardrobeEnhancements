using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class Badges : WardrobePage
    {
        public override string DisplayName => "BADGES";
        public override bool OverrideItems => false;

        public override PageType Type => PageType.Category;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.Badge;
    }
}
