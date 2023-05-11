using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private CurrencyExchangeUI _currencyExchange;
    [SerializeField]
    private ReservesUI _reserves;
    private IDisplayedElement _currentLocalWindow;

    private UIDocument _main;
    private VisualElement _rootVisualElement;

    private const string _coinValueName = "CoinValue";
    private const string _creditValueName = "CreditValue";
    private const string _currencyExchangeButtonName = "CurrencyExchangeButton";
    private const string _reservesButtonName = "ReservesButton";
    private const string _medpackValueName = "MedpackValue";
    private const string _armorPlateValueName = "ArmorPlateValue";

    private Label _coinValue;
    private Label _creditValue;
    private Label _medpackValue;
    private Label _armorPlateValue;

    private void Awake()
    {
        _main = GetComponent<UIDocument>();
        _rootVisualElement = _main.rootVisualElement;

        _coinValue = _rootVisualElement.Q<Label>(_coinValueName);
        _creditValue = _rootVisualElement.Q<Label>(_creditValueName);
        _medpackValue = _rootVisualElement.Q<Label>(_medpackValueName);
        _armorPlateValue = _rootVisualElement.Q<Label>(_armorPlateValueName);

        Button curExchangeButton = _rootVisualElement.Q<Button>(_currencyExchangeButtonName);
        Button reservesButton = _rootVisualElement.Q<Button>(_reservesButtonName);
        curExchangeButton.clicked += CurExcButtonName_clicked;
        reservesButton.clicked += ReservesButton_clicked;

        UpdateUI();
        GameModel.ModelChanged += UpdateUI;
    }

    private void CurExcButtonName_clicked()
    {
        if (GameModel.HasRunningOperations)
            return;

        _currentLocalWindow?.Hide();
        _currencyExchange.Show();
        _currentLocalWindow = _currencyExchange;
    }
    private void ReservesButton_clicked()
    {
        if (GameModel.HasRunningOperations)
            return;

        _currentLocalWindow?.Hide();
        _reserves.Show();
        _currentLocalWindow = _reserves;
    }

    private void UpdateUI()
    {
        _coinValue.text = StringExt.FormatAfterThree(GameModel.CoinCount);
        _creditValue.text = StringExt.FormatAfterThree(GameModel.CreditCount);

        _medpackValue.text = GameModel.GetConsumableCount(GameModel.ConsumableTypes.Medpack).ToString();
        _armorPlateValue.text = GameModel.GetConsumableCount(GameModel.ConsumableTypes.ArmorPlate).ToString();
    }

    private void OnDestroy()
    {
        GameModel.ModelChanged -= UpdateUI;
    }
}
