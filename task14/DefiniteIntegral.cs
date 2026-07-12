namespace task14;

public class DefiniteIntegral
{
    public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
    {
        double totalSum = 0.0;
        int totalSteps = (int)Math.Ceiling((b - a) / step);
        
        for (int i = 0; i < totalSteps; i++)
        {
            double x1 = a + i * step;
            double x2 = x1 + step;
            totalSum += (function(x1) + function(x2)) / 2.0 * step;
        }
        
        return totalSum;
    }

    public static double SolveMultiThread(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double totalSum = 0.0;

        int totalSteps = (int)Math.Ceiling((b - a) / step);
        int stepsPerThread = totalSteps / threadsNumber;
        int remainingSteps = totalSteps % threadsNumber;

        Thread[] threads = new Thread[threadsNumber];

        for (int i = 0; i < threadsNumber; i++)
        {
            int currentThreadIndex = i;

            threads[i] = new Thread(() =>
            {
                int localSteps = stepsPerThread + (currentThreadIndex == threadsNumber - 1 ? remainingSteps : 0);
                
                int startIndex = currentThreadIndex * stepsPerThread;
                double localA = a + startIndex * step;
                double localSum = 0.0;
                
                for (int j = 0; j < localSteps; j++)
                {
                    double x1 = localA + j * step;
                    double x2 = x1 + step;
                    localSum += (function(x1) + function(x2)) / 2.0 * step;
                }

                double initialValue, computedValue;
                do
                {
                    initialValue = totalSum;
                    computedValue = initialValue + localSum;
                }
                while (initialValue != Interlocked.CompareExchange(ref totalSum, computedValue, initialValue));
            });

            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return totalSum;
    }
}