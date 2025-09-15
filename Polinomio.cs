using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flappy_Pony
{
    public class Polinomio
    {
        private List<double> coeficientes;
        
        public Polinomio()
        {
            coeficientes = new List<double>();
        }
        
        public Polinomio(params double[] coefs)
        {
            coeficientes = new List<double>(coefs);
            EliminarCerosInnecesarios();
        }
        
        public void AgregarTermino(double coeficiente, int exponente)
        {
            // Expandir la lista si es necesario
            while (coeficientes.Count <= exponente)
            {
                coeficientes.Add(0);
            }
            
            coeficientes[exponente] = coeficiente;
            EliminarCerosInnecesarios();
        }
        
        public double Evaluar(double x)
        {
            double resultado = 0;
            for (int i = 0; i < coeficientes.Count; i++)
            {
                resultado += coeficientes[i] * Math.Pow(x, i);
            }
            return resultado;
        }
        
        public Polinomio Derivar()
        {
            if (coeficientes.Count <= 1)
            {
                return new Polinomio(0);
            }
            
            var nuevosCoefs = new double[coeficientes.Count - 1];
            for (int i = 1; i < coeficientes.Count; i++)
            {
                nuevosCoefs[i - 1] = coeficientes[i] * i;
            }
            
            return new Polinomio(nuevosCoefs);
        }
        
        public void Limpiar()
        {
            coeficientes.Clear();
        }
        
        public bool EstaVacio()
        {
            return coeficientes.Count == 0 || coeficientes.All(c => c == 0);
        }
        
        private void EliminarCerosInnecesarios()
        {
            while (coeficientes.Count > 0 && coeficientes[coeficientes.Count - 1] == 0)
            {
                coeficientes.RemoveAt(coeficientes.Count - 1);
            }
        }
        
        public override string ToString()
        {
            if (EstaVacio())
            {
                return "0";
            }
            
            var sb = new StringBuilder();
            bool primerTermino = true;
            
            for (int i = coeficientes.Count - 1; i >= 0; i--)
            {
                if (coeficientes[i] == 0) continue;
                
                if (!primerTermino)
                {
                    sb.Append(coeficientes[i] > 0 ? " + " : " - ");
                    sb.Append(Math.Abs(coeficientes[i]));
                }
                else
                {
                    sb.Append(coeficientes[i]);
                    primerTermino = false;
                }
                
                if (i > 1)
                {
                    sb.Append($"x^{i}");
                }
                else if (i == 1)
                {
                    sb.Append("x");
                }
            }
            
            return sb.ToString();
        }
    }
}