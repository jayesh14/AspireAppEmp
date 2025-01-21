using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AspireAppEmp.Web.Authentication
{
    public class CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token= (await localStorage.GetAsync<string>("authToken")).Value;
            var identity = string.IsNullOrEmpty(token)? new ClaimsIdentity() :  GetClaimsIdenity(token);
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public async Task MarkUserAuthenication(string token)
        {
            await localStorage.SetAsync("authToken", token);
            var identity= GetClaimsIdenity(token);
            var user=new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public ClaimsIdentity GetClaimsIdenity(string token)
        {
            var handler=new JwtSecurityTokenHandler();
            var jwtToken=handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            return  new ClaimsIdentity(claims,"jwt");
        }

        public async Task MarkUserAsLogout()
        {
            await localStorage.DeleteAsync("authToken");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
