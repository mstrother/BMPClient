using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Codecs;
using DotNetty.Buffers;
using System.Threading;
using DotNetty.Common.Concurrency;

namespace BmpListener
{
    public class BmpStation
    {
        const int ListenBacklogSize = 200; // connections allowed pending accept
        static readonly TimeSpan DefaultConnectionIdleTimeout = TimeSpan.FromSeconds(180); // connection idle timeout
        readonly int port = 11019; // default port

        readonly TaskCompletionSource closeCompletionSource;
        IEventLoopGroup parentEventLoopGroup;
        IEventLoopGroup eventLoopGroup;
        IChannel serverChannel;

        public BmpStation(int port)
            : this()
        {
            this.port = port;
        }

        public BmpStation()
        {
            closeCompletionSource = new TaskCompletionSource();
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
        
        public Task CloseCompletion => closeCompletionSource.Task;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            parentEventLoopGroup = new MultithreadEventLoopGroup(1);
            eventLoopGroup = new MultithreadEventLoopGroup();

            try
            {
                ServerBootstrap bootstrap = SetupBootstrap();
                serverChannel = await bootstrap.BindAsync(port);

                cancellationToken.Register(StopAsync);
            }
            catch (Exception ex)
            {
                StopAsync();
            }
        }

        async void StopAsync()
        {
            try
            {
                if (serverChannel != null)
                {
                    await serverChannel.CloseAsync();
                }
                if (parentEventLoopGroup != null)
                {
                    await eventLoopGroup.ShutdownGracefullyAsync();
                }
                if (eventLoopGroup != null)
                {
                    await eventLoopGroup.ShutdownGracefullyAsync();
                }
            }
            finally
            {
                closeCompletionSource.TryComplete();
            }
        }

        ServerBootstrap SetupBootstrap()
        {
            return new ServerBootstrap()
                .Group(parentEventLoopGroup, eventLoopGroup)
                .Option(ChannelOption.SoBacklog, ListenBacklogSize)
                .ChildOption(ChannelOption.Allocator, PooledByteBufferAllocator.Default)
                            .Channel<TcpServerSocketChannel>()
                            .Option(ChannelOption.SoBacklog, 100)
                            .Option(ChannelOption.ConnectTimeout, DefaultConnectionIdleTimeout)
                            .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                            {
                                IChannelPipeline pipeline = channel.Pipeline;
                                pipeline.AddLast(new LengthFieldBasedFrameDecoder(ByteOrder.BigEndian, 4096, 1, 4, -5, 0, true));
                                pipeline.AddLast(new BmpDecoder(), new BmpMessageHandler(OnMessageReceived));
                            }));
        }
    }
}
