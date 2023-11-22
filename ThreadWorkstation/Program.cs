using System;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;

public class SharedResource
{
    private int counter=0;
    private  Mutex mutex = new Mutex();

   

    public void Increment(int a)
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

    public int show()
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

        for (int m = 0; m < 10; m++)
        {
            SharedResource sharedResource1 = new SharedResource();
            Parallel.For(0, 100, i => {
                tasks1.Add(Task.Run(() => sharedResource1.Increment(1)));
            });
            Parallel.For(0, 100, i => {
                tasks2.Add(Task.Run(() => sharedResource1.Increment(1)));
            });
            await Task.WhenAll(tasks1);
            await Task.WhenAll(tasks2);

            Console.WriteLine($"Expected Answer: {200} Thread Anwer: {sharedResource1.show()}");
        }
        Console.ReadLine();
    }
}