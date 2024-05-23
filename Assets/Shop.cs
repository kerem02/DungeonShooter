using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public player player;

    public Button healthPotionButton;

    // Start is called before the first frame update
    void Start()
    {
        shopUI.SetActive(false);

        healthPotionButton.onClick.AddListener(BuyHealthPotion);
        
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

}
