using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Base.Interface;
using Base.Utils;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Base.Manager {
    public delegate void GameEventDelegate(object sender, GameEvent e);
    /// <summary>
    /// 网络信息注册类，兼做注册消息类型之用
    /// 由于需要包装的消息类型越来越多，仅靠泛型人脑管理已经不太现实，因此得使用统一协议使用反射来调用了
    /// 为了保证双端队列序列化/反序列化的兼容性，使用protobuf保证前后类型一致
    /// </summary>
    public class MessageTypeManager {
        public static MessageTypeManager Instance { get; } = new();
        private readonly Dictionary<Type, List<object>> _handlers = new();
        private readonly Dictionary<Type, List<MethodInfo>> _handlersMethod = new();
        public event GameEventDelegate? GameEventHandlers;
        
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
                _handlers.Add(type, new List<object>());
                _handlersMethod.Add(type, new List<MethodInfo>());
            }

            var allHandler = typeof(IGameEventHandler<>).GetTypesImplementingOpenGenericType(Assembly.GetExecutingAssembly());
            foreach (var (handlerType, eventType) in allHandler) {
                if (handlerType.IsAbstract || handlerType.FullName == null) continue;
                if (!_handlers.ContainsKey(eventType)) continue;
                var instance = Activator.CreateInstance(handlerType);
                _handlers[eventType].Add(instance);
                _handlersMethod[eventType].Add(handlerType.GetMethod("Run") ?? throw new InvalidOperationException());
            }
        }
        
        public void FireEvent(GameEvent gameEvent) {
            var type = gameEvent.GetType();
            LogManager.Instance.Debug(type.FullName ?? "unknown event");
            if (!_handlers.ContainsKey(type)) return;
            for (var index = 0; index < _handlers[type].Count; index++) {
                var handler = _handlers[type][index];
                var method = _handlersMethod[type][index];
                method.Invoke(handler, new object[] { gameEvent });
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