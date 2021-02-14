using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace Snake
{
	class Program
	{
		private static int speed = 100;
		private static Score score = new Score();
		private static int snakeSkin = 0;
		private static bool gameBegins = false;
		private static int hp = 0;
		private static bool invul = false;
		private static System.Timers.Timer timer;
		static void Main(string[] args)
		{
			Console.SetWindowSize(80, 25);
			Console.SetBufferSize(80, 25);

			TitleScreen();

			if (gameBegins)
			{
				//Sound muz = new Sound(@"C:\Users\opilane\Projects\snake-master\sounds");
				Sound muz = new Sound(@"C:\Users\monti\OneDrive\Desktop\snake-master\sounds");
				muz.Play();
				Walls walls = new Walls(80, 25);
				walls.Draw();
				char sym = '*';
				// Отрисовка точек
				switch (snakeSkin)
				{
					case 1:
						sym = '*';
						break;
					case 2:
						sym = '@';
						break;
					case 3:
						sym = '½';
						break;
					default:
						break;
				}
				Point p = new Point(4, 5, sym);
				Snake snake = new Snake(p, 4, Direction.RIGHT);
				snake.Draw();

				FoodCreator foodCreator = new FoodCreator(80, 25, '$');
				FoodCreator powerFoodCreator = new PowerFoodCreator(80, 25, '!');
				Point food = foodCreator.CreateFood();
				Point powerFood = powerFoodCreator.CreateFood();
				food.Draw();

				Random r = new Random();

				while (true)
				{
					if (snake.IsHitTail())
					{
						if (invul == false)
						{
							if (hp > 0)
							{
								startInvul();
							}
							else
							{
								break;
							}
						}
					}		
					else if (walls.IsHit(snake))
                    {
						break;
                    }

					if (snake.Eat(food))
					{
						food = foodCreator.CreateFood();
						food.Draw();
						score.scoreNumber++;
						if (hp <= 0)
						{
							hp++;
						}
						if (speed > 5)
						{
							ChangeSpeed();
						}
						int rInt = r.Next(0, 4);
						if (rInt >= 3)
						{
							powerFood = powerFoodCreator.CreateFood();
							powerFood.Draw();
							if (speed > 5)
							{
								ChangeSpeed();
							}
						}
					}
					if (snake.Eat(powerFood))
					{
						score.scoreNumber += 10;

						if(hp<=0)
                        {
							hp++;
                        }
					}
					else
					{
						snake.Move();
					}
					Thread.Sleep(speed);
					if (Console.KeyAvailable)
					{
						ConsoleKeyInfo key = Console.ReadKey();
						snake.HandleKey(key.Key);
					}
					DisplayScore(score.scoreNumber);
					DisplayHp();
				}
				WriteGameOver();
				Console.ReadLine();
			}
		}	


		static void WriteGameOver()
		{ 
			Console.Clear();
			int xOffset = 25;
			int yOffset = 8;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition( xOffset, yOffset++ );
			WriteText( "============================", xOffset, yOffset++ );
			WriteText( "GAME OVER", xOffset + 10, yOffset++ );

			BestScore(xOffset, yOffset);

		}

		static void TitleScreen()
        {
			string titleText = "Choose a skin for your snake, type 1 (*), 2 (@) or 3 (½)";
			WriteText(titleText, 10, 12);
			do
			{
				string input = Console.ReadLine();
				snakeSkin = Int16.Parse(input);
				switch (snakeSkin)
				{
					case 1:
						Console.Clear();
						gameBegins = true;
						break;
					case 2:
						Console.Clear();
						gameBegins = true;
						break;
					case 3:
						Console.Clear();
						gameBegins = true;
						break;
					default:
						break;
				}
			} while (snakeSkin != 1 && snakeSkin != 2 && snakeSkin != 3);

		}

		static void WriteText( String text, int xOffset, int yOffset )
		{
			Console.SetCursorPosition( xOffset, yOffset );
			Console.WriteLine( text );
		}

		static void DisplayScore(int scoreToDisplay)
        {
			WriteText("SCORE: " + scoreToDisplay, 35, 1);
		}
		static void DisplayHp()
        {
			WriteText("Hp: " + hp, 60, 1);
		}

		static void ChangeSpeed()
        {
			speed -= 5;
		}

		static void BestScore(int xOffset, int yOffset)
        {
			//string filePath = @"C:\Users\opilane\Projects\snake-master\saves\save.txt";
			string filePath = @"C:\Users\monti\OneDrive\Desktop\snake-master\saves\save.txt";
			List<string> lines = new List<string>();
			lines = File.ReadAllLines(filePath).ToList();
			foreach (String line in lines)
			{ 
				WriteText(line, xOffset + 10, yOffset);
				yOffset += 1;
			}
			string playerName = Console.ReadLine();
			lines.Add(playerName + "   " + score.scoreNumber);
			lines = lines.OrderByDescending(q => q).ToList();
			File.WriteAllLines(filePath, lines);

			Console.Read();
		}
		static void startInvul()
        {
			invul = true;
			timer = new System.Timers.Timer(2000);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;

			if (hp > 0)
			{
				hp--;
			}

			Console.ForegroundColor = ConsoleColor.Yellow;
		}

		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			invul = false;
			timer.Enabled = false;
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
