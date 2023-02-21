using NUnit.Framework;

public sealed class ObserverStateMachineTests
{
    private TestStateMachine _stateMachine;

    [SetUp]
    public void OnInitialize()
    {
        _stateMachine = new TestStateMachine();
        _stateMachine.TryAdd(new TestState1(_stateMachine));
        _stateMachine.TryAdd(new TestState2());
    }
    
    [Test]
    public void TryChangeTo_TestState2_ReturnsTrue()
    {
        Assert.AreEqual(typeof(TestState1), _stateMachine.Current.GetType());
        var testState2 = _stateMachine.Get<TestState2>();
        var isChanged = _stateMachine.TryChangeTo(testState2);
        Assert.IsTrue(isChanged);
        Assert.AreEqual(typeof(TestState2), _stateMachine.Current.GetType());
    }
    
    [Test]
    public void Get_TestState1_ReturnsCorrect()
    {
        Assert.AreEqual(typeof(TestState1), _stateMachine.Get<TestState1>().GetType());
    }
    
    [Test]
    public void TryAdd_TestState1_ReturnsFalse()
    {
        var isAdded = _stateMachine.TryAdd(new TestState1(_stateMachine));
        Assert.IsFalse(isAdded);
    }
    
    [Test]
    public void TryRemove_TestState2_ReturnsTrue()
    {
        var isRemoved = _stateMachine.TryRemove<TestState2>();
        Assert.IsTrue(isRemoved);
    }
    
    [Test]
    public void SetTrigger_DoSomething()
    {
        Assert.AreEqual(typeof(TestState1), _stateMachine.Current.GetType());
        _stateMachine.SetTrigger(new DoSomethingTrigger());
        Assert.AreEqual(typeof(TestState2), _stateMachine.Current.GetType());
    }
    
    private sealed class TestStateMachine : BaseObserverStateMachine
    {
    }
    
    private sealed class DoSomethingTrigger : ITrigger
    {
    }
    
    private abstract class BaseTestState : IState
    {
        public void OnEnter()
        {
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
    
    private sealed class TestState1 : BaseTestState
    {
        private BaseObserverStateMachine _stateMachine;
        
        public TestState1(BaseObserverStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.AddTriggerListener<DoSomethingTrigger>(this, DoSomething);
        }
        
        private void DoSomething(DoSomethingTrigger trigger)
        {
            var testState2 = _stateMachine.Get<TestState2>();
            _stateMachine.TryChangeTo(testState2);
        }
    }
    
    private sealed class TestState2 : BaseTestState
    {
    }
}