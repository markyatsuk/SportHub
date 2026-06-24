namespace SportHub.Models.ViewModels;

// view model to pass Cart object and ReturnUrl (to have an ability to redirect on page we came from, before entering the controller) inside views 
public class CartViewModel
{
    public Cart? Cart { get; set; } = new();
    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}