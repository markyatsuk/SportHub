namespace SportHub.Models.ViewModels;

// model for pagination
public class PageInfoViewModel
{
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => this.ItemsPerPage == 0 ? 0 : (int)Math.Ceiling((decimal)this.TotalItems / this.ItemsPerPage);
}
