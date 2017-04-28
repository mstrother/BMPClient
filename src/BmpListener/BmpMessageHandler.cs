using BmpListener.Bmp;
using DotNetty.Transport.Channels;
using System;
using System.Threading.Tasks;

namespace BmpListener
{
    public class BmpMessageHandler : SimpleChannelInboundHandler<BmpMessage>
    {
        public static Func<BmpMessage, Task> ProcessMessage;

        public override void ExceptionCaught(IChannelHandlerContext context, Exception e) => context.CloseAsync();

        protected override void ChannelRead0(IChannelHandlerContext context, BmpMessage msg)
        {
            try
            {
                ProcessMessage?.Invoke(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
