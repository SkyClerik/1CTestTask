using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ReservesUI : MonoBehaviour, IDisplayedElement
{
    [SerializeField]
    private ConsumableIconsData _consumableIcons;
    private UIDocument _reservesWindow;
    private VisualElement _rootVisualElement;

    private const string _closeButtonName = "CloseButton";
    private const string _medpackButtonName = "MedpackButton";
    private const string _armorPlateButtonName = "ArmorPlateButton";
    private const string _medpackCostName = "MedpackCost";
    private const string _armorPlateCostName = "ArmorPlateCost";
    private const string _medpackCostIconName = "MedpackCostIcon";
    private const string _armorPlateCostIconName = "ArmorPlateCostIcon";
    private const string _medpackValueName = "MedpackValue";
    private const string _armorPlateValueName = "ArmorPlateValue";
    private const string _coinButtonColorName = "coin-button-color";
    private const string _creditButtonColorName = "credit-button-color";

    private Label _medpackValue;
    private Label _armorPlateValue;

    private void Awake()
    {
        _reservesWindow = GetComponent<UIDocument>();
        _rootVisualElement = _reservesWindow.rootVisualElement;
        _reservesWindow.enabled = true;
        _rootVisualElement.visible = false;

        Button closeButton = _rootVisualElement.Q<Button>(_closeButtonName);
        Button medpackButton = _rootVisualElement.Q<Button>(_medpackButtonName);
        Button armorPlateButton = _rootVisualElement.Q<Button>(_armorPlateButtonName);

        _medpackValue = _rootVisualElement.Q<Label>(_medpackValueName);
        _armorPlateValue = _rootVisualElement.Q<Label>(_armorPlateValueName);

        Label medpackCost = medpackButton.Q<Label>(_medpackCostName);
        Label armorPlateCost = armorPlateButton.Q<Label>(_armorPlateCostName);
        VisualElement medpackCostIcon = medpackButton.Q<VisualElement>(_medpackCostIconName);
        VisualElement armorPlateCostIcon = armorPlateButton.Q<VisualElement>(_armorPlateCostIconName);

        GameModel.ConsumableTypes medpackType = GameModel.ConsumableTypes.Medpack;
        GameModel.ConsumableTypes armorPlateType = GameModel.ConsumableTypes.ArmorPlate;
        GameModel.ConsumableConfig consumableConfig;

        if (GameModel.ConsumablesPrice.TryGetValue(medpackType, out consumableConfig))
            SetButtonCost(medpackCost, medpackCostIcon, medpackButton, ref consumableConfig, medpackType);

        if (GameModel.ConsumablesPrice.TryGetValue(armorPlateType, out consumableConfig))
            SetButtonCost(armorPlateCost, armorPlateCostIcon, armorPlateButton, ref consumableConfig, armorPlateType);

        closeButton.clicked += CloseButton_clicked;
    }

    private void SetButtonCost(Label costLable, VisualElement icon, Button button, ref GameModel.ConsumableConfig consumableConfig, GameModel.ConsumableTypes consumableTypes)
    {
        if (consumableConfig.CoinPrice > consumableConfig.CreditPrice)
        {
            costLable.text = consumableConfig.CoinPrice.ToString();
            icon.style.backgroundImage = new StyleBackground(_consumableIcons.CoinIcon);
            button.AddToClassList(_coinButtonColorName);
            button.clicked += () =>
            {
                if (!GameModel.HasRunningOperations)
                    GameModel.BuyConsumableForGold(consumableTypes);
            };
        }
        else
        {
            costLable.text = consumableConfig.CreditPrice.ToString();
            icon.style.backgroundImage = new StyleBackground(_consumableIcons.CreditIcon);
            button.AddToClassList(_creditButtonColorName);
            button.clicked += () =>
            {
                if (!GameModel.HasRunningOperations)
                    GameModel.BuyConsumableForSilver(consumableTypes);
            };
        }
    }

    private void CloseButton_clicked()
    {
        if (!GameModel.HasRunningOperations)
            Hide();
    }

    public void Show()
    {
        if (_rootVisualElement.visible)
            return;

        UpdateUI();
        GameModel.OperationComplete += GameModel_OperationComplete;
        _rootVisualElement.visible = true;
    }

    public void Hide()
    {
        GameModel.OperationComplete -= GameModel_OperationComplete;
        _rootVisualElement.visible = false;
    }

    private void GameModel_OperationComplete(GameModel.OperationResult obj)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _medpackValue.text = GameModel.GetConsumableCount(GameModel.ConsumableTypes.Medpack).ToString();
        _armorPlateValue.text = GameModel.GetConsumableCount(GameModel.ConsumableTypes.ArmorPlate).ToString();
    }
}