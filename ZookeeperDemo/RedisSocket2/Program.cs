using Sodao.FastSocket.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace RedisSocket2
{
    class Program
    {
        private static Timer _timer;
        private static AsyncBinarySocketClient client;

        static void Main(string[] args)
        {
            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 130000;
            _timer.Start();

            Sodao.FastSocket.SocketBase.Log.Trace.EnableConsole();
            Sodao.FastSocket.SocketBase.Log.Trace.EnableDiagnostic();
            client = new Sodao.FastSocket.Client.AsyncBinarySocketClient(8192, 8192, 3000, 3000);
            //注册服务器节点，这里可注册多个(name不能重复）
            client.RegisterServerNode("192.168.140.23:8369", new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.140.23"), 8369));
            //client.RegisterServerNode("127.0.0.1:8402", new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.2"), 8401));


            byte[] bytes = SendCommand(RedisCommand.PING);



            //发送sum命令
            client.Send(bytes, res => res.Buffer).ContinueWith(c =>
            {
                if (c.IsFaulted)
                {
                    Console.WriteLine(c.Exception.ToString());
                    return;
                }
                Console.WriteLine(c.Result);
            });




            Console.ReadLine();
        }

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            byte[] bytes = SendCommand(RedisCommand.PING);
            //发送sum命令
            client.Send("sum", bytes, res => BitConverter.ToInt32(res.Buffer, 0)).ContinueWith(c =>
            {
                if (c.IsFaulted)
                {
                    Console.WriteLine(c.Exception.ToString());
                    return;
                }
                Console.WriteLine(c.Result);
            });
        }

        public static byte[] SendCommand(RedisCommand command, params string[] args)
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
            return Encoding.UTF8.GetBytes(sb.ToString());
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
