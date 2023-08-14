using GorillaExtensions;
using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WardrobeEnhancements.PageLib;
using Zenject;

namespace WardrobeEnhancements.Behaviours
{
    public class Main : MonoBehaviour, IInitializable
    {
        public static Main Instance { get; private set; }

        public List<WardrobePage> _wardrobePages;
        public PriceHelper _priceHelper;

        public List<Tuple<Text, Text, Text, Text>> _wardrobeLabels;
        public int _currentWardrobeItem;

        private bool _hasUpdatedCart;
        private List<CosmeticsController.CosmeticItem>[] _storedArray;
        private readonly Mesh _buttonMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

        [Inject]
        public void Construct(List<WardrobePage> wardrobePages, PriceHelper priceHelper)
        {
            _wardrobePages = wardrobePages;
            _priceHelper = priceHelper;
        }

        public void Initialize()
        {
            Instance = this;

            CosmeticsController cosmeticsController = FindObjectOfType<CosmeticsController>();
            var wardrobeList = cosmeticsController.wardrobes.ToList();

            _storedArray = Enumerable.Repeat(new List<CosmeticsController.CosmeticItem>(), _wardrobePages.Count).ToArray();

            foreach (var wardrobe in wardrobeList)
            {
                bool isCityLocation = wardrobe.selfDoll == wardrobeList[0].selfDoll;

                Transform wardrobeParent = wardrobe.wardrobeItemButtons[0].transform.parent;
                List<WardrobeFunctionButton> wardrobeBtns = wardrobeParent.GetComponentsInChildren<WardrobeFunctionButton>(true).Where(a => a.function != "left" && a.function != "right").ToList();

                void FixButton(WardrobeFunctionButton button)
                {
                    button.GetComponent<MeshFilter>().mesh = _buttonMesh;
                    button.myText.gameObject.SetActive(false);
                }
                wardrobeBtns.ForEach(a => FixButton(a));

                void DisableButton(WardrobeFunctionButton button)
                {
                    button.GetComponent<Renderer>().forceRenderingOff = true;
                    button.GetComponent<Collider>().enabled = false;
                }

                float balancedHeight = Mathf.Lerp(wardrobeBtns[0].transform.position.y, wardrobeBtns[2].transform.position.y, 0.5f);
                DisableButton(wardrobeBtns[0]); DisableButton(wardrobeBtns[1]); wardrobeBtns.ForEach(a => a.myText.enabled = false);

                wardrobeBtns[2].transform.position = wardrobeBtns[2].transform.position.WithY(balancedHeight);
                wardrobeBtns[3].transform.position = wardrobeBtns[3].transform.position.WithY(balancedHeight);

                var wardrobeCanvas = new GameObject("WardrobeCanvas").AddComponent<Canvas>();
                wardrobeCanvas.gameObject.AddComponent<GraphicRaycaster>();

                Font baseFont = wardrobeBtns[0].myText.font;
                Font outlinedFont = GorillaTagger.Instance.offlineVRRig.playerText.font;
                Text CreateText(Font myFont)
                {
                    var text = new GameObject().AddComponent<Text>();
                    text.transform.SetParent(wardrobeCanvas.transform, true);
                    text.font = myFont; text.alignByGeometry = true;
                    text.alignment = TextAnchor.MiddleCenter;
                    return text;
                }

                Text leftArrowTxt = CreateText(baseFont);
                leftArrowTxt.text = !isCityLocation ? Constants.LeftArrow : Constants.RightArrow; leftArrowTxt.color = Color.black;

                leftArrowTxt.transform.position = wardrobeBtns[2].transform.position + wardrobeBtns[2].transform.forward * 0.0501f;
                leftArrowTxt.transform.rotation = wardrobeBtns[2].transform.rotation * Quaternion.Euler(Vector3.up * 180f);
                leftArrowTxt.transform.localScale = Vector3.one * 0.002f;

                Text rightArrowTxt = CreateText(baseFont);
                rightArrowTxt.text = isCityLocation ? Constants.LeftArrow : Constants.RightArrow; rightArrowTxt.color = Color.black;

                rightArrowTxt.transform.position = wardrobeBtns[3].transform.position + wardrobeBtns[3].transform.forward * 0.0501f;
                rightArrowTxt.transform.rotation = wardrobeBtns[3].transform.rotation * Quaternion.Euler(Vector3.up * 180f);
                rightArrowTxt.transform.localScale = Vector3.one * 0.002f;

                Text pageNameTxt = CreateText(outlinedFont); pageNameTxt.text = "ILY LUNA"; // She's the best frfr
                pageNameTxt.horizontalOverflow = HorizontalWrapMode.Overflow; pageNameTxt.verticalOverflow = VerticalWrapMode.Overflow;

                pageNameTxt.transform.position = Vector3.Lerp(wardrobeBtns[2].transform.position, wardrobeBtns[3].transform.position, 0.5f) + (Vector3.up * wardrobeBtns[2].transform.localScale.y) + wardrobeBtns[3].transform.forward * 0.03f;
                pageNameTxt.transform.rotation = wardrobeBtns[3].transform.rotation * Quaternion.Euler(Vector3.up * 180f);
                pageNameTxt.transform.localScale = Vector3.one * 0.0036f;

                Text pageTypeTxt = CreateText(outlinedFont); pageTypeTxt.text = "CATEGORY";
                pageNameTxt.horizontalOverflow = HorizontalWrapMode.Overflow; pageTypeTxt.verticalOverflow = VerticalWrapMode.Overflow;

                pageTypeTxt.transform.position = Vector3.Lerp(wardrobeBtns[2].transform.position, wardrobeBtns[3].transform.position, 0.5f) + (Vector3.up * (wardrobeBtns[2].transform.localScale.y + 0.03f)) + wardrobeBtns[3].transform.forward * 0.03f;
                pageTypeTxt.transform.rotation = wardrobeBtns[3].transform.rotation * Quaternion.Euler(Vector3.up * 180f);
                pageTypeTxt.transform.localScale = Vector3.one * 0.0027f;

                Text itemInfoTxt = CreateText(outlinedFont); itemInfoTxt.text = "WELCOME TO\nGORILLA TAG!";
                itemInfoTxt.alignment = TextAnchor.UpperCenter; itemInfoTxt.verticalOverflow = VerticalWrapMode.Overflow;

                itemInfoTxt.transform.position = Vector3.Lerp(wardrobeBtns[2].transform.position, wardrobeBtns[3].transform.position, 0.5f) + (Vector3.up * (wardrobeBtns[2].transform.localScale.y - 0.3f)) + wardrobeBtns[3].transform.forward * 0.03f;
                itemInfoTxt.transform.rotation = wardrobeBtns[3].transform.rotation * Quaternion.Euler(Vector3.up * 180f);
                itemInfoTxt.transform.localScale = Vector3.one * 0.0027f;

                Text itemSubInfoTxt = CreateText(outlinedFont); itemSubInfoTxt.text = "COST: 1000 | CATEGORY: HAT";
                itemSubInfoTxt.alignment = TextAnchor.MiddleCenter; itemSubInfoTxt.verticalOverflow = VerticalWrapMode.Overflow; itemSubInfoTxt.horizontalOverflow = HorizontalWrapMode.Overflow;

                itemSubInfoTxt.transform.position = Vector3.Lerp(wardrobeBtns[2].transform.position, wardrobeBtns[3].transform.position, 0.5f) + (Vector3.up * (wardrobeBtns[2].transform.localScale.y - 0.25f)) + wardrobeBtns[3].transform.forward * 0.03f;
                itemSubInfoTxt.transform.rotation = wardrobeBtns[3].transform.rotation * Quaternion.Euler(Vector3.up * 180f);
                itemSubInfoTxt.transform.localScale = Vector3.one * 0.0014f;

                wardrobeBtns[2].function = !isCityLocation ? Constants.LeftFunction : Constants.RightFunction;
                wardrobeBtns[3].function = isCityLocation ? Constants.LeftFunction : Constants.RightFunction;

                _wardrobeLabels ??= new List<Tuple<Text, Text, Text, Text>>();
                _wardrobeLabels.Add(Tuple.Create(pageNameTxt, pageTypeTxt, itemInfoTxt, itemSubInfoTxt));
            }

            // Output the item array into our cosmetics controller
            AccessTools.Field(cosmeticsController.GetType(), "itemLists").SetValue(cosmeticsController, _storedArray);
            cosmeticsController.cosmeticsPages = new int[_wardrobePages.Count];

            // Set our current page based on our playerpref
            _currentWardrobeItem = Constants.BareItem;
            SetPage(_currentWardrobeItem, cosmeticsController);
        }

        public void FilterCosmetics(CosmeticsController cosmeticsController)
        {
            if (!_hasUpdatedCart)
            {
                _hasUpdatedCart = true;

                bool IsSetEmpty(CosmeticsController cosmeticsController, CosmeticsController.CosmeticSet mySet)
                {
                    bool hasNull = true;
                    foreach (var item in mySet.items)
                    {
                        if (item.itemName != cosmeticsController.nullItem.itemName)
                        {
                            hasNull = false;
                            break;
                        }
                    }
                    return hasNull;
                }

                _currentWardrobeItem = IsSetEmpty(cosmeticsController, cosmeticsController.currentWornSet) ? Constants.BareItem : Constants.DefaultItem;
                SetPage(_currentWardrobeItem, cosmeticsController);
            }

            _storedArray = Enumerable.Repeat(new List<CosmeticsController.CosmeticItem>(), _wardrobePages.Count).ToArray();
            for (int i = 0; i < _wardrobePages.Count; i++)
            {
                if (i == 0)
                {
                    List<CosmeticsController.CosmeticItem> standItems = new();
                    cosmeticsController.cosmeticStands.Where(a => a.gameObject.activeSelf).ToList().ForEach(a => standItems.Add(a.thisCosmeticItem));
                    _storedArray[i] = standItems;
                    continue;
                }
                if (i == 1)
                {
                    _storedArray[i] = cosmeticsController.currentWornSet.items.Where(a => !a.isNullItem).ToList();
                    continue;
                }
                if (i == 2)
                {
                    _storedArray[i] = cosmeticsController.unlockedCosmetics.Where(a => a.itemCategory != CosmeticsController.CosmeticCategory.Set).ToList();
                    continue;
                }
                _storedArray[i] = cosmeticsController.unlockedCosmetics.Where(a => a.itemCategory == _wardrobePages[i].ItemCategory).ToList();
            }
            AccessTools.Field(cosmeticsController.GetType(), "itemLists").SetValue(cosmeticsController, _storedArray);

            if (cosmeticsController.cosmeticsPages[_currentWardrobeItem] > (_storedArray[_currentWardrobeItem].Count - 1) / 3) cosmeticsController.cosmeticsPages[_currentWardrobeItem] = 0;
            cosmeticsController.UpdateWardrobeModelsAndButtons();
        }

        public void UpdatePage(int index)
        {
            var wardrobePage = _wardrobePages[index];
            _wardrobeLabels.ForEach(a =>
            {
                a.Item1.text = wardrobePage.DisplayName;
                a.Item2.text = $"{wardrobePage.Type.ToString().ToUpper()} ({_storedArray[index].Count})";
            });
        }

        public void UpdateInfo(string infoTxt, CosmeticsController.CosmeticItem referenceItem)
        {
            _wardrobeLabels.ForEach(a =>
            {
                a.Item3.text = infoTxt;
                a.Item4.text = referenceItem.isNullItem ? "" : $"COST: {_priceHelper.GetPrice(referenceItem)} | CATEGORY: {referenceItem.itemCategory.ToString().ToUpper()}";
            });
        }

        public void SetPage(int index, CosmeticsController cosmeticsController)
        {
            UpdatePage(index); UpdateInfo("SELECT AN ITEM TO VIEW MORE ABOUT IT.", cosmeticsController.nullItem);
            if (cosmeticsController != null)
            {
                AccessTools.Field(cosmeticsController.GetType(), "wardrobeType").SetValue(cosmeticsController, index);
                cosmeticsController.UpdateWardrobeModelsAndButtons();
            }
        }

        public void NextPage()
        {
            _currentWardrobeItem = (_currentWardrobeItem + 1) % _wardrobePages.Count;
            SetPage(_currentWardrobeItem, FindObjectOfType<CosmeticsController>());
        }

        public void PreviousPage()
        {
            _currentWardrobeItem = _currentWardrobeItem <= 0 ? _wardrobePages.Count - 1 : _currentWardrobeItem - 1;
            SetPage(_currentWardrobeItem, FindObjectOfType<CosmeticsController>());
        }
    }
}
