﻿using System;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;

public class SharedResource
{
    private static Semaphore semaphore = new Semaphore(1,1);
    private int counter=0;

   

    public void Increment(int a)
    {
        semaphore.WaitOne();
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
            semaphore.Release();
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
        var tasks = new List<Task>();
        for (int m = 0; m < 10; m++)
        {
            SharedResource sharedResource1 = new SharedResource();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => sharedResource1.Increment(1)));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine($"Expected Answer: {100} Thread Anwer: {sharedResource1.show()}");
        }
        Console.ReadLine();
    }
}