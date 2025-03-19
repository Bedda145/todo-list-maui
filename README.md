MainPage.xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="CalculatorApp2.MainPage"
             Title="CalculatorApp2">
    <ContentPage.Resources>
        <Style TargetType="Entry">
            <Setter Property="FontSize" Value="36"/>
        </Style>
        <Style TargetType="Button">
            <Setter Property="FontSize" Value="24"/>
        </Style>
    </ContentPage.Resources>
    <Grid RowSpacing="5" ColumnSpacing="5" Padding="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <Entry x:Name="DisplayEntry" Grid.Row="0" HeightRequest="80"
           HorizontalTextAlignment="End" VerticalOptions="Center"
           IsReadOnly="True" Text="0"/>
        <Grid Grid.Row="1">
            <Grid.RowDefinitions>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>

            <Button Text="C" Grid.Row="0" Grid.Column="0" Clicked="OnButtonClicked"/>
            <Button Text="⌫" Grid.Row="0" Grid.Column="1" Clicked="OnButtonClicked"/>
            <Button Text="%" Grid.Row="0" Grid.Column="2" Clicked="OnButtonClicked"/>
            <Button Text="÷" Grid.Row="0" Grid.Column="3" Clicked="OnButtonClicked"/>

            <Button Text="7" Grid.Row="1" Grid.Column="0" Clicked="OnButtonClicked"/>
            <Button Text="8" Grid.Row="1" Grid.Column="1" Clicked="OnButtonClicked"/>
            <Button Text="9" Grid.Row="1" Grid.Column="2" Clicked="OnButtonClicked"/>
            <Button Text="x" Grid.Row="1" Grid.Column="3" Clicked="OnButtonClicked"/>

            <Button Text="4" Grid.Row="2" Grid.Column="0" Clicked="OnButtonClicked"/>
            <Button Text="5" Grid.Row="2" Grid.Column="1" Clicked="OnButtonClicked"/>
            <Button Text="6" Grid.Row="2" Grid.Column="2" Clicked="OnButtonClicked"/>
            <Button Text="-" Grid.Row="2" Grid.Column="3" Clicked="OnButtonClicked"/>

            <Button Text="1" Grid.Row="3" Grid.Column="0" Clicked="OnButtonClicked"/>
            <Button Text="2" Grid.Row="3" Grid.Column="1" Clicked="OnButtonClicked"/>
            <Button Text="3" Grid.Row="3" Grid.Column="2" Clicked="OnButtonClicked"/>
            <Button Text="+" Grid.Row="3" Grid.Column="3" Clicked="OnButtonClicked"/>

            <Button Text="0" Grid.Row="4" Grid.Column="0" Clicked="OnButtonClicked"/>
            <Button Text="." Grid.Row="4" Grid.Column="1" Clicked="OnButtonClicked"/>
            <Button Text="±" Grid.Row="4" Grid.Column="2" Clicked="OnButtonClicked"/>
            <Button Text="=" Grid.Row="4" Grid.Column="3" Clicked="OnButtonClicked"/>
        </Grid>
    </Grid>
</ContentPage>
__________________________________________________________________________________________

MainPage.xaml.cs

using System.Globalization;

namespace CalculatorApp2
{
    public partial class MainPage : ContentPage
    {
        double storedValue = 0;
        string currentOperator = "";
        bool isNewEntry = true;

        public MainPage()
        {
            InitializeComponent();
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            string buttonText = pressedButton.Text;

            if (buttonText == "C")
            {
                ClearAll();
            }
            else if (buttonText == "⌫")
            {
                Backspace();
            }
            else if (buttonText == "=")
            {
                Calculate();
            }
            else if (buttonText == "±")
            {
                ToggleSign();
            }
            else if (buttonText == "%")
            {
                Percentage();
            }
            else
            {
                
                if (buttonText == "+")
                {
                    ProcessOperator(buttonText);
                }
                else if (buttonText == "-")
                {
                    ProcessOperator(buttonText);
                }
                else if (buttonText == "x")
                {
                    ProcessOperator(buttonText);
                }
                else if (buttonText == "÷")
                {
                    ProcessOperator(buttonText);
                }
                else
                {
                    ProcessDigit(buttonText);
                }
            }
        }

        void ProcessDigit(string digit)
        {
            if (isNewEntry)
            {
                DisplayEntry.Text = digit;
                isNewEntry = false;
            }
            else
            {
                if (DisplayEntry.Text == "0")
                {
                    DisplayEntry.Text = digit;
                }
                else
                {
                    DisplayEntry.Text = DisplayEntry.Text + digit;
                }
            }
        }

        void ProcessOperator(string op)
        {
            try
            {
                double currentValue = double.Parse(DisplayEntry.Text, CultureInfo.InvariantCulture);
                if (currentOperator != "")
                {
                    storedValue = PerformCalculation(storedValue, currentValue, currentOperator);
                    DisplayEntry.Text = storedValue.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    storedValue = currentValue;
                }
                currentOperator = op;
                isNewEntry = true;
            }
            catch
            {
                DisplayEntry.Text = "Error";
            }
        }

        void Calculate()
        {
            try
            {
                if (currentOperator == "")
                {
                    return;
                }
                double currentValue = double.Parse(DisplayEntry.Text, CultureInfo.InvariantCulture);
                double result = PerformCalculation(storedValue, currentValue, currentOperator);
                DisplayEntry.Text = result.ToString(CultureInfo.InvariantCulture);
                storedValue = result;
                currentOperator = "";
                isNewEntry = true;
            }
            catch
            {
                DisplayEntry.Text = "Error";
            }
        }

        double PerformCalculation(double a, double b, string op)
        {
            if (op == "+")
            {
                return a + b;
            }
            if (op == "-")
            {
                return a - b;
            }
            if (op == "x")
            {
                return a * b;
            }
            if (op == "÷")
            {
                return a / b;
            }
            return b;
        }

        void ClearAll()
        {
            DisplayEntry.Text = "0";
            storedValue = 0;
            currentOperator = "";
            isNewEntry = true;
        }

        void Backspace()
        {
            if (isNewEntry == false)
            {
                if (DisplayEntry.Text.Length > 0)
                {
                    DisplayEntry.Text = DisplayEntry.Text.Substring(0, DisplayEntry.Text.Length - 1);
                    if (DisplayEntry.Text == "")
                    {
                        DisplayEntry.Text = "0";
                        isNewEntry = true;
                    }
                }
            }
        }

        void ToggleSign()
        {
            try
            {
                double currentValue = double.Parse(DisplayEntry.Text, CultureInfo.InvariantCulture);
                currentValue = -currentValue;
                DisplayEntry.Text = currentValue.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                
            }
        }

        void Percentage()
        {
            try
            {
                double currentValue = double.Parse(DisplayEntry.Text, CultureInfo.InvariantCulture);
                currentValue = currentValue / 100;
                DisplayEntry.Text = currentValue.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                
            }
        }
    }
}
