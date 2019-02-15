using BBL.Utils.PacketDispatcher.Packet;
using BBL.Utils.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BBL.Utils.PacketDispatcher
{
    public sealed class PacketDispatcher<GamePacketHandlingType>
    {
        private static readonly Dictionary<ClientPackages, GamePacket> packets = new Dictionary<ClientPackages, GamePacket>();
        private static readonly Lazy<PacketDispatcher<GamePacketHandlingType>> lazy =
            new Lazy<PacketDispatcher<GamePacketHandlingType>>(() => new PacketDispatcher<GamePacketHandlingType>());

        public static PacketDispatcher<GamePacketHandlingType> Instance { get { return lazy.Value; } }

        private List<GamePacket> GetGamePackets()
        {
            List<GamePacket> references = new List<GamePacket>();
            foreach (Type handleType in typeof(GamePacketHandlingType).Assembly.GetTypes().Where(a => a.Name == typeof(GamePacketHandlingType).Name))
            {
                references.AddRange(handleType.GetMethods()
                    .Where(method => typeof(IPacket).IsAssignableFrom(method.GetParameters().FirstOrDefault()?.ParameterType) &&
                           method.GetParameters().Any(c => typeof(ISession).IsAssignableFrom(typeof(ISession))))
                    .Select(methodInfo => new GamePacket(DelegateBuilder.BuildDelegate<Action<IPacket, ISession>>(methodInfo),
                                                         methodInfo.GetCustomAttributes(typeof(GamePacketAttribute), true).FirstOrDefault() as GamePacketAttribute)));
            }
            return references;
        }

        private PacketDispatcher()
        {
            Console.WriteLine($"[PacketDispatcher] Begin Register");
            foreach (GamePacket gp in GetGamePackets())
            {
                Console.WriteLine($"[PacketDispatcher] Register {gp.Id}");
                packets.Add(gp.Id, gp);
            }
        }

        public bool Dispatch(IPacket packet, ISession session)
        {
            if (!packets.TryGetValue(packet.Id, out GamePacket handler))
            {
                Console.Error.WriteLine($"[PacketDispatcher] Unknown packet ID {packet.Id}");
                return false;
            }
            handler.Execute(packet, session);
            return true;
        }
    }
}
