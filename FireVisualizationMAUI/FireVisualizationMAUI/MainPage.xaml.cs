using System.Reflection;
using FireVisualizationMAUI.MyPages;
using Contract;

namespace FireVisualizationMAUI
{
    public partial class MainPage : ContentPage
    {
        private Assembly? _assembly;
        private Type? _typeAssembly;
        public MainPage()
        {
            InitializeComponent();
            OnAssemblyButtonClick();
        }

        private async void ToSettingFieldParamsPage(object? sender, EventArgs? e)
        {
            await Navigation.PushAsync(new SettingValuesPage(_typeAssembly!));
        }

        private void OnAssemblyButtonClick()
        {
            AssemblyButton.Clicked += (sender,e) =>
            {
                ChooseAssembly();
            };
        }
        private async void ChooseAssembly()
        {
            var fileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>()
                {
                    {DevicePlatform.WinUI, new []{".dll"}}
                });
            PickOptions options = new()
            {
                PickerTitle = "Choose assembly",
                FileTypes = fileType
            };
            var res = await FilePicker.PickAsync(options);
            string? pathAssembly = res?.FullPath;
            InitializeAssembly(pathAssembly);
        }

        private void InitializeAssembly(string? path)
        {
            if (path == null)
            {
                DisplayAlert("Notification", "File not selected", "OK");
            }
            else
            {
                var asm = Assembly.LoadFrom(path);
                _assembly = asm;
                bool isAssemblyCorrect = CheckAssemblyWithContract();
                
                if (isAssemblyCorrect)
                {
                    BtnStart.IsEnabled = true;
                }
                else
                {
                    DisplayAlert("Error", "Сould not find implementation IProcessFire", "OK");
                    BtnStart.IsEnabled = false;
                }
            }
        }

        private bool CheckAssemblyWithContract()
        {
            var types = _assembly!.GetTypes();
            _typeAssembly = types.FirstOrDefault(type =>
                type.IsClass &&
                type.GetInterfaces()
                    .Count(_face => _face.Name == typeof(IProcessFire).Name ||
                                    (_face.IsGenericType && _face.Name == typeof(IField<>).Name)) == 2);
            
            if (_typeAssembly == null)
            {
                return false;
            }
            return true;
        }
    }

}
