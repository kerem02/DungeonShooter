using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public player player;

    public Button healthPotionButton;
    public Button buyDoublePistolButton;
    public Button buyRifleButton;

    public Button equipDoublePistolButton;
    public Button equipRifleButton;
    public Button equipPistolButton;

    public TextMeshProUGUI equipPistolText;
    public TextMeshProUGUI equipDoublePistolText;
    public TextMeshProUGUI equipRifleText;

    // Start is called before the first frame update
    void Start()
    {
        shopUI.SetActive(false);

        healthPotionButton.onClick.AddListener(BuyHealthPotion);

        buyDoublePistolButton.onClick.AddListener(BuyDoublePistol);

        buyRifleButton.onClick.AddListener(BuyRifle);

        equipDoublePistolButton.onClick.AddListener(EquipDoublePistol);
  
        equipRifleButton.onClick.AddListener(EquipRifle);

        equipPistolButton.onClick.AddListener(EquipPistol);

        equipDoublePistolButton.gameObject.SetActive(false);
        equipRifleButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleShop();
        }
        
    }

    private void ToggleShop()
    {
        bool isActive = shopUI.activeSelf;
        shopUI.SetActive(!shopUI.activeSelf);
        Time.timeScale = isActive ? 1 : 0;
    }

    public void BuyHealthPotion()
    {
        int cost = 5;
        if(player.gold >= cost)
        {
            player.gold -= cost;
            player.UpdateGoldUI();
            player.AddPotion(1);
            Debug.Log("Satýn alýndý");
          
        }
        else
        {
            Debug.Log("Not enough coin");
        }
    }

    public void BuyDoublePistol()
    {
        int cost = 1;
        if(player.gold >= cost)
        {
            player.gold -= cost;
            player.UpdateGoldUI();
            player.BuyWeapon(1);

            buyDoublePistolButton.gameObject.SetActive(false);
            equipDoublePistolButton.gameObject.SetActive(true);
        }
    }

    public void BuyRifle()
    {
        int cost = 1;
        if(player.gold >= cost)
        {
            player.gold -= cost;
            player.UpdateGoldUI();
            player.BuyWeapon(2);

            buyRifleButton.gameObject.SetActive(false);
            equipRifleButton.gameObject.SetActive(true);
        }
    }

    public void EquipRifle()
    {
        player.EquipWeaponByType(WeaponType.Rifle);
        equipRifleText.text = "EQUIPPED";
        equipDoublePistolText.text = "EQUIP";
        equipPistolText.text = "EQUIP";
    }

    public void EquipDoublePistol()
    {
        player.EquipWeaponByType(WeaponType.DoublePistol);
        equipRifleText.text = "EQUIP";
        equipDoublePistolText.text = "EQUIPPED";
        equipPistolText.text = "EQUIP";
    }

    public void EquipPistol()
    {
        player.EquipWeaponByType(WeaponType.Pistol);
        equipRifleText.text = "EQUIP";
        equipDoublePistolText.text = "EQUIP";
        equipPistolText.text = "EQUIPPED";
    }
}
