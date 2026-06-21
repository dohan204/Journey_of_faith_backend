using FirebaseAdmin.Auth;
using Journey_of_faith.Application.common.interfaces;

namespace Journey_of_faith.Infrastructure.services;
#nullable disable
public class FirebaseAuthService : IFirebaseAuthService
{
    public async Task<string?> VerifyIdTokenAsync(string idToken)
    {
        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;
        } catch
        {
            return null;
        }
    }
}