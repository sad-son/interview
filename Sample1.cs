	static void Main()
    {
        int x = 42;
        Console.WriteLine($"Before: {x}");

        ChangeBoxed(x);
        Console.WriteLine($"After ChangeBoxed: {x}");
    }

    static void ChangeBoxed(object obj)
    {
        if (obj is int n)
        {
            n = 200; 
        }
    }