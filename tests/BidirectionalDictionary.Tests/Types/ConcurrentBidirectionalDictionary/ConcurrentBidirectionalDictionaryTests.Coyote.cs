using System.Collections;
using System.Collections.Concurrent;
using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using Microsoft.Coyote.SystematicTesting;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void Coyote_TryAddDuplicateKeyAndDuplicateValueRaces_PreservesOneToOneMapping()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(),
            [
                Worker(
                    dictionary =>
                    {
                        var added = dictionary.TryAdd(1, 'a');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                        AssertTryAddResultIsConsistent(dictionary, 1, 'a', added);
                    }),
                Worker(
                    dictionary =>
                    {
                        var added = dictionary.TryAdd(1, 'b');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                        AssertTryAddResultIsConsistent(dictionary, 1, 'b', added);
                    }),
                Worker(
                    dictionary =>
                    {
                        var added = dictionary.TryAdd(2, 'a');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                        AssertTryAddResultIsConsistent(dictionary, 2, 'a', added);
                    }),
            ],
            dictionary =>
            {
                AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                Assert.True(dictionary.Count is 1 or 2);
                Assert.True(dictionary.ContainsKey(1));
                Assert.True(dictionary.Inverse.ContainsKey('a'));
            });
    }

    [Fact]
    public void Coyote_MixedForwardAndInverseMutations_PreserveBothDirections()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(),
            [
                Worker(
                    dictionary =>
                    {
                        dictionary.TryAdd(1, 'a');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        if (dictionary.ContainsKey(1))
                        {
                            dictionary[1] = 'b';
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.TryRemove(1, out _);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        dictionary.Inverse.TryAdd('c', 2);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        if (dictionary.ContainsKey(2))
                        {
                            dictionary[2] = 'd';
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.Inverse.TryRemove('d', out _);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        dictionary.TryAdd(3, 'e');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.TryUpdate(3, 'f', 'e');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.Inverse.TryRemove('f', out _);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
            ],
            AssertConcurrentBidirectionalDictionaryInvariants);
    }

    [Fact]
    public void Coyote_TryRemoveRaces_RemoveAtMostOnceAndPreserveBothDirections()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(
            [
                new KeyValuePair<int, char>(1, 'a'),
                new KeyValuePair<int, char>(2, 'b'),
                new KeyValuePair<int, char>(3, 'c'),
            ]),
            [
                Worker(
                    dictionary =>
                    {
                        var removed = dictionary.TryRemove(1, out var value);
                        if (removed)
                        {
                            Assert.Equal('a', value);
                        }
                        else
                        {
                            Assert.Equal(default, value);
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        var removed = dictionary.Inverse.TryRemove('a', out var key);
                        if (removed)
                        {
                            Assert.Equal(1, key);
                        }
                        else
                        {
                            Assert.Equal(default, key);
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        var removed = dictionary.TryRemove(new KeyValuePair<int, char>(2, 'b'));
                        if (removed)
                        {
                            Assert.False(dictionary.ContainsKey(2));
                            Assert.False(dictionary.Inverse.ContainsKey('b'));
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.TryUpdate(3, 'd', 'c');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
            ],
            dictionary =>
            {
                AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                Assert.False(dictionary.ContainsKey(1) && dictionary.Inverse.ContainsKey('a'));
                Assert.False(dictionary.ContainsKey(2) && dictionary.Inverse.ContainsKey('b'));
            });
    }

    [Fact]
    public void Coyote_ClearRaces_DoNotLeaveHalfClearedMappings()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(
            [
                new KeyValuePair<int, char>(1, 'a'),
                new KeyValuePair<int, char>(2, 'b'),
                new KeyValuePair<int, char>(3, 'c'),
            ]),
            [
                Worker(
                    dictionary =>
                    {
                        dictionary.Clear();
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        dictionary.TryAdd(4, 'd');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary[5] = 'e';
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        dictionary.TryRemove(1, out _);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        AssertSnapshotIsSelfConsistent(dictionary.ToArray());
                        AssertSnapshotIsSelfConsistent(dictionary.Inverse.ToArray());
                    }),
            ],
            dictionary =>
            {
                AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                Assert.Equal(dictionary.Count == 0, dictionary.IsEmpty);
            });
    }

    [Fact]
    public void Coyote_GetOrAddRaces_ReturnExistingValueOrRejectDuplicateValue()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(),
            [
                Worker(
                    dictionary =>
                    {
                        var value = dictionary.GetOrAdd(1, _ => 'a');
                        Assert.Equal(value, dictionary[1]);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        var value = dictionary.GetOrAdd(1, _ => 'b');
                        Assert.Equal(value, dictionary[1]);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        try
                        {
                            Assert.Equal('x', dictionary.GetOrAdd(2, _ => 'x'));
                        }
                        catch (ArgumentException)
                        {
                            Assert.True(dictionary.Inverse.ContainsKey('x'));
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        try
                        {
                            Assert.Equal('x', dictionary.GetOrAdd(3, _ => 'x'));
                        }
                        catch (ArgumentException)
                        {
                            Assert.True(dictionary.Inverse.ContainsKey('x'));
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
            ],
            dictionary =>
            {
                Assert.Equal(dictionary[1], dictionary.GetOrAdd(1, 'c'));
                AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                Assert.True(dictionary.Inverse.ContainsKey('x'));
            });
    }

    [Fact]
    public void Coyote_AddOrUpdateRaces_RetryAndDuplicateValueRulesPreserveBothDirections()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(
            [
                new KeyValuePair<int, char>(1, 'a'),
                new KeyValuePair<int, char>(2, 'b'),
            ]),
            [
                Worker(
                    dictionary =>
                    {
                        var value = dictionary.AddOrUpdate(1, 'x', (_, oldValue) => oldValue == 'a' ? 'c' : oldValue);
                        Assert.Equal(value, dictionary[1]);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        dictionary.TryRemove(1, out _);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        var value = dictionary.AddOrUpdate(1, _ => 'x', (_, oldValue) => oldValue == 'a' ? 'c' : oldValue);
                        Assert.Equal(value, dictionary[1]);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        try
                        {
                            dictionary.AddOrUpdate(3, 'b', (_, oldValue) => oldValue == 'b' ? 'd' : oldValue);
                        }
                        catch (ArgumentException)
                        {
                            Assert.True(dictionary.Inverse.ContainsKey('b'));
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        if (dictionary.ContainsKey(1))
                        {
                            try
                            {
                                dictionary.AddOrUpdate(1, 'x', (_, _) => 'b');
                            }
                            catch (ArgumentException)
                            {
                                Assert.True(dictionary.Inverse.ContainsKey('b'));
                            }
                        }

                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
            ],
            AssertConcurrentBidirectionalDictionaryInvariants);
    }

    [Fact]
    public void Coyote_SnapshotsDuringMutations_AreInternallyConsistent()
    {
        RunCoyoteTest(
            new ConcurrentBidirectionalDictionary<int, char>(),
            [
                Worker(
                    dictionary =>
                    {
                        dictionary.TryAdd(1, 'a');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.TryAdd(2, 'b');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.TryAdd(3, 'c');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        dictionary.TryUpdate(1, 'd', 'a');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.TryRemove(2, out _);
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    },
                    dictionary =>
                    {
                        dictionary.GetOrAdd(4, 'e');
                        AssertConcurrentBidirectionalDictionaryInvariants(dictionary);
                    }),
                Worker(
                    dictionary =>
                    {
                        AssertSnapshotIsSelfConsistent(dictionary.ToArray());
                        AssertSnapshotIsSelfConsistent(dictionary.Inverse.ToArray());
                    },
                    dictionary =>
                    {
                        _ = dictionary.Keys.ToArray();
                        _ = dictionary.Values.ToArray();
                        _ = dictionary.Count;
                        AssertSnapshotIsSelfConsistent(dictionary.ToArray());
                    },
                    dictionary =>
                    {
                        foreach (var _ in dictionary)
                        {
                        }

                        AssertSnapshotIsSelfConsistent(dictionary.Inverse.ToArray());
                    },
                    dictionary =>
                    {
                        var count = dictionary.Count;
                        var pairs = new KeyValuePair<int, char>[count + 4];
                        ((ICollection<KeyValuePair<int, char>>)dictionary).CopyTo(pairs, 1);
                        AssertSnapshotIsSelfConsistent(pairs.Skip(1).Take(count).ToArray());
                    },
                    dictionary =>
                    {
                        var count = dictionary.Count;
                        var entries = new DictionaryEntry[count + 4];
                        ((ICollection)dictionary).CopyTo(entries, 1);
                        Assert.Equal(count, entries.Skip(1).Take(count).Count(static entry => entry.Key is not null));
                    }),
            ],
            AssertConcurrentBidirectionalDictionaryInvariants);
    }

    private static ScenarioWorkerEvent Worker(params Action<ConcurrentBidirectionalDictionary<int, char>>[] steps) =>
        new(null!, null!, steps);

    private static void RunCoyoteTest(
        ConcurrentBidirectionalDictionary<int, char> dictionary,
        ScenarioWorkerEvent[] workers,
        Action<ConcurrentBidirectionalDictionary<int, char>> assertFinalState)
    {
        var configuration = Configuration.Create()
            .WithTestingIterations(100)
            .WithMaxSchedulingSteps(1_000);

        var engine = TestingEngine.Create(
            configuration,
            runtime => runtime.CreateActor(
                typeof(ScenarioCoordinatorActor),
                new ScenarioEvent(dictionary, workers, assertFinalState)));

        engine.Run();

        Assert.True(
            engine.TestReport.NumOfFoundBugs == 0,
            engine.TestReport.GetText(configuration, string.Empty));
    }

    private static void AssertConcurrentBidirectionalDictionaryInvariants<TKey, TValue>(
        ConcurrentBidirectionalDictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : notnull
    {
        var forward = dictionary.ToArray();
        var inverse = dictionary.Inverse.ToArray();

        Assert.Equal(forward.Length, inverse.Length);
        Assert.Equal(forward.Length, dictionary.Count);
        Assert.Equal(inverse.Length, dictionary.Inverse.Count);
        AssertSnapshotIsSelfConsistent(forward);
        AssertSnapshotIsSelfConsistent(inverse);

        foreach (var pair in forward)
        {
            Assert.True(dictionary.Inverse.TryGetValue(pair.Value, out var key));
            Assert.Equal(pair.Key, key);
        }

        foreach (var pair in inverse)
        {
            Assert.True(dictionary.TryGetValue(pair.Value, out var value));
            Assert.Equal(pair.Key, value);
        }
    }

    private static void AssertTryAddResultIsConsistent(
        ConcurrentBidirectionalDictionary<int, char> dictionary,
        int key,
        char value,
        bool added)
    {
        if (added)
        {
            Assert.True(dictionary.TryGetValue(key, out var storedValue));
            Assert.Equal(value, storedValue);
            Assert.True(dictionary.Inverse.TryGetValue(value, out var storedKey));
            Assert.Equal(key, storedKey);
            return;
        }

        var keyExists = dictionary.ContainsKey(key);
        var valueExists = dictionary.Inverse.ContainsKey(value);

        if (!keyExists)
        {
            Assert.True(valueExists, $"TryAdd({key}, '{value}') returned false without a value conflict.");
        }

        if (!valueExists)
        {
            Assert.True(keyExists, $"TryAdd({key}, '{value}') returned false without a key conflict.");
        }
    }

    private static void AssertSnapshotIsSelfConsistent<TKey, TValue>(KeyValuePair<TKey, TValue>[] snapshot)
        where TKey : notnull
        where TValue : notnull
    {
        Assert.Equal(snapshot.Length, snapshot.Select(pair => pair.Key).Distinct().Count());
        Assert.Equal(snapshot.Length, snapshot.Select(pair => pair.Value).Distinct().Count());
    }

    private sealed class ScenarioEvent(
        ConcurrentBidirectionalDictionary<int, char> dictionary,
        ScenarioWorkerEvent[] workers,
        Action<ConcurrentBidirectionalDictionary<int, char>> assertFinalState)
        : Event
    {
        public ConcurrentBidirectionalDictionary<int, char> Dictionary { get; } = dictionary;
        public ScenarioWorkerEvent[] Workers { get; } = workers;
        public Action<ConcurrentBidirectionalDictionary<int, char>> AssertFinalState { get; } = assertFinalState;
    }

    private sealed class ScenarioWorkerEvent(
        ActorId coordinator,
        ConcurrentBidirectionalDictionary<int, char> dictionary,
        Action<ConcurrentBidirectionalDictionary<int, char>>[] steps)
        : Event
    {
        public ActorId Coordinator { get; } = coordinator;
        public ConcurrentBidirectionalDictionary<int, char> Dictionary { get; } = dictionary;
        public Action<ConcurrentBidirectionalDictionary<int, char>>[] Steps { get; } = steps;
    }

    private sealed class ContinueEvent : Event
    {
        public static ContinueEvent Instance { get; } = new();
    }

    private sealed class DoneEvent : Event
    {
        public static DoneEvent Instance { get; } = new();
    }

    [OnEventDoAction(typeof(DoneEvent), nameof(OnWorkerDone))]
    private sealed class ScenarioCoordinatorActor : Actor
    {
        private ConcurrentBidirectionalDictionary<int, char> _dictionary = null!;
        private Action<ConcurrentBidirectionalDictionary<int, char>> _assertFinalState = null!;
        private int _remainingWorkers;

        protected override Task OnInitializeAsync(Event initialEvent)
        {
            var scenario = (ScenarioEvent)initialEvent;
            _dictionary = scenario.Dictionary;
            _assertFinalState = scenario.AssertFinalState;
            _remainingWorkers = scenario.Workers.Length;

            foreach (var worker in scenario.Workers)
            {
                CreateActor(
                    typeof(ScenarioWorkerActor),
                    new ScenarioWorkerEvent(Id, _dictionary, worker.Steps));
            }

            return Task.CompletedTask;
        }

        private void OnWorkerDone()
        {
            _remainingWorkers--;
            if (_remainingWorkers == 0)
            {
                _assertFinalState(_dictionary);
                RaiseHaltEvent();
            }
        }
    }

    [OnEventDoAction(typeof(ContinueEvent), nameof(Continue))]
    private sealed class ScenarioWorkerActor : Actor
    {
        private ActorId _coordinator = null!;
        private Action<ConcurrentBidirectionalDictionary<int, char>>[] _steps = [];
        private int _nextStep;
        private ConcurrentBidirectionalDictionary<int, char> _dictionary = null!;

        protected override Task OnInitializeAsync(Event initialEvent)
        {
            var worker = (ScenarioWorkerEvent)initialEvent;
            _coordinator = worker.Coordinator;
            _dictionary = worker.Dictionary;
            _steps = worker.Steps;

            SendEvent(Id, ContinueEvent.Instance);
            return Task.CompletedTask;
        }

        private void Continue()
        {
            if (_nextStep < _steps.Length)
            {
                _steps[_nextStep](_dictionary);
                _nextStep++;
                SendEvent(Id, ContinueEvent.Instance);
                return;
            }

            SendEvent(_coordinator, DoneEvent.Instance);
            RaiseHaltEvent();
        }
    }
}
