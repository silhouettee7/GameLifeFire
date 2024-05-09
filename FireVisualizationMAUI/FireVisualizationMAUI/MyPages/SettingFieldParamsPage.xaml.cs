using System.Reflection;
using Contract;
namespace FireVisualizationMAUI.MyPages;

public partial class SettingFieldParamsPage : ContentPage
{
    private Type _typeAssembly;
    private int _sizeX;
    private int _sizeY;
    private ImageButton[,] _fireField;
	public SettingFieldParamsPage(Type typeAssembly)
	{
		_typeAssembly = typeAssembly;
		InitializeComponent();
	}

    private void RowsCompleted(object? obj, EventArgs? e)
    {
        int.TryParse(RowsEntry.Text, out _sizeX);
    }

    private void ColumnCompleted(object? obj, EventArgs? e)
    {
        int.TryParse(ColumnsEntry.Text, out _sizeY);
    }

    private void OnBuildFieldButtonClick(object? obj, EventArgs e)
    {
        if (_sizeX > 0 && _sizeY > 0)
        {
            BuildField();
        }
        else
        {
            DisplayAlert("Notification", "Parametrs no", "OK");
        }
    }

    private void BuildField()
    {
        _fireField = new ImageButton[_sizeX, _sizeY];
        var field = GetDataFromTypeAssembly();
        for (int i = 0; i < _sizeX; i++)
        {
            GridField.RowDefinitions.Add(new RowDefinition());
        }

        for (int j = 0; j < _sizeY; j++)
        {
            GridField.ColumnDefinitions.Add(new ColumnDefinition());
        }

        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                _fireField[i, j] = new ImageButton()
                {
                    Margin = new Thickness(5),
                    BorderWidth = 0.5,
                    BorderColor = Colors.Black
                };
                if (field[i, j].State == CellState.Bush)
                {
                    _fireField[i, j].Source = ImageSource.FromFile("bush.png");
                }
                else if (field[i, j].State == CellState.Tree)
                {
                    _fireField[i, j].Source = ImageSource.FromFile("tree.png");
                }
                GridField.Add(_fireField[i, j], j, i);
            }
        }
    }

    private ICell[,] GetDataFromTypeAssembly()
    {
        var fieldInstance = Activator.CreateInstance(_typeAssembly, new object[]{_sizeX,_sizeY});
        var initialize = _typeAssembly.GetMethod("Initialize");
        initialize.Invoke(fieldInstance, null);
        var field = _typeAssembly.GetField("_field", BindingFlags.Instance | BindingFlags.NonPublic);
        var res = (ICell[,])field.GetValue(fieldInstance);

        return res;
    }

    private void OnSetCellBurningButton(object? obj, EventArgs e)
    {

    }
}