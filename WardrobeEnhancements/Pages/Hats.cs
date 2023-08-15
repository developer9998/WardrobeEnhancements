using GorillaNetworking;
using WardrobeEnhancements.PageLib;

namespace WardrobeEnhancements.Pages
{
    public class Hats : WardrobePage
    {
        public override string DisplayName => "HATS";
        public override bool OverrideItems => false;

        public override PageType Type => PageType.Category;
        public override CosmeticsController.CosmeticCategory ItemCategory => CosmeticsController.CosmeticCategory.Hat;
    }
}
