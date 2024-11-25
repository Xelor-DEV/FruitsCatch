using UnityEngine;

public class SelectOperation : MonoBehaviour
{
    [SerializeField] private DBConn dbConn;

    public void SetLoginOperation()
    {
        dbConn.SelectedOperation = OperationType.Login;
        Debug.Log("Operaci�n seleccionada: Login");
    }

    public void SetCreateUserOperation()
    {
        dbConn.SelectedOperation = OperationType.CreateUser;
        Debug.Log("Operaci�n seleccionada: CreateUser");
    }

    public void SetUpdateUserOperation()
    {
        dbConn.SelectedOperation = OperationType.UpdateUser;
        Debug.Log("Operaci�n seleccionada: UpdateUser");
    }

    public void SetDeleteUserOperation()
    {
        dbConn.SelectedOperation = OperationType.DeleteUser;
        Debug.Log("Operaci�n seleccionada: DeleteUser");
    }

    public void SetRecordScoreOperation()
    {
        dbConn.SelectedOperation = OperationType.RecordScore;
        Debug.Log("Operaci�n seleccionada: RecordScore");
    }

}
