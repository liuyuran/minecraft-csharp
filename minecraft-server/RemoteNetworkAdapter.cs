using System.Diagnostics.CodeAnalysis;
using Base.NetworkAdapters;
using Base.Utils;

namespace Server {
    /// <summary>
    /// 远程网络适配器，用于构建C/S架构的独立服务器
    /// 在此模式下，仅需实现服务端的相关接口
    /// 客户端也需要实现一个与其对应的类
    /// </summary>
    public interface RemoteNetworkAdapter: INetworkAdapter {
    }
}