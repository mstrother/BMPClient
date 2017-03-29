using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Codecs;
using DotNetty.Buffers;
using System.Threading;
using DotNetty.Common.Concurrency;
using BmpListener.Bmp;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace BmpListener
{
    public class BmpStation
    {
        const int ListenBacklogSize = 200; // connections allowed pending accept
        static readonly TimeSpan DefaultConnectionIdleTimeout = TimeSpan.FromSeconds(180); // connection idle timeout
        
        readonly TaskCompletionSource closeCompletionSource;
        readonly IEventLoopGroup parentEventLoopGroup;
        readonly IEventLoopGroup eventLoopGroup;
        readonly ServerBootstrap bootstrap;
        IChannel channel;

        public BmpStation(int port)
            : this()
        {
            Port = port;
        }

        public BmpStation()
        {
            bootstrap = new ServerBootstrap();
            closeCompletionSource = new TaskCompletionSource();
            parentEventLoopGroup = new MultithreadEventLoopGroup(1);
            eventLoopGroup = new MultithreadEventLoopGroup();
        }

        public Task CloseCompletion => closeCompletionSource.Task;

        //public IObservable<BmpMessage> OnMessageReceived
        //{
        //    get { return ((BmpMessageHandler)channel.Pipeline.Last()).OnMessageReceived; }
        //}

        public int Port { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Bootstrap();
                channel = await bootstrap.BindAsync(Port);                
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
                if (channel != null)
                {
                    await channel.CloseAsync();
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
                
        void Bootstrap()
        {
            bootstrap.Group(parentEventLoopGroup, eventLoopGroup);
            bootstrap.Option(ChannelOption.SoBacklog, ListenBacklogSize);
            bootstrap.ChildOption(ChannelOption.Allocator, PooledByteBufferAllocator.Default);
            bootstrap.Channel<TcpServerSocketChannel>();
            bootstrap.Option(ChannelOption.SoBacklog, 100);
            bootstrap.Option(ChannelOption.ConnectTimeout, DefaultConnectionIdleTimeout);
            bootstrap.ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                IChannelPipeline pipeline = channel.Pipeline;
                pipeline.AddLast(new LengthFieldBasedFrameDecoder(ByteOrder.BigEndian, 4096, 1, 4, -5, 0, true));
                pipeline.AddLast(new BmpDecoder(), new BmpMessageHandler());
            }));
        }


    }
}
