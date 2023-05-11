using UnityEngine;

public class Starter : MonoBehaviour
{
    private void Start()
    {
        GameModel.OperationComplete += GameModel_OperationComplete;
    }

    private void GameModel_OperationComplete(GameModel.OperationResult obj)
    {
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
