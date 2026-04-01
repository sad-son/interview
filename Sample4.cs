		int[] numbers = { 10, 20, 30 };
        IList<Action> actions = new List<Action>();

        foreach (var n in numbers)
        {
            actions.Add(() => Debug.Log($"foreach: n = {n}"));
        }

        foreach (var a in actions)
            a();