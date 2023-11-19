using System;
using System.Threading;
using System.Threading.Channels;

public class SharedResource
{
    private Mutex mutex;
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
        for (int m = 0; m < 100; m++)
        {
            SharedResource sharedResource1 = new SharedResource();
            for (int i = 1; i <= 100; i++)
            {
                   var th = new Thread(() =>
                   {
                        sharedResource1.Increment(i);
                    });
                   
                   th.Start();

            }

            Console.WriteLine($"Expected Answer: {5050} Thread Anwer: {sharedResource1.show()}");
        }
        Console.ReadLine();
    }
}