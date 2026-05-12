using System.Collections.Concurrent;

#pragma warning disable xUnit1051 // long-running stress test runs on its own threads; CancellationToken not needed

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void Stress_LockFreeReadsDoNotCrashUnderConcurrentWrites_AndFinalStateIsConsistent()
    {
        const int writerThreadCount = 4;
        const int readerThreadCount = 4;
        const int operationsPerWriter = 5_000;
        const int keySpace = 200;

        var dictionary = new ConcurrentBidirectionalDictionary<int, int>();
        using var stopReaders = new ManualResetEventSlim(false);

        var writerTasks = new Task[writerThreadCount];
        for (int t = 0; t < writerThreadCount; t++)
        {
            int seed = t;
            writerTasks[t] = Task.Run(() =>
            {
                var random = new Random(seed);
                for (int i = 0; i < operationsPerWriter; i++)
                {
                    int key = random.Next(keySpace);
                    int value = random.Next(keySpace);
                    switch (random.Next(4))
                    {
                        case 0:
                            dictionary.TryAdd(key, value);
                            break;
                        case 1:
                            dictionary.TryRemove(key, out _);
                            break;
                        case 2:
                            try { dictionary[key] = value; } catch (ArgumentException) { }
                            break;
                        case 3:
                            try { dictionary.AddOrUpdate(key, value, (_, _) => value); } catch (ArgumentException) { }
                            break;
                    }
                }
            });
        }

        var readerTasks = new Task[readerThreadCount];
        var readerExceptions = new ConcurrentBag<Exception>();
        for (int t = 0; t < readerThreadCount; t++)
        {
            int seed = 1000 + t;
            readerTasks[t] = Task.Run(() =>
            {
                try
                {
                    var random = new Random(seed);
                    while (!stopReaders.IsSet)
                    {
                        int key = random.Next(keySpace);
                        int value = random.Next(keySpace);
                        _ = dictionary.TryGetValue(key, out _);
                        _ = dictionary.ContainsKey(key);
                        _ = dictionary.ContainsValue(value);
                        _ = dictionary.Inverse.TryGetValue(value, out _);
                        _ = dictionary.Count;
                        foreach (var pair in dictionary.ToArray())
                        {
                            _ = pair.Key;
                            _ = pair.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    readerExceptions.Add(ex);
                }
            });
        }

        Task.WaitAll(writerTasks);
        stopReaders.Set();
        Task.WaitAll(readerTasks);

        Assert.Empty(readerExceptions);

        var forward = dictionary.ToArray();
        var inverse = dictionary.Inverse.ToArray();

        Assert.Equal(forward.Length, inverse.Length);
        Assert.Equal(forward.Length, dictionary.Count);
        Assert.Equal(forward.Length, forward.Select(p => p.Key).Distinct().Count());
        Assert.Equal(forward.Length, forward.Select(p => p.Value).Distinct().Count());

        foreach (var pair in forward)
        {
            Assert.True(dictionary.Inverse.TryGetValue(pair.Value, out int key));
            Assert.Equal(pair.Key, key);
        }

        foreach (var pair in inverse)
        {
            Assert.True(dictionary.TryGetValue(pair.Value, out int value));
            Assert.Equal(pair.Key, value);
        }
    }
}
