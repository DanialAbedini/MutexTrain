using System;
using System.Threading;
using System.Threading.Channels;

public class SharedResource
{
    private static Mutex mutex;
    private int counter;

    
    public SharedResource()
    {
        mutex = new Mutex();
        counter = 0;
    }

    public void Increment(int a)
    {
        mutex.WaitOne();
        try
        {
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
    static void Main()
    {
        var tasks = new List<Task>();
        for (int m = 0; m < 10; m++)
        {
            SharedResource sharedResource1 = new SharedResource();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => sharedResource1.Increment(1)));                               
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Expected Answer: {100} Thread Anwer: {sharedResource1.show()}");
        }
        Console.ReadLine();
    }
}