using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public PlayerResources playerResources;
    public OceanMap map;
    public IslandFactory islandFactory;

    public Camera boatCamera;
    public BoatCameraAnimation boatCameraAnimation;
    public BoatMovement boatMovement;
    [Space(10)]
    public Camera surfaceCamera;
    public SurfaceMovement surfaceMovement;
    [Space(10)]
    public float dayLength;
    public float chanceMultiplierPerIsland = 0.97f;
    public float islandInteractionDistance = 0.5f;
    public Vector2 boatBreakingRange;
    public Vector2Int hungerRange;
    public KeyCode pauseKey;
    public KeyCode interactionKey;
    public KeyCode inventoryKey;
    [Header("Interface")]
    public Text dayText;
    public ToolView[] toolViews;
    public Text moneyText;
    public MaskableGraphicColorGradient starvingFrame;
    public GameObject starvingSign;
    public InteractionWindow[] interactionWindows;
    public HintManager hintManager;
    public GameObject menu;
    public InventoryWindow inventoryWindow;
    public CraftingWindow craftingWindow;
    public HarvestSign harvestSign;
    private int activeInteractableWindow = -1;
    [Space(10)]
    public string[] endingsDictionaryKeys;

    private int day = 1;
    private float currentDaytime = 0f;
    private Vector3 prevBoatPosition;
    private bool swimming;
    private float starvingDays;
    private float islandChance;
    private float chanceValueModifier = 10f;
    private bool nearIsland;
    private bool isIslandRandom;
    [HideInInspector] public OceanIsland activeIsland;
    private InteractableObject interactableObject;
    private Movement activeMovement;
    private bool actionsUnlocked = true;
    private bool pauseMenuUnlocked = true;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        Vector3 startPosition = map.GenerateMap();
        SetPlayerResources();
        inventoryWindow.CreateInventory(playerResources);
        craftingWindow.CreateCraftingWindow(playerResources);
        boatMovement.SetPosition(startPosition);
        boatMovement.SetBorders(Vector3.zero, map.GetBorder());
        prevBoatPosition = startPosition;
        CloseAllWindows();
        ChangeGameMod(true);
        boatCameraAnimation.StartSmoothMovement();
    }

    private void SetPlayerResources()
    {
        playerResources = new PlayerResources();
        for (int i = 0; i < toolViews.Length; ++i)
        {
            toolViews[i].SetTrackableTool(playerResources.tools[i]);
        }
        moneyText.text = playerResources.Money.ToString();
    }

    private void Update()
    {
        if (pauseMenuUnlocked)
        {
            if (Input.GetKeyDown(pauseKey))
            {
                LockActionsAndOpenMenu(!actionsUnlocked);
            }
            if (actionsUnlocked)
            {
                if (Input.GetKeyDown(inventoryKey))
                {
                    inventoryWindow.SetActive(!inventoryWindow.IsActive);
                }
                if (interactableObject && Input.GetKeyDown(interactionKey))
                {
                    interactableObject.Interact();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (swimming)
        {
            Vector3 boatPosition = activeMovement.GetPosition();
            if (activeIsland)
            {
                float distance = (boatPosition - activeIsland.transform.position).magnitude;
                if (distance > islandInteractionDistance)
                {
                    LeaveIsland();
                }
            }
            else
            {
                OceanIsland island = map.GetIslandByCoordinates(boatPosition);
                nearIsland = island;
                if (nearIsland)
                {
                    float distance = (boatPosition - island.transform.position).magnitude;
                    if (distance < islandInteractionDistance)
                    {
                        MeetIsland(island, false);
                    }
                }
            }
            float magnitude = (boatPosition - prevBoatPosition).magnitude;
            currentDaytime += magnitude;
            if (currentDaytime > dayLength)
            {
                EndDay();
            }
            prevBoatPosition = boatPosition;
        }
    }

    public void ChangeGameMod(bool isSwimming)
    {
        swimming = isSwimming;
        DropInteractableObject(interactableObject);
        if (swimming)
        {

            surfaceCamera.enabled = false;
            surfaceMovement.SetActive(false);
            boatCamera.enabled = true;
            boatMovement.SetActive(true);
            activeMovement = boatMovement;
            SetInteractableObject(activeIsland);
            harvestSign.gameObject.SetActive(false);
        }
        else
        {
            boatCamera.enabled = false;
            boatMovement.SetActive(false);
            surfaceCamera.enabled = true;
            surfaceMovement.SetActive(true);
            activeMovement = surfaceMovement;
        }
    }

    public void EndGame(int resultId)
    {
        Debug.Log(endingsDictionaryKeys[resultId]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EndDay()
    {
        currentDaytime -= dayLength;
        day++;
        float boatSpoils = UnityEngine.Random.Range(boatBreakingRange.x, boatBreakingRange.y);
        bool afloat = playerResources.tools[0].UseTool(boatSpoils);
        if (afloat)
        {
            float foodSpoils = UnityEngine.Random.Range(hungerRange.x, hungerRange.y);
            bool wellFed = playerResources.tools[1].UseTool(foodSpoils);
            if (wellFed)
            {
                starvingDays = 0f;
            }
            else
            {
                starvingDays++;
                Debug.Log("Starving day " + starvingDays);
                if (starvingDays > 3f)
                {
                    EndGame(1); // Died of hunger
                    return;
                }
            }
            RefreshUI();
            if (!nearIsland) GetNewIsland();
        }
        else
        {
            EndGame(0); // Sank
        }
    }

    public void RefreshUI()
    {
        dayText.text = day.ToString();
        starvingSign.SetActive(starvingDays > 0);
        starvingFrame.SetValue(starvingDays / 3f);
        for (int i = 0; i < toolViews.Length; ++i)
        {
            toolViews[i].ResetDurabilityValue();
        }
        moneyText.text = playerResources.Money.ToString();
    }

    private void GetNewIsland()
    {
        islandChance += UnityEngine.Random.Range(3f * chanceValueModifier, 5f * chanceValueModifier);
        if (islandChance > 100f)
        {
            islandChance -= 100f;
            chanceValueModifier *= chanceMultiplierPerIsland;
            int islandValue = UnityEngine.Random.Range(0, 10);
            if (islandValue > 6)
            {
                MeetIsland(islandFactory.CreateIsland(day, playerResources.GearScore, true, activeMovement.transform.position), true);
            }
            else
            {
                MeetIsland(islandFactory.CreateIsland(day, playerResources.GearScore, false, activeMovement.transform.position), true);
            }
        }
    }

    private void MeetIsland(OceanIsland island, bool isRandom)
    {
        activeIsland = island;
        isIslandRandom = isRandom;
        SetInteractableObject(activeIsland);
        ToggleWindowVisibility(true);
    }

    private void LeaveIsland()
    {
        DropInteractableObject();
        if (isIslandRandom)
        {
            Destroy(activeIsland.gameObject);
        }
        activeIsland = null;
    }

    public void SetInteractableObject(InteractableObject newInteractableObject)
    {
        DropInteractableObject();
        interactableObject = newInteractableObject;
        if (interactableObject)
        {
            activeInteractableWindow = (int)interactableObject.interactionType;
            hintManager.SetHint(interactableObject.hintType);
        }
    }

    public void ToggleWindowVisibility(bool value)
    {
        interactionWindows[activeInteractableWindow].ToggleWindowVisibility(value);
    }

    public void ToggleWindowVisibility()
    {
        interactionWindows[activeInteractableWindow].ToggleWindowVisibility();
    }

    public void DropInteractableObject()
    {
        interactableObject = null;
        hintManager.HideHint();
        if (activeInteractableWindow > -1)
        {
            interactionWindows[activeInteractableWindow].ToggleWindowVisibility(false);
            activeInteractableWindow = -1;
        }
    }

    public void DropInteractableObject(InteractableObject droppableInteractableObject)
    {
        if (interactableObject == droppableInteractableObject)
        {
            interactableObject = null;
            hintManager.HideHint();
            if (activeInteractableWindow > -1)
            {
                interactionWindows[activeInteractableWindow].ToggleWindowVisibility(false);
                activeInteractableWindow = -1;
            }
        }
    }

    private void CloseAllWindows()
    {
        for (int i = 0; i < interactionWindows.Length; ++i)
        {
            interactionWindows[i]?.ToggleWindowVisibility(false);
        }
        hintManager.HideHint();
        activeInteractableWindow = -1;
        menu.SetActive(false);
        starvingSign.SetActive(false);
        inventoryWindow.SetActive(false);
        harvestSign.gameObject.SetActive(false);
    }

    public void LockActionsAndOpenMenu(bool value)
    {
        menu.SetActive(actionsUnlocked);
        actionsUnlocked = value;
        activeMovement.enabled = actionsUnlocked;
    }

    public void RefreshInventory()
    {
        if (inventoryWindow.IsActive) inventoryWindow.RefreshVisibilityAndAmount();
    }

    public void HarvestItem(int itemId, Vector2Int range, int toolId, int toolMinimumTier)
    {
        int quantity = UnityEngine.Random.Range(range.x, range.y + 1);
        int harvest = playerResources.HarvestItem(itemId, quantity, toolId, toolMinimumTier);
        string itemName = null;
        Sprite icon = null;
        if (harvest == 0)
        {
            itemName = ItemModel.instance.itemData[itemId].dictionaryName;
            icon = ItemModel.instance.GetItemIcon(itemId);
            RefreshUI();
            RefreshInventory();
        }
        else
        {
            icon = ItemModel.instance.GetToolIcon(playerResources.tools[toolId]);
        }
        harvestSign.SetValues(harvest, itemName, icon, quantity);
    }
}