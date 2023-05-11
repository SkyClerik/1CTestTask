using UnityEngine;

public class Starter : MonoBehaviour
{
    [SerializeField]
    private ShopMainPageUI _shopMainPageUi;

    private void Start()
    {
        _shopMainPageUi.Show();
        GameModel.OperationComplete += GameModel_OperationComplete;
    }

    private void GameModel_OperationComplete(GameModel.OperationResult obj)
    {
        if (string.IsNullOrEmpty(obj.ErrorDescription))
            return;

        Debug.Log($"{obj.ErrorDescription}");
    }

    void Update()
    {
        GameModel.Update();
    }

    private void OnDestroy()
    {
        GameModel.OperationComplete -= GameModel_OperationComplete;
    }
}
