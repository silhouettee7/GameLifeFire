namespace FireVisualizationMAUI.MyPages;
using Contract;
public partial class VisualizationPage : ContentPage
{
    private readonly IField<ICell>? _field;
    private readonly IProcessFire? _process;
    private readonly int _sizeX;
    private readonly int _sizeY;
    private ImageButton[,]? _fireField;
    private bool _isSetStartBurning;
    private bool _isFireStopped;
    private bool _isFireContinued;
    public VisualizationPage(int sizeX, int sizeY, IProcessFire process, IField<ICell> field)
	{
        _sizeX = sizeX;
        _sizeY = sizeY;
        _process = process;
        _field = field;
		InitializeComponent();
        FrameFire.WidthRequest *= sizeX;
        FrameFire.HeightRequest *= sizeY;
        BuildField();
	}
    private void BuildField()
    {
        _fireField = new ImageButton[_sizeX, _sizeY];
        var field = _field!.GetField();
        for (int i = 0; i < _sizeY; i++)
        {
            GridField.RowDefinitions.Add(new RowDefinition{Height = new GridLength(80)});
        }

        for (int j = 0; j < _sizeX; j++)
        {
            GridField.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(80)});
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
                _fireField[i, j].Clicked += OnSetCellBurningButtonClick;
                if (field[i, j].State == CellState.Bush)
                {
                    _fireField[i, j].Source = ImageSource.FromFile("bush.png");
                }
                else if (field[i, j].State == CellState.Tree)
                {
                    _fireField[i, j].Source = ImageSource.FromFile("tree.png");
                }
                GridField.Add(_fireField[i, j], i, j);
            }
        }
    }
    private async void OnSetCellBurningButtonClick(object? obj, EventArgs e)
    {
        if (_isSetStartBurning)
        {
            await DisplayAlert("Notification", "Fire began/ended", "OK");
        }
        else
        {
            _isSetStartBurning = true;
            
            var btn = (ImageButton)obj!;
            btn!.Source = ImageSource.FromFile("burning.jpg");
            
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
                if (_fireField![i, j] == btn)
                {
                    return (i, j);
                }
            }
        }
        return (0, 0);
    }
    private async void StartFire()
    {
        while (!_process!.CheckEndFire() && !_isFireStopped)
        {
            GridField.Clear();
            _process.UpdateFieldAfterFire();
            var field = _field!.GetField();
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    if (field[i, j].State == CellState.Burning)
                    {
                        _fireField![i, j].Source = ImageSource.FromFile("burning.jpg");
                    }
                    if (field[i, j].State == CellState.Empty)
                    {
                        _fireField![i, j].Source = null;
                    }

                    GridField.Add(_fireField![i, j], i, j);
                }
            }
            await Task.Delay(2000);
        }
    }

    private void OnStopFireButtonClicked(object? sender, EventArgs e)
    {
        _isFireStopped = true;
        _isFireContinued = false;
    }

    private void OnContinueButtonClick(object? sender, EventArgs e)
    {
        _isFireStopped = false;
        if (!_isFireContinued)
        {
            StartFire();
        }
        _isFireContinued = true;
    }
}