using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public GameObject ItemUIPrefab;
    public GameObject SelectedItemObject;
    public GameObject UnselectedItemObject;
    public Image SelectedItem;
    public ItemContainer SelectedItemContainer;
    public TextMeshProUGUI SelectedItemTitle;
    public TextMeshProUGUI SelectedItemDescription;
    public TextMeshProUGUI SelectedItemValue;
    public TextMeshProUGUI ItemCountValue;
    public TextMeshProUGUI TotalValue;
    private List<ItemContainer> addedItemSlots = new List<ItemContainer>();
    public Sprite Grain;
    public Sprite Fish;
    public Sprite Oil;
    public Sprite Wood;
    public Sprite Brick;
    public Sprite Iron;
    public Sprite Rum;
    public Sprite Silk;
    public Sprite Silverware;
    public Sprite Emerald;
    public AudioSource SellStuff;
    public AudioSource ClickSound; 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 15; i++)
        {
            var added = Instantiate(ItemUIPrefab, GetComponent<Transform>());
            ItemContainer itemContainer = added.GetComponent<ItemContainer>();
            itemContainer.Parent = this;
            addedItemSlots.Add(itemContainer);
        }
        var inv = new Inventory();
        inv.Items.Add(new Item(System.DateTimeOffset.UtcNow, LootType.Emerald, 5));
        inv.Items.Add(new Item(System.DateTimeOffset.UtcNow, LootType.Rum, 52));
        PlayerPrefs.SetString("Inventory", JsonConvert.SerializeObject(inv));
        PlayerPrefs.Save();
        init();
    }
    private void init()
    {
        var currentInventory = PlayerPrefs.GetString("Inventory");
        Inventory inventory = null;
        if (!string.IsNullOrWhiteSpace(currentInventory))
        {
            inventory = JsonConvert.DeserializeObject<Inventory>(currentInventory);
        }
        else
        {
            inventory = new Inventory();
        }
        var items = inventory.Items.OrderByDescending(i => i.Obtained).ToList();
        int i = 0;
        foreach (var item in items)
        {
            addedItemSlots[i].GetComponent<ItemContainer>().Item = item;
            var image = addedItemSlots[i].GetComponent<Transform>().GetChild(0).GetComponent<Image>();
            image.enabled = true;
            image.sprite = GetSprite(item.LootName);
            var textComponent = addedItemSlots[i].GetComponent<Transform>().GetChild(1).GetComponent<TextMeshProUGUI>();
            textComponent.text = item.Count.ToString();
            textComponent.enabled = true;
            i++;
        }
        for (int x = i; x < addedItemSlots.Count - 1; x++)
        {
            addedItemSlots[x].GetComponent<ItemContainer>().Item = null;
            var image = addedItemSlots[x].GetComponent<Transform>().GetChild(0).GetComponent<Image>();
            image.enabled = false;
            image.sprite = null;
            var textComponent = addedItemSlots[x].GetComponent<Transform>().GetChild(1).GetComponent<TextMeshProUGUI>();
            textComponent.text = "";
            textComponent.enabled = true;
        }
        ItemCountValue.text = $"{items.Sum(i => i.Count)}";
        TotalValue.text = $"{items.Sum(i => (int)i.LootName * i.Count)}";
        SelectedItemObject.SetActive(false);
        UnselectedItemObject.SetActive(true);
    }
    private void DeselectAll()
    {
        foreach (var slot in addedItemSlots)
        {
            slot.GetComponent<Transform>().GetChild(2).GetComponent<Image>().enabled = false;
        }
    }
    public void SellAll()
    {
        var currentInventory = JsonConvert.DeserializeObject<Inventory>(PlayerPrefs.GetString("Inventory"));
        if (currentInventory != null)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + currentInventory.Items.Sum(i => (int)i.LootName * i.Count));
        }
        var inventory = new Inventory();
        PlayerPrefs.SetString("Inventory", JsonConvert.SerializeObject(inventory));
        PlayerPrefs.Save();
        SellStuff.Play();
        DeselectAll();
        init();
    }
    public void SellItems()
    {
        var inventory = JsonConvert.DeserializeObject<Inventory>(PlayerPrefs.GetString("Inventory"));
        var selectedItem = SelectedItemContainer.Item;
        if (inventory != null && inventory.Items.Count > 0 && inventory.Items.Any(i => i.LootName == selectedItem.LootName))
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + inventory.Items.Where(i => i.LootName == selectedItem.LootName).Sum(i => (int)i.LootName * i.Count));
            inventory.Items = inventory.Items.Where(i => i.LootName != selectedItem.LootName).ToList();
        }
        PlayerPrefs.SetString("Inventory", JsonConvert.SerializeObject(inventory));
        PlayerPrefs.Save();
        SellStuff.Play();
        DeselectAll();
        init();
    }
    public void Click(GameObject clicked)
    {
        bool enabled = !clicked.GetComponent<Transform>().GetChild(2).GetComponent<Image>().enabled;
        DeselectAll();
        clicked.GetComponent<Transform>().GetChild(2).GetComponent<Image>().enabled = enabled;
        ClickSound.Play();
        if (enabled)
        {
            SelectedItemContainer = clicked.GetComponent<ItemContainer>();
            SelectedItem.enabled = true;
            Item item = clicked.GetComponent<ItemContainer>().Item;
            LootType lootName = item.LootName;
            SelectedItem.sprite = GetSprite(lootName);
            SelectedItemTitle.text = lootName.ToString();
            SelectedItemDescription.text = GetDescription(lootName);
            SelectedItemValue.text = $"{(int)item.LootName * item.Count}";
            SelectedItemObject.SetActive(true);
            UnselectedItemObject.SetActive(false);
        }
        else
        {
            SelectedItemContainer = null;
            SelectedItem.enabled = false;
            SelectedItemTitle.text = "";
            SelectedItemDescription.text = "";
            SelectedItemValue.text = "";
            SelectedItemObject.SetActive(false);
            UnselectedItemObject.SetActive(true);
        }
    }
    private Sprite GetSprite(LootType type)
    {
        switch (type)
        {
            case LootType.Grain: return Grain;
            case LootType.Fish: return Fish;
            case LootType.Oil: return Oil;
            case LootType.Wood: return Wood;
            case LootType.Brick: return Brick;
            case LootType.Iron: return Iron;
            case LootType.Rum: return Rum;
            case LootType.Silk: return Silk;
            case LootType.Silverware: return Silverware;
            case LootType.Emerald: return Emerald;
            default: throw new System.Exception("Couldn't find Sprite for LootType");
        }
    }
    private string GetDescription(LootType type)
    {
        switch (type)
        {
            case LootType.Grain:
                return "Arr, me hearties! Grain be the sustenance that fills our bellies and keeps our crew as strong as the mighty sea itself.";
            case LootType.Fish:
                return "Shiver me timbers! Fish be the bounty of the sea, their flavorful flesh a feast fit for a scurvy-ridden pirate crew, satisfyin' our hunger and remindin' us of the treasures that await beneath the waves.";
            case LootType.Oil:
                return "Blow me down! Oil be the black gold that fuels our pirate ships, its fiery power drivin' us through treacherous waters and givin' us the might to conquer the seven seas!";
            case LootType.Wood:
                return "Avast, me hearties! Wood be the sturdy plunder we pirates rely on to craft our ships, sturdy as a kraken's grip and resilient against the ferocious storms that dare to challenge our dominion on the high seas!";
            case LootType.Brick:
                return "Arr, me mateys! Brick be the solid treasure we pirates stack high, fortifyin' our hideaways and creatin' impenetrable walls that withstand cannonballs and the plunderin' attempts of landlubbers!";
            case LootType.Iron:
                return "Avast, ye scallywags! Iron be the rugged plunder that forges our weapons and reinforces our ships, imbuing 'em with the strength to cut through our enemies and endure the relentless onslaught of the tempestuous seas!";
            case LootType.Rum:
                return "Avast, me hearties! Rum be the liquid gold that warms our pirate souls, its fiery taste bringin' merriment and courage to our crew, fuelin' our tales of adventure and washin' away the toils of the day with each hearty swig!";
            case LootType.Silk:
                return "Avast, me mateys!Silk be the luxurious loot that drapes us in elegance, its soft touch a reminder of the riches we've plundered, as we sail the high seas with the grace and splendor befitting true pirate royalty!";
            case LootType.Silverware:
                return "Avast, me buccaneers! Silverware be the gleaming treasure that adorns our pirate feasts, its polished shine a mark of refinement amidst the ruggedness of the sea, as we dine like kings and queens upon our hard-earned spoils!";
            case LootType.Emerald:
                return "Avast, me hearties! Emeralds be the precious gems that hold the secrets of the deep, their brilliant green hues like the glimmering treasures of hidden isles, signifying our pirate prowess and adorning us with the riches of the sea!";
            default:
                return "";
        }
    }
}
