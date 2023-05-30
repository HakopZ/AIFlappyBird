using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flappyBird
{
    public class ActivationFunction
    {
        private Func<double, double> function;
        private Func<double, double> derivative;
        public ActivationFunction(Func<double, double> func, Func<double, double> deriv)
        {
            function = func;
            derivative = deriv;
        }
        public double Function(double input)
        {
            return function.Invoke(input);
        }
        public double Derivative(double input)
        {
            return derivative.Invoke(input);
        }
    }
    public class ErrorFunction
    {
        private Func<double, double, double> function;
        private Func<double, double, double> derivative;
        public ErrorFunction(Func<double, double, double> func, Func<double, double, double> deriv)
        {
            function = func;
            derivative = deriv;
        }
        public double Funciton(double output, double desiredOutput)
        {
            return function.Invoke(output, desiredOutput);
        }
        public double Derivative(double output, double desiredOutput)
        {
            return derivative.Invoke(output, desiredOutput);
        }

    }
    public class ActivationAndErrorFunction
    {
        public static double BinaryStep(double input)
        {
            if (input > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static double Identity(double input)
        {
            return input;
        }

        public static double IdentityDerivative(double input)
        {
            return 1;
        }
        public static double TanHDerivative(double input)
        {
            return 1 - Math.Pow(Math.Tanh(input), 2);
        }
        public static ActivationFunction BinaryStepActivationFunction = new ActivationFunction(BinaryStep, (x => 0));
        public static ActivationFunction IdentityActivationFunction = new ActivationFunction(Identity, IdentityDerivative);
        public static ActivationFunction TanHActivationFunction = new ActivationFunction(Math.Tanh, TanHDerivative);
        public static double MeanAbsoluteError(double output, double desiredOutput)
        {
            return Math.Abs(desiredOutput - output);
        }
        public static double MeanSquaredError(double output, double desiredOutput)
        {
            return Math.Pow(desiredOutput - output, 2);
        }

        public static double MeanAbsoluteErrorDerivative(double output, double desiredOutput)
        {
            if (desiredOutput - output >= 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        public static double MeanSquaredErrorDerivative(double output, double desiredOutput)
        {
            return -2 * (desiredOutput - output);
        }

        public static ErrorFunction MeanAbsoluteErrorFunction = new ErrorFunction(MeanAbsoluteError, MeanAbsoluteErrorDerivative);
        public static ErrorFunction MeanSqauredErrorFunction = new ErrorFunction(MeanSquaredError, MeanSquaredErrorDerivative);
    }
}
