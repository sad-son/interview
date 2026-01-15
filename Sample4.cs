		int[] numbers = { 10, 20, 30 };
        var actions = new List<Action>();

        foreach (var n in numbers)
        {
            actions.Add(() => Debug.Log($"foreach: n = {n}"));
        }

        foreach (var a in actions)
            a();