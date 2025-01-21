using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AspireAppEmp.Web.Authentication
{
    public abstract class AuthorizedPageBase : ComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected abstract string RequiredRole { get;}

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity?.IsAuthenticated ?? true ||
                (!string.IsNullOrEmpty(RequiredRole) && !user.IsInRole(RequiredRole)))
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}
