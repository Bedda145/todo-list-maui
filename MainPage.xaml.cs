using System.Collections.ObjectModel;

namespace TodoApp;

public partial class MainPage : ContentPage
{
    public ObservableCollection<string> Items { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this; 
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(AddListItem.Text))
        {
            Items.Add(AddListItem.Text);
            AddListItem.Text = "";
        }
    }

    private void OnDeleteSwipe(object sender, EventArgs e)
    {
        var item = (sender as SwipeItem)?.BindingContext as string;
        if (item != null && Items.Contains(item))
        {
            Items.Remove(item);
        }
    }
}