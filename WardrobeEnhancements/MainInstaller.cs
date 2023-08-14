using GorillaLocomotion;
using UnityEngine;
using WardrobeEnhancements.Behaviours;
using WardrobeEnhancements.PageLib;
using WardrobeEnhancements.Pages;
using Zenject;

namespace WardrobeEnhancements
{
    public class MainInstaller : Installer
    {
        public GameObject Player => Object.FindObjectOfType<Player>().gameObject;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Main>().FromNewComponentOn(Player).AsSingle();
            Container.BindInterfacesAndSelfTo<PriceHelper>().AsSingle();

            // Outfits
            Container.Bind<WardrobePage>().To<Pages.ShoppingCart>().AsSingle();
            Container.Bind<WardrobePage>().To<Current>().AsSingle();

            // Catagories
            Container.Bind<WardrobePage>().To<Cosmetics>().AsSingle();
            Container.Bind<WardrobePage>().To<Hats>().AsSingle();
            Container.Bind<WardrobePage>().To<Faces>().AsSingle();
            Container.Bind<WardrobePage>().To<Badges>().AsSingle();
            Container.Bind<WardrobePage>().To<Holdables>().AsSingle();
            Container.Bind<WardrobePage>().To<Gloves>().AsSingle();
            Container.Bind<WardrobePage>().To<Slingshots>().AsSingle();
            Container.Bind<WardrobePage>().To<Sets>().AsSingle();
        }
    }
}
