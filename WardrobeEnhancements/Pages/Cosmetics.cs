using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class Cosmetics : WardrobePage
    {
        public override string DisplayName => "COSMETICS";
        public override bool OverrideItems => true;

        public override PageType Type => PageType.Category;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.None;
    }
}
