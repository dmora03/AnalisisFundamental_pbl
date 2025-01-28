LEEME

1. BootStrp/Bootstrapper actualizar
	- SelectAssemblies:			Solo si los Views y ViewModels estan en otros ensamblados que no es el de arranque
	- SpecialViewModelMappings:	Solo si existen Views y ViewModels que no siguen el nombre estandar y necesitan ser mapeados
	- ConfigureForRuntime:		Registrar la INTERFAZ con la CLASE que la implementara en el modo de Ejecución
	- ConfigureForDesignTime:	Registrar la INTERFAZ con la CLASE que la implementara en el modo de Diseño

2. DesignTimeViewModel			Poner las propiedades de cada view para ver información dummy de esta en tiempo de diseño

3. ShellView.xaml				Agregar los DataTemplates de cada View-ViewModel que seran mostrados en el ShellView

4. Por cada VIEW nuevo			Crear su clase xxxViewModel y hacerla publica (o usar el Template NameViewModel)
	- Agregar codigo XML:		
								xmlns:conv="clr-namespace:WPF.Converters"
								xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
								xmlns:mvvm="clr-namespace:MvvmFramework;assembly=MvvmFramework"
								d:DataContext="{d:DesignInstance Type=local:NameViewModel}"

	- d:DataContext:			Este puede ser una de estas dos formas:
					="{d:DesignInstance Type=local:DesignTimeViewModel, IsDesignTimeCreatable=True}"	Se podra ver en tiempo de Diseño los valores dummy, PERO el intellisense NO funcionara
					="{d:DesignInstance Type=local:NameViewModel}"										NO se podran ver valores dummy en tiempo de Diseño, PERO el intellisense SI funcionara









TODO: Agregar a templates
1. Carpeta de Bahaviors
2. Template de Behaviours (evaluar si conviene o no)
3. En el XML agregar la referencia a la carpeta Behaviors
	En el XML pero de codigo C# comentar linea 54 para no generar errores al agregar uno nuevo (_ = windowManager.ShowDialog(IoC.Get<PopViewModel>());)

4. Actualizar template de Convertidores para que mueste que los objetos pueden recibir o devolver NULL y cambiar la variable parameter por converterParameter
		public object? Convert(object? value, Type targetType, object? converterParameter, CultureInfo culture)
5. Agregar Carpeta ExtensionMethods
6. Template de ExtensionMethod (evaluar si conviene o no)
7. Agregar en README las opciones para Compilar la applicacion y distribuirla
		Dotnet publish -c Release --self-contained -r win-x64 -p:PublishSingleFile=true

8. Agregar al TEMPLATE MVVM en el ShellView y ShellViewModel el codigo para guardar Settings del usuario
		xmlns:p="clr-namespace:WPF.Properties"
		Top="{Binding Source={x:Static p:Settings.Default}, Path=Shell_Top, Mode=TwoWay}"
		<i:Interaction.Triggers>
			<i:EventTrigger EventName="Closing">
				<i:CallMethodAction TargetObject="{Binding}" MethodName="OnClosing"/>
			</i:EventTrigger>
		</i:Interaction.Triggers>
		public static void OnClosing() { Properties.Settings.Default.Save(); }










ACTUALIZACIONES DEL PROGRAMA ANALISIS FUNDAMENTAL
2024/08/07	Se mejoro la seleccion de los textboxes para buscar cuando el delimitador se repite dentro del mismo Ej. Hola [como [estan] todos] ustedes
2024/09/04	Se implemento que en el campo de Precio de la accion al presionar CTRL+F usa la API de Yahoo para agregar 
			el precio de cierre del ultimo dia habil del trimestre y abre la pagina de yahoo del historial para verificar que se agrego el dato correcto
