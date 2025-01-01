namespace ITEC102FINALPROJECT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bool isMainMenu = true;
            bool isPlaying = false;
            string title = "BOUNCE";
            string title2 = "WARS";

            string figletText = Figgle.FiggleFonts.Standard.Render(title);
            string figletText2 = Figgle.FiggleFonts.Rectangles.Render(title2);

            if (isMainMenu && !isPlaying)
            {
                // Display the title once
                CenterFigletText(figletText);
                CenterFigletText(figletText2);

                Console.SetCursorPosition(50, 15);
                Console.WriteLine($"Press Enter to Start...");
                

                // Wait for the user to press Enter to start the game
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true);

                        if (key.Key == ConsoleKey.Enter)
                        {
                            isMainMenu = false;
                            isPlaying = true;
                            Console.Clear(); // Clear the screen before starting the game
                            break;
                        }
                    }
                }
            }
            // Mainmenu

            
            MainMenu();
        }

        static void CenterText(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int padding = (windowWidth - textLength) / 2;

            if (padding > 0)
            {
                Console.WriteLine(new string(' ', padding) + text);
            }
            else
            {
                Console.WriteLine(text);
            }
        }

        static void CenterFigletText(string figletText)
        {
            string[] lines = figletText.Split(Environment.NewLine);

            foreach (string line in lines)
            {
                CenterText(line); // Center each line
            }
        }
        static void MainMenu()
        {
        MainMenu:
            Console.Clear();
            Console.WriteLine($"1. PLAY");
            Console.WriteLine($"2. HOW TO PLAY GAME");
            Console.WriteLine($"3. ABOUT US");
            Console.WriteLine($"4. EXIT");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1)
                {
                    // Start the game when the user selects option 1
                    var game = new BounceWars();
                    game.Start();
                    Console.ReadKey();
                    return;
                }
                else if (key == ConsoleKey.D2)
                {
                    // Show how to play the game when the user selects option 2
                    ShowHowToPlay();
                    Console.ReadKey();
                    goto MainMenu;
                }
                else if (key == ConsoleKey.D3)
                {
                    // Show about us when the user selects option 3
                    ShowAboutUs();
                    Console.ReadKey();
                    goto MainMenu;
                }
                else if (key == ConsoleKey.D4)
                {
                    // Exit the program when the user selects option 4
                    Console.WriteLine($"\nPress any key to Confirm Exit...");                   
                    return;
                }
            }
        }

        static void ShowHowToPlay()
        {
            Console.Clear();
            Console.WriteLine($"HOW TO PLAY THE GAME");
            Console.WriteLine($"1. Player 1 uses W and S to move the paddle up and down.");
            Console.WriteLine($"2. Player 2 uses Up and Down arrow keys to move the paddle up and down.");
            Console.WriteLine($"3. The ball will bounce off the paddles and the walls.\n   But once the ball hits the wall where the players paddle are aligned to, a point will be given to the Player.");
            Console.WriteLine($"4. The first player to reach 10 points wins the game.");
            Console.Write($"\nPress any key to go back to the main menu...");
       
        }     
        static void ShowAboutUs()
        {
            Console.Clear();
            Console.WriteLine($"ABOUT US");
            Console.WriteLine($"Bounce Wars, inspired by the game 'Pong' This game was created as part of a final project for ITEC102.\nIt's Purpose is to also test the developers knowledge and their capability in Programming.\n");
            Console.WriteLine($"Developed by: MANGUNDAYAO, KING ALFONZE V.");
            Console.WriteLine($"              HIDALGO, DAREN PAOLO O.");
            Console.WriteLine($"              MATUNDAN, JAKE H.");
            Console.Write($"\nPress any key to go back to the main menu...");
            return;
        }
    }
    class BounceWars
    {
        const int GameWidth = 40;   //game border width
        const int GameHeight = 20;  //game border height
        const char Ball = 'O';      //the ball that will bounce
        const char PaddleL = '[';   //Paddle for Player 2 (or AI)
        const char PaddleR = ']';   //Paddle for Player 1
        const char Border = '#';    //Border of the Game
        int ballX = GameWidth / 2;           // position of the ball in x axis
        int ballY = GameHeight / 2;          // position of the ball in y axis
        int paddlePlayerL = GameHeight / 2;  //Player 1 paddle position (left)
        int paddlePlayerR = GameHeight / 2;  //Player 2 paddle position (right)
        int paddleHeight = 4;                //paddle height

        int player1Score = 0;       //Player 1 score
        int player2Score = 0;       //Player 2 score
        string leader = "";


        int ballVelocityX = 1;
        int ballVelocityY = 1;

        bool isAI = false; // Flag to check if the game mode is "1 Player vs AI"

        public BounceWars()
        {
            Console.SetWindowSize(GameWidth + 2, GameHeight + 2);
            Console.CursorVisible = false;
            ChooseGameMode();
        }
        void ChooseGameMode()
        {
            Console.Clear();
            Console.WriteLine($"Press 1 or 2 to choose the game mode.\nPress Escape to return to Main Menu\n");
            Console.WriteLine($"1 - 2 Player");
            Console.WriteLine($"2 - 1 Player vs Simple AI");
            
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1)
                {
                    isAI = false; // 2 Player mode
                    break;  // Proceed with the game
                }
                else if (key == ConsoleKey.D2)
                {
                    isAI = true; // 1 Player vs AI mode
                    break;  // Proceed with the game
                }
                else if (key == ConsoleKey.Escape)
                {
                    // If Escape is pressed, return to the main menu
                    return;  // Exit the method and go back to Main Menu
                }
            }
        }
        public void Start()      //makes the game constantly running
        {
            while (true)
            {
                // Check if any player has reached 10 points
                if (player1Score == 10 || player2Score == 10)
                {

                    EndGame(); // End the game if someone wins
                    
                    break; // Exit the game loop

                }
 
                Draw();
                HandleInput();
                UpdateGame();
                Thread.Sleep(50); // Game speed
            }

        }

        void Draw()
        {
            Console.Clear();

            // Draw the top border
            Console.WriteLine(new string(Border, GameWidth));

            // Draw the game area
            for (int y = 0; y < GameHeight; y++)
            {
                for (int x = 0; x < GameWidth; x++)
                {
                    if (x == 0 || x == GameWidth - 1) // Draw borders
                    {
                        Console.Write(Border);
                    }
                    else if (x == ballX && y == ballY) // Ball position
                    {
                        Console.Write(Ball);
                    }
                    else if (x == 1 && y >= paddlePlayerL && y < paddlePlayerL + paddleHeight) // Player 1's paddle
                    {
                        Console.Write(PaddleR);
                    }
                    else if (x == GameWidth - 2 && y >= paddlePlayerR && y < paddlePlayerR + paddleHeight) // Player 2's paddle (AI or Player)
                    {
                        Console.Write(PaddleL);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }

            // Draw the bottom border
            Console.WriteLine(new string(Border, GameWidth));

            Console.SetCursorPosition(GameWidth / 2 - 5, GameHeight + 1);
            Console.Write($"\nPlayer 1: {player1Score} \nPlayer 2: {player2Score}\nLeader:{leader}");

            // Determine the score leader or if it's tied
            if (player1Score > player2Score)
            {
                leader = "Player1";
            }
            else if (player1Score < player2Score)
            {
                leader = "Player2";
            }
            else
            {
                leader = "Tied      ";
            }
            // Clear the leader part to avoid overwriting errors    
            Console.Write(new string(' ', 40)); // Clear 20 characters of space (enough to clear the leader)
                                                // Draw the leader message

            if (leader == "Tied")
            {
                Console.Write($"Tied    "); // Display "Tied" when scores are equal
            }

        }

        void HandleInput()
        {
            // Always handle Player 1's paddle movement
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                // Move Player 1's paddle
                if (key == ConsoleKey.W && paddlePlayerL > 0)
                    paddlePlayerL--;
                if (key == ConsoleKey.S && paddlePlayerL + paddleHeight < GameHeight)
                    paddlePlayerL++;

                // Move Player 2's paddle
                if (key == ConsoleKey.UpArrow && paddlePlayerR > 0)
                    paddlePlayerR--;
                if (key == ConsoleKey.DownArrow && paddlePlayerR + paddleHeight < GameHeight)
                    paddlePlayerR++;
            }

            // Only handle Player 2's paddle movement if it's a 2 Player game
            if (!isAI && Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                // Move Player 2's paddle
                if (key == ConsoleKey.UpArrow && paddlePlayerR > 0)
                    paddlePlayerR--;
                if (key == ConsoleKey.DownArrow && paddlePlayerR + paddleHeight < GameHeight)
                    paddlePlayerR++;
            }
        }
        void UpdateGame()
        {
            // Move the ball
            ballX += ballVelocityX;
            ballY += ballVelocityY;

            // Ball collision with top and bottom walls
            if (ballY <= 0 || ballY >= GameHeight - 1)
            {
                ballVelocityY = -ballVelocityY; // Reverse direction
            }

            // Ball collision with paddles
            if (ballX == 1 && ballY >= paddlePlayerL && ballY < paddlePlayerL + paddleHeight) // Left paddle
            {
                ballVelocityX = -ballVelocityX; // Reverse direction
            }
            if (ballX == GameWidth - 2 && ballY >= paddlePlayerR && ballY < paddlePlayerR + paddleHeight) // Right paddle
            {
                ballVelocityX = -ballVelocityX; // Reverse direction
            }

            // Ball out of bounds (left or right side)
            if (ballX <= 0)
            {
                player2Score++;
                ResetBall();
            }
            if (ballX >= GameWidth - 1)
            {
                player1Score++;
                ResetBall();
            }

            // AI movement for Player 2 (if AI is enabled)
            if (isAI)
            {
                AILogic();
            }

        }

        void AILogic()
        {
            // Simple AI logic: Move the AI paddle to follow the ball

            Random rng = new Random();
            int offset = rng.Next(1, 2); // Random offset to make the AI less predictable
            if (ballY < paddlePlayerR)
            {
                if (paddlePlayerR > 0)
                {
                    offset = rng.Next(1, 3);
                    paddlePlayerR -= offset;
                }

            }
            else if (ballY > paddlePlayerR + paddleHeight - 1)
            {
                if (paddlePlayerR + paddleHeight < GameHeight)
                {
                    offset = rng.Next(1, 3);
                    paddlePlayerR += offset;
                }
            }
        }

        void ResetBall()
        {
            ballX = GameWidth / 2;
            ballY = GameHeight / 2;
            ballVelocityX = -ballVelocityX; // Reverse direction after scoring
            ballVelocityY = 1; // Reset vertical direction
        }

        void EndGame()
        {
            Console.Clear();

            
            // Display winner message
            if (player1Score >= 10)
            {
                Console.WriteLine($"P layer 1 wins!");
            }
            else if (player2Score >= 10)
            {
                Console.WriteLine($"Player 2 wins!");
            }

            // Wait for a moment before closing
            Console.WriteLine($"\nPress any key to exit...");
            Console.ReadKey(); // Wait for user input to close
        }

    }
}