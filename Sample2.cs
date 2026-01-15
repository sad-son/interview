	static void Main()
    {
        int? a = 42;
        int? b = null;

        Test(a);
        Test(b);
    }

    static void Test(int? n)
    {
        Console.WriteLine($"HasValue={n.HasValue}, Value={ (n.HasValue ? n.Value.ToString() : "null") }");
    }