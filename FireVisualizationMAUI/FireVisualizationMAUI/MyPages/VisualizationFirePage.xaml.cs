
using Contract;
namespace FireVisualizationMAUI.MyPages;

public partial class VisualizationFirePage : ContentPage
{
    private Type _typeAssembly;
    private IField<ICell>? _field;
    private IProcessFire? _process;
    private int _sizeX;
    private int _sizeY;
    private ImageButton[,] _fireField;
    private bool _isSetStartBurning;
	public VisualizationFirePage(Type typeAssembly)
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
            frameFire.HeightRequest *= _sizeX;
            frameFire.WidthRequest *= _sizeY;
            GridField.BackgroundColor = Colors.Chocolate;
            frameFire.BackgroundColor = Colors.Chocolate;
            BuildField();
        }
        else
        {
            DisplayAlert("Notification", "No values entered", "OK");
        }
    }

    private void BuildField()
    {
        _fireField = new ImageButton[_sizeX, _sizeY];
        InitializeDataFromTypeAssembly();
        var field = _field!.GetField();
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
                    BackgroundColor = Colors.White
                };
                _fireField[i, j].Clicked += OnSetCellBurningButton;
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

    private async void OnSetCellBurningButton(object? obj, EventArgs e)
    {
        if (_isSetStartBurning)
        {
            await DisplayAlert("Notification", "Fire began/ended", "OK");
        }
        else
        {
            var btn = (ImageButton)obj;
            btn.Source = ImageSource.FromFile("burning.jpg");
            _isSetStartBurning = true;
            var starts = GetStartsValuesFromButton(btn);
            _field!.SetStartBurningCell(starts.x, starts.y);
            await Task.Delay(1500);
            StartFire();
        }
    }

    private (int x, int y) GetStartsValuesFromButton(ImageButton btn)
    {
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                if (_fireField[i, j] == btn)
                {
                    return (i, j);
                }
            }
        }
        return (0, 0);
    }
    private async void StartFire()
    {
        while (!_process!.CheckEndFire())
        {
            GridField.Clear();
            _process.UpdateFieldAfterFire();
            var field = _field.GetField();
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    if (field[i, j].State == CellState.Burning)
                    {
                        _fireField[i,j].Source = ImageSource.FromFile("burning.jpg");
                    }
                    if (field[i, j].State == CellState.Empty)
                    {
                        _fireField[i, j].Source = null;
                    }

                    GridField.Add(_fireField[i, j], j, i);
                }
            }

            await Task.Delay(2000);
        }
    }
}