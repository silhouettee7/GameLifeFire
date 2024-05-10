
using Contract;
namespace FireVisualizationMAUI.MyPages;

public partial class SettingValuesPage : ContentPage
{
    private Type _typeAssembly;
    private IField<ICell>? _field;
    private IProcessFire? _process;
    private int _sizeX;
    private int _sizeY;

	public SettingValuesPage(Type typeAssembly)
	{
		_typeAssembly = typeAssembly;
		InitializeComponent();
	}

    private void RowsTextChanged(object? obj, EventArgs? e)
    {
        int.TryParse(RowsEntry.Text, out _sizeX);
    }

    private void ColumnTextChanged(object? obj, EventArgs? e)
    {
        int.TryParse(ColumnsEntry.Text, out _sizeY);
    }

    private void OnBuildFieldButtonClick(object? obj, EventArgs e)
    {
        if (_sizeX > 0 && _sizeY > 0)
        {
            InitializeDataFromTypeAssembly();
            Navigation.PushAsync(new VisualizationPage(_sizeX,_sizeY, _process!, _field!));
        }
        else
        {
            DisplayAlert("Notification", "No values entered", "OK");
        }
    }

    private void InitializeDataFromTypeAssembly()
    {
        var instance = Activator.CreateInstance(_typeAssembly, new object[]{_sizeX,_sizeY});
        
        if (instance == null)
        {
            throw new NullReferenceException();
        }
        
        _field = (IField<ICell>)instance;
        _process = (IProcessFire)instance;
        _field.Initialize();
    }

    private void OnBuildButtonPressed(object? sender, EventArgs e)
    {
        var btn = (Button)sender;
        btn.HeightRequest -= 7;
        btn.WidthRequest -= 12;
    }

    private void OnBuildButtonReleased(object? sender, EventArgs e)
    {
        var btn = (Button)sender;
        btn.HeightRequest += 7;
        btn.WidthRequest += 12;
    }
}