using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class CurrencyExchangeUI : MonoBehaviour, IDisplayedElement
{
    private UIDocument _currencyExchange;
    private VisualElement _rootVisualElement;

    private const string _coinValueName = "CoinValue";
    private const string _creditValueName = "CreditValue";
    private const string _valueFieldName = "ValueField";
    private const string _creditResultValueName = "CreditResultValue";
    private const string _exchangeButtonName = "ExchangeButton";
    private const string _cancelButtonName = "CancelButton";
    private const string _coinToCreditRateName = "CoinToCreditRate";
    private const string _zeroResultText = "0";

    private Label _coinValue;
    private Label _creditValue;
    private TextField _valueFieldElement;
    private Label _creditResultValue;
    private int _coinToConvert = 9;

    private void Awake()
    {
        _currencyExchange = GetComponent<UIDocument>();
        _rootVisualElement = _currencyExchange.rootVisualElement;
        _currencyExchange.enabled = true;
        _rootVisualElement.visible = false;

        _coinValue = _rootVisualElement.Q<Label>(_coinValueName);
        _creditValue = _rootVisualElement.Q<Label>(_creditValueName);
        _creditResultValue = _rootVisualElement.Q<Label>(_creditResultValueName);

        Label coinToCreditRate = _rootVisualElement.Q<Label>(_coinToCreditRateName);
        coinToCreditRate.text = StringExt.FormatAfterThree(GameModel.CoinToCreditRate);

        _valueFieldElement = _rootVisualElement.Q<TextField>(_valueFieldName);
        _valueFieldElement.RegisterCallback<ChangeEvent<string>>(FieldValueChange);

        Button exchangeButton = _rootVisualElement.Q<Button>(_exchangeButtonName);
        Button cancelButton = _rootVisualElement.Q<Button>(_cancelButtonName);

        exchangeButton.clicked += ExchangeButton_clicked;
        cancelButton.clicked += CancelButton_clicked;
    }

    private void ExchangeButton_clicked()
    {
        if (!GameModel.HasRunningOperations)
            GameModel.ConvertCoinToCredit(_coinToConvert);
    }

    private void CancelButton_clicked()
    {
        if (!GameModel.HasRunningOperations)
            Hide();
    }

    private void FieldValueChange(ChangeEvent<string> v)
    {
        if (string.IsNullOrEmpty(v.newValue))
        {
            _creditResultValue.text = _zeroResultText;
            return;
        }

        if (int.TryParse(v.newValue, out _coinToConvert))
        {
            UpdateUI();
        }
        else
        {
            _valueFieldElement.value = v.newValue.Remove(v.newValue.Length - 1);
        }
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

    public void UpdateUI()
    {
        _coinValue.text = StringExt.FormatAfterThree(GameModel.CoinCount);
        _creditValue.text = StringExt.FormatAfterThree(GameModel.CreditCount);

        _valueFieldElement.value = _coinToConvert.ToString();

        var coinToCreditRate = _coinToConvert * GameModel.CoinToCreditRate;        
        _creditResultValue.text = StringExt.FormatAfterThree(coinToCreditRate);
    }
}
