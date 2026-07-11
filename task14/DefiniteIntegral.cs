namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double totalSum = 0.0;

        int totalSteps = (int)Math.Ceiling((b - a) / step);
        int stepsPerThread = totalSteps / threadsNumber;
        int remainingSteps = totalSteps % threadsNumber;

        using Barrier barrier = new Barrier(threadsNumber + 1);

        for (int i = 0; i < threadsNumber; i++)
        {
            int currentThreadIndex = i;

            Thread thread = new Thread(() =>
            {
                int localSteps = stepsPerThread + (currentThreadIndex == threadsNumber - 1 ? remainingSteps : 0);
                double localA = a + currentThreadIndex * stepsPerThread * step;
                double localSum = 0.0;
                double initialValue, computedValue;
                
                for (int j = 0; j < localSteps; j++)
                {
                    double x1 = localA + j * step;
                    double x2 = x1 + step;
                    localSum += (function(x1) + function(x2)) / 2.0 * step;
                }

                do
                {
                    initialValue = totalSum;
                    computedValue = initialValue + localSum;
                }
                while (initialValue != Interlocked.CompareExchange(ref totalSum, computedValue, initialValue));
                

                barrier.SignalAndWait();

            });

            thread.Start();
        }

        barrier.SignalAndWait();
        return totalSum;
    }
}
