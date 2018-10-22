using ReactAdvantage.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Domain
{
    public class DisposeActionShould
    {
        [Fact]
        public void RunActionOnDispose()
        {
            //Given
            var actionCalled = false;

            var disposeAction = new DisposeAction(() => { actionCalled = true; });
            
            //When
            disposeAction.Dispose();

            //Then
            Assert.True(actionCalled);
        }

        [Fact]
        public void NotCallDisposeActionTwice()
        {
            //Given
            var actionCalled = false;

            var disposeAction = new DisposeAction(() => { actionCalled = true; });
            disposeAction.Dispose();
            actionCalled = false;

            //When
            disposeAction.Dispose();

            //Then
            Assert.False(actionCalled);
        }
    }
}
