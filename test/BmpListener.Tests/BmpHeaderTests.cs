using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace BmpListener.Tests
{
    public class BmpHeaderTests
    {
        [Fact]
        public void PassingTest()
        {
            var data = new byte[] { 3, 0, 0, 0, 0, 6 };
            //var bmpHeader = new BgpHeader
          
            Assert.Equal(4, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
