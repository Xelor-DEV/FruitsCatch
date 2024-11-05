using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DBConn : MonoBehaviour
{
    private string baseUrl = "http://localhost/";

    [SerializeField] private User user;
    [SerializeField] private ServerResponse response;
    [SerializeField] private OperationType selectedOperation;

    public OperationType SelectedOperation 
    { 
        get 
        { 
            return selectedOperation; 
        } 
        set 
        { 
            selectedOperation = value; 
        } 
    }

    private string GetUrl(OperationType operation)
    {
        switch (operation)
        {
            case OperationType.Login:
                return baseUrl + "user_login.php";
            case OperationType.CreateUser:
                return baseUrl + "db_create_user_game.php";
            case OperationType.DeleteUser:
                return baseUrl + "db_delete_user_game.php";
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
            default:
                return UnityWebRequest.kHttpVerbPOST;
        }
    }

    public void SetUsername(string username)
    {
        user.username = username;
    }

    public void SetPassword(string password)
    {
        user.password = password;
    }

    public void PerformOperation()
    {
        StartCoroutine(HandleRequest(selectedOperation));
    }

    private IEnumerator HandleRequest(OperationType operation)
    {
        string url = GetUrl(operation);
        string requestType = GetRequestType(operation);

        string jsonString = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest(url, requestType);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            response = JsonUtility.FromJson<ServerResponse>(responseText);
            Debug.Log(response.message);
        }
    }
}

[System.Serializable]
public class User
{
    public string username;
    public string password;
}

[System.Serializable]
public class ServerResponse
{
    public string message;
}

[System.Serializable]
public enum OperationType
{
    Login,
    CreateUser,
    DeleteUser,
    RecordScore,
}