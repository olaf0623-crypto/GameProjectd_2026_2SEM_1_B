using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class SimpleInventory : MonoBehaviour
{

    [Header("UI 연결")]
    [SerializeField] private GameObject inventoryPanel;                //인벤토리 UI 패널
    [SerializeField] private TextMeshProUGUI[] slotTexts;              //인벤토리 슬롯에 표시할 텍스트

    private List<InventoryItem> items = new List<InventoryItem>();      //items 들 관리 하는 List 자료구조 선언

    private PlayerController playerController;
    private PlayerPickup playerPickup;
    private ThirdPersonCamera thirdPersonCamera;                        //인벤토리가 오픈 되었을때 임의로 카메라는 움직이지 않게 하기 위해
    private bool isOpen;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerPickup = GetComponent<PlayerPickup>();

        if (Camera.main != null)
        {
            thirdPersonCamera = Camera.main.GetComponent<ThirdPersonCamera>();
        }

        inventoryPanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null) return;

        if (keyboard.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (isOpen == false && playerController.IsNormalState() == false)
        {
            return;
        }

        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            playerController.ChangeState(PlayerState.Inventory);
            playerPickup.enabled = false;

            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerController.ChangeState(PlayerState.Normal);
            playerPickup.enabled = true;

            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.enabled = true;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            if (i < items.Count)
            {
                slotTexts[i].text = items[i].itemName + "\n" + items[i].amount + "개";
            }
            else
            {
                slotTexts[i].text = "비어 있음";
            }
        }
    }

    public bool AddItem(string itemName, int amount)
    {
        foreach (InventoryItem item in items)              //Slot을 순환하면서 아이템 이름이 같은 것이 있을 경우 수량을 + 해준다.
        {
            if (item.itemName == itemName)
            {
                item.amount += amount;
                UpdateUI();
                return true;
            }
        }

        if (items.Count >= slotTexts.Length)
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
            return false;
        }

        InventoryItem newItem = new InventoryItem();
        newItem.itemName = itemName;
        newItem.amount = amount;

        items.Add(newItem);                                 //아이템 리스트에 넣어준다.
        UpdateUI();                                         //UI를 업데이트 한다.

        return true;
    }
}
