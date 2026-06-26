using SportHub.Models.Domain;

namespace SportHub.Models.ViewModels;

// view model to pass Cart object and ReturnUrl (to have an ability to redirect on page we came from, before entering the controller) inside views 
public class CartViewModel
{
    public Cart? Cart { get; init; }
    public Uri ReturnUrl { get; init; } = new Uri("/", UriKind.Relative);
}