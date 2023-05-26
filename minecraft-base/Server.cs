using System.Threading;

namespace Base {
    /// <summary>
    /// 服务器主类，可是该怎么写呢？
    /// </summary>
    public class Server {
        private void Update() {
            //
        }
        
        public void Start(bool isLocal) {
            while (true) {
                Thread.Sleep(1);
                Update();
            }
        }
    }
}