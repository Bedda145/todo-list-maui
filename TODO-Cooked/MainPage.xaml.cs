using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace TODO_Cooked;

public partial class MainPage : ContentPage
{
    // TodoItem model för att hantera kategorier
    public class TodoItem
    {
        public string Text { get; set; }
        public string Category { get; set; }
        public string CategoryColor { get; set; }

        public TodoItem(string text, string category)
        {
            Text = text;
            Category = category;
            CategoryColor = GetColorForCategory(category);
        }

        // Ger en färg baserat på kategori med förbättrad synlighet
        private string GetColorForCategory(string category)
        {
            return category switch
            {
                "Arbete" => "#1976D2",      // Mörkblå
                "Personligt" => "#D32F2F",  // Mörkröd
                "Inköp" => "#F57C00",       // Orange
                "Viktigt" => "#388E3C",     // Mörkgrön
                _ => "#616161",             // Mörkgrå för övrigt
            };
        }
    }

    // Samlingar för uppgifter
    public ObservableCollection<TodoItem> Items { get; set; } = new();
    public ObservableCollection<TodoItem> FilteredItems { get; set; } = new();

    // Command för att hantera ta bort via swipe eller knapp
    public ICommand DeleteCommand { get; private set; }

    // Kategori-konstanter
    private readonly List<string> _categories = new()
    {
        "Arbete",
        "Personligt",
        "Inköp",
        "Viktigt"
    };

    // Filnamn för permanent lagring
    private readonly string _fileName = "todoitems.json";

    public MainPage()
    {
        InitializeComponent();

        // Skapa command för att hantera borttagning
        DeleteCommand = new Command<TodoItem>(DeleteItem);

        BindingContext = this;

        // Konfigurera kategoripickers
        SetupPickers();

        // Ladda sparade uppgifter
        LoadItems();

        // Visa alla uppgifter initialt
        UpdateFilteredItems();
    }

    private void SetupPickers()
    {
        // Konfigurera kategorival
        foreach (var category in _categories)
        {
            CategoryPicker.Items.Add(category);
        }
        CategoryPicker.SelectedIndex = 0;

        // Konfigurera filterval
        FilterPicker.Items.Add("Alla");
        foreach (var category in _categories)
        {
            FilterPicker.Items.Add(category);
        }
        FilterPicker.SelectedIndex = 0;
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(AddListItem.Text))
        {
            // Skapa ny uppgift med vald kategori
            string selectedCategory = CategoryPicker.SelectedItem?.ToString() ?? "Övrigt";
            var newItem = new TodoItem(AddListItem.Text, selectedCategory);

            // Lägg till i samlingen
            Items.Add(newItem);

            // Uppdatera filtrerade uppgifter
            UpdateFilteredItems();

            // Spara uppgifterna
            SaveItems();

            // Rensa inmatningsfältet
            AddListItem.Text = "";
        }
    }

    // Metod för att ta bort via kommando (används för både SwipeView och knappen)
    private void DeleteItem(TodoItem item)
    {
        if (item != null && Items.Contains(item))
        {
            // Visa bekräftelsedialog
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                bool confirm = await DisplayAlert("Bekräfta borttagning",
                    $"Är du säker på att du vill ta bort \"{item.Text}\"?",
                    "Ja", "Avbryt");

                if (confirm)
                {
                    Items.Remove(item);
                    UpdateFilteredItems();
                    SaveItems();
                }
            });
        }
    }

    private void OnFilterChanged(object sender, EventArgs e)
    {
        UpdateFilteredItems();
    }

    private void UpdateFilteredItems()
    {
        // Rensa befintliga filtrerade uppgifter
        FilteredItems.Clear();

        // Filtrera baserat på valt filter
        if (FilterPicker.SelectedIndex == 0)
        {
            // Visa alla uppgifter
            foreach (var item in Items)
            {
                FilteredItems.Add(item);
            }
        }
        else
        {
            // Filtrera efter specifik kategori
            string selectedCategory = FilterPicker.SelectedItem?.ToString();
            foreach (var item in Items.Where(i => i.Category == selectedCategory))
            {
                FilteredItems.Add(item);
            }
        }
    }

    // Spara uppgifter till fil
    private void SaveItems()
    {
        try
        {
            string appDataPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(appDataPath, _fileName);

            // Skapa JSON från samlingen
            string jsonData = JsonSerializer.Serialize(Items);

            // Spara filen
            File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            // Visa felmeddelande till användaren vid problem
            DisplayAlert("Fel vid sparande", $"Det gick inte att spara uppgifterna: {ex.Message}", "OK");
        }
    }

    // Ladda uppgifter från fil
    private void LoadItems()
    {
        try
        {
            string appDataPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(appDataPath, _fileName);

            // Kontrollera om filen existerar
            if (File.Exists(filePath))
            {
                // Läs JSON-data
                string jsonData = File.ReadAllText(filePath);

                // Konvertera till objekt
                var savedItems = JsonSerializer.Deserialize<List<TodoItem>>(jsonData);

                // Lägg till i samlingen
                if (savedItems != null)
                {
                    Items.Clear();
                    foreach (var item in savedItems)
                    {
                        Items.Add(item);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Visa felmeddelande vid problem
            DisplayAlert("Fel vid inläsning", $"Det gick inte att läsa in sparade uppgifter: {ex.Message}", "OK");
        }
    }
}
