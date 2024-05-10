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
    private async void BuildField()
    {
        _fireField = new ImageButton[_sizeX, _sizeY];
        var field = _field!.GetField();
        InitializeField();
        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                _fireField[i, j].Margin = new Thickness(5);
                _fireField[i, j].BackgroundColor = Colors.White;
                _fireField[i, j].Clicked += OnSetCellBurningButtonClick;
                await Task.Run(() =>
                {
                    Dispatcher.DispatchAsync(() => 
                        _fireField[i, j].Source = field[i, j].State == CellState.Tree ? ImageSource.FromFile("tree.png")
                            : field[i, j].State == CellState.Bush ? ImageSource.FromFile("bush.png") 
                            : null);
                });
                await Task.Delay(30);
            }
        }
    }

    private async void InitializeField()
    {
        for (int i = 0; i < _sizeY; i++)
        {
            GridField.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
        }

        for (int j = 0; j < _sizeX; j++)
        {
            GridField.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
        }

        for (int i = 0; i < _sizeX; i++)
        {
            for (int j = 0; j < _sizeY; j++)
            {
                _fireField[i, j] = new ImageButton();
                _fireField[i, j].BorderColor = Colors.Black;
                _fireField[i, j].BorderWidth = 1;
                _fireField[i, j].Pressed += OnImageButtonPressed;
                _fireField[i, j].Released += OnImageButtonReleased;
                GridField.Add(_fireField[i, j], i, j);
            }
        }

        await Task.Delay(1000);
    }
    private async void OnSetCellBurningButtonClick(object? obj, EventArgs e)
    {
        if (_isSetStartBurning)
        {
            await DisplayAlert("Notification", "Fire began/ended", "OK");
        }
        else
        {
            await Task.Delay(50);
            var btn = (ImageButton)obj!;
            var starts = GetStartsValuesFromButton(btn);

            if (_field.GetField()[starts.x, starts.y].State == CellState.Empty)
            {
                await Task.Delay(50);
            }
            else
            {
                _isSetStartBurning = true;

                btn.Source = ImageSource.FromFile("burning.jpg");

                _field!.SetStartBurningCell(starts.x, starts.y);

                await Task.Delay(1500);

                StartFire();
            }
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
                }
            }
            await Task.Delay(1200);
        }
    }

    private void OnImageButtonPressed(object? sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        btn.Margin = 8;
    }
    private void OnImageButtonReleased(object? sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        btn.Margin = 5;
    }
    private void OnStopFireButtonClicked(object? sender, EventArgs e)
    {
        _isFireStopped = true;
        _isFireContinued = false;
        ContinueFire.Clicked += OnContinueButtonClick;
    }

    private void OnContinueButtonClick(object? sender, EventArgs e)
    {
        _isFireStopped = false;
        if (!_isFireContinued )
        {
            StartFire();
        }
        _isFireContinued = true;
    }

    private void OnStopFireButtonPressed(object? sender, EventArgs e)
    {
        var btn = (Button)sender!;
        btn.HeightRequest -= 5;
        btn.WidthRequest -= 10;
    }
    private void OnStopFireButtonReleased(object? sender, EventArgs e)
    {
        var btn = (Button)sender!;
        btn.HeightRequest += 5;
        btn.WidthRequest += 10;
    }
    private void OnContinueFireButtonPressed(object? sender, EventArgs e)
    {
        var btn = (Button)sender!;
        btn.HeightRequest -= 5;
        btn.WidthRequest -= 10;
    }
    private void OnContinueFireButtonReleased(object? sender, EventArgs e)
    {
        var btn = (Button)sender!;
        btn.HeightRequest += 5;
        btn.WidthRequest += 10;
    }
    private void OnChangeThemeFireButtonPressed(object? sender, EventArgs e)
    {
        var btn = (Button)sender!;
        btn.HeightRequest -= 5;
        btn.WidthRequest -= 10;
    }
    private void OnChangeThemeFireButtonReleased(object? sender, EventArgs e)
    {
        var btn = (Button)sender!;
        btn.HeightRequest += 5;
        btn.WidthRequest += 10;
    }
    private void ChangeTheme(object sender, EventArgs e)
    {
        if (ThemeManager.SelectedTheme == nameof(FireVisualizationMAUI.Resources.Themes.Dark))
        {
            ThemeManager.SetTheme(nameof(FireVisualizationMAUI.Resources.Themes.Light));
        }
        else
        {
            ThemeManager.SetTheme(nameof(FireVisualizationMAUI.Resources.Themes.Dark));
        }
    }

}