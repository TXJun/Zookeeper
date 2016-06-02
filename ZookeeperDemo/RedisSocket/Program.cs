using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RedisSocket
{
    class Program
    {
        private static Timer _timer;
        private static RedisBaseClient redis;


        static void Main(string[] args)
        {
            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 1000;
            _timer.Start();

            Configuration conf = new Configuration();

            redis = new RedisBaseClient(conf);
            redis.Connect();



            Console.ReadLine();
        }

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var result = redis.SendCommand(RedisCommand.PING);

            Console.WriteLine(result == null ? "Redis断开" : result.Substring(0, 5));
        }
    }

    public class Configuration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// Socket 是否正在使用 Nagle 算法。
        /// </summary>
        public bool NoDelaySocket { get; set; }

        public Configuration()
        {
            Host = "192.168.140.23";
            Port = 8369;
            NoDelaySocket = false;
        }
    }

    public class RedisBaseClient
    {
        //配置文件
        private Configuration configuration;
        //通信socket
        private Socket socket;
        //接收字节数组
        private byte[] ReceiveBuffer = new byte[100000];
        private readonly object locker = new object();

        public RedisBaseClient(Configuration config)
        {
            configuration = config;
        }

        public RedisBaseClient()
            : this(new Configuration())
        {
        }

        public void Connect()
        {
            if (socket != null && socket.Connected)
                return;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = configuration.NoDelaySocket
            };
            socket.Connect(configuration.Host, configuration.Port);
            if (socket.Connected)
                return;
            Close();
        }

        /// <summary>
        /// 关闭client
        /// </summary>
        public void Close()
        {
            //try
            //{
            //    socket.Disconnect(false);
            //    socket.Close();
            //}
            //catch (Exception)
            //{

              
            //}
        }

        public string SendCommand(RedisCommand command, params string[] args)
        {
            //请求头部格式， *<number of arguments>\r\n
            const string headstr = "*{0}\r\n";
            //参数信息       $<number of bytes of argument N>\r\n<argument data>\r\n
            const string bulkstr = "${0}\r\n{1}\r\n";

            var sb = new StringBuilder();
            sb.AppendFormat(headstr, args.Length + 1);

            var cmd = command.ToString();
            sb.AppendFormat(bulkstr, cmd.Length, cmd);

            foreach (var arg in args)
            {
                sb.AppendFormat(bulkstr, arg.Length, arg);
            }
            byte[] c = Encoding.UTF8.GetBytes(sb.ToString());
            try
            {
                Connect();
                socket.Send(c);

                socket.Receive(ReceiveBuffer);
                Close();
                return ReadData();
            }
            catch (SocketException e)
            {
                Close();
            }
            return null;
        }

        private string ReadData()
        {
            var data = Encoding.UTF8.GetString(ReceiveBuffer);
            char c = data[0];
            //错误消息检查。
            if (c == '-') //异常处理。
                throw new Exception(data);
            //状态回复。
            if (c == '+')
                return data;
            return data;
        }
    }

    public enum RedisCommand
    {
        GET, //获取一个key的值
        INFO, //Redis信息。  
        SET, //添加一个值
        EXPIRE, //设置过期时间
        MULTI, //标记一个事务块开始
        EXEC, //执行所有 MULTI 之后发的命令
        PING,
    }


}
