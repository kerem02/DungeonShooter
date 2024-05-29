using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI; // Reference to the shop UI panel
    public player player; // Reference to the player script

    public Button healthPotionButton; // Button to buy health potions
    public Button buyDoublePistolButton; // Button to buy double pistols
    public Button buyRifleButton; // Button to buy a rifle

    public Button equipDoublePistolButton; // Button to equip double pistols
    public Button equipRifleButton; // Button to equip a rifle
    public Button equipPistolButton; // Button to equip a pistol

    public TextMeshProUGUI equipPistolText; // Text for pistol equip button
    public TextMeshProUGUI equipDoublePistolText; // Text for double pistol equip button
    public TextMeshProUGUI equipRifleText; // Text for rifle equip button

    // Start is called before the first frame update
    void Start()
    {
        shopUI.SetActive(false); // Hide the shop UI at the start

        healthPotionButton.onClick.AddListener(BuyHealthPotion); // Add listener to buy health potion button

        buyDoublePistolButton.onClick.AddListener(BuyDoublePistol); // Add listener to buy double pistols button

        buyRifleButton.onClick.AddListener(BuyRifle); // Add listener to buy rifle button

        equipDoublePistolButton.onClick.AddListener(EquipDoublePistol); // Add listener to equip double pistols button

        equipRifleButton.onClick.AddListener(EquipRifle); // Add listener to equip rifle button

        equipPistolButton.onClick.AddListener(EquipPistol); // Add listener to equip pistol button

        equipDoublePistolButton.gameObject.SetActive(false); // Hide the equip double pistols button
        equipRifleButton.gameObject.SetActive(false); // Hide the equip rifle button
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleShop(); // Toggle the shop UI visibility
        }
    }

    // Method to toggle the shop UI visibility
    private void ToggleShop()
    {
        bool isActive = shopUI.activeSelf;
        shopUI.SetActive(!shopUI.activeSelf);
        Time.timeScale = isActive ? 1 : 0; // Pause or resume the game
    }

    // Method to buy a health potion
    public void BuyHealthPotion()
    {
        int cost = 5;
        if (player.gold >= cost)
        {
            player.gold -= cost;
            player.UpdateGoldUI();
            player.AddPotion(1);
            Debug.Log("Purchased");
        }
        else
        {
            Debug.Log("Not enough coin");
        }
    }

    // Method to buy double pistols
    public void BuyDoublePistol()
    {
        int cost = 20;
        if (player.gold >= cost)
        {
            player.gold -= cost;
            player.UpdateGoldUI();
            player.BuyWeapon(1);

            buyDoublePistolButton.gameObject.SetActive(false);
            equipDoublePistolButton.gameObject.SetActive(true);
        }
    }

    // Method to buy a rifle
    public void BuyRifle()
    {
        int cost = 40;
        if (player.gold >= cost)
        {
            player.gold -= cost;
            player.UpdateGoldUI();
            player.BuyWeapon(2);

            buyRifleButton.gameObject.SetActive(false);
            equipRifleButton.gameObject.SetActive(true);
        }
    }

    // Method to equip a rifle
    public void EquipRifle()
    {
        player.EquipWeaponByType(WeaponType.Rifle);
        equipRifleText.text = "EQUIPPED";
        equipDoublePistolText.text = "EQUIP";
        equipPistolText.text = "EQUIP";
    }

    // Method to equip double pistols
    public void EquipDoublePistol()
    {
        player.EquipWeaponByType(WeaponType.DoublePistol);
        equipRifleText.text = "EQUIP";
        equipDoublePistolText.text = "EQUIPPED";
        equipPistolText.text = "EQUIP";
    }

    // Method to equip a pistol
    public void EquipPistol()
    {
        player.EquipWeaponByType(WeaponType.Pistol);
        equipRifleText.text = "EQUIP";
        equipDoublePistolText.text = "EQUIP";
        equipPistolText.text = "EQUIPPED";
    }
}

