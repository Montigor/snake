using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class PowerFoodCreator : FoodCreator
    {
		int mapWidht;
		int mapHeight;
		char sym;

		Random random = new Random();

		public PowerFoodCreator(int mapWidht, int mapHeight, char sym) : base(mapWidht, mapHeight, sym)
		{
			this.mapWidht = mapWidht;
			this.mapHeight = mapHeight;
			this.sym = sym;
		}

		public Point CreatePowerFood ()
		{
			int x = random.Next(2, mapWidht - 2);
			int y = random.Next(2, mapHeight - 2);
			return new Point(x, y, sym);
		}
	}
}
