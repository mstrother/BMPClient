using DotNetty.Codecs;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using BmpListener.Bmp;

namespace BmpListener
{
    public class BmpDecoder : ByteToMessageDecoder
    {   

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            try
            {
                var length = input.ReadableBytes;

                if (length < 6)
                {
                    return;
                }

                // check bytes

                var data = new byte[length];
                input.ReadBytes(data);
                var bmpMessage = BmpMessage.Create(data);

                if (bmpMessage != null)
                {
                    output.Add(bmpMessage);
                }
            }
            catch (DecoderException)
            {
                input.SkipBytes(input.ReadableBytes);
                throw;
            }
        }
    }
}
