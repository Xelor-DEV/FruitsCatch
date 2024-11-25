using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DBConn : MonoBehaviour
{
    [SerializeField] private string baseUrl = "http://localhost/";

    private RequestBase requestData;
    [SerializeField] private ServerResponse response;
    [SerializeField] private OperationType selectedOperation;
    [SerializeField] private UserSessionData sessionData;

    public static event Action OnLoginSuccess;
    public static event Action OnLogout;

    public OperationType SelectedOperation
    {
        get
        {
            return selectedOperation;
        }
        set
        {
            selectedOperation = value;
            requestData = CreateRequestData(selectedOperation);
        }
    }

    private string GetUrl(OperationType operation)
    {
        switch (operation)
        {
            case OperationType.Login:
                return baseUrl + "db_create_login.php";
            case OperationType.CreateUser:
                return baseUrl + "db_create_user_game.php";
            case OperationType.DeleteUser:
                return baseUrl + "db_delete_user_game.php";
            case OperationType.UpdateUser:
                return baseUrl + "db_update_user_game.php";
            case OperationType.RecordScore:
                return baseUrl + "db_create_game_4.php";
            default:
                return baseUrl;
        }
    }

    private string GetRequestType(OperationType operation)
    {
        switch (operation)
        {
            case OperationType.DeleteUser:
                return UnityWebRequest.kHttpVerbDELETE;
            case OperationType.UpdateUser:
                return UnityWebRequest.kHttpVerbPUT;
            default:
                return UnityWebRequest.kHttpVerbPOST;
        }
    }

    private RequestBase CreateRequestData(OperationType operation)
    {
        switch (operation)
        {
            case OperationType.Login:
                return new LoginRequest();
            case OperationType.CreateUser:
                return new CreateUserRequest();
            case OperationType.UpdateUser:
                return new UpdateUserRequest { userId = sessionData.userId };
            case OperationType.DeleteUser:
                return new DeleteUserRequest { user_id = sessionData.userId };
            default:
                return new UserDataBase();
        }
    }

    public void SetUsername(string username)
    {
        if (requestData is UserDataBase userData)
        {
            userData.username = username;
        }
    }

    public void SetPassword(string password)
    {
        if (requestData is UserDataBase userData)
        {
            userData.password = password;
        }
    }
    public void SetEmail(string email)
    {
        if (requestData is CreateUserRequest createUserRequest)
        {
            createUserRequest.email = email;
        }
        else if (requestData is UpdateUserRequest updateUserRequest)
        {
            updateUserRequest.email = email;
        }
    }

    public void SendScoreToDatabase(int score, int userId)
    {
        selectedOperation = OperationType.RecordScore;

        // Crear los datos del puntaje
        requestData = new RecordScoreRequest
        {
            userid = userId,
            score = score,
            created_by = "admin"
        };

        PerformOperation();
    }
    public void PerformOperation()
    {
        StartCoroutine(HandleRequest(selectedOperation));
    }

    private IEnumerator HandleRequest(OperationType operation)
    {
        string url = GetUrl(operation);
        string requestType = GetRequestType(operation);

        // Validar la sesión
        if (operation == OperationType.CreateUser && sessionData.userId > 0)
        {
            Debug.Log("Debe cerrar sesión para crear una nueva cuenta.");
            yield break;
        }
        if ((operation == OperationType.DeleteUser || operation == OperationType.UpdateUser) && sessionData.userId <= 0)
        {
            Debug.Log("Debe iniciar sesión para realizar esta operación.");
            yield break;
        }

        // Convertir a JSON
        string jsonString = JsonUtility.ToJson(requestData);
        Debug.Log("JSON Payload: " + jsonString);

        // Enviar solicitud
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        request.SetRequestHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS, DELETE, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            response = JsonUtility.FromJson<ServerResponse>(responseText);

            if (operation == OperationType.Login && response.userId > 0)
            {
                sessionData.userId = response.userId;
                OnLoginSuccess?.Invoke();
            }
            else if (operation == OperationType.DeleteUser)
            {
                // Restablece la sesión y activa el evento OnLogout
                sessionData.userId = 0;
                OnLogout?.Invoke();
            }

            Debug.Log("Server Message: " + response.message);
        }
    }
}


[System.Serializable]
public class RequestBase { }

[System.Serializable]
public class UserDataBase : RequestBase
{
    public string username;
    public string password;
}

[System.Serializable]
public class LoginRequest : UserDataBase { }

[System.Serializable]
public class CreateUserRequest : UserDataBase
{
    public string email;
    public string created_by = "admin";
}

[System.Serializable]
public class UpdateUserRequest : UserDataBase
{
    public int userId;
    public string email;
    public string modified_by = "admin";
    public int state = 1;
}

[System.Serializable]
public class DeleteUserRequest : RequestBase
{
    public int user_id;  // Cambiado de 'userId' a 'user_id'
}


[System.Serializable]
public class ServerResponse
{
    public string message;
    public int userId;
}

[System.Serializable]
public class RecordScoreRequest : RequestBase
{
    public int userid;
    public int score;
    public string created_by;
}

[System.Serializable]
public enum OperationType
{
    Login,
    CreateUser,
    DeleteUser,
    UpdateUser,
    RecordScore,
}
