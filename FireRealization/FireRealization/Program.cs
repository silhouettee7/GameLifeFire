using Contract;
using FireRealization;

int sizeX = 3;
int sizeY = 3;
var field = new FireField(sizeX, sizeY);
field.Initialize();
int.TryParse(Console.ReadLine(), out int startX);
int.TryParse(Console.ReadLine(), out int startY);

field.SetStartBurningCell(startX, startY);

Cell[,] newField;

for (int k = 0; k < 10; k++)
{
    newField = field.UpdateFieldAfterFire();
    for (int i = 0; i < sizeX; i++)
    {
        for (int j = 0; j < sizeY; j++)
        {
            Console.Write($"{(int)newField[i, j].State}  ");
        }

        Console.WriteLine();
    }

    Console.WriteLine();
    Thread.Sleep(1000);
}
