using System.Reflection;

namespace FireVisualizationMAUI.MyPages;

public partial class SettingFieldParamsPage : ContentPage
{
    private Type _typeAssembly;
	public SettingFieldParamsPage(Type typeAssembly)
	{
		_typeAssembly = typeAssembly;
		InitializeComponent();
	}

}