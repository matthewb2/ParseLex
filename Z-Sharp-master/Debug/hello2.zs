func Main()
{
    int screenWidth = 500
    int screenHeight = 500
    ZS.Graphics.Init("Title of window", screenWidth, screenHeight)
    // After graphics are initialized, the main function will not finish.
    // Instead, Start() will be called a single time, then Update() every frame after that.
}

// Runs once at start of graphics initialization
func Start()
{
    // Vec2 are initialized using function 'NVec2(x, y)'
    Vec2 position = NVec2(250, 250)
    Vec2 scale = NVec2(20, 20)
    float rotation = 0

    // Sprite object, stores (and loads from file) the texture, location, scale, and rotation
    global Sprite exampleSprite = ZS.Graphics.Sprite("./square.png", position, scale, rotation)
}

// Executes each frame
func Update(deltaTime)
{
    // Draws the image created in Start(). This is usually at the end of update.
    ZS.Graphics.Draw(exampleSprite)   
}