using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableIconsData", menuName = "UI/Data/ConsumableIconsData")]
public class ConsumableIconsData : ScriptableObject
{
    [SerializeField]
    private Sprite _credit;
    [SerializeField]
    private Sprite _coin;

    public Sprite CreditIcon => _credit;
    public Sprite CoinIcon => _coin;
}
