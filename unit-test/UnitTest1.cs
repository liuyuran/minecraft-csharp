using Base;

namespace unit_test;

public class Tests {
    [SetUp]
    public void Setup() {
        // 启动服务器
        new Thread(() => Game.Start("default")).Start();
    }

    [Test]
    public void Test1() {
        //
    }
}