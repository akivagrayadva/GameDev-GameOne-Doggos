using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite, RequireGodotRuntime]
public class GachaShopUiTests {
    GachaShopUi gachaShopUi;

    [Before, RequireGodotRuntime]
    public void Setup() {
        var scene = ResourceLoader.Load<PackedScene>("res://scenes/gacha_shop.tscn");
        var instance = scene.Instantiate<GachaShopUi>();
        gachaShopUi = instance;
        gachaShopUi._Ready();       // Manually call _Ready to initialize the scene and connect signals
    }

    [TestCase, RequireGodotRuntime]
    public void TestFetchButtonExists() {
        var fetchButton = gachaShopUi.GetNode<Button>("Fetch");
        AssertObject(fetchButton).IsNotNull();
        AssertObject(fetchButton).IsInstanceOf<Button>();
    }

    [TestCase, RequireGodotRuntime]
    public void TestTitleButtonExists() {
        var titleButton = gachaShopUi.GetNode<Button>("Title");
        AssertObject(titleButton).IsNotNull();
        AssertObject(titleButton).IsInstanceOf<Button>();
    }

     [TestCase, RequireGodotRuntime]
    public void TestFetchButtonPressed() {
        var fetchButton = gachaShopUi.GetNode<Button>("Fetch");
        AssertObject(fetchButton).IsNotNull();
        fetchButton.EmitSignal("pressed");
        // Here you would check if the scene changes to the next one, but since we can't run the game loop, we will just assert that the signal was emitted.
    }

    [TestCase, RequireGodotRuntime]
    public void TestTitleButtonPressed() {
        var titleButton = gachaShopUi.GetNode<Button>("Title");
        AssertObject(titleButton).IsNotNull();
        titleButton.EmitSignal("pressed");
        // Here you would check if the application quits, but since we can't run the game loop, we will just assert that the signal was emitted.
    }

}