using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flappy_Pony
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PolinomioPage : ContentPage
    {
        private Polinomio polinomioActual;

        public PolinomioPage()
        {
            InitializeComponent();
            polinomioActual = new Polinomio();
        }

        private void OnAgregarTerminoClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EntryCoeficiente.Text) || 
                    string.IsNullOrWhiteSpace(EntryExponente.Text))
                {
                    DisplayAlert("Error", "Por favor, ingrese tanto el coeficiente como el exponente.", "OK");
                    return;
                }

                double coeficiente = double.Parse(EntryCoeficiente.Text);
                int exponente = int.Parse(EntryExponente.Text);

                if (exponente < 0)
                {
                    DisplayAlert("Error", "El exponente debe ser un número entero no negativo.", "OK");
                    return;
                }

                polinomioActual.AgregarTermino(coeficiente, exponente);
                ActualizarVisualizacionPolinomio();

                // Limpiar campos
                EntryCoeficiente.Text = string.Empty;
                EntryExponente.Text = string.Empty;

                DisplayAlert("Éxito", "Término agregado al polinomio.", "OK");
            }
            catch (FormatException)
            {
                DisplayAlert("Error", "Por favor, ingrese valores numéricos válidos.", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al agregar término: {ex.Message}", "OK");
            }
        }

        private void OnLimpiarClicked(object sender, EventArgs e)
        {
            polinomioActual.Limpiar();
            ActualizarVisualizacionPolinomio();
            LabelResultadoEvaluacion.Text = "Resultado aparecerá aquí";
            DisplayAlert("Información", "Polinomio limpiado.", "OK");
        }

        private void OnMostrarClicked(object sender, EventArgs e)
        {
            if (polinomioActual.EstaVacio())
            {
                DisplayAlert("Información", "No hay polinomio creado.", "OK");
                return;
            }

            DisplayAlert("Polinomio Actual", polinomioActual.ToString(), "OK");
        }

        private void OnDerivarClicked(object sender, EventArgs e)
        {
            if (polinomioActual.EstaVacio())
            {
                DisplayAlert("Error", "No hay polinomio para derivar. Cree un polinomio primero.", "OK");
                return;
            }

            var derivada = polinomioActual.Derivar();
            polinomioActual = derivada;
            ActualizarVisualizacionPolinomio();
            DisplayAlert("Derivada", $"Derivada calculada: {derivada.ToString()}", "OK");
        }

        private void OnEvaluarClicked(object sender, EventArgs e)
        {
            try
            {
                if (polinomioActual.EstaVacio())
                {
                    DisplayAlert("Error", "No hay polinomio para evaluar. Cree un polinomio primero.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(EntryValorX.Text))
                {
                    DisplayAlert("Error", "Por favor, ingrese un valor para x.", "OK");
                    return;
                }

                double valorX = double.Parse(EntryValorX.Text);
                double resultado = polinomioActual.Evaluar(valorX);

                LabelResultadoEvaluacion.Text = $"P({valorX}) = {resultado:F4}";
                
                DisplayAlert("Resultado de Evaluación", 
                    $"Para x = {valorX}\nP(x) = {resultado:F4}", "OK");
            }
            catch (FormatException)
            {
                DisplayAlert("Error", "Por favor, ingrese un valor numérico válido para x.", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al evaluar: {ex.Message}", "OK");
            }
        }

        private void OnVolverJuegoClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ActualizarVisualizacionPolinomio()
        {
            if (polinomioActual.EstaVacio())
            {
                LabelPolinomio.Text = "No hay polinomio creado";
            }
            else
            {
                LabelPolinomio.Text = polinomioActual.ToString();
            }
        }
    }
}