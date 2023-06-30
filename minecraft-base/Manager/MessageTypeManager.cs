using System;
using System.IO;
using Base.Interface;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Base.Manager {
    public delegate void GameEventHandler(object sender, GameEvent e);
    /// <summary>
    /// 网络信息注册类，兼做注册消息类型之用
    /// 由于需要包装的消息类型越来越多，仅靠泛型人脑管理已经不太现实，因此得使用统一协议使用反射来调用了
    /// 为了保证双端队列序列化/反序列化的兼容性，使用protobuf保证前后类型一致
    /// </summary>
    public class MessageTypeManager {
        public static MessageTypeManager Instance { get; } = new();
        public event GameEventHandler? GameEventHandlers;
        
        private MessageTypeManager() {
            var subTypeDefine = RuntimeTypeModel.Default
                .Add(typeof(GameEvent));
            var assem = typeof(GameEvent).Assembly;
            var baseType = typeof(GameEvent);
            var types = assem.GetExportedTypes();
            for (var index = 0; index < types.Length; index++) {
                var type = types[index];
                if (type.IsAbstract || type.FullName == null) continue;
                if (!type.IsSubclassOf(baseType)) continue;
                subTypeDefine.AddSubType(index + 1, type);
            }
        }
        
        public string Serialize(GameEvent gameEvent) {
            var msTestString = new MemoryStream();
            Serializer.Serialize(msTestString, gameEvent);
            msTestString.Position = 0;
            var srRegBlock = new StreamReader(msTestString);
            var bytes = System.Text.Encoding.Default.GetBytes(srRegBlock.ReadToEnd());
            return Convert.ToBase64String(bytes);
        }
        
        public GameEvent Deserialize(string str) {
            var bytes = Convert.FromBase64String(str);
            var msTestString = new MemoryStream(bytes);
            return Serializer.Deserialize<GameEvent>(msTestString);
        }

        public void OnGameEventHandlers(GameEvent e) {
            GameEventHandlers?.Invoke(this, e);
        }
    }
}