using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    private async void Start ()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SignIn ()
    {
        await AnonymousSignIn();
    }

    private async Task AnonymousSignIn ()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
            Logger.ScreenLog("Authentication Manager", string.Concat("Player ID: " + AuthenticationService.Instance.PlayerId));

        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
    }

}
