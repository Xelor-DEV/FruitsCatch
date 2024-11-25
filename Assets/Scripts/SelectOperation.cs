using UnityEngine;

public class SelectOperation : MonoBehaviour
{
    [SerializeField] private DBConn dbConn;

    public void SetLoginOperation()
    {
        dbConn.SelectedOperation = OperationType.Login;
        Debug.Log("Operación seleccionada: Login");
    }

    public void SetCreateUserOperation()
    {
        dbConn.SelectedOperation = OperationType.CreateUser;
        Debug.Log("Operación seleccionada: CreateUser");
    }

    public void SetUpdateUserOperation()
    {
        dbConn.SelectedOperation = OperationType.UpdateUser;
        Debug.Log("Operación seleccionada: UpdateUser");
    }

    public void SetDeleteUserOperation()
    {
        dbConn.SelectedOperation = OperationType.DeleteUser;
        Debug.Log("Operación seleccionada: DeleteUser");
    }

    public void SetRecordScoreOperation()
    {
        dbConn.SelectedOperation = OperationType.RecordScore;
        Debug.Log("Operación seleccionada: RecordScore");
    }

}
