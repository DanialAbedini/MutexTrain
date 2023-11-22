using System;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;

public static class SharedResource
{
    private static int counter=0;
    private static Mutex mutex = new Mutex();

   

    public static void Increment(int a)
    {
        try
        {
            mutex.WaitOne();
            counter += a;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    public static int show()
    {
        return counter;
    }
}

class Program
{
    static async Task Main()
    {
        var tasks1 = new List<Task>();
        var tasks2 = new List<Task>();
        var counter = 0;
        for (int m = 0; m < 10; m++)
        {
            Parallel.For(0, 100, i => {
                tasks1.Add(Task.Run(() => SharedResource.Increment(1)));

            });
            Parallel.For(0, 100, i => {
                tasks2.Add(Task.Run(() => SharedResource.Increment(1)));
            });
            await Task.WhenAll(tasks1);
            await Task.WhenAll(tasks2);

            Console.WriteLine($"Expected Answer: {counter+=200} Thread Anwer: {SharedResource.show()}");
        }
        Console.ReadLine();
    }
}